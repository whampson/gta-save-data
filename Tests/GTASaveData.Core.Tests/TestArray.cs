using Bogus;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using WpfEssentials;
using Xunit;

namespace GTASaveData.Core.Tests
{
    public class TestArray
    {
        [Fact]
        public void CtorCount()
        {
            StaticArray<TestObject> a = new StaticArray<TestObject>(10);

            Assert.Equal(10, a.Count);
            Assert.True(a.IsFixedSize);
            Assert.False(a.IsReadOnly);
            Assert.Equal(new TestObject(), a[9]);
        }

        [Fact]
        public void CtorList()
        {
            List<TestObject> items = Enumerable.Repeat(0, 10).Select(x => TestObject.GenerateRandom()).ToList();

            StaticArray<TestObject> a = new StaticArray<TestObject>(items);

            Assert.Equal(10, a.Count);
            Assert.True(a.IsFixedSize);
            Assert.False(a.IsReadOnly);
            Assert.Equal(items[9], a[9]);
        }

        [Fact]
        public void Add()
        {
            List<NotifyCollectionChangedEventArgs> raisedEvents = new List<NotifyCollectionChangedEventArgs>();

            StaticArray<int> a = new StaticArray<int>(10);
            DynamicArray<int> b = new DynamicArray<int>(10);

            b.CollectionChanged += delegate (object sender, NotifyCollectionChangedEventArgs e)
            {
                raisedEvents.Add(e);
            };

            Assert.Throws<NotSupportedException>(() => a.Add(1234));
            b.Add(1234);

            Assert.Equal(11, b.Count);
            Assert.Equal(1234, b[10]);
            Assert.Single(raisedEvents);
            Assert.Equal(NotifyCollectionChangedAction.Add, raisedEvents[0].Action);
            Assert.Equal(1234, raisedEvents[0].NewItems[0]);
        }


        [Fact]
        public void Clear()
        {
            List<TestObject> items = Enumerable.Repeat(0, 10).Select(x => TestObject.GenerateRandom()).ToList();
            List<NotifyCollectionChangedEventArgs> raisedEvents = new List<NotifyCollectionChangedEventArgs>();

            StaticArray<TestObject> a = new StaticArray<TestObject>(items);
            DynamicArray<TestObject> b = new DynamicArray<TestObject>(items);

            a.CollectionChanged += delegate (object sender, NotifyCollectionChangedEventArgs e)
            {
                raisedEvents.Add(e);
            };
            b.CollectionChanged += delegate (object sender, NotifyCollectionChangedEventArgs e)
            {
                raisedEvents.Add(e);
            };

            a.Clear();
            b.Clear();

            Assert.Equal(10, a.Count);
            foreach (TestObject o in a)
            {
                Assert.Equal(new TestObject(), o);
            }

            Assert.Empty(b);

            Assert.Equal(2, raisedEvents.Count);
            Assert.Equal(NotifyCollectionChangedAction.Reset, raisedEvents[0].Action);
            Assert.Equal(NotifyCollectionChangedAction.Reset, raisedEvents[1].Action);
        }

        [Fact]
        public void Insert()
        {
            List<NotifyCollectionChangedEventArgs> raisedEvents = new List<NotifyCollectionChangedEventArgs>();

            StaticArray<int> a = new StaticArray<int>(10);
            DynamicArray<int> b = new DynamicArray<int>(10);

            b.CollectionChanged += delegate (object sender, NotifyCollectionChangedEventArgs e)
            {
                raisedEvents.Add(e);
            };

            Assert.Throws<NotSupportedException>(() => a.Insert(5, 1234));
            b.Insert(5, 1234);

            Assert.Equal(11, b.Count);
            Assert.Equal(1234, b[5]);
            Assert.Single(raisedEvents);
            Assert.Equal(NotifyCollectionChangedAction.Add, raisedEvents[0].Action);
            Assert.Equal(1234, raisedEvents[0].NewItems[0]);
            Assert.Equal(5, raisedEvents[0].NewStartingIndex);
        }

