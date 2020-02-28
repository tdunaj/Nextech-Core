using NextechAppWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NextechAppWebApi.Data
{
    public interface INewsRepository
    {
        Task<IEnumerable<NewsItem>> GetMostRecentNews();
        Task<string> GetArticle(int id);
    }
}
