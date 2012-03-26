using System.Threading.Tasks;
using MarkPad.Services.Interfaces;
using MarkPad.Services.Metaweblog;
using MarkPad.Services.Settings;

namespace MarkPad.Services.Implementation
{
    public class MetaWeblogService : IMetaWeblogService
    {
        readonly MetaWeblog proxy;

        public MetaWeblogService(string url)
        {
            proxy = new MetaWeblog(url);
        }

        public Task<Post[]> GetRecentPostsAsync(BlogSetting setting, int i)
        {
            return proxy.GetRecentPostsAsync(setting.BlogInfo.blogid, setting.Username, setting.Password, i);
        }

        public Task<BlogInfo[]> GetUsersBlogsAsync(string key, BlogSetting currentBlog)
        {
            return proxy.GetUsersBlogsAsync(key, currentBlog.Username, currentBlog.Password);
        }

        public Task<string> NewPostAsync(BlogSetting blog, Post newpost, bool publish)
        {
            return proxy.NewPostAsync(blog.BlogInfo.blogid, blog.Username, blog.Password, newpost, publish);
        }

        public Task<Post> GetPostAsync(string postid, BlogSetting blog)
        {
            return proxy.GetPostAsync(postid, blog.Username, blog.Password);
        }

        public Task<bool> EditPostAsync(string postid, BlogSetting blog, Post newpost, bool publish)
        {
            return proxy.EditPostAsync(postid, blog.Username, blog.Password, newpost, publish);
        }
    }
}
