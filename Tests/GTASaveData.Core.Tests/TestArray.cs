using Bogus;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using TestFramework;
using WpfEssentials;
using Xunit;

namespace GTASaveData.Core.Tests
{
    public class TestArray
    {
        private const string IndexerPropertyName = "Items[]";
        private const string CountPropertyName = "Count";

        [Fact]
        public void CtorList()
        {
            List<TestObject> items = Enumerable.Repeat(0, 10).Select(x => TestObject.GenerateRandom()).ToList();

            Array<TestObject> a = new Array<TestObject>(items);

            Assert.Equal(10, a.Count);
            Assert.Equal(items[9], a[9]);
        }

        [Fact]
        public void Add()
        {
            var raisedCollectionEvents = new List<NotifyCollectionChangedEventArgs>();
            var raisedPropertyEvents = new List<PropertyChangedEventArgs>();

            Array<int> a = ArrayHelper.CreateArray<int>(10);

            a.CollectionChanged += delegate (object sender, NotifyCollectionChangedEventArgs e)
            {
                raisedCollectionEvents.Add(e);
            };
            a.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e)
            {
                raisedPropertyEvents.Add(e);
            };

            a.Add(1234);

            Assert.Equal(11, a.Count);
            Assert.Equal(1234, a[10]);
            Assert.Single(raisedCollectionEvents);
            Assert.Equal(NotifyCollectionChangedAction.Add, raisedCollectionEvents[0].Action);
            Assert.Equal(1234, raisedCollectionEvents[0].NewItems[0]);
            Assert.Equal(2, raisedPropertyEvents.Count);
            Assert.Single(raisedPropertyEvents.Where(x => x.PropertyName == CountPropertyName));
            Assert.Single(raisedPropertyEvents.Where(x => x.PropertyName == IndexerPropertyName));
        }


        [Fact]
        public void Clear()
        {
            var raisedCollectionEvents = new List<NotifyCollectionChangedEventArgs>();
            var raisedPropertyEvents = new List<PropertyChangedEventArgs>();

            Array<int> a = ArrayHelper.CreateArray<int>(10);
            a.CollectionChanged += delegate (object sender, NotifyCollectionChangedEventArgs e)
            {
                raisedCollectionEvents.Add(e);
            };
            a.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e)
            {
                raisedPropertyEvents.Add(e);
            };

            a.Clear();

