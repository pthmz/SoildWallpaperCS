using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace SoildWallpaperCS
{
    internal class Program
    {
        [DllImport("user32.dll")]
        private static extern int SystemParametersInfo(uint uiAction, uint uiParam, IntPtr pvParam, uint fWinIni);

        // SPI_SETDESKWALLPAPER = 20
        private const uint SPI_SETDESKWALLPAPER = 0x0014;

        // SPIF_UPDATEINIFILE = 1
        // SPIF_SENDCHANGE = 2
        private const uint SPIF_UPDATEINIFILE = 0x01;
        private const uint SPIF_SENDCHANGE = 0x02;

        static void Main()
        {
            Console.WriteLine("Setting desktop background to solid black...");
            string solidColorPath = @"C:\Windows\System32\solidcolor.bmp";

            // Generate a solid black bitmap file
            GenerateSolidColorBmp(solidColorPath);

            // Set the desktop wallpaper
            SetSolidDesktopBackground(solidColorPath);

            Console.WriteLine("Desktop background set to solid black.");
        }

        static void GenerateSolidColorBmp(string path)
        {
            using (Bitmap bmp = new Bitmap(1, 1))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.Clear(Color.Black); // Set to solid black
                }

                bmp.Save(path, ImageFormat.Bmp);
            }
        }

        static void SetSolidDesktopBackground(string imagePath)
        {
            IntPtr ptr = Marshal.StringToHGlobalAnsi(imagePath);
            try
            {
                SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, ptr, SPIF_UPDATEINIFILE | SPIF_SENDCHANGE);
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }
    }
}