using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osb_generator.generator
{
    public enum EaseType
    {
        Linear, EasingOut, EasingIn, QuadIn, QuadOut, QuadInOut, CubicIn, CubicOut, CubicInOut, QuartIn, QuartOut, QuartInOut, QuintIn, QuintOut, QuintInOut, SineIn, SineOut, SineInOut, ExpoIn, ExpoOut, ExpoInOut, CircIn, CircOut, CircInOut, ElasticIn, ElasticOut, ElasticHalfOut, ElasticQuarterOut, ElasticInOut, BackIn, BackOut, BackInOut, BounceIn, BounceOut, BounceInOut
    }

    static class Easing
    {
        /// <summary>
        /// Calculates the value of t given the following parameters
        /// </summary>
        /// <param name="e">The Ease function calculated</param>
        /// <param name="currt">The current time</param>
        /// <param name="d">The duration of the function (Ex: 50 seconds ~ 2 minutes would be 1 minutes and 10 seconds long)</param>
        /// <returns>The result of the Ease function in double</returns>
        public static double CalculateEase(EaseType e, double currt, double d)
        {
            var t = currt / d;
            var v = 0D;
            var f = 0D;
            switch (e)
            {
                case EaseType.Linear:
                    v = t;
                    break;
                case EaseType.EasingIn:
                    v = t * t;
                    break;
                case EaseType.EasingOut:
                    v = -t * (t - 2);
                    break;
                case EaseType.QuadIn:
                    v = t * t;
                    break;
                case EaseType.QuadOut:
                    v = -t * (t - 2);
                    break;
                case EaseType.QuadInOut:
                    v = t < .5f ? 2 * t * t : (-2 * t * t) + (4 * t) - 1;
                    break;
                case EaseType.CubicIn:
                    v = t * t * t;
                    break;
                case EaseType.CubicOut:
                    f = t - 1;
                    v = f * f * f + 1;
                    break;
                case EaseType.CubicInOut:
                    f = ((2 * t) - 2);
                    v = t < .5 ? 4 * t * t * t : .5 * f * f * f + 1;
                    break;
                case EaseType.QuartIn:
                    v = t * t * t * t;
                    break;
                case EaseType.QuartOut:
                    f = t - 1;
                    v = f * f * f * (1 - t) + 1;
                    break;
                case EaseType.QuartInOut:
                    f = t - 1;
                    v = t < .5 ? 8 * t * t * t * t : -8 * f * f * f * f + 1;
                    break;
                case EaseType.QuintIn:
                    v = t * t * t * t * t;
                    break;
                case EaseType.QuintOut:
                    f = t - 1;
                    v = f * f * f * f * f + 1;
                    break;
                case EaseType.QuintInOut:
                    f = ((2 * t) - 2);
                    v = t < .5 ? 16 * t * t * t * t * t : .5 * f * f * f * f * f + 1;
                    break;
                case EaseType.SineIn:
                    v = Math.Sin(t - 1 * (Math.PI / 2)) + 1;
                    break;
                case EaseType.SineOut:
                    v = Math.Sin(t * (Math.PI / 2));
                    break;
                case EaseType.SineInOut:
                    v = .5 * (1 - Math.Cos(t * Math.PI));
                    break;
                case EaseType.ExpoIn:
                    v = t == 1.0 ? t : Math.Pow(2, 10 * (t - 1));
                    break;
                case EaseType.ExpoOut:
                    v = t == 1.0 ? t : 1 - Math.Pow(2, -10 * t);
                    break;
                case EaseType.ExpoInOut:
                    if (t == 0 || t == 1)
                    {
                        v = t;
                    }
                    else
                    {
                        v = t < .5 ? .5 * Math.Pow(2, (20 * t) - 10) : -.5 * Math.Pow(2, (-20 * t) + 10) + 1;
                    }
                    break;
                case EaseType.CircIn:
                    v = 1 - Math.Sqrt(1 - (t * t));
                    break;
                case EaseType.CircOut:
                    v = Math.Sqrt((2 - t) * t);
                    break;
                case EaseType.CircInOut:
                    v = t < .5 ? .5 * (1 - Math.Sqrt(1 - 4 * (t * t))) : .5 * Math.Sqrt(-((2 * t) - 3) * ((2 * t) - 1)) + 1;
                    break;
                case EaseType.ElasticIn:
                    v = Math.Sin(13 * (Math.PI / 2) * Math.Pow(2, 10 * (t - 1)));
                    break;
                case EaseType.ElasticOut:
                    v = Math.Sin(-13 * (Math.PI / 2) * (t + 1)) * Math.Pow(2, -10 * t) + 1;
                    break;
                //To be Implemented
                case EaseType.ElasticHalfOut:
                    break;
                //To be Implemented
                case EaseType.ElasticQuarterOut:
                    break;
                case EaseType.ElasticInOut:
                    v = t < .5 ? .5 * Math.Sin(13 * (Math.PI / 2) * (2 * t)) * Math.Pow(2, 10 * ((2 * t) - 1)) : .5 * (Math.Sin(-13 * (Math.PI / 2) * ((2 * t - 1) + 1)) * Math.Pow(2, -10 * (2 * t - 1)) + 2);
                    break;
                case EaseType.BackIn:
                    v = t * t * t - t * Math.Sin(t * Math.PI);
                    break;
                case EaseType.BackOut:
                    f = (1 - t);
                    v = 1 - (f * f * f - f * Math.Sin(f * Math.PI));
                    break;
                case EaseType.BackInOut:
                    if (t < .5)
                    {
                        f = 2 * t;
                        v = .5 * (f * f * f - f * Math.Sin(f * Math.PI));
                    }
                    else
                    {
                        f = (1 - (2 * t - 1));
                        v = .5 * (1 - (f * f * f - f * Math.Sin(f * Math.PI))) + .5;
                    }
                    break;
                case EaseType.BounceIn:
                    v = 1 -  CalculateEase(EaseType.BounceOut, currt, d);
                    break;
                case EaseType.BounceOut:
                    if (t < 4 / 11f)
                    {
                        v = (121 * t * t) / 16f;
                    }
                    else if (t < 8/11f)
                    {
                        v = (363 / 40f * t * t) - (99 / 10f * t) + 17 / 5f;
                    }
                    else if (t < 9/10f)
                    {
                        v = (4356 / 361f * t * t) - (35442 / 1805f * t) + (16061 / 1805f);
                    }
                    else
                    {
                        v = (54 / 5f * t * t) - (513 / 25f * t) + 268 / 25f;
                    }
                    break;
                case EaseType.BounceInOut:
                    v = t > .5 ? .5 * t * CalculateEase(EaseType.BounceIn, currt * 2, d) : .5 * CalculateEase(EaseType.BounceOut, (currt * 2) + d, d) + .5;
                    break;
            }
            return v;
        }

        public static Value CalculateEase(EaseType e, Value c, double t, double d, Value b)
        {
            var v = c * CalculateEase(e, t, d) + b;
            return v;
        }
    }
}
