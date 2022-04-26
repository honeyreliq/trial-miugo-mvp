using NUnit.Framework;
using System.Threading.Tasks;

namespace IUGOCare.Application.IntegrationTests
{
    public class TestBase
    {
        [SetUp]
        public async Task TestSetUp()
        {
            await Testing.ResetState();
        }
    }
}
