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
    public class TemplatedListStatus : UserControl
    {
        private Rectangle sliderBounds;
        private Rectangle trackBounds;
        private Rectangle sliderTextBounds;

        private List<Hotspot> hotspots = new List<Hotspot>();
        private Hotspot minus = new Hotspot(HotspotType.Minus, Properties.Resources.minus, Properties.Resources.minus_down, Properties.Resources.minus_over);
        private Hotspot plus = new Hotspot(HotspotType.Plus, Properties.Resources.plus, Properties.Resources.plus_down, Properties.Resources.plus_over);
        private Hotspot thumb = new Hotspot(HotspotType.Thumb, Properties.Resources.slidetab, Properties.Resources.slidetab_down, Properties.Resources.slidetab_over);

        public event EventHandler ValueChanged;

        private Image _statusIcon;
        public Image StatusIcon
        {
            get { return _statusIcon; }
            set
            {
                _statusIcon = value;
                Invalidate();
            }
        }

        private string _statusText;
        public string StatusText
        {
            get { return _statusText; }
            set
            {
                _statusText = value;
                Invalidate();
            }
        }

        private int _value = 100;
        [DefaultValue(100)]
        public int Value
        {
            get { return _value; }
            set
            {
                if (_value != value)
                {
                    _value = value;

                    UpdateThumbLayout();

                    if (ValueChanged != null)
                        ValueChanged(this, EventArgs.Empty);

                    Invalidate();
                }
            }
        }

        private int _minimum = 50;
        [DefaultValue(50)]
        public int Minimum
        {
            get { return _minimum; }
            set { _minimum = value; }
        }

        private int _maximum = 250;
        [DefaultValue(250)]
        public int Maximum
        {
            get { return _maximum; }
            set { _maximum = value; }
        }

        private int _increment = 25;
        [DefaultValue(25)]
        public int Increment
        {
            get { return _increment; }
            set { _increment = value; }
        }

        private int _sliderWidth = 140;
        [DefaultValue(140)]
        public int SliderWidth
        {
            get { return _sliderWidth; }
            set { _sliderWidth = value; }
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

        private Color _topBorderColor = Color.FromArgb(122, 122, 122);
        [DefaultValue(typeof(Color), "122, 122, 122")]
        [Category("Appearance")]
        public Color TopBorderColor
        {
            get { return _topBorderColor; }
            set { _topBorderColor = value; }
        }

        private Color _topGradientColor = Color.FromArgb(230, 230, 230);
        [DefaultValue(typeof(Color), "230, 230, 230")]
        [Category("Appearance")]
        public Color TopGradientColor
        {
            get { return _topGradientColor; }
            set { _topGradientColor = value; }
        }

        private Color _bottomGradientColor = Color.FromArgb(210, 210, 210);
        [DefaultValue(typeof(Color), "210, 210, 210")]
        [Category("Appearance")]
        public Color BottomGradientColor
        {
            get { return _bottomGradientColor; }
            set { _bottomGradientColor = value; }
        }

        private int range
        {
            get
            {
                return Maximum - Minimum;
            }
        }

        private Hotspot _hotspotDown;
        private Hotspot hotspotDown
        {
            get { return _hotspotDown; }
            set
            {
                if (_hotspotDown != value)
                {
                    if (_hotspotDown != null)
                        _hotspotDown.Down = false;

                    _hotspotDown = value;

                    if (_hotspotDown != null)
                        _hotspotDown.Down = true;

                    Invalidate();
                }
            }
        }

        private Hotspot _hotspotOver;
        private Hotspot hotspotOver
        {
            get { return _hotspotOver; }
            set
            {
                if (_hotspotOver != value)
                {
                    if (_hotspotOver != null)
                        _hotspotOver.Over = false;

                    _hotspotOver = value;

                    if (_hotspotOver != null)
                        _hotspotOver.Over = true;

                    Invalidate();
                }
            }
        }

        public TemplatedListStatus()
        {
            this.SetStyle(ControlStyles.DoubleBuffer |
                          ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.UserPaint |
                          ControlStyles.ResizeRedraw, true);

            Margin = new Padding(0, 0, 0, 0);
            Padding = new Padding(5, 2, 5, 2);

            Font = new Font("Segoe UI", 9f, GraphicsUnit.Point);

            hotspots.AddRange(new Hotspot[] {
                thumb,
                minus,
                plus
            });
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            UpdateLayout();
        }

        public virtual void UpdateLayout()
        {
            sliderBounds = new Rectangle(
                ClientSize.Width - SliderWidth - Padding.Right,
                Padding.Top,
                SliderWidth,
                ClientSize.Height - Padding.Vertical
            );

            sliderTextBounds = Rectangle.FromLTRB(
                Padding.Left, Padding.Top, sliderBounds.Left - 5, ClientSize.Height - Padding.Bottom
            );

            minus.Bounds = new Rectangle(
                sliderBounds.Left,
                sliderBounds.Top + (sliderBounds.Height - minus.Image.Height) / 2,
                minus.Image.Width,
                minus.Image.Height
            );

            plus.Bounds = new Rectangle(
                sliderBounds.Right - plus.Image.Width,
                sliderBounds.Top + (sliderBounds.Height - plus.Image.Height) / 2,
                plus.Image.Width,
                plus.Image.Height
            );

            trackBounds = Rectangle.FromLTRB(
                minus.Bounds.Right + (thumb.Image.Width / 2),
                sliderBounds.Top,
                plus.Bounds.Left - (thumb.Image.Width / 2) - 1,
                sliderBounds.Bottom
            );

            UpdateThumbLayout();
        }

        private void UpdateThumbLayout()
        {
            float xValue = Value - Minimum;
            float xOffsetRatio = xValue / range;
            int xOffset = (int)Math.Round(trackBounds.Width * xOffsetRatio);

            thumb.Bounds = new Rectangle(
                trackBounds.Left + xOffset - (thumb.Image.Width / 2),
                trackBounds.Top + ((trackBounds.Height - thumb.Image.Height) / 2),
                thumb.Image.Width,
                thumb.Image.Height
            );
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            #region Background
            using (Pen xBorderPen = new Pen(TopBorderColor, 1.0f))
            using (Pen xEmbossPen = new Pen(Color.FromArgb(238, 242, 245), 1.0f))
            using (LinearGradientBrush xBackgroundBrush = new LinearGradientBrush(ClientRectangle, TopGradientColor, BottomGradientColor, LinearGradientMode.Vertical))
            {
                g.FillRectangle(xBackgroundBrush, ClientRectangle);

                g.DrawLine(xBorderPen, new Point(0, 0), new Point(ClientSize.Width, 0));
                g.DrawLine(xEmbossPen, new Point(0, 1), new Point(ClientSize.Width, 1));
            }
            #endregion

            DrawSlider(g);

            using (StringFormat xSliderTextFormat = new StringFormat(StringFormatFlags.LineLimit | StringFormatFlags.NoWrap))
            {
                xSliderTextFormat.Alignment = StringAlignment.Far;
                xSliderTextFormat.LineAlignment = StringAlignment.Center;

                g.DrawString(String.Format("{0}%", Value), Font, Brushes.Black, sliderTextBounds, xSliderTextFormat);
            }
        }

        private void DrawSlider(Graphics g)
        {
            int xMiddleY = trackBounds.Top + trackBounds.Height / 2;
            g.DrawLine(Pens.Gray, new Point(sliderBounds.Left, xMiddleY), new Point(sliderBounds.Right - 1, xMiddleY));
            g.DrawLine(Pens.White, new Point(sliderBounds.Left, xMiddleY + 1), new Point(sliderBounds.Right - 1, xMiddleY + 1));

            foreach (Hotspot xHotspot in hotspots)
            {
                g.DrawImage(
                    xHotspot.CurrentImage,
                    xHotspot.Bounds,
                    new Rectangle(0, 0, xHotspot.CurrentImage.Width, xHotspot.CurrentImage.Height),
                    GraphicsUnit.Pixel
                );
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button == MouseButtons.Left)
            {
                foreach (Hotspot xHotspot in hotspots)
                {
                    if (xHotspot.Bounds.Contains(e.Location))
                    {
                        hotspotDown = xHotspot;
                        break;
                    }
                }

                if (hotspotDown == null)
                {
                    if (trackBounds.Contains(e.Location))
                    {
                        hotspotDown = thumb;

                        UpdateThumb(e.Location);
                    }
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (hotspotDown != null && hotspotDown.Bounds.Contains(e.Location))
            {
                switch (hotspotDown.HotspotType)
                {
                    case HotspotType.Minus:
                        Value = (int)Math.Max(Minimum, Value - Increment);
                        break;

                    case HotspotType.Plus:
                        Value = (int)Math.Min(Maximum, Value + Increment);
                        break;
                }
            }

            hotspotDown = null;
        }

        private void UpdateThumb(Point iLocation)
        {
            int xValue = (int)Math.Max(0, Math.Min(trackBounds.Width, iLocation.X - trackBounds.Left));
            float xRatio = (float)xValue / (float)trackBounds.Width;

            Value = (int)Math.Round((range * xRatio) + Minimum);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (hotspotDown == thumb)
            {
                UpdateThumb(e.Location);
            }
            else
            {
                bool xFound = false;

                foreach (Hotspot xHotspot in hotspots)
                {
                    if (xHotspot.Bounds.Contains(e.Location))
                    {
                        hotspotOver = xHotspot;
                        xFound = true;
                        break;
                    }
                }

                if (!xFound)
                    hotspotOver = null;
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            hotspotOver = null;
        }

        protected override void OnClientSizeChanged(EventArgs e)
        {
            base.OnClientSizeChanged(e);

            UpdateLayout();
        }

        private enum HotspotType
        {
            Minus,
            Plus,
            Thumb,
        }

        private class Hotspot
        {
            private HotspotType _hotspotType;
            public HotspotType HotspotType
            {
                get { return _hotspotType; }
                set { _hotspotType = value; }
            }

            private Image _image;
            public Image Image
            {
                get { return _image; }
                set { _image = value; }
            }

            private Image _imageDown;
            public Image ImageDown
            {
                get { return _imageDown; }
                set { _imageDown = value; }
            }

            private Image _imageOver;
            public Image ImageOver
            {
                get { return _imageOver; }
                set { _imageOver = value; }
            }

            private Rectangle _bounds;
            public Rectangle Bounds
            {
                get { return _bounds; }
                set { _bounds = value; }
            }

            private bool _over;
            public bool Over
            {
                get { return _over; }
                set { _over = value; }
            }

            private bool _down;
            public bool Down
            {
                get { return _down; }
                set { _down = value; }
            }

            public Image CurrentImage
            {
                get
                {
                    if (Down)
                        return ImageDown;
                    else if (Over)
                        return ImageOver;
                    else
                        return Image;
                }
            }

            public Hotspot(HotspotType iType, Image iImage, Image iImageDown, Image iImageOver)
            {
                HotspotType = iType;
                Image = iImage;
                ImageDown = iImageDown;
                ImageOver = iImageOver;
            }
        }
    }
}
