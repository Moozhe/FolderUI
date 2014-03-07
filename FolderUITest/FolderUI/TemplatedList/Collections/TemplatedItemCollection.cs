using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.ComponentModel.Design;
using System.Collections;

namespace FolderUI
{
    public partial class TemplatedList
    {
        public class TemplatedItemCollection : IList<TemplatedItem>, ICollection<TemplatedItem>, IEnumerable<TemplatedItem>, IList, ICollection, IEnumerable
        {
            public event EventHandler CollectionChanged;

            private List<TemplatedItem> innerList;
            private TemplatedList owner;

            public int Count
            {
                get { return innerList.Count; }
            }

            public bool IsReadOnly
            {
                get { return false; }
            }

            public TemplatedItem this[int index]
            {
                get
                {
                    return innerList[index];
                }
                set
                {
                    if (innerList[index] != null)
                        innerList[index].TemplatedList = null;

                    innerList[index] = value;

                    if (innerList[index] != null)
                        innerList[index].TemplatedList = owner;

                    OnCollectionChanged(EventArgs.Empty);
                }
            }

            public TemplatedItem this[string key]
            {
                get
                {
                    foreach (TemplatedItem xItem in this)
                    {
                        if (xItem.Name == key)
                            return xItem;
                    }

                    throw new ArgumentException("Key does not exist.");
                }
            }

            public TemplatedItemCollection(TemplatedList iOwner)
            {
                owner = iOwner;

                innerList = new List<TemplatedItem>();
            }

            private void AddItem(TemplatedItem item)
            {
                innerList.Add(item);
                item.TemplatedList = owner;
            }

            private void RemoveItem(TemplatedItem item)
            {
                item.TemplatedList = null;
                innerList.Remove(item);
            }

            public int IndexOf(TemplatedItem item)
            {
                return innerList.IndexOf(item);
            }

            public void Insert(int index, TemplatedItem item)
            {
                innerList.Insert(index, item);
                item.TemplatedList = owner;

                OnCollectionChanged(EventArgs.Empty);
            }

            public void RemoveAt(int index)
            {
                RemoveItem(this[index]);

                OnCollectionChanged(EventArgs.Empty);
            }

            public void Add(TemplatedItem item)
            {
                AddItem(item);

                OnCollectionChanged(EventArgs.Empty);
            }

            public void AddRange(IEnumerable<TemplatedItem> items)
            {
                foreach (TemplatedItem item in items)
                    AddItem(item);

                OnCollectionChanged(EventArgs.Empty);
            }

            public void Clear()
            {
                foreach (TemplatedItem xItem in innerList.ToArray())
                    RemoveItem(xItem);

                OnCollectionChanged(EventArgs.Empty);
            }

            public bool Contains(TemplatedItem item)
            {
                return innerList.Contains(item);
            }

            public void CopyTo(TemplatedItem[] array, int arrayIndex)
            {
                innerList.CopyTo(array, arrayIndex);
            }

            public bool Remove(TemplatedItem item)
            {
                RemoveItem(item);

                OnCollectionChanged(EventArgs.Empty);

                return true;
            }

            public IEnumerator<TemplatedItem> GetEnumerator()
            {
                return innerList.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return innerList.GetEnumerator();
            }

            public TemplatedItem[] ToArray()
            {
                return innerList.ToArray();
            }

            public override string ToString()
            {
                return String.Format("Count = {0}", Count);
            }

            protected virtual void OnCollectionChanged(EventArgs e)
            {
                if (CollectionChanged != null)
                    CollectionChanged(this, e);
            }

            #region IList and ICollection Members
            int IList.Add(object value)
            {
                Add(value as TemplatedItem);
                return IndexOf(value as TemplatedItem);
            }

            void IList.Clear()
            {
                Clear();
            }

            bool IList.Contains(object value)
            {
                return Contains(value as TemplatedItem);
            }

            int IList.IndexOf(object value)
            {
                return IndexOf(value as TemplatedItem);
            }

            void IList.Insert(int index, object value)
            {
                Insert(index, value as TemplatedItem);
            }

            bool IList.IsFixedSize
            {
                get { return false; }
            }

            bool IList.IsReadOnly
            {
                get { return false; }
            }

            void IList.Remove(object value)
            {
                Remove(value as TemplatedItem);
            }

            void IList.RemoveAt(int index)
            {
                RemoveAt(index);
            }

            object IList.this[int index]
            {
                get
                {
                    return this[index];
                }
                set
                {
                    this[index] = value as TemplatedItem;
                }
            }

            void ICollection.CopyTo(Array array, int index)
            {
                CopyTo(array as TemplatedItem[], index);
            }

            int ICollection.Count
            {
                get { return Count; }
            }

            bool ICollection.IsSynchronized
            {
                get { return false; }
            }

            object ICollection.SyncRoot
            {
                get { return null; }
            }
            #endregion
        }
    }
}
