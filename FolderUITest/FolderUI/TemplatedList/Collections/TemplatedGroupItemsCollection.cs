using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace FolderUI
{
    public partial class TemplatedList
    {
        public class TemplatedGroupItemsCollection : IList<TemplatedItem>
        {
            private TemplatedGroup owner;

            public TemplatedItem this[int index]
            {
                get
                {
                    List<TemplatedItem> xItems = new List<TemplatedItem>();

                    foreach (TemplatedItem xItem in this)
                        xItems.Add(xItem);

                    return xItems[index];
                }
                set
                {
                    throw new NotSupportedException();
                }
            }

            public int Count
            {
                get
                {
                    int count = 0;

                    foreach (TemplatedItem xItem in this)
                        count++;

                    return count;
                }
            }

            public bool IsReadOnly
            {
                get { return true; }
            }

            public TemplatedGroupItemsCollection(TemplatedGroup iOwner)
            {
                owner = iOwner;
            }

            public IEnumerator<TemplatedItem> GetEnumerator()
            {
                if (owner.TemplatedList != null)
                {
                    foreach (TemplatedItem xItem in owner.TemplatedList.Items)
                    {
                        if (xItem.Group == owner)
                            yield return xItem;
                    }
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                if (owner.TemplatedList != null)
                {
                    foreach (TemplatedItem xItem in owner.TemplatedList.Items)
                    {
                        if (xItem.Group == owner)
                            yield return xItem;
                    }
                }
            }

            public bool Contains(TemplatedItem item)
            {
                foreach (TemplatedItem xItem in this)
                {
                    if (xItem == item)
                        return true;
                }

                return false;
            }

            public int IndexOf(TemplatedItem item)
            {
                List<TemplatedItem> xItems = new List<TemplatedItem>();

                foreach (TemplatedItem xItem in this)
                    xItems.Add(xItem);

                return xItems.IndexOf(item);
            }

            public override string ToString()
            {
                return String.Format("Count = {0}", Count);
            }

            #region Explicit Implementation
            void IList<TemplatedItem>.Insert(int index, TemplatedItem item)
            {
                throw new NotSupportedException();
            }

            void IList<TemplatedItem>.RemoveAt(int index)
            {
                throw new NotSupportedException();
            }

            void ICollection<TemplatedItem>.Add(TemplatedItem item)
            {
                throw new NotSupportedException();
            }

            void ICollection<TemplatedItem>.Clear()
            {
                throw new NotSupportedException();
            }

            void ICollection<TemplatedItem>.CopyTo(TemplatedItem[] array, int arrayIndex)
            {
                throw new NotSupportedException();
            }

            bool ICollection<TemplatedItem>.Remove(TemplatedItem item)
            {
                throw new NotSupportedException();
            }
            #endregion
        }
    }
}
