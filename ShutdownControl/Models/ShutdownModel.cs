using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShutdownControl.Models
{
    public class ShutdownModel
    {
        public string _EndTime { get; set; }
        public string _SecondVis { get; set; }
        public string _StartVis { get; set; }
        public int _End10Second { get; set; }
    }
}
