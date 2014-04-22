using System.Windows.Forms;

namespace VixenPlus.Dialogs {
    public sealed class DataGridViewDisableButtonColumn : DataGridViewButtonColumn {
        public DataGridViewDisableButtonColumn() {
            CellTemplate = new DataGridViewDisableButtonCell();
        }
    }
}