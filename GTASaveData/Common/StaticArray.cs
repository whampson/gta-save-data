using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using WpfEssentials;

namespace GTASaveData.Common
{
    public delegate void NotifyItemStateChangedEventHandler(object sender, ItemPropertyChangedEventArgs e);

    public class StaticArray<T> : IEnumerable, IEnumerable<T>, INotifyCollectionChanged
        where T : new()
    {
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public event NotifyItemStateChangedEventHandler ItemStateChanged;

        private readonly int m_count;
        private readonly List<T> m_items;
        private readonly bool m_itemsAreObservable;

        public StaticArray(int count)
        {
            m_count = count;
            m_items = new List<T>(count);
            m_itemsAreObservable = typeof(T).GetInterface(nameof(INotifyPropertyChanged)) != null;

            Initialize();
        }

        public T this[int index]
        {
            get { return GetItem(index); }
            set { SetItem(value, index); }
        }

        public int Length
        {
            get { return m_count; }
        }

        private void Initialize()
        {
            for (int i = 0; i < m_count; i++)
            {
                T item = new T();
                RegisterStateChangedHandler(item);
                m_items.Add(item);
            }
        }

        private T GetItem(int index)
        {
            return m_items[index];
        }

        private void SetItem(T item, int index)
        {
            T oldItem = m_items[index];
            m_items[index] = item;

            UnregisterStateChangedHandler(oldItem);
            RegisterStateChangedHandler(item);
            OnItemReplaced(item, oldItem, index);
        }

        public void Reset()
        {
            for (int i = 0; i < m_count; i++)
            {
                m_items[i] = new T();
            }
        }

        private void RegisterStateChangedHandler(T item)
        {
            if (item == null || !m_itemsAreObservable)
            {
                return;
            }

            ((INotifyPropertyChanged) item).PropertyChanged += ItemStateChangedHandler;
        }

        private void UnregisterStateChangedHandler(T item)
        {
            if (item == null || !m_itemsAreObservable)
            {
                return;
            }

            ((INotifyPropertyChanged)item).PropertyChanged -= ItemStateChangedHandler;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return m_items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private void OnItemReplaced(object newItem, object oldItem, int index)
        {
            var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, newItem, oldItem, index);
            CollectionChanged?.Invoke(this, args);
        }

        private void ItemStateChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            if (sender is T typedSender)
            {
                int index = m_items.IndexOf(typedSender);
                if (index > -1)
                {
                    ItemStateChanged?.Invoke(this, new ItemPropertyChangedEventArgs(index, e));
                }
            }
        }
    }
}
