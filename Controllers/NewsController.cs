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
            return await cache.GetOrCreateAsync<IEnumerable<NewsItem>>("NewsList",
                cacheEntry => {
                    return this.newsRepository.GetMostRecentNews();
                });         
        }

        [HttpGet("{author}")]
        public async Task<IEnumerable<NewsItem>> Get(string author)
        {
            var list =  await cache.GetOrCreateAsync<IEnumerable<NewsItem>>("NewsList",
                cacheEntry => {
                    return this.newsRepository.GetMostRecentNews();
                });

            return list.Where(x => x.By == author);
        }
    }
}