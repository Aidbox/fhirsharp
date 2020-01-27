using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Aidbox.RestClient.Models
{
    public class AidboxRestClientSettings
    {
        public string BaseUrl { get; set; }
        public bool IsWithoutAuthentication { get; set; }
        public AidboxAuthenticationSettings AuthenticationSettings { get; set; }
    }

    public class AidboxAuthenticationSettings
    {
        [JsonProperty("client_id")]
        public string ClientId { get; set; }
        [JsonProperty("client_secret")]
        public string ClientSecret { get; set; }
        [JsonProperty("username")]
        public string Username { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
        [JsonProperty("grant_type")]
        public string GrantType { get; set; } = "password";

        public bool IsValid()
        {
            return !(string.IsNullOrEmpty(ClientId) ||
                     string.IsNullOrEmpty(ClientSecret) ||
                     string.IsNullOrEmpty(Username) ||
                     string.IsNullOrEmpty(Password));
        }
    }
}
