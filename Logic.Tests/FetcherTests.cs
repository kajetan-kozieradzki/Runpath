using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Runpatch.Logic;
using Runpath.Logic.Implementation;
using System.Linq;

namespace Logic.Tests
{
    [TestClass]
    public class FetcherTests
    {
        private IDataFetcher dataFetcher;

        [TestInitialize]
        public void Init()
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        
            dataFetcher = new DataFetcher(config);
        }

        [TestMethod]
        public void FetchAllAlbums_ShouldReturn100Items()
        {
            var albums = this.dataFetcher.FetchAlbums(null).Result;

            albums.Length.Should().Be(100);
        }

        [TestMethod]
        public void FetchAllAlbums_ShouldReturn5000Photos()
        {
            var albums = this.dataFetcher.FetchAlbums(null).Result;

            albums.SelectMany(x => x.Photos).Count().Should().Be(5000);
        }

        [TestMethod]
        public void FetchAlbumsForUser1_ShouldReturn10Items()
        {
            var userId = 1;
            var albums = this.dataFetcher.FetchAlbums(userId).Result;

            albums.Length.Should().Be(10);
        }

        [TestMethod]
        public void FetchAlbumsForUser1_ShouldReturn50Photos()
        {
            var userId = 1;
            var albums = this.dataFetcher.FetchAlbums(userId).Result;
            var photos = albums.SelectMany(x => x.Photos);

            photos.Count().Should().Be(500);
        }

        [TestMethod]
        public void FetchAlbumsForUser2_AllAlbumsSHouldBelongToUser2()
        {
            var userId = 2;
            var albums = this.dataFetcher.FetchAlbums(userId).Result;

            albums.Where(x => x.UserId != userId).Count().Should().Be(0);
        }

        [TestMethod]
        public void FetchAlbumsForUser2_AllPhotosSouldBelongToUser2()
        {
            var userId = 2;
            var albums = this.dataFetcher.FetchAlbums(userId).Result;
            var photos = albums.SelectMany(x => x.Photos);

            photos.Where(x => albums.All(a => a.Id != x.AlbumId)).Count().Should().Be(0);
            photos.Where(x => albums.First(a => a.Id == x.AlbumId).UserId != userId).Count().Should().Be(0);
        }

        [TestMethod]
        public void FetchAlbumsForNonExistingUser_ShouldReturnEmptyResult()
        {
            var userId = 12;
            var albums = this.dataFetcher.FetchAlbums(userId).Result;

            albums.Length.Should().Be(0);
        }
    }
}
