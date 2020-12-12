using System.Text.Json;
using System.Threading.Tasks;

namespace Runpath.Platform.AlbumApi.Serializers
{
    public interface ISerializer
    {
        Task<T> DeserializeJsonAsync<T>(string jsonData, JsonSerializerOptions options = null);
    }
}
