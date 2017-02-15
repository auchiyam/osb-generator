using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//TODO: Add Intervals to the commands
//TODO: Add Func<double, double> to Functions
//TODO: Fix Loop function/class to determine conflicts
namespace osb_generator.generator
{
    public sealed class Origin
    {
        private readonly string command;
        private readonly int value;

        public static readonly Origin TopLeft = new Origin(1, "TopLeft");
        public static readonly Origin TopCentre = new Origin(2, "TopCentre");
        public static readonly Origin TopRight = new Origin(3, "TopRight");
        public static readonly Origin CentreLeft = new Origin(4, "CentreLeft");
        public static readonly Origin Centre = new Origin(5, "Centre");
        public static readonly Origin CentreRight = new Origin(6, "CentreRight");
        public static readonly Origin BottomLeft = new Origin(7, "BottomLeft");
        public static readonly Origin BottomCentre = new Origin(8, "BottomCentre");

        private Origin(int value, string command)
        {
            this.command = command;
            this.value = value;
        }

        public override string ToString()
        {
            return command;
        }
    }

    abstract class StoryboardObject
    {
        protected string image;
        protected Timeline timeline;
        protected Origin origin;
        protected int indentation;
        protected int fps;

        protected StoryboardObject() { }

        public StoryboardObject(string i) : this(i, Origin.Centre, 10)
        {
            
        }

        public StoryboardObject(string i, int fps) : this(i, Origin.Centre, fps) { }
        public StoryboardObject(string i, Origin o) : this(i, o, 10) { }

        public StoryboardObject(string i, Origin o, int fps)
        {
            image = i;
            timeline = new Timeline();
            origin = o;
            indentation = 0;
            this.fps = fps;
        }

        #region Command
        #region Move
        //value
        public void Move(double x, double y) => CreateCommand(CommandType.Move, 0, new Coordinates(x, y));
        public void Move(Coordinates c) => CreateCommand(CommandType.Move, 0, c);

        //start, value
        public void Move(double t, double x, double y) => CreateCommand(CommandType.Move, t, new Coordinates(x, y));
        public void Move(Time t, double x, double y) => CreateCommand(CommandType.Move, t, new Coordinates(x, y));

        public void Move(double t, Coordinates c) => Move(t, c.X, c.Y);
        public void Move(Time t, Coordinates c) => Move(t, c.X, c.Y);

        //start, end, a, b
        public void Move(double start, double end, double x1, double y1, double x2, double y2) => CreateCommand(CommandType.Move, start, end, new Coordinates(x1, y1), new Coordinates(x2, y2));
        public void Move(Time start, Time end, double x1, double y1, double x2, double y2) => CreateCommand(CommandType.Move, start, end, new Coordinates(x1, y1), new Coordinates(x2, y2));

        public void Move(double start, double end, Coordinates a, Coordinates b) => CreateCommand(CommandType.Move, start, end, a, b);
        public void Move(Time start, Time end, Coordinates a, Coordinates b) => CreateCommand(CommandType.Move, start, end, a, b);

        //ease, start, end, a, b
        public void Move(EaseType e, double start, double end, double x1, double y1, double x2, double y2) => CreateCommand(CommandType.Move, e, start, end, new Coordinates(x1, y1), new Coordinates(x2, y2));
        public void Move(EaseType e, Time start, Time end, double x1, double y1, double x2, double y2) => CreateCommand(CommandType.Move, e, start, end, new Coordinates(x1, y1), new Coordinates(x2, y2));

        public void Move(EaseType e, double start, double end, Coordinates a, Coordinates b) => CreateCommand(CommandType.Move, e, start, end, a, b);
        public void Move(EaseType e, Time start, Time end, Coordinates a, Coordinates b) => CreateCommand(CommandType.Move, e, start, end, a, b);
        #endregion

        #region MoveX
        //value
        public void MoveX(double x) => CreateCommand(CommandType.MoveX, 0, new CoordinateType(x));
        public void MoveX(CoordinateType c) => CreateCommand(CommandType.MoveX, 0, c);

        //start, value
        public void MoveX(double t, double a) => CreateCommand(CommandType.MoveX, t, new CoordinateType(a));
        public void MoveX(Time t, double a) => CreateCommand(CommandType.MoveX, t, new CoordinateType(a));

