namespace VixenControls
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    public class VectorImageCollection : CollectionBase
    {
        internal event OnItemsChange ItemsChange;

        public void Add(VectorImage.Image image)
        {
            base.List.Add(new VectorListItem(image));
        }

        public void AddRange(VectorImage.Image[] items)
        {
            foreach (VectorImage.Image image in items)
            {
                base.List.Add(new VectorListItem(image));
            }
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
            base.OnInsertComplete(index, value);
            this.FireItemsChange();
        }

        protected override void OnRemoveComplete(int index, object value)
        {
            base.OnRemoveComplete(index, value);
            this.FireItemsChange();
        }

        protected override void OnSetComplete(int index, object oldValue, object newValue)
        {
            base.OnSetComplete(index, oldValue, newValue);
            this.FireItemsChange();
        }

        public VectorListItem this[int index]
        {
            get
            {
                return (VectorListItem) base.List[index];
            }
        }

        internal delegate void OnItemsChange();
    }
}

