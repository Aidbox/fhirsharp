using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;
using RestSharp.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aidbox.RestClient.Helpers
{
    public class JsonNetSerializer : IRestSerializer
    {
        public string Serialize(object obj) =>
            JsonConvert.SerializeObject(obj, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

        public string Serialize(Parameter parameter) =>
            JsonConvert.SerializeObject(parameter.Value, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

        public T Deserialize<T>(IRestResponse response) =>
            JsonConvert.DeserializeObject<T>(response.Content, new JsonSerializerSettings { Error = ErrorHandler });

        public static void ErrorHandler(object sender, ErrorEventArgs args)
        {
            Console.WriteLine(args.ErrorContext.Error.Message);
            args.ErrorContext.Handled = true;
        }

        public string[] SupportedContentTypes { get; } =
        {
            "application/json", "text/json", "text/x-json", "text/javascript", "*+json"
        };

        public string ContentType { get; set; } = "application/json";

        public DataFormat DataFormat { get; } = DataFormat.Json;
    }
}