        public void MoveX(double t, CoordinateType a) => CreateCommand(CommandType.MoveX, t, a);
        public void MoveX(Time t, CoordinateType a) => CreateCommand(CommandType.MoveX, t, a);

        //start, end, a, b
        public void MoveX(double start, double end, double a, double b) => CreateCommand(CommandType.MoveX, start, end, new CoordinateType(a), new CoordinateType(b));
        public void MoveX(Time start, Time end, double a, double b) => CreateCommand(CommandType.MoveX, start, end, new CoordinateType(a), new CoordinateType(b));

        public void MoveX(double start, double end, CoordinateType a, CoordinateType b) => CreateCommand(CommandType.MoveX, start, end, a, b);
        public void MoveX(Time start, Time end, CoordinateType a, CoordinateType b) => CreateCommand(CommandType.MoveX, start, end, a, b);

        //ease, start, end, a, b
        public void MoveX(EaseType e, double start, double end, double a, double b) => CreateCommand(CommandType.MoveX, e, start, end, new CoordinateType(a), new CoordinateType(b));
        public void MoveX(EaseType e, Time start, Time end, double a, double b) => CreateCommand(CommandType.MoveX, e, start, end, new CoordinateType(a), new CoordinateType(b));

        public void MoveX(EaseType e, double start, double end, CoordinateType a, CoordinateType b) => CreateCommand(CommandType.MoveX, e, start, end, a, b);
        public void MoveX(EaseType e, Time start, Time end, CoordinateType a, CoordinateType b) => CreateCommand(CommandType.MoveX, e, start, end, a, b);
        #endregion

        #region MoveY
        //value
        public void MoveY(double x) => CreateCommand(CommandType.MoveY, 0, new CoordinateType(x));
        public void MoveY(CoordinateType c) => CreateCommand(CommandType.MoveY, 0, c);

        //start, value
        public void MoveY(double t, double a) => CreateCommand(CommandType.MoveY, t, new CoordinateType(a));
        public void MoveY(Time t, double a) => CreateCommand(CommandType.MoveY, t, new CoordinateType(a));

        public void MoveY(double t, CoordinateType a) => CreateCommand(CommandType.MoveY, t, a);
        public void MoveY(Time t, CoordinateType a) => CreateCommand(CommandType.MoveY, t, a);

        //start, end, a, b
        public void MoveY(double start, double end, double a, double b) => CreateCommand(CommandType.MoveY, start, end, new CoordinateType(a), new CoordinateType(b));
        public void MoveY(Time start, Time end, double a, double b) => CreateCommand(CommandType.MoveY, start, end, new CoordinateType(a), new CoordinateType(b));

        public void MoveY(double start, double end, CoordinateType a, CoordinateType b) => CreateCommand(CommandType.MoveY, start, end, a, b);
        public void MoveY(Time start, Time end, CoordinateType a, CoordinateType b) => CreateCommand(CommandType.MoveY, start, end, a, b);

        //ease, start, end, a, b
        public void MoveY(EaseType e, double start, double end, double a, double b) => CreateCommand(CommandType.MoveY, e, start, end, new CoordinateType(a), new CoordinateType(b));
        public void MoveY(EaseType e, Time start, Time end, double a, double b) => CreateCommand(CommandType.MoveY, e, start, end, new CoordinateType(a), new CoordinateType(b));

        public void MoveY(EaseType e, double start, double end, CoordinateType a, CoordinateType b) => CreateCommand(CommandType.MoveY, e, start, end, a, b);
        public void MoveY(EaseType e, Time start, Time end, CoordinateType a, CoordinateType b) => CreateCommand(CommandType.MoveY, e, start, end, a, b);
        #endregion

        #region Fade
        //value
        public void Fade(double x) => CreateCommand(CommandType.Fade, 0, new Opacity(x));
        public void Fade(Opacity c) => CreateCommand(CommandType.Fade, 0, c);

        //start, value
        public void Fade(double t, double a) => CreateCommand(CommandType.Fade, t, new Opacity(a));
        public void Fade(Time t, double a) => CreateCommand(CommandType.Fade, t, new Opacity(a));

        public void Fade(double t, Opacity a) => CreateCommand(CommandType.Fade, t, a);
        public void Fade(Time t, Opacity a) => CreateCommand(CommandType.Fade, t, a);

