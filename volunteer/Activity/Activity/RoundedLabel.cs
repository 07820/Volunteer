using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Activity
{
    public class RoundedLabel : Label
    {
        // 圆角的半径
        public int BorderRadius { get; set; } = 10;

        // 控件的背景颜色
        public new Color BackColor { get; set; } = Color.LightGray;

        // 添加一个属性来设置文本颜色
        public new Color ForeColor { get; set; } = Color.Black;

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // 使用GraphicsPath绘制圆角矩形
            using (GraphicsPath path = GetRoundRectangle(this.ClientRectangle, this.BorderRadius))
            {
                // 填充背景
                using (SolidBrush brush = new SolidBrush(this.BackColor))
                {
                    e.Graphics.FillPath(brush, path);
                }

                // 绘制文本
                TextRenderer.DrawText(e.Graphics, this.Text, this.Font, this.ClientRectangle, this.ForeColor, Color.Transparent, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            }
        }

        private GraphicsPath GetRoundRectangle(Rectangle rectangle, int radius)
        {
            int diameter = radius * 2;
            GraphicsPath path = new GraphicsPath();

            if (radius == 0)
            {
                path.AddRectangle(rectangle);
                return path;
            }

            Rectangle arc = new Rectangle(rectangle.Location, new Size(diameter, diameter));
            // 左上角
            path.AddArc(arc, 180, 90);
            // 右上角
            arc.X = rectangle.Right - diameter;
            path.AddArc(arc, 270, 90);
            // 右下角
            arc.Y = rectangle.Bottom - diameter;
            path.AddArc(arc, 0, 90);
            // 左下角
            arc.X = rectangle.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }
    }

}
