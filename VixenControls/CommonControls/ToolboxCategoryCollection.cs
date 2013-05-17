using System.Collections;

namespace CommonControls {
    public class ToolboxCategoryCollection : CollectionBase {
        private readonly ToolboxCategory.OnCategoryChange _categoryChange;

        internal event OnItemsChange ItemsChange;


        public ToolboxCategoryCollection() {
            _categoryChange = CategoryChange;
        }


        public void Add(ToolboxCategory toolboxCategory) {
            List.Add(toolboxCategory);
        }


        public void AddRange(ToolboxCategory[] items) {
            foreach (var category in items) {
                List.Add(category);
            }
        }


        private void CategoryChange() {
            FireItemsChange();
        }


        private void FireItemsChange() {
            if (ItemsChange != null) {
                ItemsChange();
            }
        }


        protected override void OnClearComplete() {
            base.OnClearComplete();
            FireItemsChange();
        }


        protected override void OnInsertComplete(int index, object value) {
            ((ToolboxCategory) value).CategoryChange += _categoryChange;
            base.OnInsertComplete(index, value);
            FireItemsChange();
        }


        protected override void OnRemoveComplete(int index, object value) {
            ((ToolboxCategory) value).CategoryChange -= _categoryChange;
            base.OnRemoveComplete(index, value);
            FireItemsChange();
        }


        protected override void OnSetComplete(int index, object oldValue, object newValue) {
            ((ToolboxCategory) oldValue).CategoryChange -= _categoryChange;
            ((ToolboxCategory) newValue).CategoryChange += _categoryChange;
            base.OnSetComplete(index, oldValue, newValue);
            FireItemsChange();
        }


        public ToolboxCategory this[string name] {
            get {
                var comparer = new CaseInsensitiveComparer();
                foreach (ToolboxCategory category in List) {
                    if (comparer.Compare(category.Name, name) == 0) {
                        return category;
                    }
                }
                return null;
            }
        }


        public ToolboxCategory this[int index] {
            get { return (ToolboxCategory) List[index]; }
            set { List[index] = value; }
        }


        internal delegate void OnItemsChange();
    }
}
