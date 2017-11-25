using System;
using System.Collections.Generic;
using System.Text;

namespace rapidyank.Rapid
{
    public struct CnlData
    {
        public static readonly CnlData Empty;

        public double Val { get; set; }
        public int Stat { get; set; }

        public CnlData(double val, int stat)
        {
            this.Val = val;
            this.Stat = stat;
        }
    }
}
