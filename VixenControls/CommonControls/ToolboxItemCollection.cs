using System.Collections;

namespace CommonControls {
    public class ToolboxItemCollection : CollectionBase {
        public void Add(ToolboxItem item) {
            List.Add(item);
        }


        public void AddRange(ToolboxItem[] items) {
            foreach (var item in items) {
                List.Add(item);
            }
        }


        public ToolboxItem this[string name] {
            get {
                var comparer = new CaseInsensitiveComparer();
                foreach (ToolboxItem item in List) {
                    if (comparer.Compare(item.Name, name) == 0) {
                        return item;
                    }
                }
                return null;
            }
        }


        public ToolboxItem this[int index] {
            get { return (ToolboxItem) List[index]; }
            set { List[index] = value; }
        }
    }
}
