using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace OhmSharp.Test
{
    [TestClass]
    public class RedisContextTest
    {
        [TestMethod]
        public void CreateContext()
        {
            using (var context = RedisContext.Create("localhost"))
            {
                Assert.IsNotNull(context);
                Assert.IsNotNull(context.Connection);
                Assert.IsNotNull(context.Database);
                Assert.IsTrue(context.Connection.IsConnected);

                context.Close();
                Assert.IsFalse(context.Connection.IsConnected);
            }
        }

        [TestMethod]
        public async Task CreateContextAsync()
        {
            using (var context = await RedisContext.CreateAsync("localhost"))
            {
                Assert.IsNotNull(context);
                Assert.IsNotNull(context.Connection);
                Assert.IsNotNull(context.Database);
                Assert.IsTrue(context.Connection.IsConnected);

                await context.CloseAsync();
                Assert.IsFalse(context.Connection.IsConnected);
            }
        }
    }
}
