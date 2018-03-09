using Microsoft.VisualStudio.TestTools.UnitTesting;
using pk_dotnet.Controllers;

namespace app.tests.Controllers
{
    [TestClass]
    public class PingControllerTests
    {
        readonly PingController _target;

        public PingControllerTests(){
            _target = new PingController();
        }

        [TestMethod]
        public void Ping_Should_ReturnPong()
        {
            var result = _target.Get();

            Assert.AreEqual("pong", result);
        }
    }
}
