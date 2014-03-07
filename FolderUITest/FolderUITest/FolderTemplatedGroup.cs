using System;
using System.Collections.Generic;
using System.Text;
using FolderUI;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.Windows.Forms;

namespace FolderUITest
{
    public class FolderTemplatedGroup : TemplatedGroup
    {
        private string _header;
        [DefaultValue("")]
        public string Header
        {
            get { return _header; }
            set { _header = value; }
        }

        private Image _icon;
        [DefaultValue(null)]
        public Image Icon
        {
            get { return _icon; }
            set { _icon = value; }
        }

        [DefaultValue(22)]
        public override int HeaderHeight
        {
            get { return base.HeaderHeight; }
            set { base.HeaderHeight = value; }
        }

        private int _folderWidth = 280;
        [DefaultValue(280)]
        public int FolderWidth
        {
            get { return _folderWidth; }
            set { _folderWidth = value; }
        }

        private Color _headerColor = Color.FromArgb(90, 149, 179);
        [DefaultValue(typeof(Color), "90, 149, 179")]
        public Color HeaderColor
        {
            get { return _headerColor; }
            set { _headerColor = value; }
        }

        private Color _textColor = Color.White;
        [DefaultValue(typeof(Color), "White")]
        public Color TextColor
        {
            get { return _textColor; }
            set { _textColor = value; }
        }

        private Padding _padding = new Padding(14, 0, 0, 0);
        [DefaultValue(typeof(Padding), "14, 0, 0, 0")]
        public Padding Padding
        {
            get { return _padding; }
            set { _padding = value; }
        }

        private int _textIconSpacing = 2;
        [DefaultValue(2)]
        public int TextIconSpacing
        {
            get { return _textIconSpacing; }
            set { _textIconSpacing = value; }
        }

        public FolderTemplatedGroup()
        {
            HeaderHeight = 22;
        }

        public FolderTemplatedGroup(string iHeaderText, Image iIcon)
            : this()
        {
            Header = iHeaderText;
            Icon = iIcon;
        }

        public FolderTemplatedGroup(string iHeaderText, Image iIcon, string iKey, object iTag)
            : this(iHeaderText, iIcon)
        {
            Name = iKey;
            Tag = iTag;
        }

        protected override void OnRender(Graphics g)
        {
            base.OnRender(g);

            #region Draw Rows
            using (Pen xPen = new Pen(Color.FromArgb(133, 194, 223), 1.0f))
            {
                for (int i = 0; i < RowCount; i++)
                {
                    Rectangle xRowRect = new Rectangle(
                        ItemContainer.Left,
                        ItemContainer.Top + ((TemplatedList.ActualTileSize.Height + TemplatedList.ItemMargins.Vertical) * i),
                        ItemContainer.Width - 1,
                        ItemContainer.Height / RowCount
                    );

                    Rectangle xFillRect = Rectangle.FromLTRB(xRowRect.Left, xRowRect.Top, xRowRect.Right, xRowRect.Bottom);

                    using (LinearGradientBrush xRowBrush = new LinearGradientBrush(xFillRect, Color.FromArgb(223, 240, 247), Color.FromArgb(241, 252, 254), LinearGradientMode.Vertical))
                    {
                        g.FillRectangle(xRowBrush, xFillRect);
                    }

                    if (i > 0)
                        g.DrawLine(Pens.White, new Point(xFillRect.Left, xFillRect.Top + 1), new Point(xFillRect.Right, xFillRect.Top + 1));

                    g.DrawRectangle(xPen, xRowRect);
                }
            }
            #endregion

            DrawHeader(g, new Rectangle(Bounds.X, Bounds.Y, Bounds.Width, HeaderHeight));

            #region Draw Separator Lines
            Color xLineColor = Color.FromArgb(173, 229, 254);

            using (Pen xLinePen = new Pen(xLineColor, 1.0f))
            {
                foreach (TemplatedItem xItem in this.Items)
                {
                    int xIndex = Items.IndexOf(xItem);

                    if ((xIndex + 1) % ItemsPerRow == 0 || xIndex == Items.Count - 1)
                        continue;

                    Point xPoint1 = new Point(xItem.Bounds.Right + TemplatedList.ItemMargins.Right, xItem.Bounds.Top);
                    Point xPoint2 = new Point(xPoint1.X, xItem.Bounds.Bottom);

                    g.DrawLine(xLinePen, xPoint1, xPoint2);
                    g.DrawLine(Pens.White, new Point(xPoint1.X + 1, xPoint1.Y), new Point(xPoint2.X + 1, xPoint2.Y));
                }
            }
            #endregion
        }

