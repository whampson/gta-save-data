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
    /// A collection of objects with sequential and contiguous storage.
    /// </summary>
    /// <typeparam name="T">The item type.</typeparam>
    /// <remarks>
    /// This collection is WPF-ready. Changes to items in the collection and to the collection itself
    /// are observable by WPF UIs via the CollectionChanged and ItemStateChanged events.
    /// </remarks>
    public abstract class Array<T> : IEnumerable, IEnumerable<T>, IList, IList<T>,
        INotifyCollectionChanged, INotifyItemStateChanged
        where T : new()
    {
        /// <summary>
        /// Occurs when the array changes, such as an item being added, removed, or replaced.
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>
        /// Occurs when the state of an item within the array changes.
        /// </summary>
        public event NotifyItemStateChangedEventHandler ItemStateChanged;

        private readonly List<T> m_items;
        private readonly bool m_itemsAreObservable;

        public int Count => m_items.Count;
        public bool IsReadOnly => false;
        public bool IsSynchronized => false;
        public object SyncRoot => this;

        public abstract bool IsFixedSize { get; }

        public T this[int index]
        {
            get { return ItemAt(index); }
            set { Replace(value, index); }
        }

        object IList.this[int index]
        {
            get { return this[index]; }
            set { this[index] = (T)value; }
        }
        
        /// <summary>
        /// Creates a new <see cref="Array{T}"/> with the specified number of items.
        /// </summary>
        /// <param name="count">The number of items to pre-allocate.</param>
        protected Array(int count)
        {
            m_items = new List<T>(count);
            m_itemsAreObservable = typeof(T).IsObservable();

            Populate(count);
        }

        /// <summary>
        /// Creates a new <see cref="Array{T}"/> with items from the specified enumerable.
        /// </summary>
        /// <param name="items">The items to initalize the array with.</param>
        protected Array(IEnumerable<T> items)
        {
            m_items = new List<T>();
            m_itemsAreObservable = typeof(T).IsObservable();

            Populate(items);
        }

        /// <summary>
        /// Creates a new <see cref="Array{T}"/> with items from the specified list.
        /// </summary>
        /// <param name="items">The items to initalize the array with.</param>
        protected Array(List<T> items)
            : this((IEnumerable<T>) items)
        { }

        private void Populate(int count)
        {
            for (int i = 0; i < count; i++)
            {
                T item = new T();
                RegisterStateChangedHandler(item);
                m_items.Add(item);
            }
        }

        private void Populate(IEnumerable<T> items)
        {
            foreach (T item in items)
            {
                RegisterStateChangedHandler(item);
                m_items.Add(item);
            }
        }

        /// <summary>
        /// Adds an item to the array, if allowed.
        /// </summary>
        /// <param name="value">The item to add.</param>
        /// <returns>The index of the new item, or -1 if not allowed.</returns>
        public int Add(object value)
        {
            if (IsFixedSize)
            {
                return -1;
            }

            Add((T) value);
            return m_items.Count - 1;
        }

        public void Add(T item)
        {
            if (IsFixedSize)
            {
                throw NotSupportedDueToFixedSize();
            }

            RegisterStateChangedHandler(item);
            m_items.Add(item);

            OnCollectionChanged(NotifyCollectionChangedAction.Add, item);
        }

        public void Clear()
        {
            if (IsFixedSize)
            {
                // Set all items to default value
                for (int i = 0; i < m_items.Count; i++)
                {
                    if (i < m_items.Count)
                    {
                        T oldItem = m_items[i];
                        UnregisterStateChangedHandler(oldItem);
                    }

                    T newItem = new T();
                    RegisterStateChangedHandler(newItem);
                    m_items[i] = newItem;
                }
            }
            else
            {
                // Empty-out the list
                m_items.ForEach(item => UnregisterStateChangedHandler(item));
                m_items.Clear();
            }

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public bool Contains(object value)
        {
            return Contains((T) value);
        }

        public bool Contains(T item)
        {
            return m_items.Contains(item);
        }

        public void CopyTo(Array array, int index)
        {
            for (int i = 0; i < m_items.Count; i++)
            {
                array.SetValue(m_items[i], index + i);
            }
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            m_items.CopyTo(array, arrayIndex);
        }

        public int IndexOf(object value)
        {
            return IndexOf((T) value);
        }

        public int IndexOf(T item)
        {
            return m_items.IndexOf(item);
        }

        public void Insert(int index, object value)
        {
            Insert(index, (T) value);
        }

        public void Insert(int index, T item)
        {
            if (IsFixedSize)
            {
                throw NotSupportedDueToFixedSize();
            }

            m_items.Insert(index, item);

            RegisterStateChangedHandler(item);
            OnCollectionChanged(NotifyCollectionChangedAction.Add, item, index);
        }

        public T ItemAt(int index)
        {
            return m_items[index];
        }

        public void Remove(object value)
        {
            Remove((T) value);
        }

        public bool Remove(T item)
        {
            if (IsFixedSize)
            {
                throw NotSupportedDueToFixedSize();
            }

            bool found = m_items.Remove(item);
            if (found)
            {
                UnregisterStateChangedHandler(item);
                OnCollectionChanged(NotifyCollectionChangedAction.Remove, item);
            }

            return found;
        }

        public void RemoveAt(int index)
        {
            if (IsFixedSize)
            {
                throw NotSupportedDueToFixedSize();
            }

            T item = m_items[index];
            m_items.RemoveAt(index);

            UnregisterStateChangedHandler(item);
            OnCollectionChanged(NotifyCollectionChangedAction.Remove, item, index);
        }

        public void Replace(T item, int index)
        {
            T oldItem = m_items[index];
            m_items[index] = item;

            UnregisterStateChangedHandler(oldItem);
            RegisterStateChangedHandler(item);
            OnCollectionChanged(NotifyCollectionChangedAction.Replace, item, oldItem, index);
        }

        public T[] ToArray()
        {
            return m_items.ToArray();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return m_items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        protected void RegisterStateChangedHandler(T item)
        {
            if (item != null && m_itemsAreObservable)
            {
                ((INotifyPropertyChanged) item).PropertyChanged += ItemPropertyChangedHandler;
            }
        }

        protected void UnregisterStateChangedHandler(T item)
        {
            if (item != null && m_itemsAreObservable)
            {
                ((INotifyPropertyChanged) item).PropertyChanged -= ItemPropertyChangedHandler;
            }
        }

        private void ItemPropertyChangedHandler(object sender, PropertyChangedEventArgs e)
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

        protected void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged?.Invoke(this, e);
        }

        protected void OnCollectionChanged(NotifyCollectionChangedAction action, object item)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, item));
        }

        protected void OnCollectionChanged(NotifyCollectionChangedAction action, object item, int index)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, item, index));
        }

        protected void OnCollectionChanged(NotifyCollectionChangedAction action, object item, int index, int oldIndex)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, item, index, oldIndex));
        }

        protected void OnCollectionChanged(NotifyCollectionChangedAction action, object newItem, object oldItem, int index)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, newItem, oldItem, index));
        }

        private NotSupportedException NotSupportedDueToFixedSize()
        {
            return new NotSupportedException(Strings.Error_NotSupported_FixedSizeCollection);
        }
    }
}
