using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Activity
{
    public class CustomBorderPanel : Panel
    {
        // 圆角的半径
        public int BorderRadius { get; set; } = 20;
        // 边框颜色
        public Color BorderColor { get; set; } = Color.Black;
        // 边框粗细
        public int BorderThickness { get; set; } = 2;

        public CustomBorderPanel()
        {
            // 优化绘制
            this.DoubleBuffered = true;
            // 设置控件接受自定义绘制
            this.SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            // 设置Dock和Anchor属性为None，可以按需调整
            this.Dock = DockStyle.None;
            this.Anchor = AnchorStyles.None;
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            // 创建一个RectangleF对象，表示要绘制的矩形区域
            RectangleF Rect = new RectangleF(0, 0, this.Width, this.Height);

            // 设置抗锯齿模式
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // 使用Pen绘制矩形的边框
            using (Pen pen = new Pen(BorderColor, BorderThickness))
            {
                e.Graphics.DrawRectangle(pen, Rect.X, Rect.Y, Rect.Width - BorderThickness, Rect.Height - BorderThickness);
            }

            // 不需要设置Panel的Region为圆角路径
        }


        /*
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            RectangleF Rect = new RectangleF(0, 0, this.Width, this.Height);
            using (GraphicsPath GraphPath = GetRoundPath(Rect, BorderRadius))
            {
                // 设置抗锯齿模式
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                // 使用Pen绘制边框
                using (Pen pen = new Pen(BorderColor, BorderThickness))
                {
                    e.Graphics.DrawPath(pen, GraphPath);
                }
                // 设置Panel的区域，使其具有圆角
                this.Region = new Region(GraphPath);
            }
        }

        private GraphicsPath GetRoundPath(RectangleF Rect, int radius)
        {
            // 创建一个用于绘制圆角矩形的GraphicsPath对象
            float r2 = radius / 2f;
            GraphicsPath GraphPath = new GraphicsPath();

            GraphPath.AddArc(Rect.X, Rect.Y, radius, radius, 180, 90);
            GraphPath.AddLine(Rect.X + r2, Rect.Y, Rect.Width - r2, Rect.Y);
            GraphPath.AddArc(Rect.X + Rect.Width - radius, Rect.Y, radius, radius, 270, 90);
            GraphPath.AddLine(Rect.Width, Rect.Y + r2, Rect.Width, Rect.Height - r2);
            GraphPath.AddArc(Rect.X + Rect.Width - radius, Rect.Y + Rect.Height - radius, radius, radius, 0, 90);
            GraphPath.AddLine(Rect.Width - r2, Rect.Height, Rect.X + r2, Rect.Height);
            GraphPath.AddArc(Rect.X, Rect.Y + Rect.Height - radius, radius, radius, 90, 90);
            GraphPath.AddLine(Rect.X, Rect.Height - r2, Rect.X, Rect.Y + r2);

            GraphPath.CloseFigure();
            return GraphPath;
        }
       */
    }
}
