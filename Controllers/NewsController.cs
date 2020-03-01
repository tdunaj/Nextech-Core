using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using NextechAppWebApi.Data;
using NextechAppWebApi.Models;

namespace NextechAppWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly INewsRepository newsRepository;
        private IMemoryCache cache;

        public NewsController(INewsRepository newsRepository, IMemoryCache cache)
        {
            this.newsRepository = newsRepository;
            this.cache = cache;
        }

        public async Task<IEnumerable<NewsItem>> Get()
        {
            return await this.GetNews();
        }

        [HttpGet("{author}")]
        public async Task<IEnumerable<NewsItem>> Get(string author)
        {

            return (await this.GetNews()).Where(x => x.By == author);
        }

        [HttpGet("article/{articleId}")]
        public async Task<string> Get(int articleId)
        {
            return await this.newsRepository.GetArticle(articleId);
        }

        private async Task<IEnumerable<NewsItem>> GetNews()
        {
            return await cache.GetOrCreateAsync<IEnumerable<NewsItem>>("NewsList",
                cacheEntry => {
                    return this.newsRepository.GetMostRecentNews();
            });
        }
    }
}