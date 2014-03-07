using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace FolderUI
{
    public class FolderTabs : UserControl
    {
        #region Static
        private static ColorBlend BackgroundBlend;

        static FolderTabs()
        {
            BackgroundBlend = new ColorBlend(4);
            BackgroundBlend.Colors = new Color[] { Color.Empty, Color.Empty, Color.Empty, Color.Empty };
            BackgroundBlend.Positions = new float[] { 0f, 0.12f, 0.80f, 1f };
        }
        #endregion

        public event EventHandler SelectedItemChanged;

        private const float curveWidth = 17f;

        #region Appearance
        private Color _gradientTop = Color.White;
        [Category("Appearance")]
        [DefaultValue(typeof(Color), "White")]
        public Color GradientTop
        {
            get { return _gradientTop; }
            set { _gradientTop = value; }
        }

        private Color _gradientBottom = Color.FromArgb(224, 225, 227);
        [Category("Appearance")]
        [DefaultValue(typeof(Color), "224, 225, 227")]
        public Color GradientBottom
        {
            get { return _gradientBottom; }
            set { _gradientBottom = value; }
        }

        private Color _inactiveTabColor = Color.FromArgb(199, 199, 199);
        [Category("Appearance")]
        [DefaultValue(typeof(Color), "199, 199, 199")]
        public Color InactiveTabColor
        {
            get { return _inactiveTabColor; }
            set { _inactiveTabColor = value; }
        }

        private Color _hoverTabColor = Color.FromArgb(209, 209, 209);
        [Category("Appearance")]
        [DefaultValue(typeof(Color), "209, 209, 209")]
        public Color HoverTabColor
        {
            get { return _hoverTabColor; }
            set { _hoverTabColor = value; }
        }

        private Color _tabColor = Color.FromArgb(2, 187, 254);
        [Category("Appearance")]
        [DefaultValue(typeof(Color), "2, 187, 254")]
        public Color TabColor
        {
            get { return _tabColor; }
            set { _tabColor = value; }
        }

        private int _minTabWidth = 80;
        [Category("Appearance")]
        [DefaultValue(80)]
        public int MinTabWidth
        {
            get { return _minTabWidth; }
            set { _minTabWidth = value; }
        }

        private int _maxTabWidth = 134;
        [Category("Appearance")]
        [DefaultValue(134)]
        public int MaxTabWidth
        {
            get { return _maxTabWidth; }
            set { _maxTabWidth = value; }
        }

        private Padding _folderPadding = new Padding(25, 14, 10, 0);
        [Category("Appearance")]
        [DefaultValue(typeof(Padding), "25, 14, 10, 0")]
        public Padding FolderPadding
        {
            get { return _folderPadding; }
            set { _folderPadding = value; }
        }

        private int _tabSpacing = 10;
        [Category("Appearance")]
        [DefaultValue(10)]
        public int TabSpacing
        {
            get { return _tabSpacing; }
            set { _tabSpacing = value; }
        }

        private Color _barColor = Color.FromArgb(2, 187, 254);
        [Category("Appearance")]
        [DefaultValue(typeof(Color), "2, 187, 254")]
        public Color BarColor
        {
            get { return _barColor; }
            set { _barColor = value; }
        }

        private int _barHeight = 6;
        [Category("Appearance")]
        [DefaultValue(6)]
        public int BarHeight
        {
            get { return _barHeight; }
            set { _barHeight = value; }
        }

        private Color _textColor = Color.White;
        [Category("Appearance")]
        [DefaultValue(typeof(Color), "White")]
        public Color TextColor
        {
            get { return _textColor; }
            set { _textColor = value; }
        }

        private Color _inactiveTextColor = Color.Black;
        [Category("Appearance")]
        [DefaultValue(typeof(Color), "Black")]
        public Color InactiveTextColor
        {
            get { return _inactiveTextColor; }
            set { _inactiveTextColor = value; }
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
        #endregion

        private ObservableList<FolderTabItem> _items = new ObservableList<FolderTabItem>();
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ObservableList<FolderTabItem> Items
        {
            get { return _items; }
        }

        public FolderTabItem this[string name]
        {
            get
            {
                foreach (FolderTabItem xItem in Items)
                    if (xItem.Name == name)
                        return xItem;

                return null;
            }
        }

        private FolderTabItem _selectedItem;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public FolderTabItem SelectedItem
        {
            get
            {
                if (_selectedItem == null && Items.Count > 0)
                    return Items[0];
                else
                    return _selectedItem;
            }
            set
            {
                if (_selectedItem != value)
                {
                    _selectedItem = value;

                    Invalidate();

                    if (SelectedItemChanged != null)
                        SelectedItemChanged(this, EventArgs.Empty);
                }
            }
        }

        private FolderTabItem _hoverItem;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public FolderTabItem HoverItem
        {
            get { return _hoverItem; }
            set
            {
                if (_hoverItem != value)
                {
                    _hoverItem = value;

                    Invalidate();
                }
            }
        }

        public FolderTabs()
        {
            this.SetStyle(ControlStyles.DoubleBuffer |
                          ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.UserPaint |
                          ControlStyles.ResizeRedraw, true);

            this.DoubleBuffered = true;

            Font = new Font("Segoe UI", 9f, GraphicsUnit.Point);
            Margin = new Padding(0, 0, 0, 0);

            Items.CollectionChanged += new EventHandler(Items_CollectionChanged);
        }

        private void UpdateLayout()
        {
            if (Items.Count == 0)
                return;

            int xTabWidth = Math.Min(MaxTabWidth, Math.Max(MinTabWidth, (ClientSize.Width - FolderPadding.Horizontal - (Items.Count * TabSpacing)) / Items.Count));

            for (int i = 0; i < Items.Count; i++)
            {
                FolderTabItem xItem = Items[i];

                xItem.Path.Reset();

                Rectangle xItemBounds = new Rectangle(
                    FolderPadding.Left + (i * (xTabWidth + TabSpacing)),
                    FolderPadding.Top,
                    xTabWidth,
                    ClientSize.Height - BarHeight - FolderPadding.Top
                );

                PointF xLastPoint;

                PointF xLeftStartPoint = new PointF(xItemBounds.Left, xItemBounds.Bottom);
                PointF xLeftEndPoint = new PointF(xItemBounds.Left + curveWidth, xItemBounds.Top);

                PointF xRightStartPoint = new PointF(xItemBounds.Right - curveWidth, xItemBounds.Top);
                PointF xRightEndPoint = new PointF(xItemBounds.Right, xItemBounds.Bottom);

                xItem.Path.AddBezier(
                    xLeftStartPoint,
                    xLeftStartPoint,
                    new PointF(xLeftStartPoint.X + 3f, xLeftStartPoint.Y),
                    xLastPoint = new PointF(xLeftStartPoint.X + 5f, xLeftStartPoint.Y - 4f)
                );

                xItem.Path.AddLine(
                    xLastPoint,
                    xLastPoint = new PointF(xLeftEndPoint.X - 5f, xLeftEndPoint.Y + 4f)
                );

                xItem.Path.AddBezier(
                    xLastPoint,
                    xLastPoint,
                    new PointF(xLastPoint.X + 1f, xLastPoint.Y - 4f),
                    new PointF(xLeftEndPoint.X, xLeftEndPoint.Y)
                );

                xItem.Path.AddLine(xLeftEndPoint, xRightStartPoint);

                xLastPoint = new PointF(xRightStartPoint.X + 5f, xRightStartPoint.Y + 4f);

                xItem.Path.AddBezier(
                    xRightStartPoint,
                    new PointF(xLastPoint.X - 1f, xLastPoint.Y - 4f),
                    xLastPoint,
                    xLastPoint
                );

                xItem.Path.AddLine(
                    xLastPoint,
                    xLastPoint = new PointF(xRightEndPoint.X - 5f, xRightEndPoint.Y - 4f)
                );

                xItem.Path.AddBezier(
                    xLastPoint,
                    new PointF(xRightEndPoint.X - 3f, xRightEndPoint.Y),
                    xRightEndPoint,
                    xRightEndPoint
                );
            }

        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            #region Background
            BackgroundBlend.Colors[0] = GradientTop;
            BackgroundBlend.Colors[1] = GradientTop;
            BackgroundBlend.Colors[2] = GradientBottom;
            BackgroundBlend.Colors[3] = GradientBottom;

            Rectangle xBackgroundRect = new Rectangle(0, 0, ClientSize.Width, ClientSize.Height - BarHeight);

            using (LinearGradientBrush xBackgroundBrush = new LinearGradientBrush(xBackgroundRect, GradientTop, GradientBottom, LinearGradientMode.Vertical))
            {
                xBackgroundBrush.InterpolationColors = BackgroundBlend;

                g.FillRectangle(xBackgroundBrush, xBackgroundRect);
            }
            #endregion

            #region Items
            g.SmoothingMode = SmoothingMode.AntiAlias;

            using (SolidBrush xInactiveBrush = new SolidBrush(InactiveTabColor))
            using (Pen xInactivePen = new Pen(InactiveTabColor, 1.0f))
            using (SolidBrush xHoverBrush = new SolidBrush(HoverTabColor))
            using (Pen xHoverPen = new Pen(HoverTabColor, 1.0f))
            using (SolidBrush xActiveBrush = new SolidBrush(TabColor))
            using (Pen xActivePen = new Pen(TabColor, 1.0f))
            using (SolidBrush xInactiveTextBrush = new SolidBrush(InactiveTextColor))
            using (SolidBrush xActiveTextBrush = new SolidBrush(TextColor))
            using (StringFormat xStringFormat = new StringFormat(StringFormatFlags.LineLimit | StringFormatFlags.NoClip))
            {
                xStringFormat.Trimming = StringTrimming.EllipsisCharacter;
                xStringFormat.Alignment = StringAlignment.Center;
                xStringFormat.LineAlignment = StringAlignment.Center;

                foreach (FolderTabItem xItem in Items)
                {
                    Brush xBackgroundBrush;
                    Pen xBackgroundPen;

                    if (xItem == SelectedItem)
                    {
                        xBackgroundBrush = xActiveBrush;
                        xBackgroundPen = xActivePen;
                    }
                    else if (xItem == HoverItem)
                    {
                        xBackgroundBrush = xHoverBrush;
                        xBackgroundPen = xHoverPen;
                    }
                    else
                    {
                        xBackgroundBrush = xInactiveBrush;
                        xBackgroundPen = xInactivePen;
                    }

                    g.FillPath(xBackgroundBrush, xItem.Path);
                    g.DrawPath(xBackgroundPen, xItem.Path);

                    if (!String.IsNullOrEmpty(xItem.Text))
                    {
                        RectangleF xPathBounds = xItem.Path.GetBounds();
                        RectangleF xTextBounds = RectangleF.FromLTRB(
                            xPathBounds.Left + curveWidth,
                            xPathBounds.Top,
                            xPathBounds.Right - curveWidth,
                            xPathBounds.Bottom
                        );

                        g.DrawString(xItem.Text, Font, xItem == SelectedItem ? xActiveTextBrush : xInactiveTextBrush, xTextBounds, xStringFormat);
                    }
                }
            }
            #endregion

            #region Bar
            Rectangle xBarRect = new Rectangle(0, ClientSize.Height - BarHeight, ClientSize.Width, BarHeight);
            using (SolidBrush xBarBrush = new SolidBrush(BarColor))
            using (Pen xBarPen = new Pen(BarColor, 1.0f))
            {
                g.FillRectangle(xBarBrush, xBarRect);
                g.DrawRectangle(xBarPen, new Rectangle(xBarRect.X, xBarRect.Y, xBarRect.Width - 1, xBarRect.Height - 1));
            }
            #endregion
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            foreach (FolderTabItem xItem in Items)
            {
                if (xItem.Path.IsVisible(e.Location))
                {
                    SelectedItem = xItem;
                    break;
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            bool found = false;

            foreach (FolderTabItem xItem in Items)
            {
                if (xItem.Path.IsVisible(e.Location))
                {
                    HoverItem = xItem;
                    found = true;
                }
            }

            if (!found)
                HoverItem = null;
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            HoverItem = null;
        }

        protected override void OnClientSizeChanged(EventArgs e)
        {
            base.OnClientSizeChanged(e);

            UpdateLayout();
        }

        private void Items_CollectionChanged(object sender, EventArgs e)
        {
            UpdateLayout();
            Invalidate();

            if (Items.Count == 1)
                SelectedItem = Items[0];
        }
    }
}
