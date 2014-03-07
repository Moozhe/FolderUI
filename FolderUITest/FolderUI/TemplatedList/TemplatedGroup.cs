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
    [DesignTimeVisible(false)]
    [ToolboxItem(false)]
    public class TemplatedGroup : Component
    {
        #region Properties
        private TemplatedList _templatedList;
        [Browsable(false)]
        public TemplatedList TemplatedList
        {
            get { return _templatedList; }
            set { _templatedList = value; }
        }

        private string _name;
        [Browsable(false)]
        public string Name
        {
            get
            {
                if (base.Site != null)
                {
                    _name = base.Site.Name;
                }

                return _name;
            }
            set { _name = value; }
        }

        private int _headerHeight;
        [DefaultValue(0)]
        public virtual int HeaderHeight
        {
            get { return _headerHeight; }
            set { _headerHeight = value; }
        }

        private object _tag;
        [DefaultValue(null), TypeConverter(typeof(StringConverter))]
        public object Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        private TemplatedList.TemplatedGroupItemsCollection _items;
        public TemplatedList.TemplatedGroupItemsCollection Items
        {
            get { return _items; }
            private set { _items = value; }
        }

        private Rectangle _bounds;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Rectangle Bounds
        {
            get { return _bounds; }
            set { _bounds = value; }
        }

        private Rectangle _itemContainer;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Rectangle ItemContainer
        {
            get { return _itemContainer; }
            set { _itemContainer = value; }
        }

        private int _itemsPerRow;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int ItemsPerRow
        {
            get { return _itemsPerRow; }
            set { _itemsPerRow = value; }
        }

        private int _rowCount;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int RowCount
        {
            get { return _rowCount; }
            set { _rowCount = value; }
        }
        #endregion

        #region Constructor
        public TemplatedGroup()
        {
            Items = new TemplatedList.TemplatedGroupItemsCollection(this);
        }
        #endregion

        #region Methods
        public void Render(Graphics g)
        {
            OnRender(g);
        }

        protected virtual void OnRender(Graphics g)
        {
        }

        public override string ToString()
        {
            return Name;
        }
        #endregion
    }
}
