using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using CommonUtils;

using VixenPlus;


namespace VixenEditor {
    public partial class GroupPickerDialog : Form {

        private readonly List<TreeNode> _nodes; 

        public List<string> SelectedItems {
            get {
                return lbGroups.SelectedItems.Cast<string>().ToList();
            }
        }

        public GroupPickerDialog(List<TreeNode> nodes) {
            InitializeComponent();
            _nodes = nodes;
            InitListbox();
        }


        private void InitListbox() {
            foreach (var n in _nodes) {
                lbGroups.Items.Add(n.Name);
            }
        }

        private void lbGroups_DrawItem(object sender, DrawItemEventArgs e) {
            var lb = sender as ListBox;
            if (lb == null) {
                return;
            }
            var item = _nodes[e.Index];
            Utils.DrawItem(lb, e, item.Name, ((GroupTagData)item.Tag).NodeColor);
        }
    }
}
