namespace VixenControls
{
    using System;
    using System.Collections;
    using System.Reflection;

    public class ToolboxItemCollection : CollectionBase
    {
        public void Add(ToolboxItem item)
        {
            base.List.Add(item);
        }

        public void AddRange(ToolboxItem[] items)
        {
            foreach (ToolboxItem item in items)
            {
                base.List.Add(item);
            }
        }

        public ToolboxItem this[string name]
        {
            get
            {
                CaseInsensitiveComparer comparer = new CaseInsensitiveComparer();
                foreach (ToolboxItem item in base.List)
                {
                    if (comparer.Compare(item.Name, name) == 0)
                    {
                        return item;
                    }
                }
                return null;
            }
        }

        public ToolboxItem this[int index]
        {
            get
            {
                return (ToolboxItem) base.List[index];
            }
            set
            {
                base.List[index] = value;
            }
        }
    }
}

