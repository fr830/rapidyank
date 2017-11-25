using System;
using System.Collections.Generic;
using System.Text;

namespace rapidyank.Rapid
{
    /// <summary>
    /// Hourly data of input channels
    /// </summary>
    public class HourCnlData
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public HourCnlData(double hour)
        {
            Hour = hour;
            Modified = false;
            CnlDataExtArr = null;
        }

        /// <summary>
        /// Receive an hour from the beginning of the day
        /// </summary>
        public double Hour { get; private set; }
        /// <summary>
        /// Get or set the data change flag regarding the timestamp in the query
        /// </summary>
        public bool Modified { get; set; }
        /// <summary>
        /// Get or set an array of extended input channel data
        /// </summary>
        public CnlDataExt[] CnlDataExtArr { get; set; }
    }
}
