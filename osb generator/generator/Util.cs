using System.Drawing;
using System.IO;

namespace osb_generator.generator
{
    class Util
    {
        public static double GetTime(int min, int sec, int millisec)
        {
            return new Time(min, sec, millisec).OSBTime;
        }
        public static void Initialize(string loc)
        {
            GenerateLine(loc);
        }

        public static Sprite Line(double width, double height) => Line(0, width, height);
        public static Sprite Line(double width, double height, Origin o) => Line(0, width, height, o);
        public static Sprite Line(double start, double width, double height) => Line(start, height, width, Origin.Centre);
        public static Sprite Line(double start, double width, double height, Origin o)
        {
            Sprite a = new Sprite("sb/etc/line", o);
            a.Vector(start, new VectorSize(width / 9, height / 9));
            return a;
        }

        public static Sprite Circle(double width, double height) => Circle(height, width, Origin.Centre);
        public static Sprite Circle(double width, double height, Origin o)
        {
            Sprite a = new Sprite("sb/etc/circle", o);
            a.Vector(new VectorSize(width / 300, height / 300));
            return a;
        }

        private static void GenerateLine(string loc)
        {
            Bitmap a = new Bitmap(9, 9);
            for (var i = 0; i < a.Width; i++)
            {
                for (var j = 0; j < a.Height; j++)
                {
                    a.SetPixel(i, j, System.Drawing.Color.White);
                }
            }
            var q = loc + "\\etc\\line.jpg";
            a.Save(q, System.Drawing.Imaging.ImageFormat.Jpeg);

            a.Dispose();
        }
    }
}
