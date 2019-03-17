using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Runpatch.Logic;
using Runpath.Models;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Runpath.Logic.Implementation
{
    public class DataFetcher : IDataFetcher
    {
        private IConfiguration configuration;

        private HttpClient httpClient;

        public DataFetcher(IConfiguration configuration)
        {
            this.configuration = configuration;

            httpClient = new HttpClient();
        }

        public async Task<Album[]> FetchAlbums(int? userId)
        {
            var albums = await FetchAlbums();
            var photos = await FetchPhotos();

            var filteredAlbums = FilterAlbums(albums, userId);
            MatchPhotos(albums, photos);

            return filteredAlbums;
        }

        private Album[] FilterAlbums(Album[] albums, int? userId)
        {
            if (!userId.HasValue)
            {
                return albums;
            }

            return albums.Where(x => x.UserId == userId).ToArray();
        }

        private void MatchPhotos(Album[] albums, Photo[] photos)
        {
            var photoDict = photos.GroupBy(x => x.AlbumId).ToDictionary(g => g.Key, g => g.ToArray());
            foreach (var album in albums)
            {
                if (photoDict.ContainsKey(album.Id))
                {
                    album.Photos = photoDict[album.Id];
                }
            }
        }

        private async Task<Album[]> FetchAlbums()
        {
            var albumsJson = await httpClient.GetStringAsync(configuration["AlbumsUrl"]);
            return JsonConvert.DeserializeObject<Album[]>(albumsJson);
        }

        private async Task<Photo[]> FetchPhotos()
        {
            var photosJson = await httpClient.GetStringAsync(configuration["PhotosUrl"]);
            return JsonConvert.DeserializeObject<Photo[]>(photosJson);
        }
    }
}
