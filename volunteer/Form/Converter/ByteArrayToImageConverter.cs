using System;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Form.Converter
{
    public class ByteArrayToImageConverter : IValueConverter
    {

        public static byte[] GetDefaultAvatar()
        {
            using (var ms = new MemoryStream())
            {
                // 将Bitmap对象保存到内存流中
                Properties.Resources.DefaultAvatar.Save(ms, ImageFormat.Png);

                // 将内存流转换为byte数组
                return ms.ToArray();
            }
        }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            byte[] byteArray = value as byte[];


            // 如果传入的字节数组为空或长度为0，并且参数指示需要默认头像，则加载默认头像
            if ((byteArray == null || byteArray.Length == 0)    )  
            {
               
                if (parameter as string == "Avatar")
                {
                    byteArray =  GetDefaultAvatar(); // 确保实现了获取默认头像的方法
                    if (byteArray == null) return null; // 如果默认头像也没有，则返回null
                }
                else
                {
                    // 如果不是头像（即可能是话题图片），且参数不要求显示默认图片，则返回null
                    return null;
                }
            }

            BitmapImage image = new BitmapImage();
            using (MemoryStream stream = new MemoryStream(byteArray))
            {
                stream.Position = 0;
                image.BeginInit();
                image.StreamSource = stream;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.EndInit();
            }

            return image;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
