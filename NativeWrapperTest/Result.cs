using System;
using System.Collections.Generic;
using System.Text;

namespace Xaudio2Test
{
    public struct Result
    {
        public int HResult { get; set; }

        public Result(int hResultCode)
        {
            HResult = hResultCode;
        }

        public static implicit operator int(Result hr) => hr.HResult;
        public static implicit operator Result(int code) => new Result(code);

        public void CheckError()
        {
            if (HResult==0)
                return;
            else
                throw new Exception($"Error: {HResult}");
        }
    }
}
