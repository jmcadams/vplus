namespace VixenControls
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    public class ToolboxCategoryCollection : CollectionBase
    {
        private ToolboxCategory.OnCategoryChange m_categoryChange;

        internal event OnItemsChange ItemsChange;

        public ToolboxCategoryCollection()
        {
            this.m_categoryChange = new ToolboxCategory.OnCategoryChange(this.CategoryChange);
        }

        public void Add(ToolboxCategory toolboxCategory)
        {
            base.List.Add(toolboxCategory);
        }

        public void AddRange(ToolboxCategory[] items)
        {
            foreach (ToolboxCategory category in items)
            {
                base.List.Add(category);
            }
        }

        private void CategoryChange()
        {
            this.FireItemsChange();
        }

        private void FireItemsChange()
        {
            if (this.ItemsChange != null)
            {
                this.ItemsChange();
            }
        }

        protected override void OnClearComplete()
        {
            base.OnClearComplete();
            this.FireItemsChange();
        }

        protected override void OnInsertComplete(int index, object value)
        {
            ((ToolboxCategory) value).CategoryChange += this.m_categoryChange;
            base.OnInsertComplete(index, value);
            this.FireItemsChange();
        }

        protected override void OnRemoveComplete(int index, object value)
        {
            ((ToolboxCategory) value).CategoryChange -= this.m_categoryChange;
            base.OnRemoveComplete(index, value);
            this.FireItemsChange();
        }

        protected override void OnSetComplete(int index, object oldValue, object newValue)
        {
            ((ToolboxCategory) oldValue).CategoryChange -= this.m_categoryChange;
            ((ToolboxCategory) newValue).CategoryChange += this.m_categoryChange;
            base.OnSetComplete(index, oldValue, newValue);
            this.FireItemsChange();
        }

        public ToolboxCategory this[string name]
        {
            get
            {
                CaseInsensitiveComparer comparer = new CaseInsensitiveComparer();
                foreach (ToolboxCategory category in base.List)
                {
                    if (comparer.Compare(category.Name, name) == 0)
                    {
                        return category;
                    }
                }
                return null;
            }
        }

        public ToolboxCategory this[int index]
        {
            get
            {
                return (ToolboxCategory) base.List[index];
            }
            set
            {
                base.List[index] = value;
            }
        }

        internal delegate void OnItemsChange();
    }
}

