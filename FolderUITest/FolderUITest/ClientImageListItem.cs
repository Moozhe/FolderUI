using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using FolderUI;
using System.Drawing.Drawing2D;

namespace FolderUITest
{
    public class ClientImageListItem : TemplatedItem
    {
        private Bitmap image;

        private ClientImage _clientImage;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ClientImage ClientImage
        {
            get { return _clientImage; }
            set
            {
                if (_clientImage != value)
                {
                    _clientImage = value;

                    OnClientImageChanged(EventArgs.Empty);
                }
            }
        }

        private Padding _padding = new Padding(10, 10, 10, 10);
        [DefaultValue(typeof(Padding), "10, 10, 10, 10")]
        public Padding Padding
        {
            get { return _padding; }
            set { _padding = value; }
        }

        private Padding _imageMargins = new Padding(0, 0, 0, 0);
        [DefaultValue(typeof(Padding), "0, 0, 0, 0")]
        public Padding ImageMargins
        {
            get { return _imageMargins; }
            set { _imageMargins = value; }
        }

        private Padding _textMargins = new Padding(0, 0, 0, 0);
        [DefaultValue(typeof(Padding), "0, 0, 0, 0")]
        public Padding TextMargins
        {
            get { return _textMargins; }
            set { _textMargins = value; }
        }

        private Color _selBackColor = Color.FromArgb(128, 66, 132, 164);
        [DefaultValue(typeof(Color), "128, 66, 132, 164")]
        public Color SelectedBackColor
        {
            get { return _selBackColor; }
            set { _selBackColor = value; }
        }

        private Color _hoverBackColor = Color.FromArgb(64, 66, 132, 164);
        [DefaultValue(typeof(Color), "64, 66, 132, 164")]
        public Color HoverBackColor
        {
            get { return _hoverBackColor; }
            set { _hoverBackColor = value; }
        }

        private Color _selBorderColor = Color.FromArgb(66, 132, 164);
        [DefaultValue(typeof(Color), "66, 132, 164")]
        public Color SelectedBorderColor
        {
            get { return _selBorderColor; }
            set { _selBorderColor = value; }
        }

        private Color _hoverBorderColor = Color.FromArgb(128, 66, 132, 164);
        [DefaultValue(typeof(Color), "128, 66, 132, 164")]
        public Color HoverBorderColor
        {
            get { return _hoverBorderColor; }
            set { _hoverBorderColor = value; }
        }

        private Color _nameColor = Color.FromArgb(65, 134, 167);
        [DefaultValue(typeof(Color), "65, 134, 167")]
        public Color NameColor
        {
            get { return _nameColor; }
            set { _nameColor = value; }
        }

        private Color _textColor = Color.FromArgb(99, 100, 102);
        [DefaultValue(typeof(Color), "99, 100, 102")]
        public Color TextColor
        {
            get { return _textColor; }
            set { _textColor = value; }
        }

        private int _borderWidth = 2;
        [DefaultValue(3)]
        public int BorderWidth
        {
            get { return _borderWidth; }
            set { _borderWidth = value; }
        }

        private Color _borderColor = Color.FromArgb(127, 127, 127);
        [DefaultValue(typeof(Color), "127, 127, 127")]
        public Color ImageBorderColor
        {
            get { return _borderColor; }
            set { _borderColor = value; }
        }

        private Color _selImageBorderColor = Color.FromArgb(2, 187, 254);
        [DefaultValue(typeof(Color), "2, 187, 254")]
        public Color SelectedImageBorderColor
        {
            get { return _selImageBorderColor; }
            set { _selImageBorderColor = value; }
        }

        public ClientImageListItem()
        {
        }

        public ClientImageListItem(ClientImage iClientImage, TemplatedGroup iGroup)
        {
            ClientImage = iClientImage;
            Name = iClientImage.Id.ToString();
            Group = iGroup;
        }

        protected override void OnRender(Graphics g)
        {
            base.OnRender(g);

            Rectangle xInnerRect = Rectangle.FromLTRB(
                Bounds.Left + Padding.Left, Bounds.Top + Padding.Top, Bounds.Right - Padding.Right, Bounds.Bottom - Padding.Bottom
            );

            float xTextHeight = TemplatedList.Font.GetHeight(g);

            RectangleF xProviderRect = RectangleF.FromLTRB(
                xInnerRect.Left + TextMargins.Left,
                xInnerRect.Bottom - TextMargins.Bottom - xTextHeight,
                xInnerRect.Right - TextMargins.Right,
                xInnerRect.Bottom - TextMargins.Bottom
            );

            RectangleF xNameRect = RectangleF.FromLTRB(
                xInnerRect.Left + TextMargins.Left,
                xProviderRect.Top - xTextHeight,
                xInnerRect.Right - TextMargins.Right,
                xProviderRect.Top
            );

            RectangleF xImageBorderRect = RectangleF.FromLTRB(
                xInnerRect.Left + ImageMargins.Left,
                xInnerRect.Top + ImageMargins.Top,
                xInnerRect.Right - ImageMargins.Right,
                xNameRect.Top - TextMargins.Top - ImageMargins.Bottom
            );

            RectangleF xImageRect = RectangleF.FromLTRB(
                xImageBorderRect.Left + BorderWidth,
                xImageBorderRect.Top + BorderWidth,
                xImageBorderRect.Right - BorderWidth,
                xImageBorderRect.Bottom - BorderWidth
            );

            using (StringFormat xTextFormat = new StringFormat(StringFormatFlags.LineLimit | StringFormatFlags.NoWrap))
            {
                xTextFormat.Trimming = StringTrimming.EllipsisCharacter;
                xTextFormat.Alignment = StringAlignment.Center;

                if (!String.IsNullOrEmpty(ClientImage.Name))
                {
                    using (SolidBrush xNameBrush = new SolidBrush(NameColor))
                    {
                        g.DrawString(ClientImage.Name, TemplatedList.Font, xNameBrush, xNameRect, xTextFormat);
                    }
                }

                if (!String.IsNullOrEmpty(ClientImage.Provider))
                {
                    using (SolidBrush xTextBrush = new SolidBrush(TextColor))
                    {
                        g.DrawString(ClientImage.Provider, TemplatedList.Font, xTextBrush, xProviderRect, xTextFormat);
                    }
                }
            }

            if (image != null)
            {
                double xRatio = Math.Min((double)xImageRect.Width / (double)image.Width, (double)xImageRect.Height / (double)image.Height);

                SizeF xScaledSize = new SizeF(
                    (float)(image.Width * xRatio),
                    (float)(image.Height * xRatio)
                );

                RectangleF xScaledImageRect = new RectangleF(
                    xImageRect.Left + (xImageRect.Width - xScaledSize.Width) / 2f,
                    xImageRect.Bottom - xScaledSize.Height,
                    xScaledSize.Width,
                    xScaledSize.Height
                );

                g.DrawImage(image, Rectangle.Round(xScaledImageRect), new RectangleF(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);

                using (Pen xBorderPen = new Pen(ImageBorderColor, BorderWidth + 0.5f))
                {
                    xBorderPen.Alignment = PenAlignment.Outset;

                    g.DrawRectangle(xBorderPen, Rectangle.Round(xScaledImageRect));
                }
            }
        }

        protected virtual void OnClientImageChanged(EventArgs e)
        {
            if (image != null)
            {
                image.Dispose();
                image = null;
            }

            if (ClientImage != null)
            {
                image = new Bitmap(ClientImage.FileName);
            }
        }
    }
}
