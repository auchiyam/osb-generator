using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region TODO
/// <TODO>
/// Make To2D on Coordinate
/// Make ColorHSV
/// Add/Subtract of Value
/// </TODO>
#endregion
namespace osb_generator.generator
{
    abstract class Value
    {
        public abstract override string ToString();
        public static Value operator *(Value a, double b)
        {
            return a.Multiply(b);
        }
        public static Value operator -(Value a, Value b)
        {
            return a.Subtract(b);
        }
        public static Value operator -(Value a, double b)
        {
            return a.Subtract(b);
        }
        public static Value operator +(Value a, Value b)
        {
            return a.Add(b);
        }
        public static Value operator +(Value a, double b)
        {
            return a.Add(b);
        }
        public abstract Value Multiply(double b);
        public abstract Value Subtract(Value b);
        public abstract Value Subtract(double b);
        public abstract Value Add(Value b);
        public abstract Value Add(double b);
    }

    class Coordinates : Value
    {
        //values
        private double x, y, z;

        //Properties
        public double X
        {
            get
            {
                return this.x;
            }
        }

        public double Y
        {
            get
            {
                return this.y;
            }
        }

        public double Z
        {
            get
            {
                return this.z;
            }
        }
        
        public Coordinates(double x, double y, double z = 0)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Coordinates To2D()
        {
            return new generator.Coordinates(x, y);
        }

        public override Value Multiply(double b)
        {
            return new Coordinates(this.x * b, this.y * b, this.z * b);
        }

        public override string ToString()
        {
            var translate = To2D();
            return $"{translate.X:0.#####},{translate.Y:0.#####}";
        }

        public override Value Subtract(Value b)
        {
            Coordinates a = (Coordinates)b;
            return new Coordinates(this.x - a.X, this.y - a.Y, this.z - a.Z);
        }

        public override Value Subtract(double b)
        {
            return new Coordinates(this.x - b, this.y - b);
        }

        public override Value Add(Value b)
        {
            Coordinates a = (Coordinates)b;
            return new Coordinates(this.x + a.X, this.y + a.Y, this.z + a.Z);
        }

        public override Value Add(double b)
        {
            return new Coordinates(this.x + b, this.y + b);
        }
    }

    class CoordinateType : Value
    {
        private double c;
        public double Coordinate
        {
            get
            {
                return c;
            }
        }

        public CoordinateType(double c)
        {
            this.c = c;
        }

        public override Value Multiply(double b)
        {
            return new CoordinateType(c * b);
        }

        public override string ToString()
        {
            return $"{c:0.#####}";
        }

        public override Value Subtract(Value b)
        {
            CoordinateType c = (CoordinateType)b;
            return new CoordinateType(this.c - c.c);
        }

        public override Value Subtract(double b)
        {
            return new CoordinateType(this.c - b);
        }

        public override Value Add(Value b)
        {
            CoordinateType c = (CoordinateType)b;
            return new CoordinateType(this.c + c.c);
        }

        public override Value Add(double b)
        {
            return new CoordinateType(this.c + b);
        }
    }
        

    class Opacity : Value
    {
        private double o;
        public double Alpha
        {
            get
            {
                return o;
            }
        }

        public Opacity(double o)
        {
            this.o = o;
        }

        public override Value Multiply(double b)
        {
            return new Opacity(o * b);
        }

        public override string ToString()
        {
            return $"{o:0.#####}";
        }

        public override Value Subtract(Value b)
        {
            var a = (Opacity)b;
            return new Opacity(this.o - a.o);
        }

        public override Value Subtract(double b)
        {
            return new Opacity(this.o - b);
        }

        public override Value Add(Value b)
        {
            var a = (Opacity)b;
            return new Opacity(this.o + a.o);
        }

        public override Value Add(double b)
        {
            return new Opacity(this.o + b);
        }
    }

    class Size : Value
    {
        private double s;
        public double Scale
        {
            get
            {
                return s;
            }
        }

        public Size(double s)
        {
            this.s = s;
        }