        //start, end, a, b
        public void Fade(double start, double end, double a, double b) => CreateCommand(CommandType.Fade, start, end, new Opacity(a), new Opacity(b));
        public void Fade(Time start, Time end, double a, double b) => CreateCommand(CommandType.Fade, start, end, new Opacity(a), new Opacity(b));

        public void Fade(double start, double end, Opacity a, Opacity b) => CreateCommand(CommandType.Fade, start, end, a, b);
        public void Fade(Time start, Time end, Opacity a, Opacity b) => CreateCommand(CommandType.Fade, start, end, a, b);

        //ease, start, end, a, b
        public void Fade(EaseType e, double start, double end, double a, double b) => CreateCommand(CommandType.Fade, e, start, end, new Opacity(a), new Opacity(b));
        public void Fade(EaseType e, Time start, Time end, double a, double b) => CreateCommand(CommandType.Fade, e, start, end, new Opacity(a), new Opacity(b));

        public void Fade(EaseType e, double start, double end, Opacity a, Opacity b) => CreateCommand(CommandType.Fade, e, start, end, a, b);
        public void Fade(EaseType e, Time start, Time end, Opacity a, Opacity b) => CreateCommand(CommandType.Fade, e, start, end, a, b);
        #endregion

        #region Scale
        //value
        public void Scale(double x) => CreateCommand(CommandType.Scale, 0, new Size(x));
        public void Scale(Size c) => CreateCommand(CommandType.Scale, 0, c);

        //start, value
        public void Scale(double t, double a) => CreateCommand(CommandType.Scale, t, new Size(a));
        public void Scale(Time t, double a) => CreateCommand(CommandType.Scale, t, new Size(a));

        public void Scale(double t, Size a) => CreateCommand(CommandType.Scale, t, a);
        public void Scale(Time t, Size a) => CreateCommand(CommandType.Scale, t, a);

        //start, end, a, b
        public void Scale(double start, double end, double a, double b) => CreateCommand(CommandType.Scale, start, end, new Size(a), new Size(b));
        public void Scale(Time start, Time end, double a, double b) => CreateCommand(CommandType.Scale, start, end, new Size(a), new Size(b));

        public void Scale(double start, double end, Size a, Size b) => CreateCommand(CommandType.Scale, start, end, a, b);
        public void Scale(Time start, Time end, Size a, Size b) => CreateCommand(CommandType.Scale, start, end, a, b);

        //ease, start, end, a, b
        public void Scale(EaseType e, double start, double end, double a, double b) => CreateCommand(CommandType.Scale, e, start, end, new Size(a), new Size(b));
        public void Scale(EaseType e, Time start, Time end, double a, double b) => CreateCommand(CommandType.Scale, e, start, end, new Size(a), new Size(b));

        public void Scale(EaseType e, double start, double end, Size a, Size b) => CreateCommand(CommandType.Scale, e, start, end, a, b);
        public void Scale(EaseType e, Time start, Time end, Size a, Size b) => CreateCommand(CommandType.Scale, e, start, end, a, b);
        #endregion

        #region Rotate
        //value
        public void Rotate(double x) => CreateCommand(CommandType.Rotate, 0, new Angle(x));
        public void Rotate(Angle c) => CreateCommand(CommandType.Rotate, 0, c);

        //start, value
        public void Rotate(double t, double a) => CreateCommand(CommandType.Rotate, t, new Angle(a));
        public void Rotate(Time t, double a) => CreateCommand(CommandType.Rotate, t, new Angle(a));

        public void Rotate(double t, Angle a) => CreateCommand(CommandType.Rotate, t, a);
        public void Rotate(Time t, Angle a) => CreateCommand(CommandType.Rotate, t, a);

        //start, end, a, b
        public void Rotate(double start, double end, double a, double b) => CreateCommand(CommandType.Rotate, start, end, new Angle(a), new Angle(b));
        public void Rotate(Time start, Time end, double a, double b) => CreateCommand(CommandType.Rotate, start, end, new Angle(a), new Angle(b));

        public void Rotate(double start, double end, Angle a, Angle b) => CreateCommand(CommandType.Rotate, start, end, a, b);
        public void Rotate(Time start, Time end, Angle a, Angle b) => CreateCommand(CommandType.Rotate, start, end, a, b);

        //ease, start, end, a, b
        public void Rotate(EaseType e, double start, double end, double a, double b) => CreateCommand(CommandType.Rotate, e, start, end, new Angle(a), new Angle(b));
        public void Rotate(EaseType e, Time start, Time end, double a, double b) => CreateCommand(CommandType.Rotate, e, start, end, new Angle(a), new Angle(b));

