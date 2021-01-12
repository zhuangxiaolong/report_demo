using NLog;
using System;
using System.Text;

namespace Domain.Core.Log
{
    public class Nloger : ILog
    {
        private NLog.Logger _client;
        private string _logSource;
        public Nloger(string logName)
        {
            _logSource = logName;
            _client = NLog.LogManager.GetLogger(logName);
        }

        public LogLevels CurrentLogLevel { get; set; }
        public bool IsFatalEnabled
        {
            get { return CurrentLogLevel <= LogLevels.Fatal; }
        }

        public bool IsWarnEnabled
        {
            get { return CurrentLogLevel <= LogLevels.Warn; }
        }

        public bool IsInfoEnabled
        {
            get { return CurrentLogLevel <= LogLevels.Info; }
        }

        public bool IsDebugEnabled
        {
            get { return CurrentLogLevel <= LogLevels.Debug; }
        }

        public bool IsErrorEnabled
        {
            get { return CurrentLogLevel <= LogLevels.Error; }
        }


        public void Debug(object message)
        {
            if (IsDebugEnabled)
                _client.Debug(message.ToString());
        }

        public void Debug(object message, Exception exception)
        {
            if (IsDebugEnabled)
                _client.Debug(exception, message.ToString);
        }

        public void DebugFormat(string format, params object[] args)
        {
            if (IsDebugEnabled)
            {
                try
                {
                    _client.Debug(string.Format(format, args));
                }
                catch (Exception e)
                {
                    var str = new StringBuilder();
                    str.Append(format);
                    if (args != null)
                    {
                        foreach (var o in args)
                        {
                            str.Append(o);
                        }
                    }
                    _client.Debug(str);
                }
            }
        }

        public void Error(object message)
        {
            if (IsErrorEnabled)
                _client.Error(message.ToString());
        }

        public void Error(object message, Exception exception)
        {
            if (IsErrorEnabled)
                _client.Error(exception, message.ToString);
        }

        public void ErrorFormat(string format, params object[] args)
        {
            if (IsErrorEnabled)
            {
                try
                {
                    _client.Error(string.Format(format, args));
                }
                catch (Exception e)
                {
                    var str = new StringBuilder();
                    str.Append(format);
                    if (args != null)
                    {
                        foreach (var o in args)
                        {
                            str.Append(o);
                        }
                    }
                    _client.Error(str);
                }
            }
        }

        public void Fatal(object message)
        {
            if (IsFatalEnabled)
                _client.Fatal(message.ToString());
        }

        public void Fatal(object message, Exception exception)
        {
            if (IsFatalEnabled)
                _client.Fatal(exception, message.ToString);
        }

        public void FatalFormat(string format, params object[] args)
        {
            if (IsFatalEnabled)
            {
                try
                {
                    _client.Fatal(string.Format(format, args));
                }
                catch (Exception e)
                {
                    var str = new StringBuilder();
                    str.Append(format);
                    if (args != null)
                    {
                        foreach (var o in args)
                        {
                            str.Append(o);
                        }
                    }
                    _client.Fatal(str);
                }
            }
        }

        public void Info(object message, Exception exception)
        {
            if (IsInfoEnabled)
                _client.Info(exception, message.ToString);
        }

        public void Info(object message)
        {
            if (IsInfoEnabled)
                _client.Info(message.ToString());
        }

        public void InfoFormat(string format, params object[] args)
        {
            if (IsInfoEnabled)
            {
                try
                {
                    _client.Info(string.Format(format, args));
                }
                catch (Exception e)
                {
                    var str = new StringBuilder();
                    str.Append(format);
                    if (args != null)
                    {
                        foreach (var o in args)
                        {
                            str.Append(o);
                        }
                    }
                    _client.Info(str);
                }
            }
        }

        public void Initialize()
        {
            _client = NLog.LogManager.GetCurrentClassLogger();
        }

        public void Warn(object message)
        {
            if (IsWarnEnabled)
                _client.Warn(message.ToString());
        }

        public void Warn(object message, Exception exception)
        {
            if (IsWarnEnabled)
                _client.Warn(exception, message.ToString);
        }

        public void WarnFormat(string format, params object[] args)
        {
            if (IsWarnEnabled)
            {
                try
                {
                    _client.Warn(string.Format(format, args));
                }
                catch (Exception e)
                {
                    var str = new StringBuilder();
                    str.Append(format);
                    if (args != null)
                    {
                        foreach (var o in args)
                        {
                            str.Append(o);
                        }
                    }
                    _client.Warn(str);
                }
            }
        }
    }
}