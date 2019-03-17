using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Runpath.Models;
using System.Linq;
using System.Net.Http;

namespace WebApp.Tests
{
    [TestClass]
    public class ApiTests
    {
        private IConfiguration config;

        private HttpClient httpClient;

        private const string baseUrl = "http://localhost:49905/api";

        [TestInitialize]
        public void Init()
        {
            config = new ConfigurationBuilder().AddJsonFile("appsettings.json")
                                               .Build();

            httpClient = new HttpClient();
        }

        [TestMethod]
        public void FetchAllAlbums_ShouldReturnData()
        {
            var json = httpClient.GetStringAsync($"{baseUrl}/albums").Result;
            var albums = JsonConvert.DeserializeObject<Album[]>(json);

            albums.Length.Should().BeGreaterThan(0);
        }

        [TestMethod]
        public void FetchAllAlbums_ShouldResultSouldContainPhotos()
        {
            var json = httpClient.GetStringAsync($"{baseUrl}/albums").Result;
            var albums = JsonConvert.DeserializeObject<Album[]>(json);
            var photos = albums.SelectMany(x => x.Photos);

            photos.Count().Should().BeGreaterThan(0);
        }

        [TestMethod]
        public void FetchAlbumsForExistingUser_ShouldReturnData()
        {
            var userId = 1;
            var json = httpClient.GetStringAsync($"{baseUrl}/albums/{userId}").Result;
            var albums = JsonConvert.DeserializeObject<Album[]>(json);

            albums.Length.Should().BeGreaterThan(0);
        }

        [TestMethod]
        public void FetchAlbumsForExistingUser_ShouldContainPhotos()
        {
            var userId = 1;
            var json = httpClient.GetStringAsync($"{baseUrl}/albums/{userId}").Result;
            var albums = JsonConvert.DeserializeObject<Album[]>(json);
            var photos = albums.SelectMany(x => x.Photos);

            photos.Count().Should().BeGreaterThan(0);
        }

        [TestMethod]
        public void FetchAlbumsForNonExistingUser_ShouldReturnEmptyResult()
        {
            var userId = 12;
            var json = httpClient.GetStringAsync($"{baseUrl}/albums/{userId}").Result;
            var albums = JsonConvert.DeserializeObject<Album[]>(json);

            albums.Length.Should().Be(0);
        }
    }
}
