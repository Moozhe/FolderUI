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
using System.Runtime.InteropServices;

namespace FolderUI
{
    [DesignTimeVisible(false)]
    [ToolboxItem(false)]
    public class TemplatedItem : Component
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

        private TemplatedGroup _group;
        [Browsable(true)]
        public TemplatedGroup Group
        {
            get { return _group; }
            set { _group = value; }
        }

        private object _tag;
        [DefaultValue(null), TypeConverter(typeof(StringConverter))]
        public object Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        private ContextMenuStrip _contextMenu;
        [Category("Behavior")]
        public ContextMenuStrip ContextMenu
        {
            get { return _contextMenu; }
            set { _contextMenu = value; }
        }
        #endregion

        #region State Properties
        private bool _isSelected;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;

                    if (TemplatedList != null)
                        TemplatedList.OnSelectedItemsChanged(EventArgs.Empty);
                }
            }
        }

        private bool _isFocused;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsFocused
        {
            get { return _isFocused; }
            set
            {
                _isFocused = value;
            }
        }

        private bool _isHovered;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsHovered
        {
            get { return _isHovered; }
            set
            {
                _isHovered = value;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Index
        {
            get
            {
                if (TemplatedList != null)
                    return TemplatedList.Items.IndexOf(this);
                else
                    return -1;
            }
        }

        private Rectangle _bounds;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Rectangle Bounds
        {
            get { return _bounds; }
            set { _bounds = value; }
        }
        #endregion

        #region Constructor
        public TemplatedItem()
        {
        }

        public TemplatedItem(string iKey, object iTag, TemplatedGroup iGroup)
            : this()
        {
            Name = iKey;
            Tag = iTag;
            Group = iGroup;
        }
        #endregion

        #region Drawing
        public void Render(Graphics g)
        {
            OnRender(g);
        }

        protected virtual void OnRender(Graphics g)
        {
            using (GraphicsPath xPath = new GraphicsPath())
            {
                RectangleF xRect = Bounds;
                float xRadius = 5f;
                float xDiameter = xRadius * 2;

                xPath.AddLine(new PointF(xRect.Left + xRadius, xRect.Top), new PointF(xRect.Right - xRadius, xRect.Top));
                xPath.AddArc(new RectangleF(xRect.Right - xDiameter, xRect.Top, xDiameter, xDiameter), 270, 90);
                xPath.AddLine(new PointF(xRect.Right, xRect.Top + xRadius), new PointF(xRect.Right, xRect.Bottom - xRadius));
                xPath.AddArc(new RectangleF(xRect.Right - xDiameter, xRect.Bottom - xDiameter, xDiameter, xDiameter), 0, 90);
                xPath.AddLine(new PointF(xRect.Right - xRadius, xRect.Bottom), new PointF(xRect.Left + xRadius, xRect.Bottom));
                xPath.AddArc(new RectangleF(xRect.Left, xRect.Bottom - xDiameter, xDiameter, xDiameter), 90, 90);
                xPath.AddLine(new PointF(xRect.Left, xRect.Bottom - xRadius), new PointF(xRect.Left, xRect.Top + xRadius));
                xPath.AddArc(new RectangleF(xRect.Left, xRect.Top, xDiameter, xDiameter), 180, 90);

                Color xBorderColor = Color.Empty;
                Color xFillColor = Color.Empty;

                if (IsSelected || IsFocused)
                    xBorderColor = Color.FromArgb(64, Color.Blue);
                else if (IsHovered)
                    xBorderColor = Color.FromArgb(32, Color.Blue);

                if (IsSelected)
                    xFillColor = Color.FromArgb(32, Color.Blue);
                else if (IsHovered)
                    xFillColor = Color.FromArgb(16, Color.Blue);

                if (!xBorderColor.IsEmpty)
                {
                    using (Pen xPen = new Pen(xBorderColor, 1.0f))
                        g.DrawPath(xPen, xPath);
                }

                if (!xFillColor.IsEmpty)
                {
                    using (SolidBrush xBrush = new SolidBrush(xFillColor))
                        g.FillPath(xBrush, xPath);
                }
            }
        }
        #endregion

        #region Methods
        public void ScrollItemIntoView()
        {
            if (TemplatedList != null)
                TemplatedList.ScrollItemIntoView(this);
        }

        public TemplatedItem FindNearestItem(SearchDirectionHint iSearchDirection)
        {
            if (TemplatedList != null)
                return TemplatedList.FindNearestItem(this, iSearchDirection);
            else
                return null;
        }

        public override string ToString()
        {
            return Name;
        }
        #endregion

        #region Context Menu
        public void ShowContextMenu(Point iPosition)
        {
            if (TemplatedList != null)
            {
                ContextMenu.Tag = Tag;
                ContextMenu.Show(TemplatedList, iPosition);
            }
        }
        #endregion
    }
}
