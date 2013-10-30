using Parse;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TopTenApp.Models;
using Windows.UI.Popups;

namespace TopTenApp.Services
{
    public class ExternalDataService : IDataService
    {
        public async Task<User> CreateUser(string username, string pass, string email)
        {
            try
            {
                var user = new ParseUser()
                {
                    Username = username,
                    Password = pass,
                    Email = email
                };

                User createdUser = new User()
                {
                    Username = user.Username
                };

                await user.SignUpAsync();

                await ParseUser.LogInAsync(username, pass);
                ((App)App.Current).AuthenticatedUser = ParseUser.CurrentUser;

                return createdUser;
            }
            catch (Exception e)
            {
                new MessageDialog(e.Message).ShowAsync();
                return null;
            }
        }

        public async Task<User> Login(string username, string pass)
        {
            try
            {
                await ParseUser.LogInAsync(username, pass);
                var currentUser = ParseUser.CurrentUser;
                User createdUser = new User()
                {
                    Username = currentUser.Username
                };

                ((App)App.Current).AuthenticatedUser = currentUser;

                return createdUser;
            }
            catch (Exception e)
            {
                new MessageDialog("Грешно потребителско име или парола.").ShowAsync();
                return null;
            }
        }

        public void LogOut()
        {
            ParseUser.LogOut();
        }

        public async void ResetPassword(string email)
        {
            await ParseUser.RequestPasswordResetAsync(email);
        }

        public string GetUsername()
        {
            string username = ParseUser.CurrentUser.Username;
            return username;
        }

        public async Task<IEnumerable<Groups>> GetAllGroups()
        {
            try
            {
                var groups = await ParseObject.GetQuery("Groups").FindAsync();
                return await GroupModels(groups);
            }
            catch (HttpRequestException e)
            {
                new MessageDialog("Изгубена интернет връзка. Моле опитайте отново по - късно.").ShowAsync();
                return null;
            }
            catch (Exception e)
            {
                var message = new MessageDialog("Грешка. Моля опитайте отново.");
                message.ShowAsync();
                return null;
            }
        }

        private Task<IEnumerable<Groups>> GroupModels(IEnumerable<ParseObject> groups)
        {
            return Task.Run(() =>
            {
                var all =
                from gr in groups
                select new Groups()
                {
                    Name = (string)gr["name"],
                    ObjectId = gr.ObjectId,
                    ImageURL = (string)gr["image"],
                };
                return all;
            });
        }

        public Task<ParseObject> GetGroupByObjectId(string id)
        {
            try
            {
                ParseQuery<ParseObject> query = ParseObject.GetQuery("Groups");
                var group = query.GetAsync(id);
                return group;
            }
            catch (HttpRequestException e)
            {
                new MessageDialog("Изгубена интернет връзка. Моле опитайте отново по - късно.").ShowAsync();
                return null;
            }
            catch (Exception e)
            {
                new MessageDialog("Грешка. Моля опитайте отново.").ShowAsync();
                return null;
            }
        }

        public Task<IEnumerable<ParseObject>> GetCommentsByArticle(ParseObject article)
        {
            try
            {
                ParseQuery<ParseObject> query = ParseObject.GetQuery("Comments");
                var comments = query.Where(c => (ParseObject)c["articleParent"] == article).FindAsync();
                return comments;
            }
            catch (HttpRequestException e)
            {
                new MessageDialog("Изгубена интернет връзка. Моле опитайте отново по - късно.").ShowAsync();
                return null;
            }
            catch (Exception e)
            {
                new MessageDialog("Грешка. Моля опитайте отново.").ShowAsync();
                return null;
            }
        }

        //not in use just for now
        //public Task CreateComment(ParseObject article, ParseUser user, string content)
        //{
        //    if (user.IsAuthenticated)
        //    {
        //        ParseObject comment = new ParseObject("Comments");
        //        comment["content"] = content;

