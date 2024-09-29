using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Forms;
using System.ComponentModel;

public class ImageLabel : Label
{
    private System.Drawing.Image _image;
    private const int ImageWidth = 20;
    private const int ImageHeight = 20;
    
    public new System.Drawing.Image Image
    {
        get => _image;
        set
        {
            // 当Image属性被设置时，自动缩放图片到30x30
            _image = ResizeImage(value, ImageWidth, ImageHeight);
            Invalidate(); // 通知控件重绘
        }
    }

    public ImageLabel()
    {
        // 在构造函数中设置默认文本颜色为黑色
        this.ForeColor = Color.Black;
        this.AutoSize = true; // 默认启用AutoSize
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        int imageWidth = _image?.Width ?? 0;
        int imageHeight = _image?.Height ?? 0;

        if (_image != null)
        {
            // 绘制图片，图片垂直居中
            e.Graphics.DrawImage(_image, new Point(0, (Height - imageHeight) / 2)); 
        }
    }

    private System.Drawing.Image ResizeImage(System.Drawing.Image image, int width, int height)
    {
        if (image == null)
        {
            return null;
        }

        var destRect = new Rectangle(0, 0, width, height);
        var destImage = new Bitmap(width, height);

        destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

        using (var graphics = Graphics.FromImage(destImage))
        {
            graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
            graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

            using (var wrapMode = new System.Drawing.Imaging.ImageAttributes())
            {
                wrapMode.SetWrapMode(System.Drawing.Drawing2D.WrapMode.TileFlipXY);
                graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
            }
        }

        return destImage;
    }
}
