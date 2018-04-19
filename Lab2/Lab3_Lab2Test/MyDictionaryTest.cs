using System;
using System.Linq;
using System.Collections.Generic;
using Lab2_Lib;
using NUnit.Framework;
using System.Collections;

namespace Lab3_Lab2Test
{
    [TestFixture]
    public class MyDictionaryTest
    {
        public MyDictionary<int, string> Dictionary;
        public List<KeyValuePair<int, string>> List;

        [OneTimeSetUp]
        public void InitList()
        {
            List = new List<KeyValuePair<int, string>>();
            for (int i = 0; i < 50; i++) {
                List.Add(new KeyValuePair<int, string>(i, i.ToString()));
            }
        }

        [SetUp]
        public void InitDictionary()
        {
            Dictionary = new MyDictionary<int, string>();
            foreach (var i in List) {
                Dictionary.Add(i);
            }
        }

        [TearDown]
        public void ClearDictionary()
        {
            Dictionary.Clear();
        }

        [Test]
        public void Add_OneRecord()
        {
            var d = new MyDictionary<int, string>();
            var item = new KeyValuePair<int, string>(0, null);
            d.Add(item);
            Assert.AreEqual(1, d.Count);
            Assert.IsTrue(d.Contains(item));
        }

        [Test]
        public void Add_MultipleRecords()
        {
            MyDictionary<int, string> d = new MyDictionary<int, string>();
            foreach (var i in List) {
                d.Add(i);
            }
            Assert.AreEqual(50, d.Count);
            foreach (var i in List) {
                Assert.IsTrue(Dictionary.Contains(i));
            }
        }

        [Test]
        public void Add_ExistingKey_ArgumentException()
        {
            Assert.Throws<ArgumentException>(
                () => {
                    Dictionary.Add(0, "1");
                },
                "Add_ExistingKey_ArgumentException");
        }

        [Test]
        public void RemoveByKey_OneRecord_True()
        {
            Dictionary.Remove(0);
            Assert.AreEqual(49, Dictionary.Count);
            Assert.IsFalse(Dictionary.ContainsKey(0));
        }

        [Test]
        public void RemoveByKey_OneNonExistentRecord_False()
        {
            Dictionary.Remove(200);
            Assert.AreEqual(50, Dictionary.Count);
            Assert.IsFalse(Dictionary.ContainsKey(200));
        }

        [Test]
        public void RemoveByKey_MultipleRecords()
        {
            foreach (var i in List)
                Dictionary.Remove(i.Key);
            Assert.AreEqual(0, Dictionary.Count);
            foreach (var i in List) {
                Assert.IsFalse(Dictionary.Contains(i));
            }
        }

        [Test]
        public void RemoveByKeyValuePair_OneRecord_True()
        {
            var item = new KeyValuePair<int, string>(0, "0");
            Dictionary.Remove(item);
            Assert.AreEqual(49, Dictionary.Count);
            Assert.IsFalse(Dictionary.ContainsKey(0));
        }

        [Test]
        public void RemoveByKeyValuePair_OneNonExistentOneRecord_False()
        {
            var item = new KeyValuePair<int, string>(300, "300");
            Dictionary.Remove(item);
            Assert.AreEqual(50, Dictionary.Count);
            Assert.IsFalse(Dictionary.ContainsKey(300));
        }

        [Test]
        public void RemoveByKeyValuePair_MultipleRecords()
        {
            foreach (var i in List)
                Dictionary.Remove(i);
            Assert.AreEqual(0, Dictionary.Count);
            foreach (var i in List) {
                Assert.IsFalse(Dictionary.Contains(i));
            }
        }

        [Test]
        public void IndexerGet_CorrectIndex()
        {
            Assert.AreEqual(List[20].Value, Dictionary[List[20].Key]);
        }

        [Test]
        public void IndexerSet_CorrectIndex()
        {
            Dictionary[List[20].Key] = List[21].Value;
            Assert.AreEqual(List[21].Value, Dictionary[List[20].Key]);
        }

