using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OES.Modules.Core
{
    public class Result
    {
        public bool Success { get; set; }

        public List<ResultError> Errors { get; set; }

        public Exception AttachedException { get; set; }

        public object ReturnObject { get; set; }
    }

    public class ResultError
    {
        public string Message { get; set; }

        public string Key { get; set; }
    }
}
