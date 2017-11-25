using System;
using System.Collections.Generic;
using System.Text;

namespace rapidyank.Rapid
{
    public interface IResp
    {
        bool Success { get; set; }
        string ErrorMessage { get; set; }
    }

    public class Resp<T> : IResp
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public T Data { get; set; }
    }

    public class ArcResp<T, U> : Resp<T>
    {
        public U DataAge { get; set; }
    }
}
