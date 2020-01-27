using Aidbox.RestClient.Exceptions;
using Aidbox.RestClient.Helpers;
using Aidbox.RestClient.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aidbox.RestClient
{
    public class AidboxRestClient
    {
        private IRestClient _client;
        private const string _contentType = "application/json";
        private AidboxRestClientSettings _settings;
        private Token _token;

        public AidboxRestClient(AidboxRestClientSettings settings)
        {
            if (string.IsNullOrEmpty(settings.BaseUrl))
                throw new ArgumentException(nameof(settings.BaseUrl));
            
            if (!settings.IsWithoutAuthentication && (settings.AuthenticationSettings == null || !settings.AuthenticationSettings.IsValid()))
                throw new ArgumentException(nameof(settings.AuthenticationSettings));
            
            _settings = settings;
            _client = new RestSharp.RestClient(settings.BaseUrl).UseSerializer(() => new JsonNetSerializer());

            Authentication();
        }

        private void Authentication()
        {
            if (_settings.IsWithoutAuthentication) return;
            if (_token != null && _token.Expires > DateTime.Now) return;

            var request = new RestRequest("auth/token", Method.POST);
            request.AddHeader("Content-Type", _contentType);
            request.AddJsonBody(_settings.AuthenticationSettings);
            var response = _client.Execute<Token>(request);
            if (!response.IsSuccessful)
                throw new AuthenticationFailed();
            _token = response.Data;
        }

        private RestRequest PrepareRequest(string resource)
        {
            var request = new RestRequest(resource);
            request.AddHeader("Content-Type", _contentType);
            if (_token != null)
                request.AddParameter("Authorization", "Bearer " + _token.AccessToken, ParameterType.HttpHeader);

            return request;
        }

        public string CreateResource<T>(T resource)
        {
            Authentication();

            var request = PrepareRequest(typeof(T).Name);
            request.AddJsonBody(resource);
            var response = _client.Post(request);
            if (response.StatusCode != System.Net.HttpStatusCode.Created)
                throw new NotCreatedException(response.Content);

            if (!string.IsNullOrEmpty(response.Content))
            {
                var deserialized = JsonConvert.DeserializeObject<Dictionary<string, object>>(response.Content);
                if (deserialized.Keys.Any(x => x == "id"))
                    return deserialized["id"].ToString();
            }

            throw new CreatedIdNotFound(response.Content);
        }

        public void UpdateResource<T>(T resource, string id)
        {
            Authentication();

            var request = PrepareRequest($"{typeof(T).Name}/{id}");
            request.AddJsonBody(resource);
            var response = _client.Put(request);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                throw new NotUpdatedException(response.Content);
        }

        public Bundle<T> GetResource<T>(int start = 0, int limit = 20)
        {
            Authentication();

            var request = PrepareRequest($"{typeof(T).Name}");
            var response = _client.Get<Bundle<T>>(request);
            return response.Data;
        }

        public T GetResource<T>(string id) where T : new()
        {
            Authentication();

            var request = PrepareRequest($"{typeof(T).Name}/{id}");
            var response = _client.Get<T>(request);
            return response.Data;
        }

        public void DeleteResource<T>(string id)
        {
            Authentication();

            var request = PrepareRequest($"{typeof(T).Name}/{id}");
            var response = _client.Delete(request);
        }
    }
}
