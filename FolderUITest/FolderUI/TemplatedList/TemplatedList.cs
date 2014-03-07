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
using System.Diagnostics;

namespace FolderUI
{
    public partial class TemplatedList : ScrollableControl
    {
        #region Fields
        private VScrollBar verticalScroll;
        private bool isInitializing;
        private bool isDragging;
        private bool isDragAndDrop;
        private Point dragStart;
        private Point dragEnd;
        private Rectangle dragRectangle;
        #endregion

        #region Events
        public event EventHandler SelectedItemsChanged;
        #endregion

        #region User Properties
        private ContextMenuStrip _itemContextMenu;
        [Category("Behavior")]
        public ContextMenuStrip ItemContextMenu
        {
            get { return _itemContextMenu; }
            set { _itemContextMenu = value; }
        }

        [DefaultValue(typeof(Font), "Segoe UI, 9pt")]
        public override Font Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                base.Font = value;
            }
        }

        private TemplatedItemCollection _items;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TemplatedItemCollection Items
        {
            get { return _items; }
            private set { _items = value; }
        }

        private TemplatedGroupCollection _groups;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TemplatedGroupCollection Groups
        {
            get { return _groups; }
            private set { _groups = value; }
        }

        private bool _showGroups = true;
        [DefaultValue(true)]
        [Category("Appearance")]
        public bool ShowGroups
        {
            get { return _showGroups; }
            set { _showGroups = value; }
        }

        private Size _tileSize = new Size(130, 180);
        [DefaultValue(typeof(Size), "130, 180")]
        [Category("Appearance")]
        public Size TileSize
        {
            get { return _tileSize; }
            set { _tileSize = value; }
        }

        private Padding _padding = new Padding(10, 10, 10, 10);
        [DefaultValue(typeof(Padding), "10, 10, 10, 10")]
        [Category("Appearance")]
        public new Padding Padding
        {
            get { return _padding; }
            set { _padding = value; }
        }

        private Padding _groupMargins = new Padding(10, 10, 10, 10);
        [DefaultValue(typeof(Padding), "10, 10, 10, 10")]
        [Category("Appearance")]
        public Padding GroupMargins
        {
            get { return _groupMargins; }
            set { _groupMargins = value; }
        }

        private Padding _itemMargins = new Padding(5, 5, 5, 5);
        [DefaultValue(typeof(Padding), "5, 5, 5, 5")]
        [Category("Appearance")]
        public Padding ItemMargins
        {
            get { return _itemMargins; }
            set { _itemMargins = value; }
        }
        #endregion

        #region State Properties
        private SelectedTemplatedItemCollection _selectedItems;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SelectedTemplatedItemCollection SelectedItems
        {
            get { return _selectedItems; }
            private set { _selectedItems = value; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TemplatedItem SelectedItem
        {
            get
            {
                if (SelectedItems.Count == 1)
                    return SelectedItems[0];

                return null;
            }
            set
            {
                SelectedItems.Clear();

                if (value != null)
                    value.IsSelected = true;

                FocusedItem = value;
            }
        }

        private TemplatedItem _hoverItem;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        protected TemplatedItem HoverItem
        {
            get
            {
                return _hoverItem;
            }
            set
            {
                if (_hoverItem != value)
                {
                    if (_hoverItem != null)
                        _hoverItem.IsHovered = false;

                    _hoverItem = value;

                    if (_hoverItem != null)
                        _hoverItem.IsHovered = true;

                    Invalidate();
                }
            }
        }

        private TemplatedItem _focusedItem;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        protected TemplatedItem FocusedItem
        {
            get
            {
                return _focusedItem;
            }
            set
            {
                if (_focusedItem != value)
                {
                    if (_focusedItem != null)
                        _focusedItem.IsFocused = false;

                    _focusedItem = value;

                    if (_focusedItem != null)
                        _focusedItem.IsFocused = true;

                    Invalidate();
                }
            }
        }

        private int _zoomPercent = 100;
        [DefaultValue(100)]
        [Category("Appearance")]
        public int ZoomPercent
        {
            get { return _zoomPercent; }
            set
            {
                if (_zoomPercent != value)
                {
                    _zoomPercent = value;

                    UpdateLayout();
                }
            }
        }

        public new Rectangle ClientRectangle
        {
            get
            {
                return new Rectangle(new Point(0, 0), ClientSize);
            }
        }

        public new Size ClientSize
        {
            get
            {
                return new Size(base.ClientSize.Width - verticalScroll.Width, base.ClientSize.Height);
            }
            set
            {
                base.ClientSize = new Size(value.Width + verticalScroll.Width, value.Height);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Size ActualTileSize
        {
            get
            {
                float xRatio = ZoomPercent / 100f;

                return Size.Round(
                    new SizeF(
                        TileSize.Width * xRatio,
                        TileSize.Height * xRatio
                    )
                );
            }
        }

        protected bool CtrlDown
        {
            get
            {
                return ((System.Windows.Forms.Control.ModifierKeys & Keys.Control) == Keys.Control);
            }
        }

        protected bool AltDown
        {
            get
            {
                return ((System.Windows.Forms.Control.ModifierKeys & Keys.Alt) == Keys.Alt);
            }
        }

        protected bool ShiftDown
        {
            get
            {
                return ((System.Windows.Forms.Control.ModifierKeys & Keys.Shift) == Keys.Shift);
            }
        }
        #endregion

        #region Constructor
        public TemplatedList()
        {
            this.SetStyle(ControlStyles.DoubleBuffer |
                          ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.UserPaint |
                          ControlStyles.ResizeRedraw |
                          ControlStyles.Selectable, true);

            this.DoubleBuffered = true;
            this.AllowDrop = true;

            verticalScroll = new VScrollBar();
            verticalScroll.Scroll += new ScrollEventHandler(ScrollBar_Scroll);
            verticalScroll.Location = new Point(base.ClientSize.Width - verticalScroll.Width, 0);
            verticalScroll.Height = base.ClientSize.Height;
            verticalScroll.Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
            this.Controls.Add(verticalScroll);

            Items = new TemplatedItemCollection(this);
            Groups = new TemplatedGroupCollection(this);
            SelectedItems = new SelectedTemplatedItemCollection(this);

            Items.CollectionChanged += new EventHandler(Items_CollectionChanged);
            Groups.CollectionChanged += new EventHandler(Groups_CollectionChanged);
            
            Font = new Font("Segoe UI", 9f, GraphicsUnit.Point);
            Margin = new Padding(0, 0, 0, 0);

            BeginInit();
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            EndInit();
        }
        #endregion

        #region Methods
        public void ScrollItemIntoView(TemplatedItem iItem)
        {
            Rectangle xViewport = new Rectangle(ClientRectangle.X, ClientRectangle.Y + verticalScroll.Value, ClientRectangle.Width, ClientRectangle.Height);

            if (!xViewport.Contains(iItem.Bounds))
            {
                
            }
        }

        public void BeginInit()
        {
            isInitializing = true;
        }

        public void EndInit()
        {
            isInitializing = false;
            UpdateLayout();
        }

        public Point GetScrolledPoint(Point iPosition)
        {
            return new Point(iPosition.X, iPosition.Y + verticalScroll.Value);
        }

        public TemplatedItem FindNearestItem(TemplatedItem iItem, SearchDirectionHint iSearchDirection)
        {
            TemplatedItem xNearestItem = null;

            if (iItem != null)
            {
                TemplatedGroup xGroup = iItem.Group;
                int xItemsPerRow = xGroup.ItemsPerRow;
                int xIndex = xGroup.Items.IndexOf(iItem);
                int xColumn = xIndex % xItemsPerRow;
                int xRow = xIndex / xItemsPerRow;

                switch (iSearchDirection)
                {
                    case SearchDirectionHint.Up:
                        {
                            int xNewIndex = xIndex - xGroup.ItemsPerRow;

                            if (xNewIndex < 0)
                            {
                                if (Groups.IndexOf(xGroup) > 0)
                                {
                                    TemplatedGroup xUpperGroup = Groups[Groups.IndexOf(xGroup) - 1];

                                    int xItemCount = xUpperGroup.Items.Count;
                                    int xItemsInLastRow = xItemCount % xItemsPerRow;

                                    xNewIndex = xItemCount - xItemsInLastRow + xColumn;

                                    if (xNewIndex > xItemCount - 1)
                                        xNewIndex = xItemCount - 1;

                                    xNearestItem = xUpperGroup.Items[xNewIndex];
                                }
                            }
                            else
                            {
                                xNearestItem = xGroup.Items[xNewIndex];
                            }
                        }
                        break;

                    case SearchDirectionHint.Down:
                        {
                            int xNewIndex = xIndex + xItemsPerRow;

                            if (xNewIndex > xGroup.Items.Count - 1)
                            {
                                if (xRow < xGroup.RowCount - 1)
                                {
                                    SelectedItem = xGroup.Items[xGroup.Items.Count - 1];
                                }
                                else if (Groups.IndexOf(xGroup) < Groups.Count - 1)
                                {
                                    TemplatedGroup xLowerGroup = Groups[Groups.IndexOf(xGroup) + 1];
                                    int xItemCount = xLowerGroup.Items.Count;

                                    xNewIndex = xColumn;

                                    if (xNewIndex > xItemCount - 1)
                                        xNewIndex = xItemCount - 1;

                                    xNearestItem = xLowerGroup.Items[xNewIndex];
                                }
                            }
                            else
                            {
                                xNearestItem = xGroup.Items[xNewIndex];
                            }
                        }
                        break;

                    case SearchDirectionHint.Left:
                        {
                            int xNewIndex = xIndex - 1;

                            if (xNewIndex < 0)
                            {
                                if (Groups.IndexOf(xGroup) > 0)
                                {
                                    TemplatedGroup xUpperGroup = Groups[Groups.IndexOf(xGroup) - 1];

                                    xNearestItem = xUpperGroup.Items[xUpperGroup.Items.Count - 1];
                                }
                            }
                            else
                            {
                                xNearestItem = xGroup.Items[xNewIndex];
                            }
                        }
                        break;

                    case SearchDirectionHint.Right:
                        {
                            int xNewIndex = xIndex + 1;

                            if (xNewIndex > xGroup.Items.Count - 1)
                            {
                                if (Groups.IndexOf(xGroup) < Groups.Count - 1)
                                {
                                    TemplatedGroup xLowerGroup = Groups[Groups.IndexOf(xGroup) + 1];

                                    xNearestItem = xLowerGroup.Items[0];
                                }
                            }
                            else
                            {
                                xNearestItem = xGroup.Items[xNewIndex];
                            }
                        }
                        break;
                }
            }

            return xNearestItem;
        }

        public TemplatedItem GetItemAt(Point iPosition)
        {
            foreach (TemplatedItem xItem in Items)
            {
                if (xItem.Bounds.Contains(iPosition))
                    return xItem;
            }

            return null;
        }

        public TemplatedItem[] GetItemsWithin(Rectangle iArea)
        {
            List<TemplatedItem> xItems = new List<TemplatedItem>();

            foreach (TemplatedItem xItem in Items)
            {
                if (iArea.IntersectsWith(xItem.Bounds))
                {
                    xItems.Add(xItem);
                }
            }

            return xItems.ToArray();
        }

        public virtual void OnSelectedItemsChanged(EventArgs e)
        {
            if (SelectedItemsChanged != null)
                SelectedItemsChanged(this, e);

            Invalidate();
        }

        public virtual void UpdateLayout()
        {
            if (isInitializing || DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            Rectangle xLastGroupBounds = Rectangle.Empty;
            int xTotalHeight = Padding.Vertical;

            foreach (TemplatedGroup xGroup in Groups)
            {
                if (xGroup.Items.Count > 0)
                {
                    int xGroupWidth = ClientSize.Width - GroupMargins.Horizontal - Padding.Horizontal;
                    xGroup.ItemsPerRow = (int)Math.Max(1, xGroupWidth / (ActualTileSize.Width + ItemMargins.Horizontal));
                    xGroup.RowCount = xGroup.Items.Count / xGroup.ItemsPerRow;

                    if (xGroup.Items.Count > xGroup.ItemsPerRow * xGroup.RowCount)
                        xGroup.RowCount++;

                    int xGroupHeight = xGroup.HeaderHeight + (xGroup.RowCount * (ActualTileSize.Height + ItemMargins.Vertical));

                    xGroup.Bounds = new Rectangle(
                        Padding.Left + GroupMargins.Left,
                        Padding.Top + GroupMargins.Top + xLastGroupBounds.Bottom,
                        (int)Math.Max(xGroupWidth, ActualTileSize.Width + ItemMargins.Horizontal),
                        xGroupHeight
                    );

                    xLastGroupBounds = xGroup.Bounds;
                    xTotalHeight += xGroup.Bounds.Height + GroupMargins.Vertical;

                    xGroup.ItemContainer = Rectangle.FromLTRB(
                        xGroup.Bounds.Left,
                        xGroup.Bounds.Top + xGroup.HeaderHeight,
                        xGroup.Bounds.Right,
                        xGroup.Bounds.Bottom
                    );

                    for (int i = 0; i < xGroup.Items.Count; i++)
                    {
                        TemplatedItem xItem = xGroup.Items[i];

                        int xLeftIndex = i % xGroup.ItemsPerRow;
                        int xItemLeft = xGroup.ItemContainer.Left + ItemMargins.Left + ((ActualTileSize.Width + ItemMargins.Horizontal) * xLeftIndex);

                        int xTopIndex = i / xGroup.ItemsPerRow;
                        int xItemTop = xGroup.ItemContainer.Top + ItemMargins.Top + ((ActualTileSize.Height + ItemMargins.Vertical) * xTopIndex);

                        xItem.Bounds = new Rectangle(
                            xItemLeft,
                            xItemTop,
                            ActualTileSize.Width,
                            ActualTileSize.Height
                        );
                    }
                }
            }

            verticalScroll.Maximum = xTotalHeight;
            verticalScroll.LargeChange = ClientSize.Height;
            if (verticalScroll.Value > verticalScroll.Maximum - verticalScroll.LargeChange)
                verticalScroll.Value = verticalScroll.Maximum - verticalScroll.LargeChange + 1;

            verticalScroll.Enabled = verticalScroll.Maximum > verticalScroll.LargeChange;

            Invalidate();
        }
        #endregion

        #region Drawing
        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.TranslateTransform(0f, -verticalScroll.Value);

            g.Clear(Color.White);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            foreach (TemplatedGroup xGroup in Groups)
                xGroup.Render(g);

            foreach (TemplatedItem xItem in Items)
                xItem.Render(g);

            if (isDragging)
            {
                using (Pen xDragPen = new Pen(Color.FromArgb(196, Color.Blue), 1.0f))
                using (SolidBrush xDragBrush = new SolidBrush(Color.FromArgb(64, Color.Blue)))
                {
                    g.FillRectangle(xDragBrush, dragRectangle);
                    g.DrawRectangle(xDragPen, dragRectangle);
                }
            }

            #region Scrolling Fade
            //g.TranslateTransform(0f, verticalScroll.Value);

            //int xTopFadeLength = Padding.Top + GroupMargins.Top;
            //int xBottomFadeLength = Padding.Bottom + GroupMargins.Bottom;

            //Rectangle xTopFadeRect = new Rectangle(0, 0, ClientSize.Width, xTopFadeLength);
            //Rectangle xBottomFadeRect = new Rectangle(0, ClientSize.Height - xBottomFadeLength, ClientSize.Width, xBottomFadeLength);

            //using (LinearGradientBrush xFadeBrush = new LinearGradientBrush(xTopFadeRect, Color.White, Color.Transparent, LinearGradientMode.Vertical))
            //{
            //    g.FillRectangle(xFadeBrush, xTopFadeRect);
            //}

            //using (LinearGradientBrush xFadeBrush = new LinearGradientBrush(xBottomFadeRect, Color.Transparent, Color.White, LinearGradientMode.Vertical))
            //{
            //    g.FillRectangle(xFadeBrush, xBottomFadeRect);
            //}
            #endregion
        }
        #endregion

        #region Event Handling
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            Point xScrolledLocation = GetScrolledPoint(e.Location);

            Focus();

            TemplatedItem xItemAtLocation = GetItemAt(xScrolledLocation);

            if (xItemAtLocation != null)
            {
                if (CtrlDown || ShiftDown)
                    xItemAtLocation.IsSelected = !xItemAtLocation.IsSelected;
                else
                    SelectedItem = xItemAtLocation;
            }
            else
            {
                if (!CtrlDown && !ShiftDown)
                    SelectedItems.Clear();

                isDragging = true;
                dragStart = xScrolledLocation;
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (isDragging)
            {
                isDragging = false;
                dragStart = Point.Empty;
                dragEnd = Point.Empty;
                dragRectangle = Rectangle.Empty;
                Invalidate();
            }

            if (e.Button == MouseButtons.Right)
            {
                if (ContextMenuStrip != null)
                    ContextMenuStrip.Show(this, e.Location);
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            Point xScrolledPoint = GetScrolledPoint(e.Location);

            if (isDragging)
            {
                dragEnd = xScrolledPoint;

                dragRectangle = Rectangle.FromLTRB(
                    Math.Min(dragStart.X, dragEnd.X),
                    Math.Min(dragStart.Y, dragEnd.Y),
                    Math.Max(dragStart.X, dragEnd.X),
                    Math.Max(dragStart.Y, dragEnd.Y)
                );

                TemplatedItem[] xItemsWithin = GetItemsWithin(dragRectangle);

                if (!CtrlDown && !ShiftDown)
                    SelectedItems.Clear();

                TemplatedItem xLastItemSelected = null;

                foreach (TemplatedItem xItem in xItemsWithin)
                {
                    if (!xItem.IsSelected)
                    {
                        xItem.IsSelected = true;
                        xLastItemSelected = xItem;
                    }
                }

                if (xLastItemSelected != null)
                    FocusedItem = xItemsWithin[xItemsWithin.Length - 1];

                Invalidate();
            }

            HoverItem = GetItemAt(xScrolledPoint);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (verticalScroll.Enabled)
            {
                int xOldValue = verticalScroll.Value;

                if (e.Delta > 0)
                {
                    verticalScroll.Value = (int)Math.Max(verticalScroll.Value - (verticalScroll.SmallChange * e.Delta), 0);
                }
                else
                {
                    verticalScroll.Value = (int)Math.Min(verticalScroll.Value - (verticalScroll.SmallChange * e.Delta), verticalScroll.Maximum - (verticalScroll.LargeChange - 1));
                }

                OnScroll(new ScrollEventArgs(ScrollEventType.ThumbPosition, xOldValue, verticalScroll.Value, ScrollOrientation.VerticalScroll));
            }
        }

        protected override void OnScroll(ScrollEventArgs se)
        {
            base.OnScroll(se);

            Invalidate();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            SearchDirectionHint xDirection;

            switch (e.KeyData)
            {
                case Keys.Up:
                    xDirection = SearchDirectionHint.Up;
                    break;

                case Keys.Down:
                    xDirection = SearchDirectionHint.Down;
                    break;

                case Keys.Left:
                    xDirection = SearchDirectionHint.Left;
                    break;

                case Keys.Right:
                    xDirection = SearchDirectionHint.Right;
                    break;

                default:
                    return;
            }

            TemplatedItem xNearestItem = FindNearestItem(FocusedItem, xDirection);

            if (xNearestItem != null)
                SelectedItem = xNearestItem;
            else
                SelectedItem = FocusedItem;
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
        }

        protected override bool IsInputKey(Keys keyData)
        {
            if (keyData == Keys.Up || keyData == Keys.Down || keyData == Keys.Left || keyData == Keys.Right)
            {
                return true;
            }

            return base.IsInputKey(keyData);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
        }

        protected override void OnClientSizeChanged(EventArgs e)
        {
            base.OnClientSizeChanged(e);

            UpdateLayout();
        }

        private void ScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            OnScroll(e);
        }

        private void Items_CollectionChanged(object sender, EventArgs e)
        {
            UpdateLayout();
        }

        private void Groups_CollectionChanged(object sender, EventArgs e)
        {
            UpdateLayout();
        }
        #endregion

        public override string ToString()
        {
            return String.Format("{0} (Count = {1})", Name, Items.Count);
        }
    }
}
