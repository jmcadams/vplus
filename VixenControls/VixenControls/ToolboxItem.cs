namespace VixenControls
{
    using System;
    using System.ComponentModel;
    using System.Drawing;

    [Serializable, TypeConverter(typeof(ToolboxItemTypeConverter))]
    public class ToolboxItem
    {
        internal System.Drawing.Rectangle Bounds;
        private string m_description;
        private System.Drawing.Image m_image;
        private string m_name;

        public ToolboxItem()
        {
            this.m_image = null;
            this.m_name = "ToolboxItem";
        }

        public ToolboxItem(string name)
        {
            this.m_image = null;
            this.m_name = name;
        }

        public ToolboxItem(string name, string description)
        {
            this.m_image = null;
            this.m_name = name;
            this.m_description = description;
        }

        public ToolboxItem(string name, string description, System.Drawing.Image image)
        {
            this.m_image = null;
            this.m_name = name;
            this.m_description = description;
            this.m_image = image;
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

        public System.Drawing.Image Image
        {
            get
            {
                return this.m_image;
            }
            set
            {
                this.m_image = value;
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
            }
        }
    }
}

