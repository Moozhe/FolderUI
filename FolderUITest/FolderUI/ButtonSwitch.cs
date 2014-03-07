using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace FolderUI
{
    public class ButtonSwitch : UserControl
    {
        public event EventHandler SelectedItemChanged;

        #region Appearance
        private Color _selGradientTop = Color.FromArgb(117, 240, 255);
        [Category("Appearance")]
        [DefaultValue(typeof(Color), "117, 240, 255")]
        public Color SelectedGradientTop
        {
            get { return _selGradientTop; }
            set { _selGradientTop = value; }
        }

        private Color _selGradientBottom = Color.FromArgb(1, 187, 254);
        [Category("Appearance")]
        [DefaultValue(typeof(Color), "1, 187, 254")]
        public Color SelectedGradientBottom
        {
            get { return _selGradientBottom; }
            set { _selGradientBottom = value; }
        }

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

        private Color _hoverGradientTop = Color.WhiteSmoke; // 245, 245, 245
        [Category("Appearance")]
        [DefaultValue(typeof(Color), "WhiteSmoke")]
        public Color HoverGradientTop
        {
            get { return _hoverGradientTop; }
            set { _hoverGradientTop = value; }
        }

        private Color _hoverGradientBottom = Color.FromArgb(214, 215, 217);
        [Category("Appearance")]
        [DefaultValue(typeof(Color), "214, 215, 217")]
        public Color HoverGradientBottom
        {
            get { return _hoverGradientBottom; }
            set { _hoverGradientBottom = value; }
        }

        private Color _textColor = Color.FromArgb(67, 121, 147);
        [Category("Appearance")]
        [DefaultValue(typeof(Color), "67, 121, 147")]
        public Color TextColor
        {
            get { return _textColor; }
            set { _textColor = value; }
        }

        private Color _selTextColor = Color.White;
        [Category("Appearance")]
        [DefaultValue(typeof(Color), "White")]
        public Color SelectedTextColor
        {
            get { return _selTextColor; }
            set { _selTextColor = value; }
        }

        private float _textIconPadding = 2f;
        [Category("Appearance")]
        [DefaultValue(2f)]
        public float TextIconPadding
        {
            get { return _textIconPadding; }
            set { _textIconPadding = value; }
        }

        private Color _barColor = Color.FromArgb(2, 187, 254);
        [Category("Appearance")]
        [DefaultValue(typeof(Color), "2, 187, 254")]
        public Color BarColor
        {
            get { return _barColor; }
            set { _barColor = value; }
        }

        private int _barSize = 6;
        [Category("Appearance")]
        [DefaultValue(6)]
        public int BarSize
        {
            get { return _barSize; }
            set { _barSize = value; }
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

        private Orientation _orientation = Orientation.Horizontal;
        [Category("Appearance")]
        [DefaultValue(Orientation.Horizontal)]
        public Orientation Orientation
        {
            get { return _orientation; }
            set { _orientation = value; ToolStripButton b; }
        }

        private ToolStripItemDisplayStyle _displayStyle = ToolStripItemDisplayStyle.ImageAndText;
        [Category("Appearance")]
        [DefaultValue(ToolStripItemDisplayStyle.ImageAndText)]
        public ToolStripItemDisplayStyle DisplayStyle
        {
            get { return _displayStyle; }
            set { _displayStyle = value; }
        }

        private ObservableList<ButtonSwitchItem> _items = new ObservableList<ButtonSwitchItem>();
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ObservableList<ButtonSwitchItem> Items
        {
            get { return _items; }
        }
        #endregion

        public ButtonSwitchItem this[string name]
        {
            get
            {
                foreach (ButtonSwitchItem xItem in Items)
                    if (xItem.Name == name)
                        return xItem;

                return null;
            }
        }

        private ButtonSwitchItem _selectedItem;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ButtonSwitchItem SelectedItem
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

        private ButtonSwitchItem _hoverItem;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ButtonSwitchItem HoverItem
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

        private int ItemSize
        {
            get
            {
                if (IsHorizontal)
                    return ClientSize.Width / Items.Count;
                else
                    return ClientSize.Height / Items.Count;
            }
        }

        private bool IsHorizontal
        {
            get
            {
                return Orientation == Orientation.Horizontal;
            }
        }

        public ButtonSwitch()
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

        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            ColorBlend xBackgroundBlend = new ColorBlend(4);
            xBackgroundBlend.Positions = new float[]
            {
                0f,
                0.12f,
                0.80f,
                1f
            };

            if (Items.Count > 0)
            {
                for (int i = 0; i < Items.Count; i++)
                {
                    ButtonSwitchItem xItem = Items[i];

                    Rectangle xBounds;

                    if (IsHorizontal)
                        xBounds = new Rectangle(i * ItemSize, 0, ItemSize, ClientSize.Height - BarSize);
                    else
                        xBounds = new Rectangle(0, i * ItemSize, ClientSize.Width - BarSize, ItemSize);

                    #region State Logic
                    Color xGradientTop;
                    Color xGradientBottom;
                    Color xTextColor;
                    Image xIcon;

                    if (xItem == SelectedItem)
                    {
                        xGradientTop = SelectedGradientTop;
                        xGradientBottom = SelectedGradientBottom;
                        xTextColor = SelectedTextColor;
                        xIcon = xItem.SelectedIcon;
                    }
                    else
                    {
                        if (xItem == HoverItem)
                        {
                            xGradientTop = HoverGradientTop;
                            xGradientBottom = HoverGradientBottom;
                        }
                        else
                        {
                            xGradientTop = GradientTop;
                            xGradientBottom = GradientBottom;
                        }

                        xTextColor = TextColor;
                        xIcon = xItem.Icon;
                    }
                    #endregion

                    #region Background
                    xBackgroundBlend.Colors = new Color[]
                    {
                        xGradientTop,
                        xGradientTop,
                        xGradientBottom,
                        xGradientBottom
                    };

                    LinearGradientMode xBackgroundMode;

                    if (!IsHorizontal && xItem == SelectedItem)
                        xBackgroundMode = LinearGradientMode.Horizontal;
                    else
                        xBackgroundMode = LinearGradientMode.Vertical;

                    using (LinearGradientBrush xBackgroundBrush = new LinearGradientBrush(xBounds, xGradientTop, xGradientBottom, xBackgroundMode))
                    {
                        xBackgroundBrush.InterpolationColors = xBackgroundBlend;

                        g.FillRectangle(xBackgroundBrush, xBounds);
                    }
                    #endregion

                    #region Text and Icon
                    using (SolidBrush xTextBrush = new SolidBrush(xTextColor))
                    using (StringFormat xStringFormat = new StringFormat(StringFormatFlags.LineLimit | StringFormatFlags.NoWrap))
                    {
                        xStringFormat.Alignment = StringAlignment.Center;
                        xStringFormat.Trimming = StringTrimming.EllipsisCharacter;

                        float xTextHeight = g.MeasureString(xItem.Text, Font).Height;

                        if (xIcon != null && !String.IsNullOrEmpty(xItem.Text) && DisplayStyle == ToolStripItemDisplayStyle.ImageAndText)
                        {
                            Rectangle xIconBounds = Rectangle.Round(new RectangleF(
                                xBounds.Left + (xBounds.Width / 2 - (xIcon.Width / 2)),
                                xBounds.Top + (xBounds.Height / 2 - ((xIcon.Height + TextIconPadding + xTextHeight) / 2)),
                                xIcon.Width,
                                xIcon.Height
                            ));

                            if (Orientation == Orientation.Vertical)
                                xIconBounds = Rectangle.FromLTRB(xIconBounds.Left + 1, xIconBounds.Top, xIconBounds.Right, xIconBounds.Bottom);

                            g.DrawImage(xIcon, xIconBounds, new RectangleF(0, 0, xIcon.Width, xIcon.Height), GraphicsUnit.Pixel);

                            g.DrawString(
                                xItem.Text,
                                Font,
                                xTextBrush,
                                new RectangleF(
                                    xBounds.Left,
                                    xIconBounds.Bottom + TextIconPadding,
                                    xBounds.Width,
                                    xTextHeight
                                ),
                                xStringFormat
                            );
                        }
                        else if (xIcon != null && (DisplayStyle == ToolStripItemDisplayStyle.Image || DisplayStyle == ToolStripItemDisplayStyle.ImageAndText))
                        {
                            Rectangle xIconBounds = Rectangle.Truncate(new RectangleF(
                                xBounds.Left + (xBounds.Width / 2 - (xIcon.Width / 2)),
                                xBounds.Top + (xBounds.Height / 2 - (xIcon.Height / 2)),
                                xIcon.Width, xIcon.Height
                            ));

                            if (Orientation == Orientation.Vertical)
                                xIconBounds = Rectangle.FromLTRB(xIconBounds.Left + 1, xIconBounds.Top, xIconBounds.Right + 1, xIconBounds.Bottom);

                            g.DrawImage(xIcon, xIconBounds, new RectangleF(0, 0, xIcon.Width, xIcon.Height), GraphicsUnit.Pixel);
                        }
                        else if (!String.IsNullOrEmpty(xItem.Text) && (DisplayStyle == ToolStripItemDisplayStyle.Text || DisplayStyle == ToolStripItemDisplayStyle.ImageAndText))
                        {
                            using (StringFormat xTextOnlyFormat = new StringFormat(StringFormatFlags.LineLimit | StringFormatFlags.NoWrap))
                            {
                                xTextOnlyFormat.Alignment = StringAlignment.Center;
                                xTextOnlyFormat.LineAlignment = StringAlignment.Center;
                                xTextOnlyFormat.Trimming = StringTrimming.EllipsisCharacter;

                                g.DrawString(
                                    xItem.Text,
                                    Font,
                                    xTextBrush,
                                    xBounds,
                                    xTextOnlyFormat
                                );
                            }
                        }
                    }
                    #endregion

                    #region Drop Shadows
                    if (IsHorizontal)
                    {
                        if (xItem == SelectedItem)
                        {
                            Color[] xTopShadows = new Color[]
                            {
                                Color.FromArgb(100, Color.Black),
                                Color.FromArgb(55, Color.Black),
                                Color.FromArgb(25, Color.Black),
                                Color.FromArgb(10, Color.Black),
                                Color.FromArgb(0, Color.Black),
                            };

                            Color[] xSideShadows = new Color[]
                            {
                                Color.FromArgb(80, Color.Black),
                                Color.FromArgb(60, Color.Black),
                                Color.FromArgb(20, Color.Black),
                            };

                            using (Pen xPen = new Pen(Color.Black, 1f))
                            {
                                for (int c = 0; c < xTopShadows.Length; c++)
                                {
                                    xPen.Color = xTopShadows[c];

                                    g.DrawLine(xPen, new Point(xBounds.Left, xBounds.Top + c), new Point(xBounds.Right, xBounds.Top + c));
                                }

                                for (int c = 0; c < xSideShadows.Length; c++)
                                {
                                    xPen.Color = xSideShadows[c];

                                    if (Items.IndexOf(xItem) != 0)
                                        g.DrawLine(xPen, new Point(xBounds.Left + c, xBounds.Top), new Point(xBounds.Left + c, xBounds.Bottom));

                                    g.DrawLine(xPen, new Point(xBounds.Right - c - 1, xBounds.Top), new Point(xBounds.Right - c - 1, xBounds.Bottom));
                                }
                            }
                        }
                    }
                    else // if (IsHorizontal)
                    {
                        if (xItem != SelectedItem)
                        {
                            Workspace.DrawDropShadowInside(g, xBounds, 40, 30, 20, 10, 5);
                        }
                    }
                    #endregion

                    #region Separator Line
                    if (IsHorizontal)
                    {
                        if (xItem != SelectedItem)
                        {
                            using (Pen xPen = new Pen(Color.FromArgb(201, 201, 201), 1f))
                            {
                                g.DrawLine(xPen, new Point(xBounds.Right - 1, xBounds.Top), new Point(xBounds.Right - 1, xBounds.Bottom - 1));
                            }
                        }
                    }
                    else
                    {
                        g.DrawLine(Pens.White, new Point(xBounds.Left, xBounds.Top), new Point(xBounds.Right, xBounds.Top));
                        g.DrawLine(Pens.White, new Point(xBounds.Left, xBounds.Top), new Point(xBounds.Left, xBounds.Bottom));

                        // If last item
                        if (Items.IndexOf(xItem) == Items.Count - 1)
                        {
                            g.DrawLine(Pens.White, new Point(xBounds.Left, xBounds.Bottom - 1), new Point(xBounds.Right, xBounds.Bottom - 1));
                        }
                    }
                    #endregion
                }
            }
            else // if (Items.Count > 0)
            {
                Rectangle xBounds;

                if (IsHorizontal)
                    xBounds = new Rectangle(0, 0, ClientSize.Width, ClientSize.Height - BarSize);
                else
                    xBounds = new Rectangle(0, 0, ClientSize.Width - BarSize, ClientSize.Height);

                using (LinearGradientBrush xBrush = new LinearGradientBrush(xBounds, GradientTop, GradientBottom, IsHorizontal ? LinearGradientMode.Vertical : LinearGradientMode.Horizontal))
                {
                    xBackgroundBlend.Colors = new Color[]
                    {
                        GradientTop,
                        GradientTop,
                        GradientBottom,
                        GradientBottom
                    };

                    xBrush.InterpolationColors = xBackgroundBlend;

                    g.FillRectangle(xBrush, xBounds);
                }
            }

            using (SolidBrush xBarBrush = new SolidBrush(BarColor))
            {
                if (IsHorizontal)
                    g.FillRectangle(xBarBrush, new Rectangle(0, ClientSize.Height - BarSize, ClientSize.Width, BarSize));
                else
                    g.FillRectangle(xBarBrush, new Rectangle(ClientSize.Width - BarSize, 0, BarSize, ClientSize.Height));
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (Items.Count > 0 && ClientRectangle.Contains(e.Location))
            {
                int xIndex;

                if (IsHorizontal)
                    xIndex = e.Location.X / ItemSize;
                else
                    xIndex = e.Location.Y / ItemSize;

                if (xIndex >= 0 && xIndex < Items.Count)
                    SelectedItem = Items[xIndex];
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (Items.Count > 0)
            {
                int xIndex;

                if (IsHorizontal)
                    xIndex = e.Location.X / ItemSize;
                else
                    xIndex = e.Location.Y / ItemSize;

                if (xIndex >= 0 && xIndex < Items.Count)
                    HoverItem = Items[xIndex];
                else
                    HoverItem = null;
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            HoverItem = null;
        }

        private void Items_CollectionChanged(object sender, EventArgs e)
        {
            Invalidate();

            if (Items.Count == 1)
                SelectedItem = Items[0];
        }
    }
}
