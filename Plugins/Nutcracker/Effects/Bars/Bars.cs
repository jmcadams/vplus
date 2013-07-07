﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using CommonUtils;

using VixenPlus;

namespace Bars {
    public partial class Bars : UserControl, INutcrackerEffect {

        private readonly bool _initializing = true;
        private const string BarCount = "ID_SLIDER_Bars{0}_BarCount";
        private const string BarDirection = "ID_CHOICE_Bars{0}_Direction";
        private const string BarHighlight = "ID_CHECKBOX_Bars{0}_Highlight";
        private const string Bar3D = "ID_CHECKBOX_Bars{0}_3D";

        public Bars() {
            InitializeComponent();
            cbDirection.SelectedIndex = 0;
            _initializing = false;
        }

        public event EventHandler OnControlChanged;

        public string EffectName {
            get { return "Bars"; }
        }

        public string Notes {
            get { return String.Empty; }
        }

        public bool UsesPalette {
            get { return true; }
        }

        public bool UsesSpeed {
            get { return true; }
        }

        public List<string> Settings {
            get { return GetCurrentSettings(); }
            set { Setup(value); }
        }

        private List<string> GetCurrentSettings() {
            return new List<string>();
        }

        private void Setup(IList<string> settings) {
            var effectNum = settings[0];
            var barCount = string.Format(BarCount, effectNum);
            var barDirection = string.Format(BarDirection, effectNum);
            var barHighlight = string.Format(BarHighlight, effectNum);
            var bar3D = string.Format(Bar3D, effectNum);

            foreach (var keyValue in settings.Select(s => s.Split(new[] {'='}))) {
                if (keyValue[0].Equals(barCount)) {
                    tbRepeat.Value = Utils.GetParsedValue(keyValue[1]);
                }
                else if (keyValue[0].Equals(barDirection)) {
                    var index = cbDirection.Items.IndexOf(keyValue[1]);
                    if (index >= 0) {
                        cbDirection.SelectedIndex = index;
                    }
                }
                else if (keyValue[0].Equals(barHighlight)) {
                    cbHighlight.Checked = keyValue[1].Equals("1");
                }
                else if (keyValue[0].Equals(bar3D)) {
                    cb3D.Checked = keyValue[1].Equals("1");
                }
            }
        }


        public Color[,] RenderEffect(Color[,] buffer, Color[] palette, int eventToRender) {
            var barCount = tbRepeat.Value * palette.Length;
            var bufferHeight = buffer.GetLength(Utils.IndexRowsOrHeight);
            var bufferWidth = buffer.GetLength(Utils.IndexColsOrWidth);
            var barHeight = bufferHeight / barCount + 1;
            var halfHeight = bufferHeight / 2;
            var blockHeight = palette.Length * barHeight;
            var rowOffset = eventToRender / 4 % blockHeight;
            for (var row = 0; row < bufferHeight; row++) {
                var isMovingDown = false; // default to UP even if undefined
                switch (cbDirection.SelectedIndex) {
                    case 1: // DOWN
                        isMovingDown = true;
                        break;
                    case 2: // EXPAND
                        isMovingDown = (row <= halfHeight);
                        break;
                    case 3: // COMPRESS
                        isMovingDown = (row > halfHeight);
                        break;
                }
                var n = isMovingDown ? row + rowOffset : row - rowOffset + blockHeight;

                var hsv = HSVUtils.ColorToHSV(palette[(n % blockHeight) / barHeight]);
                if (cbHighlight.Checked && (isMovingDown ? n % barHeight == 0 : n % barHeight == barHeight - 1)) {
                    hsv.Saturation = 0f;
                }
                if (cb3D.Checked) {
                    hsv.Value *= (float) (isMovingDown ? barHeight - n % barHeight - 1 : n % barHeight) / barHeight;
                }
                for (var column = 0; column < bufferWidth; column++) {
                    buffer[row, column] = HSVUtils.HSVtoColor(hsv);
                }
            }
            return buffer;
        }


        private void Bars_ControlChanged(object sender, EventArgs e) {
            if (_initializing) return;
            OnControlChanged(this, e);
        }
    }
}