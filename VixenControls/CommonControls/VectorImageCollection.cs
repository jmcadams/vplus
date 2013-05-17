using System.Collections;

namespace CommonControls
{
    public class VectorImageCollection : CollectionBase
    {
        internal event OnItemsChange ItemsChange;

        public void Add(VectorImage.Image image)
        {
            List.Add(new VectorListItem(image));
        }

        public void AddRange(VectorImage.Image[] items)
        {
            foreach (var image in items)
            {
                List.Add(new VectorListItem(image));
            }
        }

        private void FireItemsChange()
        {
            if (ItemsChange != null)
            {
                ItemsChange();
            }
        }

        protected override void OnClearComplete()
        {
            base.OnClearComplete();
            FireItemsChange();
        }

        protected override void OnInsertComplete(int index, object value)
        {
            base.OnInsertComplete(index, value);
            FireItemsChange();
        }

        protected override void OnRemoveComplete(int index, object value)
        {
            base.OnRemoveComplete(index, value);
            FireItemsChange();
        }

        protected override void OnSetComplete(int index, object oldValue, object newValue)
        {
            base.OnSetComplete(index, oldValue, newValue);
            FireItemsChange();
        }

        public VectorListItem this[int index]
        {
            get
            {
                return (VectorListItem) List[index];
            }
        }

        internal delegate void OnItemsChange();
    }
}

