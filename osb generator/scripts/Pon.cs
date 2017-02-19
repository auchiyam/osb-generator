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
            seed = 2;
        }

        public override void MainCode()
        {
            //deletes the background
            Sprite delete = new generator.Sprite("camel.png");
            Layers.AddSprite("background", delete);

            Kiai();
            PartA();
            BridgeA();
        }

        #region Part A
        public void PartA()
        {

        }
        #endregion

        #region A to Kiai
        public void BridgeA()
        {

        }
        #endregion

        //All the logic in kiai
        #region kiai
        public void Kiai()
        {
            double start1 = GetTime(0, 53, 164), end1 = GetTime(1, 18, 394);
            double start2 = GetTime(1, 59, 625), end2 = GetTime(2, 24, 856);
            double start3 = GetTime(3, 27, 10), end3 = GetTime(3, 52, 241);

            //Character battles
            int[] left = { 4, 7, 9, 12, 2, 3, 11 };
            int[] right = { 1, 5, 10, 15, 6, 8, 13, 14 };
            DrawBattle1(start1, end1, left, right);

            //Background
            DrawBackgrounds(start1, end1);
            DrawBackgrounds(start2, end2);
            DrawBackgrounds(start3, end3);
        }

        #region Battle
        public void DrawBattle1(double start, double end, int[] side1, int[] side2)
        {
            if (!(side1.Length <= 9 && side2.Length <= 9))
            {
                throw new Exception("Side 1 and side 2 cannot be more than 9 characters");
            }

            //The grid # used for left and right grid respectively
            int[] left = { 0, 1, 2, 6, 7, 8, 12, 13, 14 };
            int[] right = { 3, 4, 5, 9, 10, 11, 15, 16, 17 };

            //the grid representing the field.  0 = empty, [1,16] = char id
            int[][] grid =
            {
                new int[] { 0, 0, 0, 0, 0, 0 },
                new int[] { 0, 0, 0, 0, 0, 0 },
                new int[] { 0, 0, 0, 0, 0, 0 }
            };

            //filling the left grid with char id
            foreach (var c in side1)
            {
                var x = 0;
                var y = 0;
                var z = 0;
                do
                {
                    z = random.Next(left.Length);
                    x = left[z] % 6;
                    y = left[z] / 6;
                } while (grid[y][x] != 0);

                grid[y][x] = c;
            }

            //filling the right grid with char id
            foreach (var c in side2)
            {
                var x = 0;
                var y = 0;
                var z = 0;
                do
                {
                    z = random.Next(right.Length);
                    x = right[z] % 6;
                    y = right[z] / 6;
                } while (grid[y][x] != 0);

                grid[y][x] = c;
            }

            DrawCharacters(start, end, grid);
        }

        public Sprite CreateCharacter(int charnum, int col, int row, double start)
        {
            Sprite s = Layers.CreateSprite($"kiai chars{row}", $"sb/char/{charnum}", Origin.BottomCentre);
            var width = 900 / 6;
            var height = 220 / 3;

            s.Move(start, col * width + 900 / 12 + (320 - 450), row * height + 220 / 6 + 240);
            return s;
        }

        public void DrawCharacters(double start, double end, int[][] grid)
        {
            for (var i = 0; i < grid.Length; i++)
            {
                for (var j = 0; j < grid[i].Length; j++)
                {
                    if (grid[i][j] == 0)
                        continue;

                    var side = 0;
                    if (j >= 3)
                    {
                        side = 1;
                    }

                    var id = grid[i][j];
                    Sprite c = CreateCharacter(id, j, i, start);
                    var t = start + random.Next((int)(beat * 1.5), (int)(beat * 3));
                    var animations = 2;
                    c.Fade(start, start + beat, 0, 1);
                    c.Fade(end - beat, end, 1, 0);
                    while (t < end - beat)
                    {
                        List<int> supporters = new List<int>{ 1, 6, 7, 8, 12 };

                        var target = 0;

                        //if supporter, target ally
                        if (supporters.Contains(id))
                        {
                            target = TargetGrid(grid, side);
                        }
                        //else target enemy
                        else
                        {
                            var s = side == 0 ? 1 : 0;
                            target = TargetGrid(grid, s);
                        }

                        AnimateCharacter(c, id, random.Next(0, animations), t, end, side == 0 ? 1 : -1, target);
                        t += beat * random.Next(6, 10);
                    }
                }
            }
        }

        public int TargetGrid(int[][] grid, int side)
        {
            //0 = left, 1 = right side

            var x = 0;
            var y = 0;

            do
            {
                x = random.Next(0 + 3 * side, 3 + 3 * side);
                y = random.Next(0, 3);
            } while (grid[y][x] == 0);
            return y * 6 + x;
        }

        public void AnimateCharacter(Sprite character, int id, int animationtype, double start, double fadeend, int dir, int target)
        {
            Coordinates origin = (Coordinates)character.Value(start)[CommandType.Move];

            switch (animationtype)
            {
                //Crouch, jump up, 1 spin, tilt back (attack), return to position
                case 0:
                    Crouch(character, start, start + beat / 2);
                    JumpUp(character, start + beat / 2 - 100, start + beat * 2.5);
                    Spin(character, start + beat, start + beat * 2, 1);
                    if (start + beat < fadeend - beat * 4)
                        Attack(id, start + beat, (Coordinates)(origin - new Coordinates(0, 60)), target);
                    Crouch(character, start + beat * 2.5, start + beat * 3);
                    break;
                
                //1 spin, tilt forward, 1 spin, tilt back (attack), 1 spin, return
                case 1:
                    Spin(character, start, start + beat, 1);
                    Tilt(character, start, start + beat * 2, -1 * dir);
                    Spin(character, start + beat * 2, start + beat * 3, 1);
                    Tilt(character, start + beat * 2, start + beat * 4, 1 * dir);
                    Spin(character, start + beat * 4, start + beat * 6, 2);
                    if (start + beat * 5 < fadeend - beat * 4)
                        Attack(id, start + beat * 5, (Coordinates)(origin), target);
                    break;

                //tilt back, jump back, tilt forward, jump forward, return, crouch, jump up, 1 spin, return to position
                case 2:
                    Tilt(character, start, start + beat, -1);
                    

                    Tilt(character, start + beat, start + beat * 2, 1);


                    break;
                
                //Crouch, jump up, 1 spin, tilt back, return, jump side
                case 3:
                    break;
            }
        }

        #region Animation - movement
        public void Crouch(Sprite character, double start, double end)
        {
            character.Vector(start, start + 30, .7, .7, .7, .6);
            character.Vector(end - 30, end, .7, .6, .7, .7);
        }

        public void Tilt(Sprite character, double start, double end, int dir)
        {
            var half = (end - start) / 2 + start;
            var tilt = 20 * dir * -1;
            character.Rotate(start, half - 30, 0, tilt);
            character.Rotate(half + 30, end, tilt, 0);
        }

        public void JumpUp(Sprite character, double start, double end)
        {
            Coordinates startpos = (Coordinates)character.Value(start)[CommandType.Move];
            Coordinates endpos = (Coordinates)(startpos - new Coordinates(0, 60));
            character.Move(EaseType.EasingOut, start, start + 30, startpos, endpos);
            character.Move(EaseType.EasingIn, end - 30, end, endpos, startpos);
        }

        public void JumpUp(Sprite character, double start, double end, Coordinates land)
        {
            Coordinates startpos = (Coordinates)character.Value(start)[CommandType.Move];
            Coordinates endpos = (Coordinates)(startpos - new Coordinates(0, 20));
            character.Move(EaseType.EasingOut, start, start + 30, startpos, endpos);
            character.Move(EaseType.EasingIn, end - 30, end, endpos, land);
        }

        public void JumpSide(Sprite character, double start, double end, int dir)
        {
            Coordinates startpos = (Coordinates)character.Value(start)[CommandType.Move];
            Coordinates endpos = (Coordinates)(startpos - new Coordinates(10 * dir, 0));
            character.Move(EaseType.EasingOut, start, start + 30, startpos, endpos);
            character.Move(EaseType.EasingIn, end - 30, end, endpos, startpos);
        }

        public void JumpSide(Sprite character, double start, double end, int dir, Coordinates land)
        {
            Coordinates startpos = (Coordinates)character.Value(start)[CommandType.Move];
            Coordinates endpos = (Coordinates)(startpos - new Coordinates(10 * dir, 0));
            character.Move(EaseType.EasingOut, start, start + 30, startpos, endpos);
            character.Move(EaseType.EasingIn, end - 30, end, endpos, land);
        }

        public void JumpBackAndForth(Sprite character, double start, double end, int dir)
        {
            Coordinates startpos = (Coordinates)character.Value(start)[CommandType.Move];
            Coordinates sidepos1 = (Coordinates)(startpos - new Coordinates(10 * dir, 0));
            Coordinates sidepos2 = (Coordinates)(startpos - new Coordinates(10 * dir * -1, 0));

            double mid = (end - start) / 2 + start;

            character.MoveX(EaseType.EasingOut, start, start + 30, startpos.X, sidepos1.X);
            character.MoveX(EaseType.QuadInOut, mid - 15, mid + 15, sidepos1.X, sidepos2.X);
            character.MoveX(EaseType.EasingIn, end - 30, end, sidepos2.X, startpos.X);
        }

        public void JumpReturn(Sprite character, double start, Coordinates origin)
        {
            Coordinates startpos = (Coordinates)character.Value(start)[CommandType.Move];
            character.Move(EaseType.EasingOut, start, start + 30, startpos, origin);
        }

        public void Spin(Sprite character, double start, double end, int amount)
        {
            var dur = (end - start) / amount / 4;
            var loop = character.CreateLoop(start, amount);
            loop.Vector(EaseType.SineIn, 0, dur, .7, .7, 0, .7);
            loop.Vector(EaseType.SineOut, dur, dur * 2, 0, .7, .7, .7);
            loop.Vector(EaseType.SineIn, dur * 2, dur * 3, .7, .7, 0, .7);
            loop.Vector(EaseType.SineOut, dur * 3, dur * 4, 0, .7, .7, .7);
            loop.Flip(dur, dur * 3, "H");
        }
        #endregion

        #region Animation - attack
        public void Attack(int id, double start, Coordinates o, int target)
        {
            switch (id)
            {
                case 1:
                    ColorRGB c = new ColorRGB(125, 255, 106);
                    Support(start, target, c);
                    break;
                case 2:
                    Shoot(start, o, target, "sb/atk/sh", false, new ColorRGB(33, 33, 33));
                    break;
                case 3:
                    Slash(start, target);
                    break;
                case 4:
                    Shoot(start, o, target, "sb/atk/st", false, new ColorHSV(random.Next(0, 360) / 360.0, .44, .89), new ColorHSV(random.Next(0, 360) / 360.0, .44, .89), new ColorHSV(random.Next(0, 360) / 360.0, .44, .89));
                    break;
                case 5:
                    Shoot(start, o, target, "sb/atk/bl", false, new ColorRGB(56, 56, 56));
                    break;
                case 6:
                    ColorRGB d = new ColorRGB(125, 255, 106);
                    Support(start, target, d);
                    break;
                case 7:
                    ColorRGB f = new ColorRGB(255, 114, 107);
                    Support(start, target, f);
                    break;
                case 8:
                    ColorRGB g = new ColorRGB(184, 184, 184);
                    Support(start, target, g);
                    break;
                case 9:
                    Shoot(start, o, target, "sb/atk/kn", false, new ColorRGB(80, 80, 80));
                    break;
                case 10:
                    Claw(start, target);
                    break;
                case 11:
                    Beam(start, o, target, new ColorRGB(95, 255, 219));
                    break;
                case 12:
                    ColorRGB e = new ColorRGB(125, 255, 106);
                    Support(start, target, e);
                    break;
                case 13:
                    Rock(start, target);
                    break;
                case 14:
                    Shoot(start, o, target, "sb/atk/wv", false, new ColorRGB(255, 123, 163));
                    break;
                case 15:
                    Skull(start, target);
                    break;
            }
        }

        public void Support(double start, int target, Color color)
        {
            var offset = 20;
            var amount = 12;
            for (var i = 0; i < amount; i++)
            {
                Sprite back = Layers.CreateSprite($"kiai spell back{target / 6}", "sb/atk/rb", Origin.BottomCentre);
                Sprite front = Layers.CreateSprite($"kiai spell front{target / 6}", "sb/atk/rf", Origin.BottomCentre);

                var stime = start + offset * i;
                var etime = start + beat / 2 + offset * i;

                var width = 900 / 6;
                var height = 220 / 3;
                var row = target / 6;
                var col = target % 6;

                Coordinates c = new Coordinates(col * width + 900 / 12 + (320 - 450), row * height + 220 / 6 + 240);

                back.Move(stime, (Coordinates)(c + new generator.Coordinates(0, -10 * i)));
                front.Move(stime, (Coordinates)(c + new generator.Coordinates(0, -10 * i)));

                back.Color(stime, color);
                front.Color(stime, color);

                back.Fade(stime, stime + 10, 0, .5);
                front.Fade(stime, stime + 10, 0, .5);

                back.Fade(etime -10, etime, .5, 0);
                front.Fade(etime - 10, etime, .5, 0);
            }
        }

        public void Shoot(double start, Coordinates origin, int target, string image, bool spin, params Color[] colors)
        {
            int amount = 3;
            var offset = 100;
            for (var i = 0; i < amount; i++)
            {
                Color cl;
                if (colors.Length == 1)
                    cl = colors[0];
                else
                    cl = colors[i];
                Sprite bullet = Layers.CreateSprite($"kiai spell front{target / 6}", image);

                var stime = start + offset * i;
                var etime = start + 500 + offset * i;

                var width = 900 / 6;
                var height = 220 / 3;
                var row = target / 6;
                var col = target % 6;

                Coordinates c = (Coordinates)(new Coordinates(col * width + 900 / 12 + (320 - 450), row * height + 220 / 6 + 240) + new Coordinates(random.Next(-40, 40), random.Next(-40, 40) - 80));
                Coordinates o = (Coordinates)(origin + new Coordinates(random.Next(-40, 40), random.Next(-40, 40) - 80));
                var angle = Math.Tan((c.Y - o.Y) / (c.X - o.X));

                bullet.Move(stime, etime, o, c);
                if (!spin)
                    bullet.Rotate(stime, new Angle(angle, true));
                else
                    bullet.Rotate(stime, etime, new Angle(angle, true), (Angle)(new Angle(angle, true) + new Angle(720)));
                if (target % 6 < 3)
                {
                    bullet.Flip(stime, etime, "H");
                }

                bullet.Color(stime, cl);
            }
        }

        public void Beam(double start, Coordinates origin, int target, params Color[] colors)
        {
            int amount = 3;
            var offset = 100;
            for (var i = 0; i < amount; i++)
            {
                Color cl;
                if (colors.Length == 1)
                    cl = colors[0];
                else
                    cl = colors[i];

                var stime = start + offset * i;
                var etime = start + 500 + offset * i;

                Sprite bullet = Line(stime, 160, 5);
                Layers.AddSprite($"kiai spell front{target / 6}", bullet);

                var width = 900 / 6;
                var height = 220 / 3;
                var row = target / 6;
                var col = target % 6;

                Coordinates c = (Coordinates)(new Coordinates(col * width + 900 / 12 + (320 - 450), row * height + 220 / 6 + 240) + new Coordinates(random.Next(-40, 40), random.Next(-40, 40) - 80));
                Coordinates o = (Coordinates)(origin + new Coordinates(random.Next(-40, 40), random.Next(-40, 40) - 80));
                var angle = Math.Tan((c.Y - o.Y) / (c.X - o.X));

                bullet.Move(stime, etime, o, c);
                bullet.Rotate(stime, (Angle)(new Angle(angle, true) + new Angle(90)));
                if (target % 6 < 3)
                {
                    bullet.Flip(stime, etime, "H");
                }

                bullet.Color(stime, cl);
            }
        }

        public void Slash(double start, int target)
        {
            var amount = random.Next(2, 7);
            var offset = 40;
            for (var i = 0; i < amount; i++)
            {
                var slash = Layers.CreateSprite($"kiai spell front{target / 6}", "sb/atk/cl");

                var stime = start + offset * i;
                var etime = stime + beat * 6;

                var width = 900 / 6;
                var height = 220 / 3;
                var row = target / 6;
                var col = target % 6;

                Coordinates c = new Coordinates(col * width + 900 / 12 + (320 - 450) + random.Next(-40, 40), row * height + 220 / 6 + 240 + random.Next(-40, 40) - 80);

                slash.Move(stime, c);
                slash.Rotate(stime, new Angle(random.Next(0, 360)));
                slash.Fade(stime, etime, 1, 0);
                slash.Scale(stime, 2);
            }
        }

        public void Claw(double start, int target)
        {
            var amount = 3;
            var offset = 0;
            for (var i = 0; i < amount; i++)
            {
                var slash = Layers.CreateSprite($"kiai spell front{target / 6}", "sb/atk/cl");

                var stime = start + offset * i;
                var etime = stime + beat * 3;

                var width = 900 / 6;
                var height = 220 / 3;
                var row = target / 6;
                var col = target % 6;

                Coordinates c = new Coordinates(col * width + 900 / 12 + (320 - 450) + -40 + 40 * i, row * height + 220 / 6 + 240 - 80);

                slash.Move(stime, c);
                slash.Rotate(stime, new Angle(-10 + 10 * i));
                slash.Fade(stime, etime, 1, 0);
                slash.Scale(stime, 1.5);
                slash.Color(stime, new ColorRGB(255, 15, 15));
            }
        }

        public void Skull(double start, int target)
        {
            var c = GetTarget(target);

            var skull = Layers.CreateSprite($"kiai spell front{target / 6}", "sb/atk/sk");

            skull.Move(start, c);
            skull.Fade(start, start + beat * 3, 1, 0);
            skull.Scale(start, .3);

            var pulse = Layers.CreateSprite($"kiai spell front{target / 6}", "sb/atk/sk");

            pulse.Move(start, c);
            pulse.Fade(start - 2, start - 1, 0, 1);
            pulse.Fade(start, start + beat * 3, 1, 0);
            pulse.Additive();
            pulse.Scale(start, start + beat * 3, .3, .4);
        }

        public void Rock(double start, int target)
        {
            bool[] used = { false, false, false, false, false };
            var offset = 100;

            var c = (Coordinates)(GetTarget(target) + new Coordinates(0, 30));

            for (var i = 0; i < 5; i++)
            {
                var stime = start + offset * i;
                var etime = stime + beat * 2;
                var type = 0;
                do
                {
                    type = random.Next(1, 6);
                } while (used[type - 1]);

                used[type - 1] = true;

                var rock = Layers.CreateSprite($"kiai spell front{target / 6}", $"sb/atk/rk{type}");

                rock.Move(stime, c);
                rock.Fade(stime, stime + 10, 0, 1);
                rock.Fade(etime - beat / 2, etime, 1, 0);
                rock.Scale(stime, 1);

                rock.Color(stime, new ColorRGB(87, 64, 41));
            }
        }
        
        public Coordinates GetTarget(int target)
        {
            var width = 900 / 6;
            var height = 220 / 3;
            var row = target / 6;
            var col = target % 6;

            return new Coordinates(col * width + 900 / 12 + (320 - 450), row * height + 220 / 6 + 240 - 80);
        }
        #endregion
        #endregion

        #region Kiai bg
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

            var dur = 5000;
            var t = start - dur + random.Next(100, 1000);
            var freq = 500;
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

        #endregion

        #region Ima Sakebe
        #endregion

        public override void SetLayers()
        {
            Layers.CreateLayer("background");

            Layers.CreateLayer("kiai bg");
            Layers.CreateLayer("kiai tiles");
            Layers.CreateLayer("kiai spell back0");
            Layers.CreateLayer("kiai chars0");
            Layers.CreateLayer("kiai spell front0");
            Layers.CreateLayer("kiai spell back1");
            Layers.CreateLayer("kiai chars1");
            Layers.CreateLayer("kiai spell front1");
            Layers.CreateLayer("kiai spell back2");
            Layers.CreateLayer("kiai chars2");
            Layers.CreateLayer("kiai spell front2");
        }
    }
}