        //        comment["username"] = user["username"];
        //        comment["articleParent"] = article;
        //        return comment.SaveAsync();
        //    }
        //    else
        //    {
        //        throw new ArgumentNullException("You must be logged in to post comments");
        //    }
        //}

        public async Task<IEnumerable<ArticlesInGroup>> GetAllArticlesByGroup(ParseObject group)
        {
            try
            {
                ParseQuery<ParseObject> query = ParseObject.GetQuery("Articles");
                var articles = await query.Where(c => (ParseObject)c["groupParent"] == group).FindAsync();
                return await this.GetArticles(articles);
            }
            catch (HttpRequestException e)
            {
                new MessageDialog("Изгубена интернет връзка. Моле опитайте отново по - късно.").ShowAsync();
                return null;
            }
            catch (Exception e)
            {
                new MessageDialog("Грешка. Моля опитайте отново.").ShowAsync();
                return null;
            }
        }

        private Task<IEnumerable<ArticlesInGroup>> GetArticles(IEnumerable<ParseObject> articles)
        {
            return Task.Run(() =>
            {
                var all =
                    from ar in articles
                    select new ArticlesInGroup()
                    {
                        Image = (string)ar["image"],
                        Name = (string)ar["title"],
                        ObjectId = ar.ObjectId,
                    };
                return all;
            });
        }

        public async Task<Article> GetArticleByObjectId(string id)
        {
            try
            {
                ParseQuery<ParseObject> query = ParseObject.GetQuery("Articles");
                var article = await query.GetAsync(id);

                var rang = (List<object>)article["list"];

                List<Ranglist> list = new List<Ranglist>();

                foreach (var item in rang)
                {
                    var a = (Dictionary<string, object>)item;

                    Ranglist currentItem = new Ranglist();
                    currentItem.Palce = (string)a["place"];
                    currentItem.Image = (string)a["image"];
                    list.Add(currentItem);

                }

                var foundArticle = new Article()
                {
                    Content = (string)article["content"],
                    Image = (string)article["image"],
                    ObjectId = article.ObjectId,
                    Title = (string)article["title"],
                    Ranglist = list,
                };
                return foundArticle;
            }
            catch (HttpRequestException e)
            {
                new MessageDialog("Изгубена интернет връзка. Моле опитайте отново по - късно.").ShowAsync();
                return null;
            }
            catch (Exception e)
            {
                new MessageDialog("Грешка. Моля опитайте отново.").ShowAsync();
                return null;
            }
        }

        public Task CreateArticle(ParseUser user, string content)
        {
            try
            {
                if (user.IsAuthenticated)
                {
                    ParseObject userArticle = new ParseObject("UserArticle");
                    userArticle["content"] = content;

                    userArticle["user"] = user;
                    return userArticle.SaveAsync();
                }
                else
                {
                    throw new ArgumentNullException("Изпращането на статии изисква логване в системата");
                }
            }
            catch (ArgumentNullException e)
            {
                new MessageDialog(e.Message).ShowAsync();
                return null;
            }
            catch (HttpRequestException e)
            {
                new MessageDialog("Изгубена интернет връзка. Моле опитайте отново по - късно.").ShowAsync();
                return null;
            }
            catch (Exception e)
            {
                new MessageDialog("Грешка. Моля опитайте отново.").ShowAsync();
                return null;
            }
        }

        public async Task<IEnumerable<ArticlesInGroup>> GetAllArticlesByQuery(string querySearch)
        {
            try
            {
                ParseQuery<ParseObject> query = ParseObject.GetQuery("Articles");
                var articles = await query.Where(a => ((string)a["title"]).Contains(querySearch)).FindAsync();
                return await this.GetArticles(articles);
            }
            catch (HttpRequestException e)
            {
                new MessageDialog("Изгубена интернет връзка. Моле опитайте отново по - късно.").ShowAsync();
                return null;
            }
            catch (Exception e)
            {
                new MessageDialog("Грешка. Моля опитайте отново.").ShowAsync();
                return null;
            }
        }
    }
}