        [Test]
        public void IndexerGet_IncorrectIndex()
        {
            Assert.Throws<KeyNotFoundException>(
                () => { var a = Dictionary[200]; },
                "IndexerGet_IncorrectIndex");
        }

        [Test]
        public void IndexerSet_IncorrectIndex()
        {
            Assert.Throws<KeyNotFoundException>(
                () => { Dictionary[200] = "200"; },
                "IndexerGet_IncorrectIndex");
        }

        [Test]
        public void GetEnumerator()
        {
            int j = 0;
            foreach (var i in (IEnumerable)Dictionary) {
                Assert.IsTrue(List[j].Equals(i));
                j++;
            }
        }

        [Test]
        public void GetEnumeratorGeneric()
        {
            int j = 0;
            foreach (var i in Dictionary) {
                Assert.IsTrue(List[j].Equals(i));
                j++;
            }
        }

        [Test]
        public void Clear()
        {
            Dictionary.Clear();
            Assert.AreEqual(0, Dictionary.Count);
        }

        [Test]
        public void ContainsKey_ExistentValue()
        {
            foreach (var i in List)
                Assert.IsTrue(Dictionary.ContainsKey(i.Key));
        }

        [Test]
        public void ContainsKey_NonExistentValue()
        {
            Random rand = new Random();
            foreach (var i in List)
                Assert.IsFalse(Dictionary.ContainsKey(rand.Next(200, 300)));
        }

        [Test]
        public void ContainsValue_ExistentValue()
        {
            foreach (var i in List)
                Assert.IsTrue(Dictionary.ContainsValue(i.Value));
        }

        [Test]
        public void ContainsValue_NonExistentValue()
        {
            Random rand = new Random();
            foreach (var i in List)
                Assert.IsFalse(Dictionary.ContainsValue(rand.Next(200, 300).ToString()));
        }

        [Test]
        public void TryAdd_NonExistentKey()
        {
            Random rand = new Random();
            var d = new MyDictionary<int, string>();
            foreach (var i in List)
                Assert.IsTrue(d.TryAdd(i));
        }

        [Test]
        public void TryAdd_ExistentKey()
        {
            Random rand = new Random();
            foreach (var i in List)
                Assert.IsFalse(Dictionary.TryAdd(i));
        }

        [Test]
        public void CopyTo()
        {
            var arr = new KeyValuePair<int, string>[50];
            Dictionary.CopyTo(arr, 0);
            var i = 0;
            foreach (var d in Dictionary) {
                Assert.AreEqual(d, arr[i]);
                i++;
            }
        }

        [Test]
        public void CopyTo_IndexLessThan0()
        {
            var arr = new KeyValuePair<int, string>[50];
            Assert.Throws<ArgumentOutOfRangeException>(() => { Dictionary.CopyTo(arr, -1); });
        }

        [Test]
        public void CopyTo_NullArray()
        {
            Assert.Throws<ArgumentNullException>(()=> { Dictionary.CopyTo(null, 0); });
        }

        [Test]
        public void CopyTo_ArrayWasNotLongEnough()
        {
            var arr = new KeyValuePair<int, string>[20];
            Assert.Throws<ArgumentException>(()=> { Dictionary.CopyTo(arr, 0); });
        }

        [Test]
        public void OnAddTest()
        {
            var isEventRunned = false;
            Dictionary.OnAdd += (s, args) => {
                isEventRunned = true;
            };

            Assert.IsFalse(isEventRunned);

            Dictionary.Add(200, "200");

            Assert.IsTrue(isEventRunned);
        }

        [Test]
        public void OnClearTest()
        {
            var isEventRunned = false;
            Dictionary.OnClear += (s, args) => {
                isEventRunned = true;
            };

            Assert.IsFalse(isEventRunned);

            Dictionary.Clear();

            Assert.IsTrue(isEventRunned);
        }

        [Test]
        public void OnRemoveTest()
        {
            var isEventRunned = false;
            Dictionary.OnRemove += (s, args) => {
                isEventRunned = true;
            };

            Assert.IsFalse(isEventRunned);

            Dictionary.Remove(1);

            Assert.IsTrue(isEventRunned);
        }
    }
}
