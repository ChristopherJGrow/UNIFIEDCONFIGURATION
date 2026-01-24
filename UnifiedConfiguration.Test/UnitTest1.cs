

using System.Data;
using System.Diagnostics;

using Config.Core;
using Config.Core.Extensions;
using Config.Core.Web;

using Models;



using UnifiedConfiguration.Business;

namespace UnifiedConfiguration.Test
{
    public class UnitTest1
    {
        //[Fact]
        //public async Task Test1()
        //{
        //    var ucp = new UnifiedConfigurationProxy( "https://localhost:7214" );

        //    var result = await ucp.TokenCreateAsync( "DEV", "IMS", "PrintAgent", "" );

        //    Assert.True( result );


        //}

        [Fact]
        public async Task TestUniqueSections()
        {
            var res = ConfigResolver.Instance;
            var result = res.GetUniqueSections("DEV","IMS");
            Assert.True( result.Sections.Count() > 0 );
        }
    }
}
