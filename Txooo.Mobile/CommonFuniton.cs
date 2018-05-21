using com.google.zxing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Txooo.Mobile
{
    /// <summary>
    /// 公众函数
    /// </summary>
    public class CommonFunction
    {
        public static string GetDatetimeNowString()
        {
            TimeSpan ts = DateTime.Now - DateTime.Parse("1970-01-01");

            return Convert.ToString(((Int64)ts.TotalSeconds));
        }

        public static DateTime GetDatetimeFromString(string datetimeString)
        {
            return DateTime.Parse("1970-01-01").AddSeconds(Convert.ToDouble(datetimeString));
        }

        /// <summary>
        /// 经纬度之间的距离
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static double CalcDistance(double fromX, double fromY, double toX, double toY)
        {
            double rad = 6371; //Earth radius in Km
            double p1X = fromX / 180 * Math.PI;
            double p1Y = fromY / 180 * Math.PI;
            double p2X = toX / 180 * Math.PI;
            double p2Y = toY / 180 * Math.PI;
            return Math.Acos(Math.Sin(p1Y) * Math.Sin(p2Y) +
                Math.Cos(p1Y) * Math.Cos(p2Y) * Math.Cos(p2X - p1X)) * rad;
        }

        public static System.Drawing.Bitmap CreateQr(string message, int width, int height)
        {
            com.google.zxing.common.ByteMatrix byteMatrix = new MultiFormatWriter().encode(message, BarcodeFormat.QR_CODE, width, height);

            Bitmap bmap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    bmap.SetPixel(x, y, byteMatrix.get_Renamed(x, y) != -1 ? ColorTranslator.FromHtml("0xFF000000") : ColorTranslator.FromHtml("0xFFFFFFFF"));
                }
            }
            return bmap;
        }

        public static string UnCodeDr(string file)
        {
            Bitmap bmap = null;
            Uri fileUri = new Uri(file);
            if (fileUri.IsFile)
            {
                if (File.Exists(fileUri.LocalPath))
                {
                    Image img = Image.FromFile(file);
                    bmap = new Bitmap(img);
                }
            }
            else
            {               
                using (WebClient client = new WebClient())
                {
                    var data = client.DownloadData(fileUri);
                    MemoryStream ms = new MemoryStream(data);
                    Image img = Image.FromStream(ms);
                    bmap = new Bitmap(img);
                }
            }
            LuminanceSource source = new RGBLuminanceSource(bmap, bmap.Width, bmap.Height);
            com.google.zxing.BinaryBitmap bitmap = new com.google.zxing.BinaryBitmap(new com.google.zxing.common.HybridBinarizer(source));
            Result result = new MultiFormatReader().decode(bitmap);
            return result.Text;
        }
    }
}