        public void Rotate(EaseType e, double start, double end, Angle a, Angle b) => CreateCommand(CommandType.Rotate, e, start, end, a, b);
        public void Rotate(EaseType e, Time start, Time end, Angle a, Angle b) => CreateCommand(CommandType.Rotate, e, start, end, a, b);
        #endregion

        #region Color
        //value
        public void Color(int r, int g, int b) => CreateCommand(CommandType.Color, 0, new ColorRGB(r, g, b));
        public void Color(Color c) => CreateCommand(CommandType.Color, 0, c);

        //start, value
        public void Color(double t, int r, int g, int b) => CreateCommand(CommandType.Color, t, new ColorRGB(r, g, b));
        public void Color(Time t, int r, int g, int b) => CreateCommand(CommandType.Color, t, new ColorRGB(r, g, b));

        public void Color(double t, Color a) => CreateCommand(CommandType.Color, t, a);
        public void Color(Time t, Color a) => CreateCommand(CommandType.Color, t, a);

        //start, end, a, b
        public void Color(double start, double end, int r1, int g1, int b1, int r2, int g2, int b2) => CreateCommand(CommandType.Color, start, end, new ColorRGB(r1, g1, b1), new ColorRGB(r2, g2, b2));
        public void Color(Time start, Time end, int r1, int g1, int b1, int r2, int g2, int b2) => CreateCommand(CommandType.Color, start, end, new ColorRGB(r1, g1, b1), new ColorRGB(r2, g2, b2));

        public void Color(double start, double end, Color a, Color b) => CreateCommand(CommandType.Color, start, end, a, b);
        public void Color(Time start, Time end, Color a, Color b) => CreateCommand(CommandType.Color, start, end, a, b);

        //ease, start, end, a, b
        public void Color(EaseType e, double start, double end, int r1, int g1, int b1, int r2, int g2, int b2) => CreateCommand(CommandType.Color, e, start, end, new ColorRGB(r1, g1, b1), new ColorRGB(r2, g2, b2));
        public void Color(EaseType e, Time start, Time end, int r1, int g1, int b1, int r2, int g2, int b2) => CreateCommand(CommandType.Color, e, start, end, new ColorRGB(r1, g1, b1), new ColorRGB(r2, g2, b2));

        public void Color(EaseType e, double start, double end, Color a, Color b) => CreateCommand(CommandType.Color, e, start, end, a, b);
        public void Color(EaseType e, Time start, Time end, Color a, Color b) => CreateCommand(CommandType.Color, e, start, end, a, b);
        #endregion

        #region Vector
        //value
        public void Vector(double x, double y) => CreateCommand(CommandType.Vector, 0, new VectorSize(x, y));
        public void Vector(VectorSize c) => CreateCommand(CommandType.Vector, 0, c);

        //start, value
        public void Vector(double t, double x, double y) => CreateCommand(CommandType.Vector, t, new VectorSize(x, y));
        public void Vector(Time t, double x, double y) => CreateCommand(CommandType.Vector, t, new VectorSize(x, y));

        public void Vector(double t, VectorSize c) => Vector(t, c.X, c.Y);
        public void Vector(Time t, VectorSize c) => Vector(t, c.X, c.Y);

        //start, end, a, b
        public void Vector(double start, double end, double x1, double y1, double x2, double y2) => CreateCommand(CommandType.Vector, start, end, new VectorSize(x1, y1), new VectorSize(x2, y2));
        public void Vector(Time start, Time end, double x1, double y1, double x2, double y2) => CreateCommand(CommandType.Vector, start, end, new VectorSize(x1, y1), new VectorSize(x2, y2));

        public void Vector(double start, double end, VectorSize a, VectorSize b) => CreateCommand(CommandType.Vector, start, end, a, b);
        public void Vector(Time start, Time end, VectorSize a, VectorSize b) => CreateCommand(CommandType.Vector, start, end, a, b);

        //ease, start, end, a, b
        public void Vector(EaseType e, double start, double end, double x1, double y1, double x2, double y2) => CreateCommand(CommandType.Vector, e, start, end, new VectorSize(x1, y1), new VectorSize(x2, y2));
        public void Vector(EaseType e, Time start, Time end, double x1, double y1, double x2, double y2) => CreateCommand(CommandType.Vector, e, start, end, new VectorSize(x1, y1), new VectorSize(x2, y2));

