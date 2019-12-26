using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace GTASaveData
{
    /// <summary>
    /// An extension of <see cref="ObservableCollection{T}"/> that can monitor changes
    /// both in each item's state and in the collection itself.
    /// </summary>
    /// <remarks>
    /// Adapted from http://code.i-harness.com/en/q/15c80f.
    /// </remarks>
    public class FullyObservableCollection<T> : ObservableCollection<T>
        where T : INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs when a property within an item changes state.
        /// </summary>
        public event EventHandler<ItemPropertyChangedEventArgs> ItemPropertyChanged;

        public FullyObservableCollection()
            : base()
        { }

        public FullyObservableCollection(List<T> list)
            : base(list)
        {
            ObserveAll();
        }

        public FullyObservableCollection(IEnumerable<T> collection)
            : base(collection)
        {
            ObserveAll();
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove ||
                e.Action == NotifyCollectionChangedAction.Replace)
            {
                foreach (T item in e.OldItems)
                {
                    item.PropertyChanged -= ChildPropertyChanged;
                }
            }

            if (e.Action == NotifyCollectionChangedAction.Add ||
                e.Action == NotifyCollectionChangedAction.Replace)
            {
                foreach (T item in e.NewItems)
                {
                    item.PropertyChanged += ChildPropertyChanged;
                }
            }

            base.OnCollectionChanged(e);
        }

        protected void OnItemPropertyChanged(ItemPropertyChangedEventArgs e)
        {
            ItemPropertyChanged?.Invoke(this, e);
        }

        protected void OnItemPropertyChanged(int index, PropertyChangedEventArgs e)
        {
            OnItemPropertyChanged(new ItemPropertyChangedEventArgs(index, e));
        }

        protected override void ClearItems()
        {
            foreach (T item in Items)
            {
                item.PropertyChanged -= ChildPropertyChanged;
            }

            base.ClearItems();
        }

        private void ObserveAll()
        {
            foreach (T item in Items)
            {
                item.PropertyChanged += ChildPropertyChanged;
            }
        }

        private void ChildPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!(sender is T typedSender))
            {
                return;
            }

            int i = Items.IndexOf(typedSender);
            if (i == -1)
            {
                return;
            }

            OnItemPropertyChanged(i, e);
        }
    }

    public class ItemPropertyChangedEventArgs : PropertyChangedEventArgs
    {
        public ItemPropertyChangedEventArgs(int itemIndex, string propertyName)
            : base(propertyName)
        {
            ItemIndex = itemIndex;
        }

        public ItemPropertyChangedEventArgs(int itemIndex, PropertyChangedEventArgs args)
            : this(itemIndex, args.PropertyName)
        { }

        public int ItemIndex { get; }
    }
}
