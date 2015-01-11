using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using ServiceStack;

namespace LearningLoop.Core.Infrastructure
{
    public class ImageProcessing
    {
        static readonly string UploadsDir = "~/uploads".MapHostAbsolutePath();

        private static string GetMd5Hash(Stream stream)
        {
            var hash = MD5.Create().ComputeHash(stream);
            var sb = new StringBuilder();
            foreach (byte t in hash)
            {
                sb.Append(t.ToString("x2"));
            }
            return sb.ToString();
        }

        public static string WriteImage(Stream ms)
        {
            var hash = GetMd5Hash(ms);

            ms.Position = 0;
            var fileName = hash + ".png";
            using (var img = Image.FromStream(ms))
            {
                img.Save(UploadsDir.CombineWith(fileName));
            }
            return fileName;
        }
    }
}
