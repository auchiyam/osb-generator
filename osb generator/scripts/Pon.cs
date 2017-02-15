using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using osb_generator.generator;
using static osb_generator.generator.Util;

namespace osb_generator.scripts
{
    class Pon : Script
    {
        public Pon(string s) : base(s)
        {
        }

        public override void Initialize()
        {
            bpm = 195;
            seed = 1;
        }

        public override void MainCode()
        {
            //deletes the background
            Sprite delete = new generator.Sprite("camel.png");
            Layers.AddSprite("background", delete);

            Kiai();
        }

        //All the logic in kiai
        #region kiai
        public void Kiai()
        {
            DrawBackgrounds(GetTime(0, 53, 164), GetTime(1, 18, 394));
            DrawBackgrounds(GetTime(1, 59, 625), GetTime(2, 24, 856));
            DrawBackgrounds(GetTime(3, 27, 10), GetTime(3, 52, 241));
        }
        public void DrawBackgrounds(double start, double end)
        {

            MegamanBackground(start, end);

            DrawTileBackground(start, end);
            DrawTiles(start, end);
        }

        #region tiles
        public void DrawTileBackground(double starttime, double endtime)
        {
            //The background colors of tiles
            Sprite tback1 = Util.Line(450, 220, Origin.TopRight);
            Sprite tback2 = Util.Line(450, 220, Origin.TopLeft);
            Sprite tside1 = Util.Line(450, 20, Origin.TopRight);
            Sprite tside2 = Util.Line(450, 20, Origin.TopLeft);

            tback1.Move(starttime, 320, 240);
            tback2.Move(starttime, 320, 240);
            tside1.Move(starttime, 320, 460);
            tside2.Move(starttime, 320, 460);

            Color red = new ColorRGB(210, 210, 210);
            Color sred = (Color)(red - 30);
            Color blue = new ColorRGB(45, 45, 45);
            Color sblue = (Color)(blue - 30);

            Color black = new ColorRGB(0, 0, 0);

            double fadestart = endtime - beat * 4;
            double fadeend = endtime;

            tback1.Color(starttime, red);
            tback1.Color(fadestart, fadeend, red, black);

            tside1.Color(starttime, sred);
            tside1.Color(fadestart, fadeend, sred, black);

            tback2.Color(starttime, blue);
            tback2.Color(fadestart, fadeend, blue, black);

            tside2.Color(starttime, sblue);
            tside2.Color(fadestart, fadeend, sblue, black);

            tback1.Fade(starttime, starttime + beat * 1.5, 0, 1);
            tback2.Fade(starttime, starttime + beat * 1.5, 0, 1);
            tside1.Fade(starttime, starttime + beat * 1.5, 0, 1);
            tside2.Fade(starttime, starttime + beat * 1.5, 0, 1);

            tback1.Fade(fadestart, fadeend, 1, 0);
            tback2.Fade(fadestart, fadeend, 1, 0);
            tside1.Fade(fadestart, fadeend, 1, 0);
            tside2.Fade(fadestart, fadeend, 1, 0);

            Layers.AddSprite("kiai tiles", tback1, tback2, tside1, tside2);
        }

        public void DrawTiles(double start, double end)
        {
            var padding = 15;
            var height = 220 / 3;
            var width = 900 / 6;

            var blue = new ColorRGB(255, 255, 255);
            var red = new ColorRGB(50, 50, 50);
            for (var i = 0; i < 6; i++)
            {
                for (var j = 0; j < 3; j++)
                {
                    var grid = Circle(height - padding, width - padding);

                    var c = i < 3 ? red : blue;

                    grid.Move(i * width + 900 / 12 + (320 - 450), j * height + 220 / 6 + 240);
                    grid.Fade(start, start + beat * 1.5, 0, 1);
                    grid.Fade(end - beat * 4, end, 1, 0);

                    grid.Color(start, (Color)(c - j * 10));

                    Layers.AddSprite("kiai tiles", grid);
                }
            }
        }
        #endregion

        #region bg
        public void MegamanBackground(double start, double end)
        {
            Sprite bg = Line(480, 900);
            bg.Color(new ColorRGB(146, 200, 190));
            bg.Fade(start, start + beat * 1.5, 0, 1);
            bg.Fade(end - beat * 4, end, 1, 0);
            Layers.AddSprite("kiai bg", bg);

            var dur = 10000;
            var t = start - dur + random.Next(100, 1000);
            var freq = 1500;
            while (t < end - dur / 2)
            {
                var size = random.Next(40, 120);
                var x = random.Next(-130, 770);

                var fadestart = t;
                var fadeend = t + dur;
                if (t < start)
                {
                    fadestart = start;
                }

                if (t + dur < start + beat * 2)
                {
                    fadeend = start + beat * 2;
                }

                if (t + dur > end - beat * 4)
                {
                    fadeend = end;
                }


                CircleDucks(size, fadestart, fadeend, t, t + dur, x, 600, x, -120);
                t += random.Next(0, freq);
            }
        }

        /// <summary>
        /// O
        /// .
        /// .
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public void CircleDucks(double size, double fadestart, double fadeend, double start, double end, double x1, double y1, double x2, double y2)
        {
            var a = Circle(size, size);
            var b = Circle(size / 3, size / 3);
            var c = Circle(size / 3, size / 3);
            a.Move(start, end, x1, y1, x2, y2);
            b.Move(start + beat * 3, end + beat * 3, x1, y1, x2, y2);
            c.Move(start + beat * 6, end + beat * 6, x1, y1, x2, y2);

            a.Fade(fadestart, fadestart + beat, 0, 1);
            b.Fade(fadestart, fadestart + beat, 0, 1);
            c.Fade(fadestart, fadestart + beat, 0, 1);

            a.Fade(fadeend - beat, fadeend, 1, 0);
            b.Fade(fadeend - beat, fadeend, 1, 0);
            c.Fade(fadeend - beat, fadeend, 1, 0);

            Color u = new ColorHSV(random.Next(0, 360) / 360.0, .4, .9);

            a.Color(Math.Min(fadestart, start), u);
            b.Color(Math.Min(fadestart, start), u);
            c.Color(Math.Min(fadestart, start), u);

            a.Additive();
            b.Additive();
            c.Additive();

            Layers.AddSprite("kiai bg", a, b, c);
        }
        #endregion
        #endregion

        public override void SetLayers()
        {
            Layers.CreateLayer("background");

            Layers.CreateLayer("kiai bg");
            Layers.CreateLayer("kiai tiles");
        }
    }
}
