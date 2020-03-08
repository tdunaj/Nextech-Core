using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

        public async Task<IEnumerable<NewsItem>> GetMostRecentNews()
        {            
            List<NewsItem> newsList = new List<NewsItem>();
            var newsIds = new List<int>();


            using (var httpClient = new HttpClient())
            {
                using var response = await httpClient.GetAsync("https://hacker-news.firebaseio.com/v0/newstories.json");
                string apiResponse = await response.Content.ReadAsStringAsync();
                newsIds = JsonConvert.DeserializeObject<List<int>>(apiResponse);
            }

            if (newsIds.Any())
            {
                using var httpClient = new HttpClient();
                for (int i = 0; i < NumberOfNews; i++)
                {
                    using var response = await httpClient.GetAsync($"https://hacker-news.firebaseio.com/v0/item/{newsIds[i]}.json");
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var newsItem = JsonConvert.DeserializeObject<NewsItem>(apiResponse);

                    if (newsItem != null)
                        newsList.Add(newsItem);
                }
            }

        
            return newsList;
        }

        public async Task<string> GetArticle(int id)
        {
            string article;

            using (var httpClient = new HttpClient())
            {
                using var response = await httpClient.GetAsync($"https://hacker-news.firebaseio.com/v0/item/{id}.json");
                string apiResponse = await response.Content.ReadAsStringAsync();
                var data = JObject.Parse(apiResponse);
                article = (string)data.SelectToken("text") ?? (string)data.SelectToken("url");
            }

            return article;
        }
    }
}
