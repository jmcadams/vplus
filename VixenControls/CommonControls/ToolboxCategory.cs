using System.ComponentModel;
using System.Drawing;

namespace CommonControls {
    [TypeConverter(typeof (ToolboxCategoryTypeConverter))]
    public class ToolboxCategory {
        internal Rectangle Bounds;
        internal Rectangle ButtonBounds;
        private string _name;

        public event OnCategoryChange CategoryChange;


        public ToolboxCategory() {
            Expanded = false;
            Items = new ToolboxItemCollection();
            _name = "ToolboxCategory";
        }


        public ToolboxCategory(string name, string description) {
            Expanded = false;
            Items = new ToolboxItemCollection();
            _name = name;
            Description = description;
        }


        public void Collapse() {
            if (!Expanded) {
                return;
            }
            Expanded = false;
            FireCategoryChange();
        }


        public void Expand() {
            if (Expanded || (Items.Count <= 0)) {
                return;
            }
            Expanded = true;
            FireCategoryChange();
        }


        private void FireCategoryChange() {
            if (CategoryChange != null) {
                CategoryChange();
            }
        }


        public string Description { get; set; }

        [Browsable(false)]
        public bool Expanded { get; private set; }


        public ToolboxItem this[int index] {
            get { return Items[index]; }
            set { Items[index] = value; }
        }


        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ToolboxItemCollection Items { get; set; }

        public string Name {
            get { return _name; }
            set {
                _name = value;
                FireCategoryChange();
            }
        }

        public delegate void OnCategoryChange();
    }
}
