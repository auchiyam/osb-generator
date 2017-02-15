using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osb_generator.generator
{
    class Layer
    {
        public Dictionary<string, Queue<StoryboardObject>> Layers;
        private List<string> order;
        private int count;

        public Layer()
        {
            Layers = new Dictionary<string, Queue<StoryboardObject>>();
            order = new List<string>();
            count = 0;
            Layers["general"] = new Queue<StoryboardObject>();
        }

        public Queue<StoryboardObject> this[string s]
        {
            get
            {
                return Layers[s];
            }
        }

        public void CreateLayer(string s)
        {
            if (!Layers.Keys.Contains(s))
            {
                order.Add(s);
                Layers[s] = new Queue<StoryboardObject>();
                count++;
            }
            else
            {
                throw new ArgumentException($"The layer with the name {s} already exists.");
            }
        }

        public List<string> LayerHierarchy()
        {
            return order;
        }

        #region Sprites
        public Sprite CreateSprite(string layer, string image) => CreateSprite(layer, image, 25);
        public Sprite CreateSprite(string layer, string image, int fps) => CreateSprite(layer, image, Origin.Centre, fps);
        public Sprite CreateSprite(string layer, string image, Origin o) => CreateSprite(layer, image, o, 25);
        public Sprite CreateSprite(string layer, string image, Origin o, int fps)
        {
            Sprite s = new Sprite(image, o, fps);
            if (Layers.Keys.Contains(layer))
            {
                Layers[layer].Enqueue(s);
            }
            else
            {
                throw new ArgumentException("The layer specified does not exist");
            }
            return s;
        }

        public void AddSprite(string layer, params Sprite[] s)
        {
            if (Layers.Keys.Contains(layer))
            {
                foreach (var spr in s)
                {
                    Layers[layer].Enqueue(spr);
                }
            }
            else
            {
                throw new ArgumentException("The layer specified does not exist");
            }
        }
        #endregion

        #region Animation
        public Animation CreateAnimation(string layer, string image, int framecount, int frameinterval, LoopType loop) => CreateAnimation(layer, image, Origin.Centre, framecount, frameinterval, loop, 25);
        public Animation CreateAnimation(string layer, string image, int framecount, int frameinterval, LoopType loop, int fps) => CreateAnimation(layer, image, Origin.Centre, framecount, frameinterval, loop, fps);
        public Animation CreateAnimation(string layer, string image, Origin o, int framecount, int frameinterval, LoopType loop) => CreateAnimation(layer, image, Origin.Centre, framecount, frameinterval, loop, 25);
        public Animation CreateAnimation(string layer, string image, Origin o, int framecount, int frameinterval, LoopType loop, int fps)
        {
            Animation s = new Animation(image, o, framecount, frameinterval, loop, fps);
            if (Layers.Keys.Contains(layer))
            {
                Layers[layer].Enqueue(s);
            }
            else
            {
                throw new ArgumentException("The layer specified does not exist");
            }
            return s;
        }

        public void AddAnimation(string layer, Animation s)
        {
            if (Layers.Keys.Contains(layer))
            {
                Layers[layer].Enqueue(s);
            }
            else
            {
                throw new ArgumentException("The layer specified does not exist");
            }
        }
        #endregion
    }
}
