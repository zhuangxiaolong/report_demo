using System;

namespace Domain.Core.Log
{
    public abstract class LogBase
    {
        protected LogBase() { }
        protected ILog _log;
        protected LogBase(string logName)
        {
            _log = LogManager.GetLogger(logName);
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="log">日志内容</param>
        /// <param name="function">功能名称</param>
        /// <param name="errorHandleType">异常处理方式</param>
        /// <param name="tryHandle">委托（Try）</param>
        /// <param name="catchHandle">委托（Catch）</param>
        /// <param name="finallyHandle">委托（Finally）</param>
        public void Logger(string function,
                           Action tryHandler,
                           ErrorHandles errorHandle = ErrorHandles.Continue,
                           LogModes logMode = LogModes.Debug,
                           Action<Exception> catchHandler = null,
                           Action finallyHandler = null)
        {
            try
            {
                if (_log != null)
                {
                    _log.Debug(function);
                }
                tryHandler();
            }
            catch (Exception ex)
            {
                _log.Error(function + "失败", ex);

                catchHandler?.Invoke(ex);

                if (errorHandle == ErrorHandles.Throw)
                    throw ex;
            }
            finally
            {
                finallyHandler?.Invoke();
            }
        }

        public T Logger<T>(string function,
                           Func<T> tryHandler,
                           ErrorHandles errorHandle = ErrorHandles.Continue,
                           LogModes logMode = LogModes.Debug,
                           Func<Exception, T> catchHandler = null,
                           Action finalHandler = null)
        {
            try
            { 
                if (_log != null)
                {
                _log.Debug(function);
                }
            return tryHandler();
            }
            catch (Exception ex)
            {
                _log.Error(function + "失败", ex);
                if (catchHandler != null)
                {
                    var result = catchHandler(ex);
                    return result;
                }
                if (errorHandle == ErrorHandles.Throw)
                    throw ex;
                return default(T);
            }
            finally
            {
                finalHandler?.Invoke();
            }
        }
    }
}