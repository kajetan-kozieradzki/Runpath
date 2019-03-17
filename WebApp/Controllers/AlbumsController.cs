using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Runpatch.Logic;
using Runpath.Models;

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlbumsController : Controller
    {
        private IDataFetcher dataFetcher;

        public AlbumsController(IDataFetcher fetcher)
        {
            this.dataFetcher = fetcher;
        }

        [HttpGet("{userId?}")]
        public async Task<Album[]> Get(int? userId)
        {
            var albums = await Task.Run(() => dataFetcher.FetchAlbums(userId));

            return albums;
        }
    }
}