using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace NotesApp.IntegrationTests.Extensions
{
    public static class HttpContentHelper
    {
        public static HttpContent ToHttpContent<T>(this T model)
        {
            var json = JsonSerializer.Serialize(model);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            return httpContent;
        }
    }
}
