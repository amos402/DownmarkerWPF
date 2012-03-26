using System.Threading.Tasks;
using MarkPad.Services.Metaweblog;
using MarkPad.Services.Settings;

namespace MarkPad.Services.Interfaces
{
    public interface IMetaWeblogService
    {
        Task<Post[]> GetRecentPostsAsync(BlogSetting setting, int i);
        Task<BlogInfo[]> GetUsersBlogsAsync(string key, BlogSetting currentBlog);
        Task<string> NewPostAsync(BlogSetting blog, Post newpost, bool publish);
        Task<Post> GetPostAsync(string postid, BlogSetting blog);
        Task<bool> EditPostAsync(string postid, BlogSetting blog, Post newpost, bool publish);
    }
}
