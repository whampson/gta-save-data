using GTASaveData.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using WpfEssentials;

namespace GTASaveData
{
    /// <summary>
    /// A collection of objects with sequential and contiguous storage.
    /// </summary>
    /// <typeparam name="T">The item type.</typeparam>
    /// <remarks>
    /// This collection is WPF-ready. Changes made to the collection are notified via the <see cref="CollectionChanged"/>
    /// and <see cref="INotifyPropertyChanged.PropertyChanged"/> events. Changes made to items in the collection are notified
    /// via the <see cref="ItemStateChanged"/> event.
    /// </remarks>
    public class ObservableArray<T> : ObservableObject,
        IEnumerable, IEnumerable<T>,
        IList, IList<T>,
        INotifyCollectionChanged, INotifyItemStateChanged
    {
        /// <summary>
        /// An empty <see cref="ObservableArray{T}"/>.
        /// </summary>
        public static readonly ObservableArray<T> Empty = new ObservableArray<T>();

        /// <summary>
        /// Occurs when the <see cref="ObservableArray{T}"/> changes, such as an item being added, removed, or replaced.
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>
        /// Occurs when the state of an item within the <see cref="ObservableArray{T}"/> changes.
        /// </summary>
        public event NotifyItemStateChangedEventHandler ItemStateChanged;

        private readonly BusyMonitor m_monitor;
        private readonly List<T> m_items;
        private readonly bool m_itemsAreObservable;

        public int Count => m_items.Count;
        public bool IsFixedSize => false;
        public bool IsReadOnly => false;
        public bool IsSynchronized => false;
        public object SyncRoot => this;

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">The index of the item to get or set.</param>
        /// <returns>The item at the specified index.</returns>
        object IList.this[int index]
        {
            get { return this[index]; }
            set { this[index] = (T) value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">The index of the item to get or set.</param>
        /// <returns>The item at the specified index.</returns>
        public T this[int index]
        {
            get { return ItemAt(index); }
            set { Replace(value, index); OnPropertyChanged(); }
        }
        
        /// <summary>
        /// Creates a new empty <see cref="ObservableArray{T}"/>.
        /// </summary>
        public ObservableArray()
        {
            m_monitor = new BusyMonitor();
            m_items = new List<T>();
            m_itemsAreObservable = typeof(T).Implements(typeof(INotifyPropertyChanged));
        }

        /// <summary>
        /// Creates a new <see cref="ObservableArray{T}"/> with items from the specified enumerable.
        /// </summary>
        /// <param name="items">The items to initalize the array with.</param>
        public ObservableArray(IEnumerable<T> items)
            : this()
        {
            foreach (T item in items)
            {
                RegisterStateChangedHandler(item);
                m_items.Add(item);
            }
        }

        /// <summary>
        /// Creates a new <see cref="ObservableArray{T}"/> with items from the specified list.
        /// </summary>
        /// <param name="items">The items to initalize the array with.</param>
        public ObservableArray(List<T> items)
            : this((IEnumerable<T>) items)
        { }

        /// <summary>
        /// Adds an item to the <see cref="ObservableArray{T}"/>.
        /// </summary>
        /// <param name="value">The item to add.</param>
        /// <returns>The index of the new item.</returns>
        public int Add(object value)
        {
            Add((T) value);

            return m_items.Count - 1;
        }

        /// <summary>
        /// Adds an item to the <see cref="ObservableArray{T}"/>.
        /// </summary>
        /// <param name="item">The item to add.</param>
        public void Add(T item)
        {
            CheckReentrancy();

            m_items.Add(item);

            RegisterStateChangedHandler(item);

            OnPropertyChanged(nameof(Count));
            OnCollectionChanged(NotifyCollectionChangedAction.Add, item);
        }

        /// <summary>
        /// Removes all items from <see cref="ObservableArray{T}"/>.
        /// </summary>
        public void Clear()
        {
            CheckReentrancy();

            m_items.ForEach(item => UnregisterStateChangedHandler(item));
            m_items.Clear();

            OnPropertyChanged(nameof(Count));
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <summary>
        /// Determines whether the <see cref="ObservableArray{T}"/> contains a specific value.
        /// </summary>
        /// <param name="value">The item to locate.</param>
        /// <returns>True if the item exists in the array.</returns>
        public bool Contains(object value)
        {
            return Contains((T) value);
        }

        /// <summary>
        /// Determines whether the <see cref="ObservableArray{T}"/> contains a specific value.
        /// </summary>
        /// <param name="item">The item to locate.</param>
        /// <returns>True if the item exists in the array.</returns>
        public bool Contains(T item)
        {
            return m_items.Contains(item);
        }

        /// <summary>
        /// Copyies the elements of this <see cref="ObservableArray{T}"/> to a compatible
        /// one-dimensional array.
        /// </summary>
        /// <param name="array">The one-dimensional array to copy to.</param>
        public void CopyTo(T[] array)
        {
            m_items.CopyTo(array);
        }

        /// <summary>
        /// Copyies the elements of this <see cref="ObservableArray{T}"/> to a compatible
        /// one-dimensional array, starting at the specifed index of the target array.
        /// </summary>
        /// <param name="array">The one-dimensional array to copy to.</param>
        /// <param name="arrayIndex">The zero-based index in the target array at which copying begins.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            m_items.CopyTo(array, arrayIndex);
        }

        void ICollection.CopyTo(Array array, int arrayIndex)
        {
            (m_items as IList).CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Copyies the elements of this <see cref="ObservableArray{T}"/> to a compatible
        /// one-dimensional array, starting at the specifed index of the target array.
        /// </summary>
        /// <param name="index">The zero-based index in the source at which copying begins.</param>
        /// <param name="array">The one-dimensional array to copy to.</param>
        /// <param name="arrayIndex">The zero-based index in the target array at which copying begins.</param>
        /// <param name="count">The number of elements to copy.</param>
        public void CopyTo(int index, T[] array, int arrayIndex, int count)
        {
            m_items.CopyTo(index, array, arrayIndex, count);
        }

        /// <summary>
        /// Determines the index of a specific item in the <see cref="ObservableArray{T}"/>.
        /// </summary>
        /// <param name="value">The item to get the index of.</param>
        /// <returns>The index of the item if found, -1 if not found.</returns>
        public int IndexOf(object value)
        {
            return IndexOf((T) value);
        }

        /// <summary>
        /// Determines the index of a specific item in the <see cref="ObservableArray{T}"/>.
        /// </summary>
        /// <param name="item">The item to get the index of.</param>
        /// <returns>The index of the item if found, -1 if not found.</returns>
        public int IndexOf(T item)
        {
            return m_items.IndexOf(item);
        }

        /// <summary>
        /// Inserts an item into the <see cref="ObservableArray{T}"/> at the specified index.
        /// </summary>
        /// <param name="index">The index at which to insert the item.</param>
        /// <param name="value">The item to insert.</param>
        public void Insert(int index, object value)
        {
            Insert(index, (T) value);
        }

        /// <summary>
        /// Inserts an item into the <see cref="ObservableArray{T}"/> at the specified index.
        /// </summary>
        /// <param name="index">The index at which to insert the item.</param>
        /// <param name="item">The item to insert.</param>
        public void Insert(int index, T item)
        {
            CheckReentrancy();

            m_items.Insert(index, item);

            RegisterStateChangedHandler(item);

            OnPropertyChanged(nameof(Count));
            OnCollectionChanged(NotifyCollectionChangedAction.Add, item, index);
        }

        /// <summary>
        /// Gets the item in the <see cref="ObservableArray{T}"/> at the specified index.
        /// </summary>
        /// <param name="index">The index of the item to get.</param>
        /// <returns></returns>
        public T ItemAt(int index)
        {
            return m_items[index];
        }

        /// <summary>
        /// Moves an item in the <see cref="ObservableArray{T}"/> to a new index.
        /// </summary>
        /// <param name="oldIndex">The index of the item to be moved.</param>
        /// <param name="newIndex">The index to move the item to.</param>
        public void Move(int oldIndex, int newIndex)
        {
            CheckReentrancy();

            T oldItem = m_items[oldIndex];

            m_items.RemoveAt(oldIndex);
            m_items.Insert(newIndex, oldItem);

            OnCollectionChanged(NotifyCollectionChangedAction.Move, oldItem, newIndex, oldIndex);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="ObservableArray{T}"/>.
        /// </summary>
        /// <param name="value">The item to remove.</param>
        public void Remove(object value)
        {
            Remove((T) value);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="ObservableArray{T}"/>.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        /// <returns>True if the item was found in the list and removed.</returns>
        public bool Remove(T item)
        {
            CheckReentrancy();

            bool found = m_items.Remove(item);
            if (!found)
            {
                return false;
            }

            UnregisterStateChangedHandler(item);

            OnPropertyChanged(nameof(Count));
            OnCollectionChanged(NotifyCollectionChangedAction.Remove, item);

            return true;
        }

        /// <summary>
        /// Removes the item at the specified index from the <see cref="ObservableArray{T}"/>.
        /// </summary>
        /// <param name="index">The index of the item to remove.</param>
        public void RemoveAt(int index)
        {
            CheckReentrancy();

            T item = m_items[index];
            m_items.RemoveAt(index);

            UnregisterStateChangedHandler(item);

            OnPropertyChanged(nameof(Count));
            OnCollectionChanged(NotifyCollectionChangedAction.Remove, item, index);
        }

        /// <summary>
        /// Replaces the item at the specified index.
        /// </summary>
        /// <param name="item">The new item.</param>
        /// <param name="index">The index of the item to replace.</param>
        public void Replace(T item, int index)
        {
            CheckReentrancy();

            T oldItem = m_items[index];
            m_items[index] = item;

            UnregisterStateChangedHandler(oldItem);
            RegisterStateChangedHandler(item);

            OnCollectionChanged(NotifyCollectionChangedAction.Replace, item, oldItem, index);
        }

        /// <summary>
        /// Converts this <see cref="ObservableArray{T}"/> into a language-native array.
        /// </summary>
        /// <returns></returns>
        public T[] ToArray()
        {
            return m_items.ToArray();
        }

        public override string ToString()
        {
            return $"{typeof(T).Name}[{Count}]";
        }

        #region IEnumerable
        public IEnumerator<T> GetEnumerator()
        {
            return m_items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion

        #region ItemStateChanged
        /// <summary>
        /// Adds a <see cref="INotifyPropertyChanged.PropertyChanged"/> handler to a particular item
        /// so changes in it's state can be detected.
        /// </summary>
        /// <param name="item">The item to add the handler to.</param>
        protected void RegisterStateChangedHandler(T item)
        {
            if (item != null && m_itemsAreObservable)
            {
                ((INotifyPropertyChanged) item).PropertyChanged += ItemStateChangedHandler;
            }
        }

        /// <summary>
        /// Removes the <see cref="INotifyPropertyChanged.PropertyChanged"/> handler from a particular item.
        /// </summary>
        /// <param name="item">The item to add the handler to.</param>
        protected void UnregisterStateChangedHandler(T item)
        {
            if (item != null && m_itemsAreObservable)
            {
                ((INotifyPropertyChanged) item).PropertyChanged -= ItemStateChangedHandler;
            }
        }

        /// <summary>
        /// Called when an item's property changes state and triggers the <see cref="ItemStateChanged"/> event.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void ItemStateChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            if (sender is T typedSender)
            {
                int index = m_items.IndexOf(typedSender);
                if (index > -1)
                {
                    ItemStateChanged?.Invoke(this, new ItemStateChangedEventArgs(index, e));
                }
            }
        }
        #endregion

        #region CollectionChanged
        /// <summary>
        /// Raises a new <see cref="CollectionChanged"/> event.
        /// </summary>
        /// <param name="e">The event arguments.</param>
        protected void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            using (BlockReentrancy())
            {
                CollectionChanged?.Invoke(this, e);
            }
        }

        /// <summary>
        /// Raises a new <see cref="CollectionChanged"/> event for a one-item change.
        /// </summary>
        /// <param name="action">The action that triggered this event.</param>
        /// <param name="item">The item affected by the event.</param>
        protected void OnCollectionChanged(NotifyCollectionChangedAction action, object item)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, item));
        }

        /// <summary>
        /// Raises a new <see cref="CollectionChanged"/> event for a one-item change.
        /// </summary>
        /// <param name="action">The action that triggered this event.</param>
        /// <param name="item">The item affected by the event.</param>
        /// <param name="index">The index of the affected item.</param>
        protected void OnCollectionChanged(NotifyCollectionChangedAction action, object item, int index)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, item, index));
        }

        /// <summary>
        /// Raises a new <see cref="CollectionChanged"/> event for a one-item
        /// <see cref="NotifyCollectionChangedAction.Move"/> change.
        /// </summary>
        /// <param name="action">The action that triggered this event.</param>
        /// <param name="changedItem">The item affected by the event.</param>
        /// <param name="index">The new index of the affected item.</param>
        /// <param name="oldIndex">The old index of the affected item.</param>
        protected void OnCollectionChanged(NotifyCollectionChangedAction action, object changedItem, int index, int oldIndex)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, changedItem, index, oldIndex));
        }

        /// <summary>
        /// Raises a new <see cref="CollectionChanged"/> event for a one-item
        /// <see cref="NotifyCollectionChangedAction.Replace"/> change.
        /// </summary>
        /// <param name="action">The action that triggered this event.</param>
        /// <param name="newItem">The item added by this event.</param>
        /// <param name="oldItem">The item removed by this event.</param>
        /// <param name="index">The index of the affected items.</param>
        protected void OnCollectionChanged(NotifyCollectionChangedAction action, object newItem, object oldItem, int index)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, newItem, oldItem, index));
        }
        #endregion

        #region Operators
        public static implicit operator ObservableArray<T>(T[] array)
        {
            return new ObservableArray<T>(array);
        }

        public static explicit operator T[](ObservableArray<T> array)
        {
            return array.ToArray();
        }

        public static explicit operator ObservableArray<T>(List<T> array)
        {
            return new ObservableArray<T>(array);
        }

        public static explicit operator List<T>(ObservableArray<T> array)
        {
            return array.ToList();
        }
        #endregion

        #region Thread Safety
        private IDisposable BlockReentrancy()
        {
            m_monitor.Enter();
            return m_monitor;
        }

        private void CheckReentrancy()
        {
            if (m_monitor.Busy)
            {
                throw new InvalidOperationException(Strings.Error_InvalidOperation_NoCollectionReentrancy);
            }
        }
        #endregion

        #region Private Types
        private class BusyMonitor : IDisposable
        {
            private int m_busyCount;

            public bool Busy => m_busyCount > 0;

            public void Enter()
            {
                ++m_busyCount;
            }

            public void Dispose()
            {
                --m_busyCount;
            }
        }
        #endregion
    }
}
