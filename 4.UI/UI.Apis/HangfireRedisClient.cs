using System;
using StackExchange.Redis;

namespace UI.Apis
{
    public class HangfireRedisClient
    {
        private static ConnectionMultiplexer _redis;
        private static string _connStr;

        public HangfireRedisClient(string connStr)
        {
            _connStr = connStr;
        }
        private static Lazy<ConnectionMultiplexer>
            lazyConnection = new Lazy<ConnectionMultiplexer>
            (() =>
            {
                var configuration = ConfigurationOptions.Parse(_connStr);
                configuration.AbortOnConnectFail = false;
                var multiplexer = ConnectionMultiplexer.Connect(configuration);
                return multiplexer;
            });
        public static ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }
    }
}