namespace TriggerResponse
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Vixen;

    internal class TriggerResponseTestDialog : Form
    {
        private Button buttonTest;
        private ComboBox comboBoxTriggerResponses;
        private IContainer components = null;
        private Label label1;
        private MappedTriggerResponse[] m_responsesRegistered;
        private ITrigger m_triggerInterface;

        public TriggerResponseTestDialog(MappedTriggerResponse[] responses, ITrigger triggerInterface)
        {
            this.InitializeComponent();
            this.m_triggerInterface = triggerInterface;
            List<string> list = new List<string>();
            foreach (MappedTriggerResponse response in responses)
            {
                if (!list.Contains(response.Key))
                {
                    this.comboBoxTriggerResponses.Items.Add(response);
                    list.Add(response.Key);
                }
                response.EcHandle = this.m_triggerInterface.RegisterResponse(response.TriggerType, response.TriggerIndex, response.SequenceFile);
            }
            this.m_responsesRegistered = responses;
        }

        private void buttonTest_Click(object sender, EventArgs e)
        {
            MappedTriggerResponse selectedItem = (MappedTriggerResponse) this.comboBoxTriggerResponses.SelectedItem;
            this.m_triggerInterface.ActivateTrigger(selectedItem.TriggerType, selectedItem.TriggerIndex);
        }

        private void comboBoxResponses_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.buttonTest.Enabled = this.comboBoxTriggerResponses.SelectedIndex != -1;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.comboBoxTriggerResponses = new ComboBox();
            this.buttonTest = new Button();
            this.label1 = new Label();
            base.SuspendLayout();
            this.comboBoxTriggerResponses.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxTriggerResponses.FormattingEnabled = true;
            this.comboBoxTriggerResponses.Location = new Point(0x16, 0x2a);
            this.comboBoxTriggerResponses.Name = "comboBoxTriggerResponses";
            this.comboBoxTriggerResponses.Size = new Size(0xf4, 0x15);
            this.comboBoxTriggerResponses.TabIndex = 0;
            this.comboBoxTriggerResponses.SelectedIndexChanged += new EventHandler(this.comboBoxResponses_SelectedIndexChanged);
            this.buttonTest.Enabled = false;
            this.buttonTest.Location = new Point(0x6d, 0x45);
            this.buttonTest.Name = "buttonTest";
            this.buttonTest.Size = new Size(0x4b, 0x17);
            this.buttonTest.TabIndex = 1;
            this.buttonTest.Text = "Test";
            this.buttonTest.UseVisualStyleBackColor = true;
            this.buttonTest.Click += new EventHandler(this.buttonTest_Click);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x13, 0x16);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x76, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Triggers with responses";
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x124, 0x72);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.buttonTest);
            base.Controls.Add(this.comboBoxTriggerResponses);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.KeyPreview = true;
            base.Name = "TriggerResponseTestDialog";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Trigger Response Test";
            base.FormClosing += new FormClosingEventHandler(this.ResponseTestDialog_FormClosing);
            base.KeyDown += new KeyEventHandler(this.ResponseTestDialog_KeyDown);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void ResponseTestDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (MappedTriggerResponse response in this.m_responsesRegistered)
            {
                this.m_triggerInterface.UnregisterResponse(response.TriggerType, response.TriggerIndex, response.EcHandle);
                response.EcHandle = 0;
            }
        }

        private void ResponseTestDialog_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                base.DialogResult = DialogResult.Cancel;
            }
        }
    }
}

