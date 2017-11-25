using System;
using System.Collections.Generic;
using System.Text;

namespace rapidyank.Db
{
    public class reading
    {
        public long id { get; set; }
        public int channel { get; set; }
        public DateTime date { get; set; }
        public double value { get; set; }
        public int status { get; set; }
        public string text { get; set; }
        public string textandunit { get; set; }
        public string color { get; set; }
    }
}
