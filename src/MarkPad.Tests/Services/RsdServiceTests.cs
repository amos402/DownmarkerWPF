using System.Threading.Tasks;
using MarkPad.Services.Metaweblog.Rsd;
using NSubstitute;
using Xunit;

namespace MarkPad.Tests.Services
{
    public class RsdServiceTests
    {
        readonly IWebRequestFactory webRequestFactory;
        readonly RsdService rsdService;

        public RsdServiceTests()
        {
            // webRequestFactory = new TestWebRequestFactory();
            webRequestFactory = Substitute.For<IWebRequestFactory>();
            rsdService = new RsdService(webRequestFactory);
        }

        [Fact]
        public void discovers_codeplex_metaweblog_api()
        {
            // arrange
            webRequestFactory.GetResult("http://project.codeplex.com")
                             .Returns(TaskEx.FromResult(CodeplexProjectPage));

            webRequestFactory.GetResult("http://project.codeplex.com/rsd")
                             .Returns(TaskEx.FromResult(CodeplexProjectRsdFile));

            // act
            var result = rsdService.DiscoverAddress("http://project.codeplex.com").Result;

            // assert
            Assert.True(result.Success);
            Assert.Equal("https://www.codeplex.com/site/metaweblog", result.MetaWebLogApiLink);
        }

        [Fact]
        public void discovers_site_with_rsdxml_file_in_root()
        {
            // arrange
            webRequestFactory.GetResult("http://funnelweblog.net/rsd.xml")
                             .Returns(TaskEx.FromResult(FunnelWebRsdFile));

            // act
            var result = rsdService.DiscoverAddress("http://funnelweblog.net").Result;

            // assert
            Assert.True(result.Success);
            Assert.Equal("http://funnelweblog.net/blogapi", result.MetaWebLogApiLink);
        }

        private const string FunnelWebRsdFile = @"<rsd version=""1.0"" xmlns=""http://archipelago.phrasewise.com/rsd"">
  <service>
    <engineName>FunnelWeblog</engineName>
    <engineLink>http://www.funnelweblog.com/</engineLink>
    <homePageLink>http://funnelweblog.net/</homePageLink>
    <apis>
      <api name=""MetaWeblog"" preferred=""true"" apiLink=""http://funnelweblog.net/blogapi"" blogId=""something"" />
    </apis>
  </service>
</rsd>";

        private const string CodeplexProjectPage = @"<!DOCTYPE html><html xmlns=""http://www.w3.org/1999/xhtml"" xml:lang=""en"" lang=""en"">
<head>
<link rel=""EditURI"" type=""application/rsd+xml"" title=""RSD"" href='http://project.codeplex.com/rsd' /></head><body></body></html>";

        private const string CodeplexProjectRsdFile = @"<rsd version=""1.0"">
<service>
<engineName>CodePlex</engineName>
<engineLink>http://www.codeplex.com</engineLink>
<homePageLink>http://phoenixframework.codeplex.com/</homePageLink>
<apis>
<api name=""MetaWeblog"" blogID=""project"" preferred=""true"" apiLink=""https://www.codeplex.com/site/metaweblog""/>
</apis>
</service>
</rsd>";
    }
}