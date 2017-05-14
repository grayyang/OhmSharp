using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace OhmSharp
{
    /// <summary>
    /// Represents the connection to Redis servers and the collections of objects stored within them
    /// </summary>
    public sealed class RedisContext : IDisposable
    {
        /// <summary>
        /// Create <see cref="RedisContext"/> with the connection configuration specified
        /// </summary>
        /// <param name="configuration">Redis connection string</param>
        /// <param name="db">numeric identifier of the database to connect</param>
        /// <returns>RedisContext created</returns>
        public static RedisContext Create(string configuration, int db = -1)
        {
            return Create(ConfigurationOptions.Parse(configuration), db);
        }

        /// <summary>
        /// Asynchronously create <see cref="RedisContext"/> with the connection configuration specified
        /// </summary>
        /// <param name="configuration">Redis connection string</param>
        /// <param name="db">numeric identifier of the database to connect</param>
        /// <returns>task represents the asynchronous create operation with TResult as RedisContext created</returns>
        public static Task<RedisContext> CreateAsync(string configuration, int db = -1)
        {
            return CreateAsync(ConfigurationOptions.Parse(configuration), db);
        }

        /// <summary>
        /// Create <see cref="RedisContext"/> with the connection configuration specified
        /// </summary>
        /// <param name="configuration">Redis connection configuration</param>
        /// <param name="db">numeric identifier of the database to connect</param>
        /// <returns>RedisContext created</returns>
        public static RedisContext Create(ConfigurationOptions configuration, int db = -1)
        {
            var context = new RedisContext();
            context.Connection = ConnectionMultiplexer.Connect(configuration);
            context.Database = context.Connection.GetDatabase(db);

            return context;
        }

        /// <summary>
        /// Asynchronously create <see cref="RedisContext"/> with the connection configuration specified
        /// </summary>
        /// <param name="configuration">Redis connection configuration</param>
        /// <param name="db">numeric identifier of the database to connect</param>
        /// <returns>task represents the asynchronous create operation with TResult as RedisContext created</returns>
        public static async Task<RedisContext> CreateAsync(ConfigurationOptions configuration, int db = -1)
        {
            var context = new RedisContext();
            context.Connection = await ConnectionMultiplexer.ConnectAsync(configuration);
            context.Database = context.Connection.GetDatabase(db);

            return context;
        }

        /// <summary>
        /// Close the underlying Redis connection 
        /// </summary>
        /// <param name="allowCommandsToComplete">whether allow Redis commands to complete</param>
        public void Close(bool allowCommandsToComplete = true)
        {
            if (!_disposed)
            {
                Connection?.Close(allowCommandsToComplete);
                Connection?.Dispose();
                GC.SuppressFinalize(this);

                _disposed = true;
            }
        }

        /// <summary>
        /// Asynchronously close the underlying Redis connection 
        /// </summary>
        /// <param name="allowCommandsToComplete">whether allow Redis commands to complete</param>
        /// <returns>task represents the asynchronous close operation</returns>
        public async Task CloseAsync(bool allowCommandsToComplete = true)
        {
            if (!_disposed)
            {
                await Connection?.CloseAsync(allowCommandsToComplete);
                Connection?.Dispose();
                GC.SuppressFinalize(this);

                _disposed = true;
            }
        }

        /// <summary>
        /// Get the collection of objects of type <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">type of objects</typeparam>
        /// <returns>interface used to query, add, update and delete object</returns>
        public IRedisObjectCollection<T> GetCollection<T>()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(RedisContext));

            throw new NotImplementedException();
        }

        /// <summary>
        /// Connection to the underlying Redis servers
        /// </summary>
        public ConnectionMultiplexer Connection { get; private set; } = null;

        /// <summary>
        /// Redis database objects are stored
        /// </summary>
        public IDatabase Database { get; private set; } = null;

        /// <summary>
        /// Close the underlying Redis connection 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private RedisContext()
        { }

        ~RedisContext()
        {
            Dispose(false);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Connection?.Close();
                    Connection?.Dispose();
                }

                _disposed = true;
            }
        }

        private bool _disposed = false;
    }
}
