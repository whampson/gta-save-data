using GTASaveData.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using WpfEssentials;

namespace GTASaveData
{
    /// <summary>
    /// Represents a collection of objects with sequential and contiguous storage.
    /// </summary>
    /// <typeparam name="T">The item type.</typeparam>
    /// <remarks>
    /// This collection is WPF-ready. Changes made to the collection are notified via the <see cref="CollectionChanged"/>
    /// and <see cref="INotifyPropertyChanged.PropertyChanged"/> events. Changes made to items in the collection are notified
    /// via the <see cref="ItemStateChanged"/> event.
    /// </remarks>
    public class Array<T> : ObservableObject,
        IEnumerable, IEnumerable<T>,
        IList, IList<T>,
        INotifyCollectionChanged, INotifyItemStateChanged
    {
        /// <summary>
        /// An empty <see cref="Array{T}"/>.
        /// </summary>
        public static readonly Array<T> Empty = new Array<T>();

        /// <summary>
        /// Occurs when the <see cref="Array{T}"/> changes, such as an item being added, removed, or replaced.
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>
        /// Occurs when the state of an item within the <see cref="Array{T}"/> changes.
        /// </summary>
        public event NotifyItemStateChangedEventHandler ItemStateChanged;

        private const string IndexerName = "Items[]";

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
            set { this[index] = (T) value; }
        }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">The index of the item to get or set.</param>
        /// <returns>The item at the specified index.</returns>
        public T this[int index]
        {
            get { return ItemAt(index); }
            set { Replace(value, index); }
        }
        
        /// <summary>
        /// Creates a new empty <see cref="Array{T}"/>.
        /// </summary>
        public Array()
        {
            m_monitor = new BusyMonitor();
            m_items = new List<T>();
            m_itemsAreObservable = typeof(T).IsObservable();
        }

        /// <summary>
        /// Creates a new <see cref="Array{T}"/> with items from the specified enumerable.
        /// </summary>
        /// <param name="items">The items to initalize the array with.</param>
        public Array(IEnumerable<T> items)
            : this()
        {
            foreach (T item in items)
            {
                RegisterStateChangedHandler(item);
                m_items.Add(item);
            }
        }

        /// <summary>
        /// Creates a new <see cref="Array{T}"/> with items from the specified list.
        /// </summary>
        /// <param name="items">The items to initalize the array with.</param>
        public Array(List<T> items)
            : this((IEnumerable<T>) items)
        { }

        /// <summary>
        /// Adds an item to the <see cref="Array{T}"/>.
        /// </summary>
        /// <param name="value">The item to add.</param>
        /// <returns>The index of the new item.</returns>
        public int Add(object value)
        {
            Add((T) value);

            return m_items.Count - 1;
        }

        /// <summary>
        /// Adds an item to the <see cref="Array{T}"/>.
        /// </summary>
        /// <param name="item">The item to add.</param>
        public void Add(T item)
        {
            CheckReentrancy();

            m_items.Add(item);

            RegisterStateChangedHandler(item);

            OnPropertyChanged(nameof(Count));
            OnPropertyChanged(IndexerName);
            OnCollectionChanged(NotifyCollectionChangedAction.Add, item);
        }

