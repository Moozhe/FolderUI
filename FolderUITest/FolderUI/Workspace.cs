using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.ComponentModel.Design;

namespace FolderUI
{
    [Designer("System.Windows.Forms.Design.ParentControlDesigner, System.Design", typeof(IDesigner))]
    public class Workspace : UserControl
    {
        #region Static
        public static void DrawDropShadowOutside(Graphics g, Rectangle iRect, params int[] iOpacityLevels)
        {
            DrawDropShadow(g, iRect, true, iOpacityLevels);
        }

        public static void DrawDropShadowInside(Graphics g, Rectangle iRect, params int[] iOpacityLevels)
        {
            DrawDropShadow(g, iRect, false, iOpacityLevels);
        }

        private static void DrawDropShadow(Graphics g, Rectangle iRect, bool iOutside, params int[] iOpacityLevels)
        {
            using (GraphicsPath xPath = new GraphicsPath())
            {
                int xSide = iOpacityLevels.Length;

                xPath.AddLines(new Point[]
                {
                    new Point(iRect.Left - xSide, iRect.Top),
                    new Point(iRect.Left, iRect.Top - xSide),
                    new Point(iRect.Right, iRect.Top - xSide),
                    new Point(iRect.Right + xSide, iRect.Top),
                    new Point(iRect.Right + xSide, iRect.Bottom),
                    new Point(iRect.Right, iRect.Bottom + xSide),
                    new Point(iRect.Left, iRect.Bottom + xSide),
                    new Point(iRect.Left - xSide, iRect.Bottom),
                    new Point(iRect.Left - xSide, iRect.Top)
                });

                using (Region xClipRegion = new Region(xPath))
                {
                    Region xOldClip = g.Clip;
                    g.Clip = xClipRegion;

                    using (Pen xPen = new Pen(Color.Empty, 1.0f))
                    {
                        for (int i = 0; i < iOpacityLevels.Length; i++)
                        {
                            xPen.Color = Color.FromArgb(iOpacityLevels[i], Color.Black);

                            int xAmt = iOutside ? i + 1 : -i;
                            Rectangle xShadowRect = Rectangle.Inflate(iRect, xAmt, xAmt);

                            g.DrawRectangle(xPen, xShadowRect);
                        }
                    }

                    g.Clip = xOldClip;
                }
            }
        }
        #endregion

        #region Appearance
        private Color _gradientTop = Color.Gray;
        [Category("Appearance")]
        [DefaultValue(typeof(Color), "Gray")]
        public Color GradientTop
        {
            get { return _gradientTop; }
            set { _gradientTop = value; }
        }

        private Color _gradientBottom = Color.FromArgb(175, 175, 175);
        [Category("Appearance")]
        [DefaultValue(typeof(Color), "175, 175, 175")]
        public Color GradientBottom
        {
            get { return _gradientBottom; }
            set { _gradientBottom = value; }
        }

        [DefaultValue(typeof(Font), "Arial, 11px")]
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

        public Workspace()
        {
            this.SetStyle(ControlStyles.DoubleBuffer |
                          ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.UserPaint |
                          ControlStyles.ResizeRedraw, true);

            this.DoubleBuffered = true;

            Font = new Font("Arial", 11f, GraphicsUnit.Pixel);
            Margin = new Padding(0, 0, 0, 0);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            using (LinearGradientBrush xBackgroundBrush = new LinearGradientBrush(ClientRectangle, Color.Empty, Color.Empty, LinearGradientMode.Vertical))
            {
                ColorBlend xBackgroundBlend = new ColorBlend(3);

                xBackgroundBlend.Colors = new Color[]
                {
                    Color.FromArgb(128, 128, 128),
                    Color.FromArgb(175, 175, 175),
                    Color.FromArgb(175, 175, 175)
                };

                xBackgroundBlend.Positions = new float[]
                {
                    0.00f,
                    0.85f,
                    1.00f
                };

                xBackgroundBrush.InterpolationColors = xBackgroundBlend;

                g.FillRectangle(xBackgroundBrush, ClientRectangle);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            #region Top Shadow
            Color[] xTopShadows = new Color[]
            {
                Color.FromArgb(80, Color.Black),
                Color.FromArgb(60, Color.Black),
                Color.FromArgb(20, Color.Black),
            };

            using (Pen xPen = new Pen(Color.Empty, 1.0f))
            {
                for (int i = 0; i < xTopShadows.Length; i++)
                {
                    xPen.Color = xTopShadows[i];

                    g.DrawLine(xPen, new Point(0, i), new Point(ClientSize.Width, i));
                }
            }
            #endregion

            #region Child Shadows
            foreach (Control xControl in Controls)
            {
                DrawDropShadow(g, new Rectangle(xControl.Bounds.X, xControl.Bounds.Y, xControl.Width - 1, xControl.Height - 1), true, 60, 40, 20, 10);
            }
            #endregion
        }
    }
}