        public void Vector(EaseType e, double start, double end, VectorSize a, VectorSize b) => CreateCommand(CommandType.Vector, e, start, end, a, b);
        public void Vector(EaseType e, Time start, Time end, VectorSize a, VectorSize b) => CreateCommand(CommandType.Vector, e, start, end, a, b);
        #endregion

        #region Create Command
        //start, value
        public void CreateCommand(CommandType t, double start, Value a) => CreateCommand(t, start, start, a, a);
        public void CreateCommand(CommandType t, Time start, Value a) => CreateCommand(t, start, start, a, a);

        //start, end, a, b
        public void CreateCommand(CommandType t, double start, double end, Value a, Value b) => CreateCommand(t, EaseType.Linear, start, end, a, b);
        public void CreateCommand(CommandType t, Time start, Time end, Value a, Value b) => CreateCommand(t, EaseType.Linear, start, end, a, b);

        //ease, start, end, a, b
        public void CreateCommand(CommandType t, EaseType e, double start, double end, Value a, Value b) => CreateCommand(t, e, new Time(start), new Time(end), a, b);
        public void CreateCommand(CommandType t, EaseType e, Time start, Time end, Value a, Value b)
        {
            Command c;
            if (t == CommandType.Move && timeline[CommandType.MoveX].Count > 0 || timeline[CommandType.MoveY].Count > 0)
            {
                var c1 = new Command(CommandType.MoveX, e, new Interval(start, end), a, b);
                var c2 = new Command(CommandType.MoveY, e, new Interval(start, end), a, b);
                timeline.AddCommand(c1);
                timeline.AddCommand(c2);
            }
            else if ((t == CommandType.MoveX || t == CommandType.MoveY) && timeline[CommandType.Move].Count > 0)
            {
                var u1 = t == CommandType.MoveX ? (CoordinateType)a : new CoordinateType(0);
                var v1 = t == CommandType.MoveY ? (CoordinateType)a : new CoordinateType(0);

                var u2 = t == CommandType.MoveX ? (CoordinateType)b : new CoordinateType(0);
                var v2 = t == CommandType.MoveY ? (CoordinateType)b : new CoordinateType(0);

                c = new Command(CommandType.Move, e, new Interval(start, end), new Coordinates(u1.Coordinate, v1.Coordinate), new Coordinates(u2.Coordinate, v2.Coordinate));
                timeline.AddCommand(c);
            }
            else
            {
                c = new Command(t, e, new Interval(start, end), a, b);
                timeline.AddCommand(c);
            }
        }

        public void Additive()
        {
            timeline.AddCommand(new Additive());
        }

        #endregion
        #endregion

        #region Function
        #region Move
        //func, start, end, st, et
        public void MoveF(Func<double, Coordinates> a, double start, double end, double startt, double endt) => CreateFunction(CommandType.Move, a, new Interval(start, end), startt, endt);
        public void MoveF(Func<double, Coordinates> a, Time start, Time end, double startt, double endt) => CreateFunction(CommandType.Move, a, new Interval(start, end), startt, endt);
        public void MoveF(Func<double, Coordinates> a, Interval i, double startt, double endt) => CreateFunction(CommandType.Move, a, i, startt, endt);

        //func, ease, start, end, st, et
        public void MoveF(Func<double, Coordinates> a, EaseType e, double start, double end, double startt, double endt) => CreateFunction(CommandType.Move, a, e, new Interval(start, end), startt, endt);
        public void MoveF(Func<double, Coordinates> a, EaseType e, Time start, Time end, double startt, double endt) => CreateFunction(CommandType.Move, a, e, new Interval(start, end), startt, endt);
        public void MoveF(Func<double, Coordinates> a, EaseType e, Interval i, double startt, double endt) => CreateFunction(CommandType.Move, a, e, i, startt, endt);
        #endregion

        #region Move
        //func, start, end, st, et
        public void MoveXF(Func<double, CoordinateType> a, double start, double end, double startt, double endt) => CreateFunction(CommandType.MoveX, a, new Interval(start, end), startt, endt);
        public void MoveXF(Func<double, CoordinateType> a, Time start, Time end, double startt, double endt) => CreateFunction(CommandType.MoveX, a, new Interval(start, end), startt, endt);
        public void MoveXF(Func<double, CoordinateType> a, Interval i, double startt, double endt) => CreateFunction(CommandType.MoveX, a, i, startt, endt);

