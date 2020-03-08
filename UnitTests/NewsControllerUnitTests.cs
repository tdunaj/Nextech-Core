using Autofac.Extras.Moq;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NextechAppWebApi.Controllers;
using NextechAppWebApi.Data;
using NextechAppWebApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UnitTests
{
    //This class should be part of a separate UnitTests project in Visual Studio.
    //If it is showing up in under a folder in the NextechAppWebApi project, please exclude the folder from
    //that project. Let me know if there are any questions, I had difficulties pushing the tests to github.
    [TestClass]
    public class NewsControllerUnitTests
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
                mock.Mock<IMemoryCache>().Setup(x => x.CreateEntry(It.IsAny<object>())).Returns(Mock.Of<ICacheEntry>);

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
