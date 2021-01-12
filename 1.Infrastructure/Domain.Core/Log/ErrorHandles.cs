namespace Domain.Core.Log
{
    public enum ErrorHandles
    {
        /// <summary>
        /// 抛出异常
        /// </summary>
        Throw = 1,
        /// <summary>
        /// 忽略异常，继续执行
        /// </summary>
        Continue = 2
    }
}