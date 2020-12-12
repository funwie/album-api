using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;

namespace Runpath.Platform.AlbumApi.Serializers
{
    /// <summary>
    /// Json Serialization.
    /// </summary>
    public class CustomJsonSerializer : ISerializer
    {
        readonly ILogger<CustomJsonSerializer> _logger;

        public CustomJsonSerializer(ILogger<CustomJsonSerializer> logger)
        {
            _logger = logger;
        }

        public T DeserializeJson<T>(string jsonData, JsonSerializerOptions options = null)
        {
            
            var defaultOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            var serializerOptions = options != null ? options : defaultOptions;

            try
            {
                return JsonSerializer.Deserialize<T>(jsonData, serializerOptions);
            }
            catch (JsonException ex)
            {
                _logger.LogDebug("Failed to deserialize {Data} into {Type}", jsonData, typeof(T).FullName);
                _logger.LogError("JsonException {Message}", ex.Message);
                throw ex;
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError("ArgumentNullException {Message}", ex.Message);
                throw ex;
            }
        }
    }
}
