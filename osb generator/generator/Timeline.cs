using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osb_generator.generator
{
    class Timeline
    {
        private OrderedList<CommandObject> move, movex, movey, fade, scale, rotate, color, vector, parameters;
        public Timeline()
        {
            Initialize();
        }

        private void Initialize()
        {
            move = new OrderedList<CommandObject>();
            movex = new OrderedList<CommandObject>();
            movey = new OrderedList<CommandObject>();
            fade = new OrderedList<CommandObject>();
            scale = new OrderedList<CommandObject>();
            rotate = new OrderedList<CommandObject>();
            color = new OrderedList<CommandObject>();
            vector = new OrderedList<CommandObject>();
            parameters = new OrderedList<CommandObject>();
        }

        public OrderedList<CommandObject> this[CommandType t]
        {
            get
            {
                switch (t.ToString())
                {
                    case "M":
                        return move;
                    case "F":
                        return fade;
                    case "S":
                        return scale;
                    case "C":
                        return color;
                    case "R":
                        return rotate;
                    case "V":
                        return vector;
                    case "MX":
                        return movex;
                    case "MY":
                        return movey;
                    case "P":
                        return parameters;
                    default:
                        return null;
                }
            }
        }

        public void AddCommand(CommandObject c)
        {
            var list = this[c.Type];
            if (list.Count > 0) { 
                for (var i = 0; i < list.Count; i++)
                {
                    if (list[i].Duration.overlaps(c.Duration))
                    {
                        throw new Exception($"The command overlaps with other command.\n{list[i].Duration} | {c.Duration}");
                    }
                    else if (list[i].Duration.EndTime <= c.Duration.StartTime)
                    {
                        list.Add(c);
                        break;
                    }
                }
            }
            else
            {
                list.Add(c);
            }
        }

        /// <summary>
        /// For the time given, Values(t) will return a dictionary of all value type.  The value will be based on the time in the argument.
        /// If no value exist at time t, the dictionary for commandtype c will return null.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public Dictionary<CommandType, Value> Values(Time t)
        {
            var lists = Lists();
            var r = new Dictionary<CommandType, Value>();
            foreach (var l in lists)
            {
                foreach (var v in l.Value)
                {
                    if (v.StartTime <= t && t <= v.StartTime)
                    {
                        r[l.Key] = v.GetValue(t);
                        break;
                    }
                }
            }
            return r;
        }

        public Dictionary<CommandType, OrderedList<CommandObject>> Lists()
        {
            var d = new Dictionary<CommandType, OrderedList<CommandObject>>();
            d[CommandType.Move] = move;
            d[CommandType.MoveX] = movex;
            d[CommandType.MoveY] = movey;
            d[CommandType.Fade] = fade;
            d[CommandType.Scale] = scale;
            d[CommandType.Rotate] = rotate;
            d[CommandType.Color] = color;
            d[CommandType.Vector] = vector;
            d[CommandType.Parameter] = parameters;
            return d;
        }
    }

    class OrderedList<T> : IList<T> where T : IComparable
    {
        T[] arr;

        public OrderedList()
        {
            arr = new T[0];
        }

        public T this[int index]
        {
            get
            {
                return arr[index];
            }

            set
            {
                arr[index] = value;
            }
        }

        public int Count
        {
            get
            {
                return arr.Length;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public void Add(T item)
        {
            var newarr = new T[arr.Length + 1];
            if (arr.Length == 0)
            {
                newarr[0] = item;
            }
            else
            {
                var sub = 0;
                for (var i = 0; i < newarr.Length; i++)
                {
                    if (i >= arr.Length)
                    {
                        newarr[i] = item;
                        break;
                    }
                    if (arr[i - sub].CompareTo(item) <= 0)
                    {
                        newarr[i] = arr[i];
                    }
                    else if (sub == 0)
                    {
                        newarr[i] = item;
                        sub = 1;
                    }
                    else
                    {
                        newarr[i] = arr[i - sub];
                    }
                }
            }
            
            arr = newarr;
        }

        public void Clear()
        {
            arr = new T[0];
        }

        public bool Contains(T item)
        {
            for (var i = 0; i < arr.Length; i++)
            {
                if (arr[i].Equals(item))
                {
                    return true;
                }
            }
            return false;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            var newarr = new T[arrayIndex <= arr.Length ? arr.Length + array.Length - (arr.Length - arrayIndex) + 1 : arrayIndex + array.Length + 1];
            for (var i = 0; i < Math.Min(arrayIndex, arr.Length); i++)
            {
                newarr[i] = arr[i];
            }

            for (var i = 0; i < array.Length; i++)
            {
                newarr[i + arrayIndex] = array[i];
            }
            arr = newarr;
        }

        public IEnumerator<T> GetEnumerator()
        {
            throw new Exception("Enumeration unavailable");
            return (IEnumerator<T>)new ListEnum<T>(arr);
        }

        public int IndexOf(T item)
        {
            for (var i = 0; i < arr.Length; i++)
            {
                if (arr[i].Equals(item))
                {
                    return i;
                }
            }
            return -1;
        }

        public void Insert(int index, T item)
        {
            Add(item);
        }

        public bool Remove(T item)
        {
            var newarr = new T[arr.Length - 1];
            var u = 0;
            for (var i = 0; i < newarr.Length; i++)
            {
                if (item.Equals(arr[i + u]))
                    u = 1;
                newarr[i] = arr[i + u];
            }
            arr = newarr;
            return u == 1;
        }

        public void RemoveAt(int index)
        {
            if (index < arr.Length)
                Remove(arr[index]);
            else
                throw new IndexOutOfRangeException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new Exception("Enumeration unavailable");
            return GetEnumerator();
        }

        class ListEnum<T> : IEnumerator
        {
            public T[] list;
            public int loc = -1;

            public ListEnum(T[] l)
            {
                list = l;
            }
            public object Current
            {
                get
                {
                    return list[loc];
                }
            }

            public bool MoveNext()
            {
                loc++;
                return loc < list.Length;
            }

            public void Reset()
            {
                loc = -1;
            }
        }
    }
}