            Assert.Empty(a);
            Assert.Single(raisedCollectionEvents);
            Assert.Equal(NotifyCollectionChangedAction.Reset, raisedCollectionEvents[0].Action);
            Assert.Equal(2, raisedPropertyEvents.Count);
            Assert.Single(raisedPropertyEvents.Where(x => x.PropertyName == CountPropertyName));
            Assert.Single(raisedPropertyEvents.Where(x => x.PropertyName == IndexerPropertyName));
        }

        [Fact]
        public void Insert()
        {
            var raisedCollectionEvents = new List<NotifyCollectionChangedEventArgs>();
            var raisedPropertyEvents = new List<PropertyChangedEventArgs>();

            Array<int> a = ArrayHelper.CreateArray<int>(10);
            a.CollectionChanged += delegate (object sender, NotifyCollectionChangedEventArgs e)
            {
                raisedCollectionEvents.Add(e);
            };
            a.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e)
            {
                raisedPropertyEvents.Add(e);
            };

            a.Insert(5, 1234);

            Assert.Equal(11, a.Count);
            Assert.Equal(1234, a[5]);
            Assert.Single(raisedCollectionEvents);
            Assert.Equal(NotifyCollectionChangedAction.Add, raisedCollectionEvents[0].Action);
            Assert.Equal(1234, raisedCollectionEvents[0].NewItems[0]);
            Assert.Equal(5, raisedCollectionEvents[0].NewStartingIndex);
            Assert.Equal(2, raisedPropertyEvents.Count);
            Assert.Single(raisedPropertyEvents.Where(x => x.PropertyName == CountPropertyName));
            Assert.Single(raisedPropertyEvents.Where(x => x.PropertyName == IndexerPropertyName));
        }

        [Fact]
        public void ModifyItem()
        {
            var raisedItemEvents = new List<ItemStateChangedEventArgs>();
            var raisedPropertyEvents = new List<PropertyChangedEventArgs>();

            Array<TestObject> a = Generator.Array(10, g => TestObject.GenerateRandom());

            a.ItemStateChanged += delegate (object sender, ItemStateChangedEventArgs e)
            {
                raisedItemEvents.Add(e);
            };
            a.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e)
            {
                raisedPropertyEvents.Add(e);
            };

            a[5].Value = 1234;

            Assert.Single(raisedItemEvents);
            Assert.Equal(5, raisedItemEvents[0].ItemIndex);
            Assert.Equal(nameof(TestObject.Value), raisedItemEvents[0].PropertyName);
            Assert.Empty(raisedPropertyEvents);
        }

        [Fact]
        public void Move()
        {
            var raisedCollectionEvents = new List<NotifyCollectionChangedEventArgs>();
            var raisedPropertyEvents = new List<PropertyChangedEventArgs>();

            Array<int> a = ArrayHelper.CreateArray<int>(10);
            a[5] = 1234;

            a.CollectionChanged += delegate (object sender, NotifyCollectionChangedEventArgs e)
            {
                raisedCollectionEvents.Add(e);
            };
            a.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e)
            {
                raisedPropertyEvents.Add(e);
            };

            a.Move(5, 6);

            Assert.Single(raisedCollectionEvents);
            Assert.Equal(NotifyCollectionChangedAction.Move, raisedCollectionEvents[0].Action);
            Assert.Equal(1234, raisedCollectionEvents[0].OldItems[0]);
            Assert.Equal(1234, raisedCollectionEvents[0].NewItems[0]);
            Assert.Equal(5, raisedCollectionEvents[0].OldStartingIndex);
            Assert.Equal(6, raisedCollectionEvents[0].NewStartingIndex);
            Assert.Single(raisedPropertyEvents);
            Assert.Equal(IndexerPropertyName, raisedPropertyEvents[0].PropertyName);
        }

        [Fact]
        public void Remove()
        {
            var raisedCollectionEvents = new List<NotifyCollectionChangedEventArgs>();
            var raisedPropertyEvents = new List<PropertyChangedEventArgs>();

            Array<int> a = ArrayHelper.CreateArray<int>(10);
            a[5] = 1234;

            a.CollectionChanged += delegate (object sender, NotifyCollectionChangedEventArgs e)
            {
                raisedCollectionEvents.Add(e);
            };
            a.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e)
            {
                raisedPropertyEvents.Add(e);
            };

            Assert.True(a.Remove(1234));
            Assert.False(a.Remove(5678));

            Assert.Equal(9, a.Count);
            Assert.Single(raisedCollectionEvents);
            Assert.Equal(NotifyCollectionChangedAction.Remove, raisedCollectionEvents[0].Action);
            Assert.Equal(1234, raisedCollectionEvents[0].OldItems[0]);
            Assert.Equal(2, raisedPropertyEvents.Count);
            Assert.Single(raisedPropertyEvents.Where(x => x.PropertyName == CountPropertyName));
            Assert.Single(raisedPropertyEvents.Where(x => x.PropertyName == IndexerPropertyName));
        }

        [Fact]
        public void RemoveAt()
        {
            var raisedCollectionEvents = new List<NotifyCollectionChangedEventArgs>();
            var raisedPropertyEvents = new List<PropertyChangedEventArgs>();

            Array<int> a = ArrayHelper.CreateArray<int>(10);
            a[5] = 1234;

            a.CollectionChanged += delegate (object sender, NotifyCollectionChangedEventArgs e)
            {
                raisedCollectionEvents.Add(e);
            };
            a.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e)
            {
                raisedPropertyEvents.Add(e);
            };

            a.RemoveAt(5);

            Assert.Equal(9, a.Count);
            Assert.Single(raisedCollectionEvents);
            Assert.Equal(NotifyCollectionChangedAction.Remove, raisedCollectionEvents[0].Action);
            Assert.Equal(1234, raisedCollectionEvents[0].OldItems[0]);
            Assert.Equal(5, raisedCollectionEvents[0].OldStartingIndex);
            Assert.Equal(2, raisedPropertyEvents.Count);
            Assert.Single(raisedPropertyEvents.Where(x => x.PropertyName == CountPropertyName));
            Assert.Single(raisedPropertyEvents.Where(x => x.PropertyName == IndexerPropertyName));
        }

        [Fact]
        public void Replace()
        {
            var raisedCollectionEvents = new List<NotifyCollectionChangedEventArgs>();
            var raisedPropertyEvents = new List<PropertyChangedEventArgs>();

            Array<int> a = ArrayHelper.CreateArray<int>(10);

            a.CollectionChanged += delegate (object sender, NotifyCollectionChangedEventArgs e)
            {
                raisedCollectionEvents.Add(e);
            };
            a.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e)
            {
                raisedPropertyEvents.Add(e);
            };

            a[5] = 1234;

            Assert.Equal(10, a.Count);
            Assert.Single(raisedCollectionEvents);
            Assert.Equal(NotifyCollectionChangedAction.Replace, raisedCollectionEvents[0].Action);
            Assert.Equal(0, raisedCollectionEvents[0].OldItems[0]);
            Assert.Equal(1234, raisedCollectionEvents[0].NewItems[0]);
            Assert.Equal(5, raisedCollectionEvents[0].NewStartingIndex);
            Assert.Single(raisedPropertyEvents);
            Assert.Equal(IndexerPropertyName, raisedPropertyEvents[0].PropertyName);
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
