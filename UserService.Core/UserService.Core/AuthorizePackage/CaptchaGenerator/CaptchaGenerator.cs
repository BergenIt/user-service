using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

using UserService.Core.AuditPackage.AuditException;

namespace UserService.Core.CaptchaGenerator
{
    /// <summary>
    /// Генератор капчи
    /// </summary>
    public class CaptchaGenerator : ICaptchaGenerator
    {
        /// <summary>
        /// Словарь символов для генерации
        /// </summary>
        private const string Letters = "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm123467890";
        private const int CaptchaLenght = 8;

        /// <summary>
        /// Валидация капчи с фронта
        /// </summary>
        /// <param name="userInputCaptcha">Введеное юзером значение</param>
        /// <param name="hashCode">Хеш исходной капчи</param>
        /// <returns></returns>
        public void ValidateCaptchaCode(string userInputCaptcha, string hashCode)
        {
            if (HashHMACSHA1(userInputCaptcha) != hashCode)
            {
                throw new CaptchaCodeException();
            }
        }

        /// <summary>
        /// Генератор изображения капчи
        /// </summary>
        /// <param name="width">Размер</param>
        /// <param name="height">Размер</param>
        /// <returns></returns>
        public CaptchaResult GenerateCaptchaImage(int width, int height)
        {
            string captchaCode = GenerateCaptchaCode();

            using Bitmap baseMap = new(width, height);
            using Graphics graph = Graphics.FromImage(baseMap);
            Random rand = new();

            graph.Clear(GetRandomLightColor(rand));

            DrawCaptchaCode(width, height, captchaCode, graph, rand);
            DrawDisorderLine(width, width, height, graph, rand);
            AdjustRippleEffect(baseMap, rand);

            MemoryStream ms = new();

            baseMap.Save(ms, ImageFormat.Png);

            return new CaptchaResult(ms.ToArray(), HashHMACSHA1(captchaCode));
        }

        private string GenerateCaptchaCode()
        {
            Random rand = new();
            int maxRand = Letters.Length - 1;

            StringBuilder sb = new();

            for (int i = 0; i < CaptchaLenght; i++)
            {
                int index = rand.Next(maxRand);
                _ = sb.Append(Letters[index]);
            }

            return sb.ToString();
        }

        private Color GetRandomLightColor(Random rand)
        {
            const int low = 180, high = 255;

            int nRend = (rand.Next(high) % (high - low)) + low;
            int nGreen = (rand.Next(high) % (high - low)) + low;
            int nBlue = (rand.Next(high) % (high - low)) + low;

            return Color.FromArgb(nRend, nGreen, nBlue);
        }

        private Color GetRandomDeepColor(Random rand)
        {
            const int redlow = 160, greenLow = 100, blueLow = 160;
            return Color.FromArgb(rand.Next(redlow), rand.Next(greenLow), rand.Next(blueLow));
        }

        private int GetFontSize(int imageWidth, int captchCodeCount)
        {
            int averageSize = imageWidth / captchCodeCount;

            return Convert.ToInt32(averageSize);
        }

        private void DrawCaptchaCode(int width, int height, string captchaCode, Graphics graph, Random rand)
        {
            SolidBrush fontBrush = new(Color.Black);
            int fontSize = GetFontSize(width, captchaCode.Length);
            Font font = new(FontFamily.GenericSerif, fontSize, FontStyle.Bold, GraphicsUnit.Pixel);
            for (int i = 0; i < captchaCode.Length; i++)
            {
                fontBrush.Color = GetRandomDeepColor(rand);

                int shiftPx = fontSize / 6;

                float x = (i * fontSize) + rand.Next(-shiftPx, shiftPx) + rand.Next(-shiftPx, shiftPx);
                int maxY = height - fontSize;
                if (maxY < 0)
                {
                    maxY = 0;
                }

                float y = rand.Next(0, maxY);

                graph.DrawString(captchaCode[i].ToString(), font, fontBrush, x, y);
            }
        }

        private void DrawDisorderLine(int widht, int width, int height, Graphics graph, Random rand)
        {
            Pen linePen = new(new SolidBrush(Color.Black), widht / 35);
            for (int i = 0; i < rand.Next(3, 5); i++)
            {
                linePen.Color = GetRandomDeepColor(rand);

                Point startPoint = new(rand.Next(0, width), rand.Next(0, height));
                Point endPoint = new(rand.Next(0, width), rand.Next(0, height));
                graph.DrawLine(linePen, startPoint, endPoint);
            }
        }

        private void AdjustRippleEffect(Bitmap baseMap, Random rand)
        {
            const short nWave = 6;
            int nWidth = baseMap.Width;
            int nHeight = baseMap.Height;

            Point[,] pt = new Point[nWidth, nHeight];

            for (int x = 0; x < nWidth; ++x)
            {
                for (int y = 0; y < nHeight; ++y)
                {
                    var xo = nWave * Math.Sin(2.0 * 3.1415 * y / 128.0);
                    var yo = nWave * Math.Cos(2.0 * 3.1415 * x / 128.0);

                    var newX = x + xo;
                    var newY = y + yo;

                    if (newX > 0 && newX < nWidth)
                    {
                        pt[x, y].X = (int)newX;
                    }
                    else
                    {
                        pt[x, y].X = 0;
                    }


                    if (newY > 0 && newY < nHeight)
                    {
                        pt[x, y].Y = (int)newY;
                    }
                    else
                    {
                        pt[x, y].Y = 0;
                    }
                }
            }

            Bitmap bSrc = (Bitmap)baseMap.Clone();

            BitmapData bitmapData = baseMap.LockBits(new Rectangle(0, 0, baseMap.Width, baseMap.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData bmSrc = bSrc.LockBits(new Rectangle(0, 0, bSrc.Width, bSrc.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            IntPtr scan0 = bitmapData.Scan0;
            IntPtr srcScan0 = bmSrc.Scan0;

            {
                List<byte> p = BitConverter.GetBytes(scan0.ToInt64()).ToList();
                List<byte> pSrc = BitConverter.GetBytes(srcScan0.ToInt64()).ToList(); // size 8

                int nOffset = bitmapData.Stride - (baseMap.Width * 3);

                for (int y = 0; y < nHeight; ++y)
                {
                    for (int x = 0; x < nWidth; ++x)
                    {
                        int xOffset = pt[x, y].X;
                        int yOffset = pt[x, y].Y;

                        if (yOffset >= 0 && yOffset < nHeight && xOffset >= 0 && xOffset < nWidth)
                        {
                            if (pSrc != null)
                            {
                                p[0] = pSrc[rand.Next(0, 7)];
                                p[1] = pSrc[rand.Next(0, 7)];
                                p[2] = pSrc[rand.Next(0, 7)];
                            }
                        }

                        p.Add(3);
                    }
                    p.AddRange(BitConverter.GetBytes(nOffset));
                }
            }

            baseMap.UnlockBits(bitmapData);
            bSrc.UnlockBits(bmSrc);
            bSrc.Dispose();
        }

        private string HashHMACSHA1(string value)
        {
            byte[] unicodeValue = new byte[value.Length * 2];

            _ = Encoding.ASCII.GetEncoder().GetBytes(value.ToCharArray(), 0, value.Length, unicodeValue, 0, true);

            using HMACSHA1 hasher = new(Encoding.UTF8.GetBytes(value));

            value += DateTime.UtcNow.Date.ToString();

            byte[] valueHash = hasher.ComputeHash(unicodeValue);

            return Convert.ToBase64String(valueHash);
        }
    }
}
