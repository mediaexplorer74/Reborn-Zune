using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace Reborn_Zune.Utilities
{
    public static class Utility
    {
        public async static Task<BitmapImage> ImageFromBytes(Byte[] bytes)
        {
            BitmapImage image = new BitmapImage();
            try
            {
                if (bytes.Length == 0) throw new Exception();
                using (InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream())
                {
                    await stream.WriteAsync(bytes.AsBuffer());
                    stream.Seek(0);
                    await image.SetSourceAsync(stream);

                }
                return image;
            }
            
            catch(Exception e)
            {
                return image;
            }
        }
    }
}