        /// <summary>
        /// Removes all items from <see cref="Array{T}"/>.
        /// </summary>
        public void Clear()
        {
            CheckReentrancy();

            m_items.ForEach(item => UnregisterStateChangedHandler(item));
            m_items.Clear();

            OnPropertyChanged(nameof(Count));
            OnPropertyChanged(IndexerName);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <summary>
        /// Determines whether the <see cref="Array{T}"/> contains a specific value.
        /// </summary>
        /// <param name="value">The item to locate.</param>
        /// <returns>True if the item exists in the array.</returns>
        public bool Contains(object value)
        {
            return Contains((T) value);
        }

        /// <summary>
        /// Determines whether the <see cref="Array{T}"/> contains a specific value.
        /// </summary>
        /// <param name="item">The item to locate.</param>
        /// <returns>True if the item exists in the array.</returns>
        public bool Contains(T item)
        {
            return m_items.Contains(item);
        }

        /// <summary>
        /// Copyies the elements of this <see cref="Array{T}"/> to an <see cref="Array"/>,
        /// starting at a particular <see cref="Array"/> index.
        /// </summary>
        /// <param name="array">The array to copy to.</param>
        /// <param name="index">The index in the destination array to copy to.</param>
        public void CopyTo(Array array, int index)
        {
            for (int i = 0; i < m_items.Count; i++)
            {
                array.SetValue(m_items[i], index + i);
            }
        }

        /// <summary>
        /// Copyies the elements of this <see cref="Array{T}"/> to an <see cref="Array"/>,
        /// starting at a particular <see cref="Array"/> index.
        /// </summary>
        /// <param name="array">The array to copy to.</param>
        /// <param name="index">The index in the destination array to copy to.</param>
        public void CopyTo(T[] array, int index)
        {
            m_items.CopyTo(array, index);
        }

        /// <summary>
        /// Determines the index of a specific item in the <see cref="Array{T}"/>.
        /// </summary>
        /// <param name="value">The item to get the index of.</param>
        /// <returns>The index of the item if found, -1 if not found.</returns>
        public int IndexOf(object value)
        {
            return IndexOf((T) value);
        }

        /// <summary>
        /// Determines the index of a specific item in the <see cref="Array{T}"/>.
        /// </summary>
        /// <param name="item">The item to get the index of.</param>
        /// <returns>The index of the item if found, -1 if not found.</returns>
        public int IndexOf(T item)
        {
            return m_items.IndexOf(item);
        }

        /// <summary>
        /// Inserts an item into the <see cref="Array{T}"/> at the specified index.
        /// </summary>
        /// <param name="index">The index at which to insert the item.</param>
        /// <param name="value">The item to insert.</param>
        public void Insert(int index, object value)
        {
            Insert(index, (T) value);
        }

        /// <summary>
        /// Inserts an item into the <see cref="Array{T}"/> at the specified index.
        /// </summary>
        /// <param name="index">The index at which to insert the item.</param>
        /// <param name="item">The item to insert.</param>
        public void Insert(int index, T item)
        {
            CheckReentrancy();

            m_items.Insert(index, item);

            RegisterStateChangedHandler(item);

            OnPropertyChanged(nameof(Count));
            OnPropertyChanged(IndexerName);
            OnCollectionChanged(NotifyCollectionChangedAction.Add, item, index);
        }

        /// <summary>
        /// Gets the item in the <see cref="Array{T}"/> at the specified index.
        /// </summary>
        /// <param name="index">The index of the item to get.</param>
        /// <returns></returns>
        public T ItemAt(int index)
        {
            return m_items[index];
        }

        public void Move(int oldIndex, int newIndex)
        {
            CheckReentrancy();

            T oldItem = m_items[oldIndex];

            m_items.RemoveAt(oldIndex);
            m_items.Insert(newIndex, oldItem);

            OnPropertyChanged(IndexerName);
            OnCollectionChanged(NotifyCollectionChangedAction.Move, oldItem, newIndex, oldIndex);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="Array{T}"/>.
        /// </summary>
        /// <param name="value">The item to remove.</param>
        public void Remove(object value)
        {
            Remove((T) value);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="Array{T}"/>.
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
            OnPropertyChanged(IndexerName);
            OnCollectionChanged(NotifyCollectionChangedAction.Remove, item);

            return true;
        }

        public void RemoveAt(int index)
        {
            CheckReentrancy();

            T item = m_items[index];
            m_items.RemoveAt(index);

            UnregisterStateChangedHandler(item);

            OnPropertyChanged(nameof(Count));
            OnPropertyChanged(IndexerName);
            OnCollectionChanged(NotifyCollectionChangedAction.Remove, item, index);
        }

        public void Replace(T item, int index)
        {
            CheckReentrancy();

            T oldItem = m_items[index];
            m_items[index] = item;

            UnregisterStateChangedHandler(oldItem);
            RegisterStateChangedHandler(item);

            OnPropertyChanged(IndexerName);
            OnCollectionChanged(NotifyCollectionChangedAction.Replace, item, oldItem, index);
        }

        /// <summary>
        /// Converts this <see cref="Array{T}"/> into a language-native array.
        /// </summary>
        /// <returns></returns>
        public T[] ToArray()
        {
            return m_items.ToArray();
        }

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
                throw new InvalidOperationException(Strings.Error_InvalidOperation_CollectionReentrancy);
            }
        }
        #endregion

        #region IEnumerable
        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An item enumerator.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return m_items.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An item enumerator.</returns>
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
        public static implicit operator Array<T>(T[] array)
        {
            return new Array<T>(array);
        }

        public static implicit operator T[](Array<T> array)
        {
            return array.ToArray();
        }

        public static implicit operator Array<T>(List<T> array)
        {
            return new Array<T>(array);
        }

        public static implicit operator List<T>(Array<T> array)
        {
            return array;
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