        public override Value Multiply(double b)
        {
            return new Size(s * b);
        }

        public override string ToString()
        {
            return $"{s:0.#####}";
        }

        public override Value Subtract(Value b)
        {
            var a = (Size)b;
            return new Size(this.s - a.s);
        }

        public override Value Subtract(double b)
        {
            return new Size(this.s - b);
        }

        public override Value Add(Value b)
        {
            var a = (Size)b;
            return new Size(this.s + a.s);
        }

        public override Value Add(double b)
        {
            return new Size(this.s + b);
        }
    }

    class Angle : Value
    {
        private double a;
        public double Degree
        {
            get
            {
                return a;
            }
        }
        public double Radian
        {
            get
            {
                return a * (Math.PI / 180);
            }
        }

        public Angle(double a, bool is_radian = false)
        {
            this.a = !is_radian ? a : a * (180 / Math.PI); 
        }

        public override Value Multiply(double b)
        {
            return new Angle(a * b);
        }

        public override string ToString()
        {
            return $"{a:0.#####}";
        }

        public override Value Subtract(Value b)
        {
            var a = (Angle)b;
            return new Angle(this.a - a.a);
        }

        public override Value Subtract(double b)
        {
            return new Angle(this.a - b);
        }

        public override Value Add(Value b)
        {
            var a = (Angle)b;
            return new Angle(this.a - a.a);
        }

        public override Value Add(double b)
        {
            return new Angle(this.a - b);
        }
    }

    abstract class Color : Value
    {
        public class InvalidColorException : Exception
        {
            public InvalidColorException(string message) : base(message) { }
        }


        protected int r, g, b;
        protected double h, s, v;

        public int Red
        {
            get
            {
                return r;
            }
        }

        public int Green
        {
            get
            {
                return g;
            }
        }

        public int Blue
        {
            get
            {
                return b;
            }
        }

        public double Hue
        {
            get
            {
                return h;
            }
        }

        public double Saturation
        {
            get
            {
                return s;
            }
        }

        public double Value
        {
            get
            {
                return v;
            }
        }

        protected Color(int r, int g, int b)
        {
            if (r > 255)
            {
                throw new InvalidColorException("The Red value is over 255.");
            }

            if (g > 255)
            {
                throw new InvalidColorException("The Green value is over 255.");
            }

            if (b > 255)
            {
                throw new InvalidColorException("The Blue value is over 255.");
            }
            this.r = r;
            this.g = g;
            this.b = b;

            CalculateHSV(r, g, b);
        }

        private void CalculateHSV(int r, int g, int b)
        {
            var rp = r / 255.0;
            var gp = g / 255.0;
            var bp = b / 255.0;

            double[] tar = { rp, gp, bp };

            var max = tar.Max();
            var min = tar.Min();

            var delta = max - min;

            if (max == rp) { h = (1 / 6) * (((gp - bp) / delta) % 6); }
            else if (max == rp) { h = (1 / 6) * (((bp - rp) / delta) + 2); }
            else if (max == rp) { h = (1 / 6) * (((rp - gp) / delta) + 4); }
            else { h = 0; }

            if (max == 0) { s = 0; }
            else { s = delta / max; }

            v = max;
        }

        protected Color(double h, double s, double v)
        {
            if (h > 1)
            {
                throw new InvalidColorException("the hue value is not within interval [0, 1)");
            }
            if (s > 1)
            {
                throw new InvalidColorException("the saturation value is not within interval [0, 1)");
            }
            if (v > 1)
            {
                throw new InvalidColorException("the value value is not within interval [0, 1)");
            }
            this.h = h;
            this.s = s;
            this.v = v;
            CalculateRGB(h, s, v);
        }