        private void DrawHeader(Graphics g, Rectangle iRect)
        {
            bool xIconVisible = (Icon != null);
            bool xTextVisible = !String.IsNullOrEmpty(Header);

            #region Draw Folder Path
            using (SolidBrush xHeaderBrush = new SolidBrush(HeaderColor))
            using (Pen xHeaderPen = new Pen(HeaderColor, 1.0f))
            using (GraphicsPath xHeaderPath = new GraphicsPath())
            {
                Point xLastPoint;
                const int xCurveWidth = 14;
                const int xLineOffset = 3;
                const int yLineOffset = 3;

                Point xBezier1Start = new Point(iRect.Left + (int)Math.Min(Bounds.Width - xCurveWidth - 1, FolderWidth), iRect.Top);
                Point xBezier1End = new Point(xBezier1Start.X + xLineOffset, xBezier1Start.Y + yLineOffset);
                Point xBezier2End = new Point(xBezier1Start.X + xCurveWidth, iRect.Bottom);
                Point xBezier2Start = new Point(xBezier2End.X - xLineOffset, xBezier2End.Y - yLineOffset);

                xHeaderPath.AddBezier(xBezier1Start, new Point(xBezier1Start.X + 2, xBezier1Start.Y), new Point(xBezier1End.X, xBezier1End.Y), xBezier1End);
                xHeaderPath.AddLine(xBezier1End, xBezier2Start);
                xHeaderPath.AddBezier(xBezier2Start, new Point(xBezier2Start.X, xBezier2Start.Y), new Point(xBezier2End.X - 2, xBezier2End.Y), xBezier2End);

                xHeaderPath.AddLine(xBezier2End, xLastPoint = new Point(iRect.Left, iRect.Bottom));
                xHeaderPath.AddLine(xLastPoint, xLastPoint = new Point(iRect.Left, iRect.Top));
                xHeaderPath.AddLine(xLastPoint, xBezier1Start);

                g.FillPath(xHeaderBrush, xHeaderPath);
                g.DrawPath(xHeaderPen, xHeaderPath);
            }
            #endregion

            Rectangle xContentRect = Rectangle.FromLTRB(
                iRect.Left + Padding.Left,
                iRect.Top + Padding.Top,
                iRect.Right - Padding.Right,
                iRect.Bottom - Padding.Bottom
            );

            if (xIconVisible)
            {
                Rectangle xIconRect = new Rectangle(
                    xContentRect.Left,
                    xContentRect.Top + ((xContentRect.Height - Icon.Height) / 2),
                    Icon.Width,
                    Icon.Height
                );

                g.DrawImage(Icon, xIconRect, new Rectangle(0, 0, Icon.Width, Icon.Height), GraphicsUnit.Pixel);

                xContentRect = new Rectangle(
                    xIconRect.Right + TextIconSpacing,
                    xContentRect.Top,
                    xContentRect.Width - xIconRect.Width - TextIconSpacing,
                    xContentRect.Height
                );
            }

            if (xTextVisible)
            {
                using (SolidBrush xTextBrush = new SolidBrush(TextColor))
                using (StringFormat xTextFormat = new StringFormat(StringFormatFlags.LineLimit | StringFormatFlags.NoWrap))
                {
                    xTextFormat.LineAlignment = StringAlignment.Center;
                    xTextFormat.Trimming = StringTrimming.EllipsisCharacter;

                    g.DrawString(Header, TemplatedList.Font, xTextBrush, xContentRect, xTextFormat);
                }
            }
        }
    }
}
