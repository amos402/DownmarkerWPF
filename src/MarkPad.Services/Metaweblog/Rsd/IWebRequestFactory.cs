using System;
using System.Net;
using System.Threading.Tasks;

namespace MarkPad.Services.Metaweblog.Rsd
{
    public interface IWebRequestFactory
    {
        Task<string> GetResult(string url);

        [Obsolete]
        WebRequest Create(Uri requestUri);
    }
}