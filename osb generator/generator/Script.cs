using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osb_generator.generator
{
    abstract class Script
    {
        public readonly Layer Layers;
        private string location;
        protected double bpm, beat;
        protected int seed;
        protected Random random;
        public Script(string s)
        {
            location = s;
            Layers = new Layer();
            Run();
        }

        /// <summary>
        /// Initialize() is a function where you can initialize all the constant variables used throughout the sb
        /// List of variables required to initialize:
        ///     bpm - the bpm of the song
        /// </summary>
        public abstract void Initialize();

        /// <summary>
        /// InitializeRest() initializes the remainder of the variables depending on the information gathered through
        /// manual initialization of Initialize()
        /// </summary>
        private void InitializeRest()
        {
            beat = 60000 / bpm;
            random = new Random(seed);
        }

        /// <summary>
        /// SetLayers() is a convenient function where you can place all your 
        /// </summary>
        public abstract void SetLayers();

        /// <summary>
        /// The MainCode() is where you write every code for the script.
        /// </summary>
        public abstract void MainCode();

        public void Run()
        {
            Initialize();
            InitializeRest();
            SetLayers();
            MainCode();
            Generate.GenerateOSB(this, location);
        }
    }
}