        //func, ease, start, end, st, et
        public void MoveXF(Func<double, CoordinateType> a, EaseType e, double start, double end, double startt, double endt) => CreateFunction(CommandType.MoveX, a, e, new Interval(start, end), startt, endt);
        public void MoveXF(Func<double, CoordinateType> a, EaseType e, Time start, Time end, double startt, double endt) => CreateFunction(CommandType.MoveX, a, e, new Interval(start, end), startt, endt);
        public void MoveXF(Func<double, CoordinateType> a, EaseType e, Interval i, double startt, double endt) => CreateFunction(CommandType.MoveX, a, e, i, startt, endt);
        #endregion

        #region Move
        //func, start, end, st, et
        public void MoveYF(Func<double, CoordinateType> a, double start, double end, double startt, double endt) => CreateFunction(CommandType.MoveY, a, new Interval(start, end), startt, endt);
        public void MoveYF(Func<double, CoordinateType> a, Time start, Time end, double startt, double endt) => CreateFunction(CommandType.MoveY, a, new Interval(start, end), startt, endt);
        public void MoveYF(Func<double, CoordinateType> a, Interval i, double startt, double endt) => CreateFunction(CommandType.MoveY, a, i, startt, endt);

        //func, ease, start, end, st, et
        public void MoveYF(Func<double, CoordinateType> a, EaseType e, double start, double end, double startt, double endt) => CreateFunction(CommandType.MoveY, a, e, new Interval(start, end), startt, endt);
        public void MoveYF(Func<double, CoordinateType> a, EaseType e, Time start, Time end, double startt, double endt) => CreateFunction(CommandType.MoveY, a, e, new Interval(start, end), startt, endt);
        public void MoveYF(Func<double, CoordinateType> a, EaseType e, Interval i, double startt, double endt) => CreateFunction(CommandType.MoveY, a, e, i, startt, endt);
        #endregion

        #region Fade
        //func, start, end, st, et
        public void FadeF(Func<double, Opacity> a, double start, double end, double startt, double endt) => CreateFunction(CommandType.Fade, a, new Interval(start, end), startt, endt);
        public void FadeF(Func<double, Opacity> a, Time start, Time end, double startt, double endt) => CreateFunction(CommandType.Fade, a, new Interval(start, end), startt, endt);
        public void FadeF(Func<double, Opacity> a, Interval i, double startt, double endt) => CreateFunction(CommandType.Fade, a, i, startt, endt);

        //func, ease, start, end, st, et
        public void FadeF(Func<double, Opacity> a, EaseType e, double start, double end, double startt, double endt) => CreateFunction(CommandType.Fade, a, e, new Interval(start, end), startt, endt);
        public void FadeF(Func<double, Opacity> a, EaseType e, Time start, Time end, double startt, double endt) => CreateFunction(CommandType.Fade, a, e, new Interval(start, end), startt, endt);
        public void FadeF(Func<double, Opacity> a, EaseType e, Interval i, double startt, double endt) => CreateFunction(CommandType.Fade, a, e, i, startt, endt);
        #endregion

        #region Scale
        //func, start, end, st, et
        public void ScaleF(Func<double, Size> a, double start, double end, double startt, double endt) => CreateFunction(CommandType.Scale, a, new Interval(start, end), startt, endt);
        public void ScaleF(Func<double, Size> a, Time start, Time end, double startt, double endt) => CreateFunction(CommandType.Scale, a, new Interval(start, end), startt, endt);
        public void ScaleF(Func<double, Size> a, Interval i, double startt, double endt) => CreateFunction(CommandType.Scale, a, i, startt, endt);

        //func, ease, start, end, st, et
        public void ScaleF(Func<double, Size> a, EaseType e, double start, double end, double startt, double endt) => CreateFunction(CommandType.Scale, a, e, new Interval(start, end), startt, endt);
        public void ScaleF(Func<double, Size> a, EaseType e, Time start, Time end, double startt, double endt) => CreateFunction(CommandType.Scale, a, e, new Interval(start, end), startt, endt);
        public void ScaleF(Func<double, Size> a, EaseType e, Interval i, double startt, double endt) => CreateFunction(CommandType.Scale, a, e, i, startt, endt);
        #endregion

