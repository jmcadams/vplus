//=====================================================================
//
//	E131SetupForm.cs - the setup dialog form
//
//		version 1.0.0.1 - 2 june 2010
//
//=====================================================================

//=====================================================================
//
// Copyright (c) 2010 Joshua 1 Systems Inc. All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification, are
// permitted provided that the following conditions are met:
//
//    1. Redistributions of source code must retain the above copyright notice, this list of
//       conditions and the following disclaimer.
//
//    2. Redistributions in binary form must reproduce the above copyright notice, this list
//       of conditions and the following disclaimer in the documentation and/or other materials
//       provided with the distribution.
//
// THIS SOFTWARE IS PROVIDED BY JOSHUA 1 SYSTEMS INC. "AS IS" AND ANY EXPRESS OR IMPLIED
// WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> OR
// CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
// SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON
// ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
// NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF
// ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//
// The views and conclusions contained in the software and documentation are those of the
// authors and should not be interpreted as representing official policies, either expressed
// or implied, of Joshua 1 Systems Inc.
//
//=====================================================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Windows.Forms;

using Controllers.E131.Controls;
using Controllers.E131.J1Sys;

namespace Controllers.E131 {
    public partial class E131SetupForm : UserControl {
        // column indexes - must be changed if column addrange code is changed
        // could refactor to a variable and initialize it at column add time
        // but then it wouldn't work well with switch/case code

        private const int ActiveColumn = 0;
        private const int UniverseColumn = 1;
        private const int StartColumn = 2;
        private const int SizeColumn = 3;
        private const int DestinationColumn = 4;
        private const int TtlColumn = 5;

        // plugin channel count as set by vixen
        private int _pluginChannelCount;

        // universe datagridview - using DataGridViewNumbered derived class


        // universe datagridview cell event arguments to track mouse entry


        // sorted lists and dictionaries for network interface tracking
        private readonly SortedList<string, int> _multicasts = new SortedList<string, int>();
        private readonly SortedList<string, int> _unicasts = new SortedList<string, int>();
        private readonly SortedList<string, int> _badIDs = new SortedList<string, int>();
        private readonly SortedDictionary<string, string> _nicIDs = new SortedDictionary<string, string>();
        private readonly SortedDictionary<string, string> _nicNames = new SortedDictionary<string, string>();


        //-------------------------------------------------------------
        //
        //	E131SetupForm() - our constructor
        //
        //		build some nic tables and initialize the component
        //
        //-------------------------------------------------------------

        public E131SetupForm() {
            // first build some sorted lists and dictionaries for the nics

            // get all the nics
            var nics = NetworkInterface.GetAllNetworkInterfaces();

            // do we have a nics?
            if (nics.Length > 0)
                // then iterate through them
                foreach (
                    var nic in
                        nics.Where(nic => nic.NetworkInterfaceType.CompareTo(NetworkInterfaceType.Tunnel) != 0).Where(nic => nic.SupportsMulticast)) {
                    // then add it to multicasts table by name
                    _multicasts.Add(nic.Name, 0);
                    // add it to available nicIDs table
                    _nicIDs.Add(nic.Id, nic.Name);
                    // add ot to available nicNames table
                    _nicNames.Add(nic.Name, nic.Id);
                }

            // finally initialize the form

            InitializeComponent();
            rowManipulationContextMenuStrip.Items.Add("Insert Row", null, new EventHandler(univDGVN_InsertRow));
            rowManipulationContextMenuStrip.Items.Add("Delete Row", null, new EventHandler(univDGVN_DeleteRow));
            rowManipulationContextMenuStrip.Items.Add("-");
            rowManipulationContextMenuStrip.Items.Add("Move Row Up", null, new EventHandler(univDGVN_MoveRowUp));
            rowManipulationContextMenuStrip.Items.Add("Move Row Down", null, new EventHandler(univDGVN_MoveRowDown)); 
            SetDestinations();
        }


        //-------------------------------------------------------------
        //
        //	Dispose() - our dispose
        //
        //-------------------------------------------------------------



        //-------------------------------------------------------------
        //
        //	PluginChannelCount - property to expose channel count
        //
        //-------------------------------------------------------------

        public int PluginChannelCount {
            set { _pluginChannelCount = value; }
        }

