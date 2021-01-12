using System;
using System.Collections.Generic;

namespace Domain.Core.Log
{
    public class LogManager
    {
        private static Lazy<IDictionary<string, ILog>> _clientDict = new Lazy<IDictionary<string, ILog>>(() =>
        {
            return new Dictionary<string, ILog>();
        });

        public static bool Exists(string name)
        {
            return _clientDict.Value.ContainsKey(name);
        }

        public static ILog GetLogger(string name)
        {
            if (Exists(name)) return _clientDict.Value[name];
            ILog log = new Nloger(name);
            _clientDict.Value[name] = log;
            return log;
        }
    }
}