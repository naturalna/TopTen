using Parse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopTenApp.Models;

namespace TopTenApp.Services
{
    public interface IDataService
    {
        string GetUsername();
        void ResetPassword(string email);
        void LogOut();
        Task<User> Login(string username, string pass);
        Task<User> CreateUser(string username, string pass, string email);
        Task<IEnumerable<Groups>> GetAllGroups();
        Task<ParseObject> GetGroupByObjectId(string id);
        Task<IEnumerable<ParseObject>> GetCommentsByArticle(ParseObject article);
       // Task CreateComment(ParseObject article, ParseUser user, string content);
        Task<IEnumerable<ArticlesInGroup>> GetAllArticlesByGroup(ParseObject group);
        Task<Article> GetArticleByObjectId(string id);
        Task CreateArticle(ParseUser user, string content);
        Task<IEnumerable<ArticlesInGroup>> GetAllArticlesByQuery(string querySearch);

    }
}
