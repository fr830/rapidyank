using System;
using System.Collections.Generic;
using System.Text;

namespace rapidyank
{
    public class Config
    {
        public static Config Json { get; set; }

        public string BaseUrl { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConnectionString { get; set; }
        public int IntervalSeconds { get; set; }
        public int MaxCommErrors { get; set; }
        public List<List<int>> Channels { get; set; }
        public List<int> Views { get; set; }

        public Config()
        {
            IntervalSeconds = 5;
        }
    }
}