        #region Rotate
        //func, start, end, st, et
        public void RotateF(Func<double, Angle> a, double start, double end, double startt, double endt) => CreateFunction(CommandType.Rotate, a, new Interval(start, end), startt, endt);
        public void RotateF(Func<double, Angle> a, Time start, Time end, double startt, double endt) => CreateFunction(CommandType.Rotate, a, new Interval(start, end), startt, endt);
        public void RotateF(Func<double, Angle> a, Interval i, double startt, double endt) => CreateFunction(CommandType.Rotate, a, i, startt, endt);

        //func, ease, start, end, st, et
        public void RotateF(Func<double, Angle> a, EaseType e, double start, double end, double startt, double endt) => CreateFunction(CommandType.Rotate, a, e, new Interval(start, end), startt, endt);
        public void RotateF(Func<double, Angle> a, EaseType e, Time start, Time end, double startt, double endt) => CreateFunction(CommandType.Rotate, a, e, new Interval(start, end), startt, endt);
        public void RotateF(Func<double, Angle> a, EaseType e, Interval i, double startt, double endt) => CreateFunction(CommandType.Rotate, a, e, i, startt, endt);
        #endregion

        #region Color
        //func, start, end, st, et
        public void ColorF(Func<double, Color> a, double start, double end, double startt, double endt) => CreateFunction(CommandType.Color, a, new Interval(start, end), startt, endt);
        public void ColorF(Func<double, Color> a, Time start, Time end, double startt, double endt) => CreateFunction(CommandType.Color, a, new Interval(start, end), startt, endt);
        public void ColorF(Func<double, Color> a, Interval i, double startt, double endt) => CreateFunction(CommandType.Color, a, i, startt, endt);

        //func, ease, start, end, st, et
        public void ColorF(Func<double, Color> a, EaseType e, double start, double end, double startt, double endt) => CreateFunction(CommandType.Color, a, e, new Interval(start, end), startt, endt);
        public void ColorF(Func<double, Color> a, EaseType e, Time start, Time end, double startt, double endt) => CreateFunction(CommandType.Color, a, e, new Interval(start, end), startt, endt);
        public void ColorF(Func<double, Color> a, EaseType e, Interval i, double startt, double endt) => CreateFunction(CommandType.Color, a, e, i, startt, endt);
        #endregion

        #region Vector
        //func, start, end, st, et
        public void VectorF(Func<double, VectorSize> a, double start, double end, double startt, double endt) => CreateFunction(CommandType.Vector, a, new Interval(start, end), startt, endt);
        public void VectorF(Func<double, VectorSize> a, Time start, Time end, double startt, double endt) => CreateFunction(CommandType.Vector, a, new Interval(start, end), startt, endt);
        public void VectorF(Func<double, VectorSize> a, Interval i, double startt, double endt) => CreateFunction(CommandType.Vector, a, i, startt, endt);

        //func, ease, start, end, st, et
        public void VectorF(Func<double, VectorSize> a, EaseType e, double start, double end, double startt, double endt) => CreateFunction(CommandType.Vector, a, e, new Interval(start, end), startt, endt);
        public void VectorF(Func<double, VectorSize> a, EaseType e, Time start, Time end, double startt, double endt) => CreateFunction(CommandType.Vector, a, e, new Interval(start, end), startt, endt);
        public void VectorF(Func<double, VectorSize> a, EaseType e, Interval i, double startt, double endt) => CreateFunction(CommandType.Vector, a, e, i, startt, endt);
        #endregion

        #region Create Function
        //func, start, end, st, et
        public void CreateFunction(CommandType t, Func<double, Value> a, double start, double end, double startt, double endt) => CreateFunction(t, a, EaseType.Linear, new Interval(start, end), startt, endt);
        public void CreateFunction(CommandType t, Func<double, Value> a, Time start, Time end, double startt, double endt) => CreateFunction(t, a, EaseType.Linear, new Interval(start, end), startt, endt);
        public void CreateFunction(CommandType t, Func<double, Value> a, Interval i, double startt, double endt) => CreateFunction(t, a, EaseType.Linear, i, startt, endt);

