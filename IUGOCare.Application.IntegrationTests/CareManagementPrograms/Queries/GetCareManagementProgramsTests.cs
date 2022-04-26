using System.Threading.Tasks;
using IUGOCare.Application.CareManagementPrograms.Queries;
using NUnit.Framework;

namespace IUGOCare.Application.IntegrationTests.CareManagementPrograms.Queries
{
    public class GetCareManagementProgramsTests : TestBase
    {
        [Test]
        public async Task ShouldReturnCareManagementProgramsViewModel()
        {            
            var query = new GetCareManagementProgramsQuery();
            var result = await Testing.SendAsync(query);

            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(CareManagementProgramsVm), result.GetType());
        }
    }
}
