using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace ttTVAdmin
{
    public class ImageManager
    {
        public const int MaxFullImageSize = 2000;

        public const int FixedMediumImageHeight = 180; // 235;
        public const int FixedMediumImageWidth = 180; // 235;

        public const int FixedSmallImageHeight = 102; //42;
        public const int FixedSmallImageWidth = 136; //56;

        public enum PhotoSize
        {
            None = 0,
            Small = 1,
            Medium = 2,
            Full = 3
        }

        public static byte[] ResizeImageFile(byte[] imageFile, PhotoSize size)
        {
            using (System.Drawing.Image original = System.Drawing.Image.FromStream(new MemoryStream(imageFile)))
            {
                int targetH, targetW;
                if (size == PhotoSize.Small || size == PhotoSize.Medium)
                {
                    // regardless of orientation,
                    // the *height* is constant for thumbnail images (UI constraint)

                    if (original.Height > original.Width)
                    {
                        if (size == PhotoSize.Small)
                            targetH = FixedSmallImageHeight;
                        else
                            targetH = FixedMediumImageHeight;

                        targetW = (int)(original.Width * ((float)targetH / (float)original.Height));
                    }
                    else
                    {
                        if (size == PhotoSize.Small)
                            targetW = FixedSmallImageWidth;
                        else
                            targetW = FixedMediumImageWidth;

                        targetH = (int)(original.Height * ((float)targetW / (float)original.Width));
                    }
                }
                else
                {
                    // for full preview, we scale proportionally according to orienation
                    if (original.Height > original.Width)
                    {
                        targetH = Math.Min(original.Height, MaxFullImageSize);
                        targetW = (int)(original.Width * ((float)targetH / (float)original.Height));
                    }
                    else
                    {
                        targetW = Math.Min(original.Width, MaxFullImageSize);
                        targetH = (int)(original.Height * ((float)targetW / (float)original.Width));
                    }
                }

                using (System.Drawing.Image imgCTOMasterPhoto = System.Drawing.Image.FromStream(new MemoryStream(imageFile)))
                {
                    // Create a new blank canvas.  The resized image will be drawn on this canvas.
                    using (Bitmap bmCTOMasterPhoto = new Bitmap(targetW, targetH, PixelFormat.Format24bppRgb))
                    {
                        bmCTOMasterPhoto.SetResolution(72, 72);

                        using (Graphics grCTOMasterPhoto = Graphics.FromImage(bmCTOMasterPhoto))
                        {
                            grCTOMasterPhoto.SmoothingMode = SmoothingMode.AntiAlias;
                            grCTOMasterPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            grCTOMasterPhoto.PixelOffsetMode = PixelOffsetMode.HighQuality;
                            grCTOMasterPhoto.DrawImage(imgCTOMasterPhoto, new Rectangle(0, 0, targetW, targetH), 0, 0, original.Width, original.Height, GraphicsUnit.Pixel);

                            MemoryStream mm = new MemoryStream();
                            bmCTOMasterPhoto.Save(mm, System.Drawing.Imaging.ImageFormat.Jpeg);
                            return mm.GetBuffer();
                        }
                    }
                }
            }
        }
    }
}