        //func, ease, start, end, func, st, et
        public void CreateFunction(CommandType t, Func<double, Value> a, EaseType e, double start, double end, double startt, double endt) => CreateFunction(t, a, e, new Interval(start, end), startt, endt);
        public void CreateFunction(CommandType t, Func<double, Value> a, EaseType e, Time start, Time end, double startt, double endt) => CreateFunction(t, a, e, new Interval(start, end), startt, endt);
        public void CreateFunction(CommandType t, Func<double, Value> a, EaseType e, Interval i, double startt, double endt)
        {
            Function c;
            c = new Function(t, e, i, a, startt, endt);
            timeline.AddCommand(c);
        }
        #endregion
        #endregion

        public Dictionary<CommandType, Value> Value(Time t)
        {
            return timeline.Values(t);
        }

        public Dictionary<CommandType, Value> Value(double t)
        {
            return timeline.Values(new Time(t));
        }

        public abstract override string ToString();

        public Loop CreateLoop(Time start, int loops)
        {
            return new Loop(indentation + 1, start, loops);
        }

        public Loop CreateLoop(double start, int loops)
        {
            return new Loop(indentation + 1, new Time(start), loops);
        }

        public Loop CreateLoop(double start, int loops, int fps)
        {
            return new Loop(indentation + 1, new generator.Time(start), loops, fps);
        }

        public Loop CreateLoop(Time start, int loops, int fps)
        {
            return new Loop(indentation + 1, start, loops, fps);
        }

        public string PrintTimeline()
        {
            var indent = String.Concat(Enumerable.Repeat(" ", indentation));
            var s = "";
            var list = timeline.Lists();
            foreach (var l in list.Values)
            {
                for (var i = 0; i < l.Count; i++)
                {
                    if (l[i].GetType() == typeof(Command) || l[i].GetType() == typeof(Additive))
                    {
                        s += $"{indent} {l[i]}";
                    }
                    else if (l[i].GetType() == typeof(Function))
                    {
                        var a = (Function)l[i];
                        s += $"{indent} {a.ToString(indentation, fps)}";
                    }
                    s += "\r\n";
                }
            }
            return s;
        }
    }

    class Sprite : StoryboardObject
    {
        public Sprite(string i) : base(i) { }
        public Sprite(string i, int fps) : base(i, fps) { }
        public Sprite(string i, Origin o) : base(i, o) { }
        public Sprite(string i, Origin o, int fps) : base(i, o, fps) { }

        public override string ToString()
        {
            var s = $"Sprite,Background,{origin},{image},320,240\r\n";
            s += PrintTimeline();
            return s;
        }
    }

    class Loop : StoryboardObject
    {
        private Time StartTime;
        private int LoopCount;
        public Loop(int ind, Time start, int loops) : this(ind, start, loops, 25) { }
        public Loop(int ind, Time start, int loops, int fps)
        {
            this.indentation = ind;
            this.StartTime = start;
            this.LoopCount = loops;
            this.fps = fps;
        }

        public override string ToString()
        {
            var indent = String.Concat(Enumerable.Repeat(" ", indentation));
            var s = indent + $"L,{StartTime.OSBTime},{LoopCount}";
            s += PrintTimeline();
            return s;
        }
    }

    class LoopType
    {
        private string s;
        public static readonly LoopType LoopOnce = new LoopType("LoopOne");
        public static readonly LoopType LoopForever = new LoopType("LoopForever");

        private LoopType(string t)
        {
            s = t;
        }

        public override string ToString()
        {
            return s;
        }
    }

    class Animation : StoryboardObject
    {
        private int FrameCount, FrameInterval;
        private LoopType loops;
        public Animation(string i, int frames, int delay, LoopType l) : base(i)
        {
            this.FrameCount = frames;
            this.FrameInterval = delay;
            this.loops = l;
        }

        public Animation(string i, int frames, int delay, LoopType l, int fps) : base(i, fps)
        {
            this.FrameCount = frames;
            this.FrameInterval = delay;
            this.loops = l;
        }

        public Animation(string i, Origin o, int frames, int delay, LoopType l) : base(i, o)
        {
            this.FrameCount = frames;
            this.FrameInterval = delay;
            this.loops = l;
        }

        public Animation(string i, Origin o, int frames, int delay, LoopType l, int fps) : base(i, o, fps)
        {
            this.FrameCount = frames;
            this.FrameInterval = delay;
            this.loops = l;
        }

        public override string ToString()
        {
            var s = $"Animation,Background,{origin},{image},320,240{FrameCount},{FrameInterval},{loops}\r\n";
            s += PrintTimeline();
            return s;
        }
    }
}
