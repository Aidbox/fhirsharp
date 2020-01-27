using Newtonsoft.Json;

namespace Aidbox.RestClient.Models
{
    public class Bundle<T>
    {
        [JsonProperty("total")]
        public uint Total { get; set; }

        [JsonProperty("entry")]
        public Entry<T>[] Entries { get; set; }
    }

    public class Entry<T>
    {
        [JsonProperty("resource")]
        public T Resource { get; set; }

        [JsonProperty("fullUrl")]
        public string FullUrl { get; set; }
    }
}