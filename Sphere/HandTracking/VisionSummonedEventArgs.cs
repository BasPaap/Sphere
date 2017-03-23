using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bas.Sphere.HandTracking
{
    public sealed class VisionSummonedEventArgs : EventArgs
    {
        public VisionType Vision { get; set; }
        public string FileName { get; set; }

        public VisionSummonedEventArgs(VisionType vision)
        {
            Vision = vision;
        }

        public VisionSummonedEventArgs(string fileName)
        {
            FileName = fileName;
        }
    }
}
