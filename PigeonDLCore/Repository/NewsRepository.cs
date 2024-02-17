using PigeonDLCore.Data;
using PigeonDLCore.Models;

namespace PigeonDLCore.Repository
{
    public class NewsRepository
    {
        private ApplicationDbContext dbContext;

        public NewsRepository()
        {
            this.dbContext = new ApplicationDbContext();
        }

        public NewsRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        //operations
        public List<News> GetAllNews()
        {
            List<News> newsList = new List<News>();

            foreach(News item in this.dbContext.News)
            {
                newsList.Add(item);
            }

            newsList = newsList.OrderByDescending(x => x.DateAdded).ToList();

            return newsList;
        }

        public void InsertNews(News news)
        {
            news.IDNews = Guid.NewGuid();

            dbContext.News.Add(news);
            dbContext.SaveChanges();
        }

        public void UpdateNews(News news)
        {
            News existingNews = dbContext.News.FirstOrDefault(x => x.IDNews == news.IDNews);

            if(existingNews != null)
            {
                existingNews.IDNews = news.IDNews;
                existingNews.IDUser = news.IDUser;
                existingNews.Title = news.Title;
                existingNews.Content = news.Content;
                existingNews.DateAdded = news.DateAdded;
                dbContext.SaveChanges();
            }
        }

        public void DeleteNews(News news)
        {
            News existingNews = dbContext.News.FirstOrDefault(x => x.IDNews == news.IDNews);

            if (existingNews != null)
            {
                dbContext.News.Remove(existingNews);
                dbContext.SaveChanges();
            }
        }
    }
}
