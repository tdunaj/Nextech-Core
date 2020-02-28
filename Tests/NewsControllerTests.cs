using Autofac.Extras.Moq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NextechAppWebApi.Controllers;
using NextechAppWebApi.Data;
using NextechAppWebApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NextechAppWebApi.Tests
{
    [TestClass]
    public class NewsControllerTests
    {
        [TestMethod]
        public async Task NewsController_GetNews_ShouldSucceed()
        {
            using (var mock = AutoMock.GetLoose())
            {
                //arrange                
                var mockNews = new List<NewsItem>()
                {
                    new NewsItem() { Title = "title 1", By = "author 1" },
                    new NewsItem() { Title = "title 2", By = "author 2" }
                };

                mock.Mock<INewsRepository>().Setup(m => m.GetMostRecentNews()).ReturnsAsync(mockNews);

                //act
                var newsController = mock.Create<NewsController>();
                var result = await newsController.Get();


                //assert                
                result.Should().NotBeNull();
                result.Count().Should().Be(2);
                result.First().Title.Should().Be("title 1");
                result.First().By.Should().Be("author 1");
            }
        }        
    }
}
