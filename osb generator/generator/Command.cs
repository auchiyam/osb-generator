using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osb_generator.generator
{
    public sealed class CommandType
    {
        private readonly string command;
        private readonly int value;

        public static readonly CommandType Move = new CommandType(1, "M");
        public static readonly CommandType Fade = new CommandType(2, "F");
        public static readonly CommandType Scale = new CommandType(3, "S");
        public static readonly CommandType Rotate = new CommandType(4, "R");
        public static readonly CommandType Color = new CommandType(5, "C");
        public static readonly CommandType Vector = new CommandType(6, "V");
        public static readonly CommandType MoveX = new CommandType(7, "MX");
        public static readonly CommandType MoveY = new CommandType(8, "MY");
        public static readonly CommandType Parameter = new CommandType(9, "P");

        private CommandType(int value, string command)
        {
            this.command = command;
            this.value = value;
        }

        public override string ToString()
        {
            return command;
        }
    }

    abstract class CommandObject : IComparable
    {
        protected EaseType ease;
        protected Time starttime, endtime;
        protected Interval duration;
        protected CommandType type;
        public EaseType Ease { get { return ease; } }
        public Time StartTime { get { return starttime; } }
        public Time EndTime { get { return endtime; } }
        public Interval Duration { get { return duration; } }
        public CommandType Type { get { return type; } }

        public abstract Value GetValue(double t);
        public abstract Value GetValue(double min, double sec, double millisec);
        public abstract Value GetValue(Time t);

        public int CompareTo(object obj)
        {
            var a = (CommandObject)obj;
            return (int)Math.Ceiling(this.Duration.StartTime.OSBTime - a.Duration.StartTime.OSBTime);
        }
    }

    class Command : CommandObject
    {
        private Value startvalue, endvalue;

        public Command(CommandType type, EaseType ease, Interval duration, Value a, Value b)
        {
            this.type = type;
            this.ease = ease;
            this.starttime = duration.StartTime;
            this.endtime = duration.EndTime;
            this.duration = duration;
            this.startvalue = a;
            this.endvalue = b;
        }

        #region GetValue
        public override Value GetValue(double t)
        {
            return GetValue(new Time(t));
        }

        public override Value GetValue(double min, double sec, double millisec)
        {
            return GetValue(new Time(min, sec, millisec));
        }

        public override Value GetValue(Time t)
        {
            if (duration.Length.OSBTime > 0)
                return Easing.CalculateEase(this.Ease, this.endvalue - this.startvalue, t.OSBTime - starttime.OSBTime, duration.Length.OSBTime, this.startvalue);
            else
                return this.startvalue;
        }
        #endregion

        public override string ToString()
        {
            string a = startvalue.ToString() + ",";
            string b = endvalue.ToString();

            string s = starttime.ToString();
            string e = endtime.ToString();
            if (type.Equals(CommandType.Rotate))
            {
                var q = (Angle)(startvalue);
                var w = (Angle)(endvalue);
                a = q.ToString() + ",";
                b = w.ToString();
            }

            if (startvalue.Equals(endvalue))
            {
                a = a.Substring(0, a.Length - 1);
                e = "";
                b = "";
            }

            return $"{type},{(int)ease},{starttime},{endtime},{a}{b}";
        }
    }

    class Function : CommandObject
    {
        private Func<double, Value> function;
        private double startt, endt;

        public Function(CommandType type, EaseType ease, Interval duration, Func<double, Value> function, double startt, double endt)
        {
            this.type = type;
            this.ease = ease;
            this.starttime = duration.StartTime;
            this.endtime = duration.EndTime;
            this.duration = duration;
            this.function = function;
            this.startt = startt;
            this.endt = endt;
        }

        public override Value GetValue(Time t)
        {
            var curt = (endt - startt) * Easing.CalculateEase(ease, t.OSBTime - starttime.OSBTime, Duration.Length.OSBTime) + startt;
            return this.function(curt);
        }

        public override Value GetValue(double t)
        {
            return GetValue(new Time(t));
        }

        public override Value GetValue(double min, double sec, double millisec)
        {
            return GetValue(new generator.Time(min, sec, millisec));
        }

        public Command[] GetCommandList(int fps)
        {
            int length = (int)(duration.Length.OSBTime / 1000 * fps) + 1;
            var list = new Command[length];
            double t = 0.0;
            Value a, b = GetValue(starttime.OSBTime);
            for (var i = 0; i < length - 1; i++)
            {
                var pt = t;
                t += 1000.0 / fps;
                a = GetValue(starttime.OSBTime + pt);
                b = GetValue(starttime.OSBTime + t);
                Interval u = new Interval(starttime.OSBTime + pt, starttime.OSBTime + t);
                list[i] = new Command(type, EaseType.Linear, u, a, b);
            }
            list[length - 1] = new Command(type, EaseType.Linear, new Interval(starttime.OSBTime + t, endtime.OSBTime), b, GetValue(endtime));
            return list;
        }

        public string ToString(int indentation, int fps)
        {
            string s = "";
            //First, check how off the rounding would cause
            int num = (int)((duration.Length.OSBTime) / (1000.0 / fps)) + 1;
            double deviation = ((1000.0 / fps) - Math.Floor(1000.0 / fps)) * num;

            //If the offset is bad enough, do a more expensive but precise version
            int tolerance = 300;
            if (deviation > tolerance)
            {
                var indent = String.Concat(Enumerable.Repeat(" ", indentation));
                var l = GetCommandList(fps);
                for (var i = 0; i < l.Length; i++)
                {
                    s += $"{indent} {l[i]}\r\n";
                }
                s = s.Substring(indentation + 1);
            }

            //Else do the one liner
            else
            {
                s += $"{type},{0},{starttime},{(int)(starttime.OSBTime + 1000.0 / fps)}";
                var t = 0.0;
                while (starttime.OSBTime + t < endtime.OSBTime)
                {
                    var a = GetValue(starttime.OSBTime + t);
                    t += 1000.0 / fps;
                    s += $",{a}";
                }
                s += $",{GetValue(endtime)}";
            }
            return s;
        }
    }

    class Additive : CommandObject
    {
        public Additive()
        {
            this.type = CommandType.Parameter;
            this.ease = 0;
            this.duration = new Interval(0, 0);
            this.starttime = duration.StartTime;
            this.endtime = duration.EndTime;
            
        }
        public override Value GetValue(Time t)
        {
            throw new NotImplementedException();
        }

        public override Value GetValue(double t)
        {
            throw new NotImplementedException();
        }

        public override Value GetValue(double min, double sec, double millisec)
        {
            throw new NotImplementedException();
        }
        public override string ToString()
        {
            return "P,0,0,0,A";
        }
    }

    class Flip : CommandObject
    {
        string fliptype;
        public Flip(Interval duration, string type)
        {
            this.type = CommandType.Parameter;
            this.ease = 0;
            this.duration = duration;
            this.starttime = duration.StartTime;
            this.endtime = duration.EndTime;
            this.fliptype = type;
        }

        public override Value GetValue(Time t)
        {
            throw new NotImplementedException();
        }

        public override Value GetValue(double t)
        {
            throw new NotImplementedException();
        }

        public override Value GetValue(double min, double sec, double millisec)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return $"P,0,{starttime},{endtime},{fliptype}";
        }
    }
}