        [Fact]
        public void ModifyItem()
        {
            List<ItemStateChangedEventArgs> raisedEvents = new List<ItemStateChangedEventArgs>();
            List<TestObject> items = Enumerable.Repeat(0, 10).Select(x => TestObject.GenerateRandom()).ToList();

            StaticArray<TestObject> a = new StaticArray<TestObject>(items);

            a.ItemStateChanged += delegate (object sender, ItemStateChangedEventArgs e)
            {
                raisedEvents.Add(e);
            };

            a[5].Value = 1234;

            Assert.Single(raisedEvents);
            Assert.Equal(5, raisedEvents[0].ItemIndex);
            Assert.Equal(nameof(TestObject.Value), raisedEvents[0].PropertyName);
        }

        [Fact]
        public void Remove()
        {
            List<NotifyCollectionChangedEventArgs> raisedEvents = new List<NotifyCollectionChangedEventArgs>();

            StaticArray<int> a = new StaticArray<int>(10);
            DynamicArray<int> b = new DynamicArray<int>(10);

            a[5] = 1234;
            b[5] = 1234;

            b.CollectionChanged += delegate (object sender, NotifyCollectionChangedEventArgs e)
            {
                raisedEvents.Add(e);
            };

            Assert.Throws<NotSupportedException>(() => a.Remove(1234));
            Assert.True(b.Remove(1234));
            Assert.False(b.Remove(5678));

            Assert.Equal(9, b.Count);
            Assert.Single(raisedEvents);
            Assert.Equal(NotifyCollectionChangedAction.Remove, raisedEvents[0].Action);
            Assert.Equal(1234, raisedEvents[0].OldItems[0]);
        }

        [Fact]
        public void RemoveAt()
        {
            List<NotifyCollectionChangedEventArgs> raisedEvents = new List<NotifyCollectionChangedEventArgs>();

            StaticArray<int> a = new StaticArray<int>(10);
            DynamicArray<int> b = new DynamicArray<int>(10);

            a[5] = 1234;
            b[5] = 1234;

            b.CollectionChanged += delegate (object sender, NotifyCollectionChangedEventArgs e)
            {
                raisedEvents.Add(e);
            };

            Assert.Throws<NotSupportedException>(() => a.RemoveAt(5));
            b.RemoveAt(5);

            Assert.Equal(9, b.Count);
            Assert.Single(raisedEvents);
            Assert.Equal(NotifyCollectionChangedAction.Remove, raisedEvents[0].Action);
            Assert.Equal(1234, raisedEvents[0].OldItems[0]);
            Assert.Equal(5, raisedEvents[0].OldStartingIndex);
        }

        [Fact]
        public void Replace()
        {
            List<NotifyCollectionChangedEventArgs> raisedEvents = new List<NotifyCollectionChangedEventArgs>();

            StaticArray<int> a = new StaticArray<int>(10);

            a.CollectionChanged += delegate (object sender, NotifyCollectionChangedEventArgs e)
            {
                raisedEvents.Add(e);
            };

            a[5] = 1234;

            Assert.Equal(10, a.Count);
            Assert.Single(raisedEvents);
            Assert.Equal(NotifyCollectionChangedAction.Replace, raisedEvents[0].Action);
            Assert.Equal(0, raisedEvents[0].OldItems[0]);
            Assert.Equal(1234, raisedEvents[0].NewItems[0]);
            Assert.Equal(5, raisedEvents[0].NewStartingIndex);
        }

        #region Test Objects
        private class TestObject : INotifyPropertyChanged, IEquatable<TestObject>
        {
            public event PropertyChangedEventHandler PropertyChanged;

            private int m_value;

            public int Value
            {
                get { return m_value; }
                set { m_value = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value))); }
            }

            public TestObject()
            {
                m_value = 0;
            }

            public static TestObject GenerateRandom()
            {
                Faker<TestObject> model = new Faker<TestObject>()
                    .RuleFor(x => x.Value, f => f.Random.Int());

                return model.Generate();
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as TestObject);
            }

            public bool Equals(TestObject other)
            {
                if (other == null)
                {
                    return false;
                }

                return m_value.Equals(other.m_value);
            }
        }
        #endregion
    }
}
