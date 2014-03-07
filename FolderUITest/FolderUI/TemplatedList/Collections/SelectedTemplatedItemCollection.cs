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
        public class SelectedTemplatedItemCollection : IList<TemplatedItem>
        {
            private TemplatedList owner;

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

            public void Clear()
            {
                foreach (TemplatedItem xItem in this)
                    xItem.IsSelected = false;
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

            public IEnumerator<TemplatedItem> GetEnumerator()
            {
                foreach (TemplatedItem xItem in owner.Items)
                {
                    if (xItem.IsSelected)
                        yield return xItem;
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                foreach (TemplatedItem xItem in owner.Items)
                {
                    if (xItem.IsSelected)
                        yield return xItem;
                }
            }

            public SelectedTemplatedItemCollection(TemplatedList iOwner)
            {
                owner = iOwner;
            }

            #region Explicit Implementation
            int IList<TemplatedItem>.IndexOf(TemplatedItem item)
            {
                throw new NotSupportedException();
            }

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
