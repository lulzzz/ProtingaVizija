﻿using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace FaceAnalysis
{
    public class HelperMethods
    {
        /// <summary>
        /// Converts bitmap to byte array
        /// </summary>
        /// <param name="img">Image in bitmap form</param>
        /// <returns>Image in byte[]</returns>
        public static byte[] ImageToByte(Bitmap img)
        {
            byte[] array = null;
            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    img.Save(stream, ImageFormat.Jpeg);
                    img.Dispose();
                    array = stream.ToArray();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return null;
            }
            return array;
        }

        /// <summary>
        /// Crops an image to a given rectangle + given percentage
        /// </summary>
        /// <param name="img">Image in bitmap form</param>
        /// <returns>Cropped image in bitmap form</returns>
        public static Bitmap CropImage(Bitmap img, Rectangle cropRectangle, int percentage)
        {
            cropRectangle.Inflate(cropRectangle.Width * percentage / 100, cropRectangle.Height * percentage / 100);
            Bitmap imgClone = new Bitmap(cropRectangle.Width, cropRectangle.Height, PixelFormat.Format24bppRgb);
            Debug.WriteLine(img.Width + " " + img.Height);
            Debug.WriteLine(cropRectangle);
            using (Graphics gr = Graphics.FromImage(imgClone))
            {
                gr.DrawImage(img, new Rectangle(0, 0, imgClone.Width, imgClone.Height),
                             cropRectangle, GraphicsUnit.Pixel);
            }
            img.Dispose();
            return imgClone;
        }

        /// <summary>
        /// Processes a bitmap so that it is (more likely to be) accepted by API.
        /// Previous version of image is disposed.
        /// Might need refinement in the future (higher res might be better than better quality in some cases)
        /// Works for now
        /// </summary>
        /// <param name="img">Image in bitmap form</param>
        /// <returns>Processed image in bitmap form</returns>
        public static Bitmap ProcessImage(Bitmap img)
        {
            const int MAX_SIZE = 1000;
            var ratio = Math.Min((double)MAX_SIZE / img.Width, (double)MAX_SIZE / img.Height);
            var newWidth = (int)(img.Width * ratio);
            var newHeight = (int)(img.Height * ratio);

            Bitmap imgClone = new Bitmap(newWidth, newHeight, PixelFormat.Format24bppRgb);
            using (Graphics gr = Graphics.FromImage(imgClone))
            {
                gr.DrawImage(img, 0, 0, imgClone.Width, imgClone.Height);
            }
            img.Dispose();
            return imgClone;
        }

    }
}