        //-------------------------------------------------------------
        //
        //	UniverseCount - property to expose universe count
        //
        //-------------------------------------------------------------

        public int UniverseCount {
            get { return univDGVN.Rows.Count; }
        }

        //-------------------------------------------------------------
        //
        //	UniverseClear() - clear the rows in the datagridview
        //
        //		probably never needed
        //
        //-------------------------------------------------------------

        public void UniverseClear() {
            univDGVN.Rows.Clear();
        }


        //-------------------------------------------------------------
        //
        //	UniverseAdd() - add a row from config to rows and tables
        //
        //-------------------------------------------------------------

        public bool UniverseAdd(bool active, Int32 universe, Int32 start, Int32 size, string unicast, string multicast, Int32 ttl) {
            string destination = null;

            // if it is unicast we add the destination to the
            // drop down list if it isn't already there
            // and we 'reformat' to text for display

            if (unicast != null) {
                if (!_unicasts.ContainsKey(unicast)) {
                    _unicasts.Add(unicast, 0);
                    destinationColumn.Items.Add("Unicast " + unicast);
                }

                destination = "Unicast " + unicast;
            }

            // if it is multicast we check for the id to match
            // a nic. if it doesn't we warn of interface changes
            // and store in bad id's so we only warn once

            if (multicast != null) {
                if (_nicIDs.ContainsKey(multicast)) {
                    destination = "Multicast " + _nicIDs[multicast];
                }

                else {
                    if (!_badIDs.ContainsKey(multicast)) {
                        _badIDs.Add(multicast, 0);
                        MessageBox.Show(@"Warning - Interface IDs have changed. Please reselect all empty destinations.", @"Network Interface Mapping",
                                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }

            // all set, add the row - convert int's to strings ourselves

            univDGVN.Rows.Add(new object[] {
                active, universe.ToString(CultureInfo.InvariantCulture), start.ToString(CultureInfo.InvariantCulture),
                size.ToString(CultureInfo.InvariantCulture), destination, ttl.ToString(CultureInfo.InvariantCulture)
            });

            return true;
        }


        //-------------------------------------------------------------
        //
        //	UniverseGet() - allow referenced retrieval of the data
        //
        //-------------------------------------------------------------

        public bool UniverseGet(int index, ref bool active, ref Int32 universe, ref Int32 start, ref Int32 size, ref string unicast,
                                ref string multicast, ref Int32 ttl) {
            var row = univDGVN.Rows[index];

            if (row.IsNewRow) return false;

            if (row.Cells[ActiveColumn].Value == null) active = false;
            else active = (bool) row.Cells[ActiveColumn].Value;

            // all numeric columns are stored as strings
            universe = OutputPlugin.TryParseInt32((string) row.Cells[UniverseColumn].Value, 1);
            start = OutputPlugin.TryParseInt32((string) row.Cells[StartColumn].Value, 1);
            size = OutputPlugin.TryParseInt32((string) row.Cells[SizeColumn].Value, 1);
            ttl = OutputPlugin.TryParseInt32((string) row.Cells[TtlColumn].Value, 1);

            // first set both unicast and multicast results to null

            unicast = null;
            multicast = null;

            // then set the selected unicast/multicast destination

            if (row.Cells[DestinationColumn].Value != null) {
                var destination = (string) row.Cells[DestinationColumn].Value;

                if (destination.StartsWith("Unicast ")) {
                    unicast = destination.Substring(8);
                }

                else if (destination.StartsWith("Multicast ")) {
                    multicast = _nicNames[destination.Substring(10)];
                }
            }

            return true;
        }


        //-------------------------------------------------------------
        //
        //	WarningsOption - property to expose warningsCheckBox status
        //
        //-------------------------------------------------------------

        public bool WarningsOption {
            get { return warningsCheckBox.Checked; }

            set { warningsCheckBox.Checked = value; }
        }

        //-------------------------------------------------------------
        //
        //	StatisticsOption - property to expose statisticsCheckBox status
        //
        //-------------------------------------------------------------

        public bool StatisticsOption {
            get { return statisticsCheckBox.Checked; }

            set { statisticsCheckBox.Checked = value; }
        }

        //-------------------------------------------------------------
        //
        //	EventRepeatCount - property to expose eventReportCountTextBox
        //					   value
        //
        //-------------------------------------------------------------

        public int EventRepeatCount {
            get {
                int count;

                if (!Int32.TryParse(eventRepeatCountTextBox.Text, out count)) count = 0;
                return count;
            }

            set { eventRepeatCountTextBox.Text = value.ToString(CultureInfo.InvariantCulture); }
        }

        //-------------------------------------------------------------
        //
        //	okButton_Click() - validate the data consistency
        //
        //-------------------------------------------------------------

        private void okButton_Click(object sender, EventArgs e) {
            var valid = true;
            var errorList = new StringBuilder();
            var universeDestinations = new SortedList<string, int>();


            // first buid a table of active universe/destination combos

            foreach (var universeDestination in from DataGridViewRow row in univDGVN.Rows
                                                where !row.IsNewRow
                                                where row.Cells[ActiveColumn].Value != null
                                                where (bool) row.Cells[ActiveColumn].Value
                                                where row.Cells[DestinationColumn].Value != null
                                                select (string) row.Cells[UniverseColumn].Value + ":" + (string) row.Cells[DestinationColumn].Value) {
                if (universeDestinations.ContainsKey(universeDestination)) universeDestinations[universeDestination] = 1;
                else universeDestinations.Add(universeDestination, 0);
            }

            // now scan for empty destinations, duplicate universe/destination combos, channels errors, etc.

            foreach (var row in from DataGridViewRow row in univDGVN.Rows
                                where !row.IsNewRow
                                where row.Cells[ActiveColumn].Value != null
                                where (bool) row.Cells[ActiveColumn].Value
                                select row) {
                // test for null destinations
                if (row.Cells[DestinationColumn].Value == null) {
                    if (!valid) errorList.Append("\r\n");
                    errorList.Append("Row ");
                    errorList.Append((row.Index + 1).ToString(CultureInfo.InvariantCulture));
                    errorList.Append(": No Destination Selected");
                    valid = false;
                }

                else {
                    // otherwise, test for duplicate universe/destination combos
                    var universeDestination = (string) row.Cells[UniverseColumn].Value + ":" + (string) row.Cells[DestinationColumn].Value;

                    if (universeDestinations[universeDestination] != 0) {
                        if (!valid) errorList.Append("\r\n");
                        errorList.Append("Row ");
                        errorList.Append((row.Index + 1).ToString(CultureInfo.InvariantCulture));
                        errorList.Append(": Duplicate Universe/Destination Combination");
                        valid = false;
                    }
                }

                // only test for range if more than 0 channels, otherwise wait for runtime
                if (_pluginChannelCount > 0) {
                    // now test for valid channel start
                    if (OutputPlugin.TryParseInt32((string) row.Cells[StartColumn].Value, 1) > _pluginChannelCount) {
                        if (!valid) errorList.Append("\r\n");
                        errorList.Append("Row ");
                        errorList.Append((row.Index + 1).ToString(CultureInfo.InvariantCulture));
                        errorList.Append(": Start Channel Out Of Range");
                        valid = false;
                    }

                    // now test for valid channel size
                    if (OutputPlugin.TryParseInt32((string) row.Cells[StartColumn].Value, 1) +
                        OutputPlugin.TryParseInt32((string) row.Cells[SizeColumn].Value, 1) - 1 > _pluginChannelCount) {
                        if (!valid) errorList.Append("\r\n");
                        errorList.Append("Row ");
                        errorList.Append((row.Index + 1).ToString(CultureInfo.InvariantCulture));
                        errorList.Append(": Start Channel + Size Out Of Range");
                        valid = false;
                    }
                }

                // now test for ttl value
                if (OutputPlugin.TryParseInt32((string) row.Cells[TtlColumn].Value, 1) != 0) {
                    continue;
                }
                if (!valid) errorList.Append("\r\n");
                errorList.Append("Row ");
                errorList.Append((row.Index + 1).ToString(CultureInfo.InvariantCulture));
                errorList.Append(": Warning - Zero TTL");
                valid = false;
            }

            if (!valid) {
                J1MsgBox.ShowMsg("Your configurations contains active entries that may cause run time errors.", errorList.ToString(),
                    "Configuration Validation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else {
                J1MsgBox.ShowMsg("Your configuration appears to be valid.",
                    "Configuration Validation", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        //-------------------------------------------------------------
        //
        //	eventRepeatCountTextBox_Validating()
        //
        //		validate our eventReportCountTextBox
        //
        //-------------------------------------------------------------

        private void eventRepeatCountTextBox_Validating(object sender, CancelEventArgs e) {
            int count;

            if (!Int32.TryParse(((TextBox) sender).Text, out count)) count = 0;
            if (count < 0 || 99 < count) e.Cancel = true;
            if (e.Cancel) MessageBeepClass.MessageBeep(MessageBeepClass.BeepType.SimpleBeep);
        }


        //-------------------------------------------------------------
        //
        //	univDGVN_EditingControlShowing()
        //
        //		for our numeric only columns we want to add a keypress
        //		event to the editing control
        //
        //		uses NumTextBox_KeyPress event handler
        //
        //-------------------------------------------------------------

        private void univDGVN_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e) {
            var columnIndex = univDGVN.CurrentCell.ColumnIndex;

            if (columnIndex == UniverseColumn || columnIndex == StartColumn || columnIndex == SizeColumn || columnIndex == TtlColumn) {
                // first remove the event handler (if previously added)
                e.Control.KeyPress -= NumTextBox_KeyPress;

                // now add our event handler
                e.Control.KeyPress += NumTextBox_KeyPress;
            }

            if (columnIndex != DestinationColumn) {
                return;
            }
            var control = e.Control as DataGridViewComboBoxEditingControl;

            if (control == null) {
                return;
            }
            if (destinationToolTip == null) destinationToolTip = new ToolTip();
            destinationToolTip.SetToolTip(e.Control, "RightClick to add a new Unicast IP Address");

            if (destinationContextMenuStrip == null) {
                destinationContextMenuStrip = new ContextMenuStrip();
                destinationContextMenuStrip.Opening += destinationContextMenuStrip_Opening;
            }

            control.ContextMenuStrip = destinationContextMenuStrip;
        }


        private void destinationContextMenuStrip_Opening(object sender, CancelEventArgs e) {
            e.Cancel = true;

            AddUnicastIP();
        }


        //-------------------------------------------------------------
        //
        //	NumTextBox_KeyPress() - event handler for a numeric textbox
        //
        //		this handler is used by the univDVGN editing control
        //		for the numeric columns and by a simple textbox control
        //		for numeric only input controls
        //
        //-------------------------------------------------------------

        private void NumTextBox_KeyPress(object sender, KeyPressEventArgs e) {
            e.Handled = true;

            if (Char.IsControl(e.KeyChar)) e.Handled = false;
            if (Char.IsDigit(e.KeyChar)) e.Handled = false;

            if (e.Handled) MessageBeepClass.MessageBeep(MessageBeepClass.BeepType.SimpleBeep);
        }


        //-------------------------------------------------------------
        //
        //	univDGCN_DefaultValuesNeeded()
        //
        //		initialize an empty row
        //
        //-------------------------------------------------------------

        private void univDGVN_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e) {
            e.Row.Cells[ActiveColumn].Value = false;
            e.Row.Cells[UniverseColumn].Value = "1";
            e.Row.Cells[StartColumn].Value = "1";
            e.Row.Cells[SizeColumn].Value = "1";
            e.Row.Cells[TtlColumn].Value = "1";
        }


        ////-------------------------------------------------------------
        ////
        ////	univDGVN_InsertRow() - contextMenuStrip click event
        ////
        ////-------------------------------------------------------------

        void univDGVN_InsertRow(object sender, EventArgs e) {
            if (univDGVNCellEventArgs == null) {
                return;
            }
            var row = univDGVN.Rows[univDGVNCellEventArgs.RowIndex];

            if (!row.IsNewRow) {
                univDGVN.Rows.Insert(row.Index, new object[] { false, "1", "1", "1", null, "1" });
            }
        }

        ////-------------------------------------------------------------
        ////
        ////	univDGVN_DeleteRow() - contextMenuStrip click event
        ////
        ////-------------------------------------------------------------

        void univDGVN_DeleteRow(object sender, EventArgs e) {
            if (univDGVNCellEventArgs == null) {
                return;
            }
            var row = univDGVN.Rows[univDGVNCellEventArgs.RowIndex];

            if (!row.IsNewRow) {
                univDGVN.Rows.RemoveAt(row.Index);
            }
        }

        ////-------------------------------------------------------------
        ////
        ////	univDGVN_MoveRowUp() - contextMenuStrip click event
        ////
        ////-------------------------------------------------------------

        void univDGVN_MoveRowUp(object sender, EventArgs e) {
            if (univDGVNCellEventArgs == null) {
                return;
            }
            var row = univDGVN.Rows[univDGVNCellEventArgs.RowIndex];
            var rowIndex = row.Index;

            if (row.IsNewRow || rowIndex <= 0) {
                return;
            }
            univDGVN.Rows.RemoveAt(rowIndex);
            univDGVN.Rows.Insert(rowIndex - 1, row);
        }

        ////-------------------------------------------------------------
        ////
        ////	univDGVN_MoveRowDown() - contextMenuStrip click event
        ////
        ////-------------------------------------------------------------

        void univDGVN_MoveRowDown(object sender, EventArgs e) {
            if (univDGVNCellEventArgs == null) {
                return;
            }
            var row = univDGVN.Rows[univDGVNCellEventArgs.RowIndex];
            var rowIndex = row.Index;

            if (row.IsNewRow) {
                return;
            }
            if (rowIndex >= univDGVN.Rows.Count - 1) {
                return;
            }
            if (univDGVN.Rows[rowIndex + 1].IsNewRow) {
                return;
            }
            univDGVN.Rows.RemoveAt(rowIndex);
            univDGVN.Rows.Insert(rowIndex + 1, row);
        }

        //-------------------------------------------------------------
        //
        //	univDGVN_CellMouseEnter() - track the mouse enter for
        //								use by the context menu strip
        //
        //-------------------------------------------------------------

        private void univDGVN_CellMouseEnter(object sender, DataGridViewCellEventArgs e) {
            univDGVNCellEventArgs = e;
        }


        //-------------------------------------------------------------
        //
        //	rowManipulationContextMenuStrip_Opening()
        //
        //		we need to gray out a few items based on row, or if
        //		it is the 'adding' row cancel the menu
        //
        //-------------------------------------------------------------

        private void rowManipulationContextMenuStrip_Opening(object sender, CancelEventArgs e) {
            var contextMenuStrip = sender as ContextMenuStrip;

            if (contextMenuStrip == null) {
                return;
            }
            if (univDGVNCellEventArgs == null) {
                return;
            }
            var row = univDGVN.Rows[univDGVNCellEventArgs.RowIndex];

            if (row.IsNewRow) e.Cancel = true;
            else {
                // enable/disable move row up
                contextMenuStrip.Items[3].Enabled = (row.Index != 0);

                // enable/disable move row down
                contextMenuStrip.Items[4].Enabled = (row.Index < univDGVN.Rows.Count - 1) && (!univDGVN.Rows[row.Index + 1].IsNewRow);
            }
        }


        //-------------------------------------------------------------
        //
        //	univDGN_CellEnter() - cell enter event
        //
        //		we just use this for destination column to issue
        //		a BeginEdit(). we feel this makes combobox more
        //		user friendly.
        //
        //-------------------------------------------------------------

        private void univDGVN_CellEnter(object sender, DataGridViewCellEventArgs e) {
            if (e.ColumnIndex == DestinationColumn) {
                univDGVN.BeginEdit(false);
            }
        }


        //-------------------------------------------------------------
        //
        //	univDGVN_CellEndEdit() - clear the errortext
        //
        //-------------------------------------------------------------

        private void univDGVN_CellEndEdit(object sender, DataGridViewCellEventArgs e) {
            univDGVN.Rows[e.RowIndex].ErrorText = String.Empty;
        }


        //-------------------------------------------------------------
        //
        //	univDGVN_CellValidating() - validate the cell
        //
        //-------------------------------------------------------------

        private void univDGVN_CellValidating(object sender, DataGridViewCellValidatingEventArgs e) {
            var cellValue = e.FormattedValue;
            var cellValueText = cellValue as string;
            var cellValueInt = 0;

            if (cellValueText != null) if (!Int32.TryParse(cellValueText, out cellValueInt)) cellValueInt = 0;

            switch (e.ColumnIndex) {
                case UniverseColumn:
                    if (cellValueText == null) e.Cancel = true;
                    else if (cellValueInt < 1 || 64000 < cellValueInt) e.Cancel = true;

                    if (e.Cancel) univDGVN.Rows[e.RowIndex].ErrorText = "Universe must be between 1 and 64000 inclusive";
                    break;

                case StartColumn:
                    if (cellValueText == null) e.Cancel = true;
                    else if (cellValueInt < 1 || 99999 < cellValueInt) e.Cancel = true;

                    if (e.Cancel) univDGVN.Rows[e.RowIndex].ErrorText = "Start must be between 1 and 99999 inclusive";
                    break;

                case SizeColumn:
                    if (cellValueText == null) e.Cancel = true;
                    else if (cellValueInt < 1 || 512 < cellValueInt) e.Cancel = true;

                    if (e.Cancel) univDGVN.Rows[e.RowIndex].ErrorText = "Size must be between 1 and 512 inclusive";
                    break;

                case TtlColumn:
                    if (cellValueText == null) e.Cancel = true;
                    else if (cellValueInt < 0 || 99 < cellValueInt) e.Cancel = true;

                    if (e.Cancel) univDGVN.Rows[e.RowIndex].ErrorText = "TTL must be between 0 and 99 inclusive";
                    break;
            }

            if (e.Cancel) {
                MessageBox.Show(univDGVN.Rows[e.RowIndex].ErrorText, @"Cell Validation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }


        //-------------------------------------------------------------
        //
        //	SetDestinations() - clear and set values for destinations
        //						combobox
        //
        //-------------------------------------------------------------

        private void SetDestinations() {
            destinationColumn.Items.Clear();

            foreach (var destination in _multicasts.Keys) {
                destinationColumn.Items.Add("Multicast " + destination);
            }

            foreach (var ipAddr in _unicasts.Keys) {
                destinationColumn.Items.Add("Unicast " + ipAddr);
            }
        }


        //-------------------------------------------------------------
        //
        //	univDGVN_CellMouseClick() - cell mouse click event
        //
        //-------------------------------------------------------------

        private void univDGVN_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e) {
            // if it's the headers - sort 'em

            if (e.RowIndex == -1) {
                if (0 < e.ColumnIndex && e.ColumnIndex < 5) {
                    var lsd = ListSortDirection.Ascending;

                    if (e.Button == MouseButtons.Right) lsd = ListSortDirection.Descending;

                    univDGVN.Sort(univDGVN.Columns[e.ColumnIndex], lsd);
                }
            }

                // if it's the rows - handle specials

            else {
                // if it's the right button

                if (e.Button == MouseButtons.Right) {
                    // if it's the destination column - they want to add a unicast ip

                    if (e.ColumnIndex == DestinationColumn) {
                        AddUnicastIP();
                    }
                }
            }
        }


        //-------------------------------------------------------------
        //
        //	AddUnicastIP()
        //
        //-------------------------------------------------------------

        private void AddUnicastIP() {
            var unicastForm = new UnicastForm();

            if (univDGVN.CurrentCell != null) {
                if (univDGVN.CurrentCell.IsInEditMode) univDGVN.EndEdit();
            }

            if (unicastForm.ShowDialog() != DialogResult.OK) {
                return;
            }
            IPAddress ipAddress;

            var valid = IPAddress.TryParse(unicastForm.IPAddrText, out ipAddress);

            if (valid) {
                var ipBytes = ipAddress.GetAddressBytes();

                if (ipBytes[0] == 0 && ipBytes[1] == 0 && ipBytes[2] == 0 && ipBytes[3] == 0) valid = false;
                if (ipBytes[0] == 255 && ipBytes[1] == 255 && ipBytes[2] == 255 && ipBytes[3] == 255) valid = false;
            }

            if (!valid) {
                MessageBox.Show(@"Error - Invalid IP Address", @"IP Address Validation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            else {
                var ipAddressText = ipAddress.ToString();

                if (_unicasts.ContainsKey(ipAddressText)) {
                    MessageBox.Show(@"Error - Duplicate IP Address", @"IP Address Validation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                else {
                    _unicasts.Add(ipAddressText, 0);
                    SetDestinations();
                }
            }
        }


        private void btnSysInfo_Click(object sender, EventArgs e) {
            Help.ShowSysClick(sender, e);
        }
    }
}