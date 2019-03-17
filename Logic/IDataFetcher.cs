using Runpath.Models;
using System.Threading.Tasks;

namespace Runpatch.Logic
{
    /// <summary>
    /// Fetches data from the sources defined in the configuration
    /// </summary>
    public interface IDataFetcher
    {
        /// <summary>
        /// Fetches albums with photos
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns></returns>
        Task<Album[]> FetchAlbums(int? userId);
    }
}
