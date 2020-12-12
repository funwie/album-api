using System.Text.Json;

namespace Runpath.Platform.AlbumApi.Serializers
{
    public interface ISerializer
    {
        T DeserializeJson<T>(string jsonData, JsonSerializerOptions options = null);
    }
}
