using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.Versioning;

namespace signatureProcess.utils
{
    public class ImagesProcessing
    {

        private const int MAX_FILE_SIZE = 100 * 1000;


        [SupportedOSPlatform("windows")]
        public static string ConvertToJpegBase64(string tiffBase64)
        {
            byte[] tiffBytes = Convert.FromBase64String(tiffBase64);
            using (MemoryStream tiffStream = new MemoryStream(tiffBytes))
            {
                try
                {
                    using (var tiffImage = Image.FromStream(tiffStream))
                    {
                        if (tiffImage != null)
                        {
                            var bwImage = FilterImageProcess(tiffImage);

                            long quality = 100;
                            byte[] jpegBytes;
                            do
                            {
                                jpegBytes = ConvertImageToJpeg(bwImage, quality);
                                Console.WriteLine($"Image size: {jpegBytes.Length}");
                                quality -= 10; // Reducir la calidad de compresión en 10 puntos
                            }
                            while (jpegBytes.Length >= MAX_FILE_SIZE && quality >= 0);

                            Console.WriteLine($"End size Image: {jpegBytes.Length}");
                            Console.WriteLine($"Con calidad:{quality}");
                            Console.WriteLine("");

                            return Convert.ToBase64String(jpegBytes);
                        }
                        else
                        {
                            return "";
                        }
                    }
                }
                catch(ArgumentException ex)
                {
                    Console.WriteLine(ex.ToString());
                    return "";
                }
            }
        }

        [SupportedOSPlatform("windows")]
        private static byte[] ConvertImageToJpeg(Image image, long quality)
        {
            using (var outputStream = new MemoryStream())
            {

                var encoder = GetEncoder(ImageFormat.Jpeg);
                var encoderParameters = new EncoderParameters(1);
                encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, quality);
                if (encoder != null)
                {
                    image.Save(outputStream, encoder, encoderParameters);
                }
                return outputStream.ToArray();
            }
        }

        [SupportedOSPlatform("windows")]
        private static ImageCodecInfo? GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            return codecs.FirstOrDefault(codec => codec.FormatID == format.Guid);
        }

        [SupportedOSPlatform("windows")]
        private static Image FilterImageProcess(Image image)
        {
            Console.WriteLine("Entro a filter");
            Bitmap bwImage = new Bitmap(image.Width, image.Height, image.PixelFormat);

            using (Graphics gr = Graphics.FromImage(bwImage))
            {
                ColorMatrix colorMatrix = new ColorMatrix(
                    new float[][]
                    {
                new float[] {0.299f, 0.299f, 0.299f, 0, 0},
                new float[] {0.587f, 0.587f, 0.587f, 0, 0},
                new float[] {0.114f, 0.114f, 0.114f, 0, 0},
                new float[] {0, 0, 0, 1, 0},
                new float[] {0, 0, 0, 0, 1}
                    });

                ImageAttributes attributes = new ImageAttributes();
                attributes.SetColorMatrix(colorMatrix);

                gr.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height),
                    0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attributes);
            }

            return bwImage;
        }

    }
  
}