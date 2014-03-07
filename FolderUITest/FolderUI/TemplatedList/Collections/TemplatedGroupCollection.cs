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
        public class TemplatedGroupCollection : IList<TemplatedGroup>, ICollection<TemplatedGroup>, IEnumerable<TemplatedGroup>, IList, ICollection, IEnumerable
        {
            public event EventHandler CollectionChanged;

            private List<TemplatedGroup> innerList;
            private TemplatedList owner;

            public int Count
            {
                get { return innerList.Count; }
            }

            public bool IsReadOnly
            {
                get { return false; }
            }

            public TemplatedGroup this[int index]
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

            public TemplatedGroup this[string key]
            {
                get
                {
                    foreach (TemplatedGroup xGroup in this)
                    {
                        if (xGroup.Name == key)
                            return xGroup;
                    }

                    throw new ArgumentException("Key does not exist.");
                }
            }

            public TemplatedGroupCollection(TemplatedList iOwner)
            {
                owner = iOwner;

                innerList = new List<TemplatedGroup>();
            }

            private void AddItem(TemplatedGroup group)
            {
                innerList.Add(group);
                group.TemplatedList = owner;
            }

            private void RemoveItem(TemplatedGroup group)
            {
                group.TemplatedList = null;
                innerList.Remove(group);
            }

            public int IndexOf(TemplatedGroup group)
            {
                return innerList.IndexOf(group);
            }

            public void Insert(int index, TemplatedGroup group)
            {
                innerList.Insert(index, group);
                group.TemplatedList = owner;

                OnCollectionChanged(EventArgs.Empty);
            }

            public void RemoveAt(int index)
            {
                RemoveItem(this[index]);

                OnCollectionChanged(EventArgs.Empty);
            }

            public void Add(TemplatedGroup group)
            {
                AddItem(group);

                OnCollectionChanged(EventArgs.Empty);
            }

            public void AddRange(IEnumerable<TemplatedGroup> groups)
            {
                foreach (TemplatedGroup group in groups)
                    AddItem(group);

                OnCollectionChanged(EventArgs.Empty);
            }

            public void Clear()
            {
                foreach (TemplatedGroup xGroup in innerList.ToArray())
                    RemoveItem(xGroup);

                OnCollectionChanged(EventArgs.Empty);
            }

            public bool Contains(TemplatedGroup group)
            {
                return innerList.Contains(group);
            }

            public bool ContainsKey(string iKey)
            {
                foreach (TemplatedGroup xGroup in this)
                {
                    if (xGroup.Name == iKey)
                        return true;
                }

                return false;
            }

            public void CopyTo(TemplatedGroup[] array, int arrayIndex)
            {
                innerList.CopyTo(array, arrayIndex);
            }

            public bool Remove(TemplatedGroup group)
            {
                RemoveItem(group);

                OnCollectionChanged(EventArgs.Empty);

                return true;
            }

            public IEnumerator<TemplatedGroup> GetEnumerator()
            {
                return innerList.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return innerList.GetEnumerator();
            }

            public TemplatedGroup[] ToArray()
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
                Add(value as TemplatedGroup);
                return IndexOf(value as TemplatedGroup);
            }

            void IList.Clear()
            {
                Clear();
            }

            bool IList.Contains(object value)
            {
                return Contains(value as TemplatedGroup);
            }

            int IList.IndexOf(object value)
            {
                return IndexOf(value as TemplatedGroup);
            }

            void IList.Insert(int index, object value)
            {
                Insert(index, value as TemplatedGroup);
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
                Remove(value as TemplatedGroup);
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
                    this[index] = value as TemplatedGroup;
                }
            }

            void ICollection.CopyTo(Array array, int index)
            {
                CopyTo(array as TemplatedGroup[], index);
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
