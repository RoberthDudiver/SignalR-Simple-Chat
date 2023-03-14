using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generic
{
    public static class LogsHelpers
    {
        static Stopwatch _timeLog;
        public static Stopwatch TimeLog
        {
            get
            {
                if (_timeLog == null)
                {
                    _timeLog = new Stopwatch();
                }
                return _timeLog;
            }
        }


    }

    public class LogsEntity
    {
        public string Error
        {
            get;
            set;
        }

        public string Message
        {
            get;
            set;
        }

        public long TimeEjecution
        {
            get;
            set;
        }

        public String Method
        {
            get;
            set;
        }
        public String Class
        {
            get;
            set;
        }
    }
}
