using osb_generator.generator;
using System;
using System.Collections.Generic;
using System.Linq;
using osb_generator.scripts;
using System.Configuration;

namespace osb_generator
{
    class Program
    {
        static void Main(string[] args)
        {
            //Util.Initialize(@"D:\Game\osu!\Songs\550344 Manami Numakura - Sakebe\sb");
            Pon pon = new scripts.Pon(ConfigurationManager.AppSettings["Sakebe"]);
        }
    }

}
