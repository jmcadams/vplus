using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

using CommonControls.Annotations;

namespace CommonControls {
    // Cyotek Color Picker controls library
    // Copyright © 2013 Cyotek. All Rights Reserved.
    // http://cyotek.com/blog/tag/colorpicker

    // If you use this code in your applications, donations or attribution are welcome

    /// <summary>
    ///     Represents a collection of colors
    /// </summary>
    /// <remarks>
    ///     <para>ColorCollection allows duplicate elements.</para>
    ///     <para>
    ///         Elements in this collection can be accessed using an integer index. Indexes in this collection are
    ///         zero-based.
    ///     </para>
    /// </remarks>
    // ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
    public class ColorCollection : Collection<Color> {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="ColorCollection" /> class.
        /// </summary>
        public ColorCollection() {}


        /// <summary>
        ///     Initializes a new instance of the <see cref="ColorCollection" /> class that contains elements copied from the
        ///     specified collection.
        /// </summary>
        /// <param name="collection">The collection whose elements are copied to the new collection.</param>
        public ColorCollection(IEnumerable<Color> collection) : this() {
            AddRange(collection);
        }

        #endregion

        #region Events

        /// <summary>
        ///     Occurs when elements in the collection are added, removed or modified.
        /// </summary>
        [UsedImplicitly]
        public event EventHandler<ColorCollectionEventArgs> CollectionChanged;

        #endregion

        /// <summary>
        ///     Removes all elements from the <see cref="T:System.Collections.ObjectModel.Collection`1" />.
        /// </summary>
        protected override void ClearItems() {
            base.ClearItems();

            OnCollectionChanged(new ColorCollectionEventArgs(-1, Color.Empty));
        }


        /// <summary>
        ///     Inserts an element into the <see cref="T:System.Collections.ObjectModel.Collection`1" /> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item" /> should be inserted.</param>
        /// <param name="item">The object to insert.</param>
        protected override void InsertItem(int index, Color item) {
            base.InsertItem(index, item);

            OnCollectionChanged(new ColorCollectionEventArgs(index, item));
        }


        /// <summary>
        ///     Removes the element at the specified index of the <see cref="T:System.Collections.ObjectModel.Collection`1" />.
        /// </summary>
        /// <param name="index">The zero-based index of the element to remove.</param>
        protected override void RemoveItem(int index) {
            Color color = this[index];

            base.RemoveItem(index);

            OnCollectionChanged(new ColorCollectionEventArgs(index, color));
        }


        /// <summary>
        ///     Replaces the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to replace.</param>
        /// <param name="item">The new value for the element at the specified index.</param>
        protected override void SetItem(int index, Color item) {
            base.SetItem(index, item);

            OnCollectionChanged(new ColorCollectionEventArgs(index, item));
        }

        #region Members

        /// <summary>Adds the elements of the specified collection to the end of the <see cref="ColorCollection" />.</summary>
        /// <param name="colors">The collection whose elements should be added to the end of the <see cref="ColorCollection" />.</param>
        // ReSharper disable once MemberCanBePrivate.Global
        public void AddRange(IEnumerable<Color> colors) {
            foreach (Color color in colors) {
                Add(color);
            }
        }


        /// <summary>
        ///     Raises the <see cref="CollectionChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected virtual void OnCollectionChanged(ColorCollectionEventArgs e) {
            EventHandler<ColorCollectionEventArgs> handler = CollectionChanged;

            if (handler != null) {
                handler(this, e);
            }
        }

        #endregion
    }
}