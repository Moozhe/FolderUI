using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace FolderUI
{
    public class ObservableList<T> : List<T>
    {
        public event EventHandler CollectionChanged;

        public new void Insert(int index, T item)
        {
            base.Insert(index, item);

            OnCollectionChanged(EventArgs.Empty);
        }

        public new void RemoveAt(int index)
        {
            base.RemoveAt(index);

            OnCollectionChanged(EventArgs.Empty);
        }

        public new T this[int index]
        {
            get
            {
                return base[index];
            }
            set
            {
                base[index] = value;

                OnCollectionChanged(EventArgs.Empty);
            }
        }

        public new void Add(T item)
        {
            base.Add(item);

            OnCollectionChanged(EventArgs.Empty);
        }

        public new void Clear()
        {
            base.Clear();

            OnCollectionChanged(EventArgs.Empty);
        }

        public new bool Remove(T item)
        {
            if (base.Remove(item))
            {
                OnCollectionChanged(EventArgs.Empty);
                return true;
            }

            return false;
        }

        protected virtual void OnCollectionChanged(EventArgs e)
        {
            if (CollectionChanged != null)
                CollectionChanged(this, e);
        }
    }
}
