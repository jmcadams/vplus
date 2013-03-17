namespace VixenControls
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    [TypeConverter(typeof(ToolboxCategoryTypeConverter))]
    public class ToolboxCategory
    {
        internal System.Drawing.Rectangle Bounds;
        internal System.Drawing.Rectangle ButtonBounds;
        private string m_description;
        private bool m_expanded;
        private ToolboxItemCollection m_items;
        private string m_name;

        public event OnCategoryChange CategoryChange;

        public ToolboxCategory()
        {
            this.m_expanded = false;
            this.m_items = new ToolboxItemCollection();
            this.m_name = "ToolboxCategory";
        }

        public ToolboxCategory(string name, string description)
        {
            this.m_expanded = false;
            this.m_items = new ToolboxItemCollection();
            this.m_name = name;
            this.m_description = description;
        }

        public void Collapse()
        {
            if (this.m_expanded)
            {
                this.m_expanded = false;
                this.FireCategoryChange();
            }
        }

        public void Expand()
        {
            if (!(this.m_expanded || (this.m_items.Count <= 0)))
            {
                this.m_expanded = true;
                this.FireCategoryChange();
            }
        }

        private void FireCategoryChange()
        {
            if (this.CategoryChange != null)
            {
                this.CategoryChange();
            }
        }

        public string Description
        {
            get
            {
                return this.m_description;
            }
            set
            {
                this.m_description = value;
            }
        }

        [Browsable(false)]
        public bool Expanded
        {
            get
            {
                return this.m_expanded;
            }
        }

        public ToolboxItem this[int index]
        {
            get
            {
                return this.m_items[index];
            }
            set
            {
                this.m_items[index] = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ToolboxItemCollection Items
        {
            get
            {
                return this.m_items;
            }
            set
            {
                this.m_items = value;
            }
        }

        public string Name
        {
            get
            {
                return this.m_name;
            }
            set
            {
                this.m_name = value;
                this.FireCategoryChange();
            }
        }

        public delegate void OnCategoryChange();
    }
}

