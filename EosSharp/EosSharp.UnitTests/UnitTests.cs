using System;
using System.Threading.Tasks;
using EosSharp.Api.v1;
using EosSharp.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace EosSharp.UnitTests
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public async Task GetInfo()
        {
            bool success = false;
            try
            {
                var api = new EosApi(new EosConfigurator()
                {
                    HttpEndpoint = "https://nodeos01.btuga.io"
                });

                var result = await api.GetInfo();
                Console.WriteLine(JsonConvert.SerializeObject(result));
                success = true;
            }
            catch(ApiException ex)
            {
                Console.WriteLine(ex.StatusCode);
                Console.WriteLine(ex.Content);
            }

            Assert.IsTrue(success);
        }
    }
}
