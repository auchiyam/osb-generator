using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osb_generator.generator
{
    class Time : IComparable
    {
        private double t, min, sec, millisec;
        public double OSBTime
        {
            get
            {
                return t;
            }
        }

        public double Minutes
        {
            get
            {
                return min;
            }
        }

        public double Seconds
        {
            get
            {
                return sec;
            }
        }

        public double Milliseconds
        {
            get
            {
                return millisec;
            }
        }

        public Time(double min, double sec, double millisec)
        {
            t = 0;
            t += millisec;
            t += sec * 1000;
            t += min * 60 * 1000;

            this.min = min;
            this.sec = sec;
            this.millisec = sec;
        }

        public Time(double t)
        {
            this.t = t;

            this.millisec = t % 1000;
            this.sec = (int)(t / 1000) % 60;
            this.min = (int)((t / (1000 * 60)) % 60);
        }

        public override string ToString()
        {
            return $"{(int)Math.Floor(t)}";
        }

        public string ToFormattedString()
        {
            return $"{min:00}:{sec:00}.{millisec:000}";
        }

        public int CompareTo(object obj)
        {
            var t = obj as Time;
            if (t != null)
            {
                return (int)Math.Ceiling(this.t - t.t);
            }
            else
            {
                throw new ArgumentException("obj is not a Time class");
            }
        }

        public static bool operator <(Time a, Time b)
        {
            return a.CompareTo(b) < 0;
        }

        public static bool operator >(Time a, Time b)
        {
            return a.CompareTo(b) > 0;
        }

        public static bool operator ==(Time a, Time b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Time a, Time b)
        {
            return !(a == b);
        }

        public static bool operator <=(Time a, Time b)
        {
            return a < b || a == b;
        }

        public static bool operator >=(Time a, Time b)
        {
            return a > b || a == b;
        }

        public override bool Equals(object obj)
        {
            try
            {
                var t = (Time)obj;
                return t.OSBTime == this.OSBTime;
            } catch (NullReferenceException e)
            {
                return false;
            }
        }
    }

    class Interval
    {
        private class InvalidTimeIntervalException : Exception
        {
            public InvalidTimeIntervalException(string message) : base(message) {}
        }
        private Time start, end;
        public Time StartTime
        {
            get
            {
                return start;
            }
        }

        public Time EndTime
        {
            get
            {
                return end;
            }
        }

        public Time Length
        {
            get
            {
                return new Time(end.OSBTime - start.OSBTime);
            }
        }

        public Interval(double startmin, double startsec, double startmillisec, double endmin, double endsec, double endmillisec) : this(new Time(startmin, startsec, startmillisec), new Time(endmin, endsec, endmillisec)) { }

        public Interval(double start, double end) : this(new Time(start), new Time(end)){ }

        public Interval(Time start, Time end)
        {
            if (end < start)
            {
                throw new InvalidTimeIntervalException("The end time is earlier than start time");
            }
            this.start = start;
            this.end = end;
        }

        public bool overlaps(Interval b)
        {
            return this.StartTime < b.EndTime && b.StartTime < this.EndTime;
        }

        public override string ToString()
        {
            return $"{start.ToFormattedString()} ~ {end.ToFormattedString()}";
        }
    }
}
