using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace osb_generator.generator
{
    static class Generate
    {
        public static void GenerateOSB(Script s, string location)
        {
            var h = s.Layers.LayerHierarchy();
            var osb = "[Events]\r\n//Background and Video events\r\n//Storyboard Layer 0 (Background)\r\n";
            for (var i = 0; i < h.Count; i++)
            {
                while (s.Layers[h[i]].Count > 0)
                {
                    osb += s.Layers[h[i]].Dequeue();
                }
            }

            osb += "//Storyboard Layer 1 (Fail)\r\n//Storyboard Layer 2 (Pass)\r\n//Storyboard Layer 3 (Foreground)\r\n//Storyboard Sound Samples";

            File.WriteAllText(location, osb);
        }
    }
}
