using Domain.Core;
using System;

namespace UI.Core
{
    public class Configuration: ConfigurationBase
    { 
        public Configuration(IConfigurationProvider provider)
            : base(provider)
        {
            Initialize();
        } 
        public override void Initialize()
        {
            ConnStr = getString("DbConnects:ConnStr");

            ExceptionBeforeCircuit = getInteger("CircuitPolicy:ExceptionBeforeCircuit", 3);
            DurationOfBreak = TimeSpan.FromMilliseconds(getInteger("CircuitPolicy:DurationOfBreak", 3000));
            TimeoutValue = TimeSpan.FromMilliseconds(getInteger("CircuitPolicy:TimeOutValue", 3000));
            
            #region 初始化Redis配置
            RedisConnStr = getString("RedisConfig:Conn");
            RedisDbIndex = getInteger("RedisConfig:DbIndex", -1);
            RedisAsyncObject = new object();
            #endregion
        }

        #region 熔断配置
        public int ExceptionBeforeCircuit { get; set; }
        public TimeSpan DurationOfBreak { get; set; }
        public TimeSpan TimeoutValue { get; set; }
        #endregion
        public string ConnStr { get; set; }
        public string RedisConnStr { get; set; }
        public int RedisDbIndex { get; set; }
        public object RedisAsyncObject { get; set; }
    }
}