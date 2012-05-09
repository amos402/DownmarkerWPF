using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using MarkPad.Services.Metaweblog.Rsd;

namespace MarkPad.Services
{
    public class WebRequestFactory : IWebRequestFactory
    {
        public Task<string> GetResult(string url)
        {
            var taskCompletionSource = new TaskCompletionSource<string>();

            var request = WebRequest.Create(url);

            request.GetResponseAsync()
                          .ContinueWith(c =>
                            {
                                if (c.IsFaulted)
                                {
                                    taskCompletionSource.SetResult(string.Empty);
                                    return;
                                }

                                if (c.IsCanceled)
                                {
                                    taskCompletionSource.SetCanceled();
                                    return;
                                }

                                if (c.Exception != null)
                                {
                                    taskCompletionSource.SetException(c.Exception);
                                    return;
                                }
                                
                                using (var stream = c.Result.GetResponseStream())
                                using (var reader = new StreamReader(stream))
                                {
                                    taskCompletionSource.SetResult(reader.ReadToEnd());
                                }
                            });

            return taskCompletionSource.Task;
        }

        public WebRequest Create(Uri requestUri)
        {
            return WebRequest.Create(requestUri);
        }
    }
}