        private void CalculateRGB(double h, double s, double v)
        {
            var hp = 360 * h;

            var rp = 0.0;
            var gp = 0.0;
            var bp = 0.0;

            if (s == 0)
            {
                rp = v;
                gp = v;
                bp = v;
            }
            else
            {
                var ht = hp / 60;
                var i = (int)Math.Floor(ht);
                var f = ht - i;
                var p = v * (1f - s);
                var q = 0.0;
                if (i % 2 == 0)
                {
                    q = v * (1f - f * s);
                }
                else
                {
                    q = v * (1f - f * s);
                }

                switch (i)
                {
                    case 0:
                        rp = v;
                        gp = q;
                        bp = p;
                        break;
                    case 1:
                        rp = q;
                        gp = v;
                        bp = p;
                        break;
                    case 2:
                        rp = p;
                        gp = v;
                        bp = q;
                        break;
                    case 3:
                        rp = p;
                        gp = q;
                        bp = v;
                        break;
                    case 4:
                        rp = q;
                        gp = p;
                        bp = v;
                        break;
                    case 5:
                        rp = v;
                        gp = p;
                        bp = q;
                        break;
                }
                r = (int)(rp * 255);
                g = (int)(gp * 255);
                b = (int)(bp * 255);
            }
        }

        public override string ToString()
        {
            return $"{r},{g},{b}";
        }
    }

    class ColorRGB : Color
    {
        public ColorRGB(int r, int g, int b) : base(r, g, b)
        {

        }

        public ColorHSV toHSV()
        {
            return new ColorHSV(h, s, v);
        }

        public override Value Multiply(double b)
        {
            return new ColorRGB((int)(r * b), (int)(g * b), (int)(this.b * b));
        }

        public override Value Subtract(Value b)
        {
            var a = (ColorRGB)b;
            return new ColorRGB(this.r - a.r, this.g - a.g, this.b - a.b);
        }

        public override Value Subtract(double b)
        {
            return new ColorRGB((int)(this.r - b), (int)(this.g - b), (int)(this.b - b));
        }

        public override Value Add(Value b)
        {
            var a = (ColorRGB)b;
            return new ColorRGB(this.r + a.r, this.g + a.g, this.b + a.b);
        }

        public override Value Add(double b)
        {
            return new ColorRGB((int)(this.r + b), (int)(this.g + b), (int)(this.b + b));
        }
    }

    class ColorHSV : Color
    {
        public ColorHSV(double h, double s, double v) : base(h, s, v)
        {

        }

        public ColorRGB toRGB()
        {
            return new ColorRGB(r, g, b);
        }

        public override Value Multiply(double b)
        {
            return new ColorRGB((int)(r * b), (int)(g * b), (int)(this.b * b)).toHSV();
        }

        public override Value Subtract(Value b)
        {
            var a = (ColorHSV)b;
            return new ColorRGB(this.r - a.r, this.g - a.g, this.b - a.b).toHSV();
        }

        public override Value Subtract(double b)
        {
            return new ColorRGB((int)(this.r - b), (int)(this.g - b), (int)(this.b - b)).toHSV();
        }

        public override Value Add(Value b)
        {
            var a = (ColorHSV)b;
            return new ColorRGB(this.r + a.r, this.g + a.g, this.b + a.b).toHSV();
        }

        public override Value Add(double b)
        {
            return new ColorRGB((int)(this.r + b), (int)(this.g + b), (int)(this.b + b)).toHSV();
        }
    }

    class VectorSize : Value
    {
        private double x, y;
        public double X
        {
            get
            {
                return x;
            }
        }

        public double Y
        {
            get
            {
                return y;
            }
        }

        public VectorSize(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public override Value Multiply(double b)
        {
            return new VectorSize(x * b, y * b);
        }

        public override string ToString()
        {
            return $"{x:0.#####},{y:0.#####}";
        }

        public override Value Subtract(Value b)
        {
            var a = (VectorSize)b;
            return new VectorSize(this.x - a.x, this.y - a.y);
        }

        public override Value Subtract(double b)
        {
            return new VectorSize(this.x + b, this.y + b);
        }

        public override Value Add(Value b)
        {
            var a = (VectorSize)b;
            return new VectorSize(this.x + a.x, this.y + a.y);
        }

        public override Value Add(double b)
        {
            return new VectorSize(this.x + b, this.y + b);
        }
    }
}
