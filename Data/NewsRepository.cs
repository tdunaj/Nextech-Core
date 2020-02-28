using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using NextechAppWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace NextechAppWebApi.Data
{
    public class NewsRepository : INewsRepository
    {
        private const int NumberOfNews = 50;

        //private IMemoryCache cache;

        //public NewsRepository() {}
        //public NewsRepository(IMemoryCache cache)
        //{
        //    this.cache = cache;
        //}

        public async Task<IEnumerable<NewsItem>> GetMostRecentNews()
        {            
            List<NewsItem> newsList = new List<NewsItem>();
            var newsIds = new List<int>();


            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://hacker-news.firebaseio.com/v0/newstories.json"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    //newsList = JsonConvert.DeserializeObject<List<NewsItem>>(apiResponse);
                    newsIds = JsonConvert.DeserializeObject<List<int>>(apiResponse);                    
                }
            }

            if (newsIds.Any())
            {
                using (var httpClient = new HttpClient())
                {
                    for(int i = 0; i < NumberOfNews; i++)
                    { 
                        using (var response = await httpClient.GetAsync($"https://hacker-news.firebaseio.com/v0/item/{newsIds[i]}.json"))
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            //newsList = JsonConvert.DeserializeObject<List<NewsItem>>(apiResponse);
                            var newsItem = JsonConvert.DeserializeObject<NewsItem>(apiResponse);

                            //if (!string.IsNullOrEmpty(newsItem.By))
                            if (newsItem != null)
                                newsList.Add(newsItem);
                        }
                    }
                }
            }

            //cache.Set("test", DateTime.Now.ToString());
            //cache.Set("NewsList", newsList);

            return newsList;
        }
    }
}
