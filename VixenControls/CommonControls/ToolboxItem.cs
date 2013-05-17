using System;
using System.ComponentModel;
using System.Drawing;

namespace CommonControls {
    [Serializable]
    [TypeConverter(typeof (ToolboxItemTypeConverter))]
    public class ToolboxItem {
        internal Rectangle Bounds;


        public ToolboxItem() {
            Image = null;
            Name = "ToolboxItem";
        }


        public ToolboxItem(string name) : this() {
            Name = name;
        }


        public ToolboxItem(string name, string description) : this(name) {
            Description = description;
        }


        public ToolboxItem(string name, string description, Image image) : this(name, description) {
            Image = image;
        }


        public string Description { get; set; }

        public Image Image { get; set; }

        public string Name { get; set; }
    }
}
