using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Aidbox.Generator.Templates;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Aidbox.Generator
{
    public class AidboxSdkGenerator
    {
        private string _username, _userpassword, _client_id, _client_secret;

        public AidboxSdkGenerator(string address, string username, string userpassword, string client_id, string client_secret)
        {
            aidboxApiUrl = address + (address.EndsWith("/") ? "" : "/");

            _username = username;
            _userpassword = userpassword;
            _client_id = client_id;
            _client_secret = client_secret;
        }

        private string aidboxApiUrl { get; set; } = "http://{address}:{port}/";

        private HttpClient httpClient = new HttpClient();

        public void MakeClasses()
        {
            var curDir = Directory.GetCurrentDirectory();
            var aidboxClasses = Directory.GetParent(curDir);
            aidboxClasses = Directory.GetParent(aidboxClasses.FullName);
            aidboxClasses = Directory.GetParent(aidboxClasses.FullName);

            var request = new HttpRequestMessage(HttpMethod.Post, aidboxApiUrl + "auth/token");

            var content = Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                grant_type = "password",
                client_id = _client_id,
                client_secret = _client_secret,
                username = _username,
                password = _userpassword
            });

            request.Content = new StringContent(content, Encoding.Default, "application/json");

            var result = httpClient.SendAsync(request).Result;

            if (result.StatusCode != HttpStatusCode.OK)
            {
                throw new ApplicationException("Aidbox server authentication error");
            }

            var text = result.Content.ReadAsStringAsync().Result;
            var responseJson = (JObject)JsonConvert.DeserializeObject(text);

            var token = ((JValue)responseJson["access_token"]).Value;

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var primitivesList = GetTypesList(httpClient, "primitive");
            var typesList = GetTypesList(httpClient, "type");
            var resourceList = GetTypesList(httpClient, "resource");
            resourceList.Add("Resource");
            resourceList.Remove("App");
            resourceList.Remove("AidboxQuery");
            resourceList.Remove("SearchQuery");

            resourceList.Clear();
            resourceList.Add("Resource");
            resourceList.Add("Patient");

            typesList.Clear();
            typesList.Add("CodeableConcept");
            typesList.Add("Narrative");
            typesList.Add("HumanName");
            typesList.Add("ContactPoint"); 
            typesList.Add("Identifier");
            typesList.Add("Attachment");
            typesList.Add("Meta");
            typesList.Add("Address");
            typesList.Add("Reference");
            typesList.Add("Period");
            typesList.Add("Coding");

            var typesCollection = typesList.Select(t => GetType(httpClient, t)).ToList();
            var resourceCollection = resourceList.Select(t => GetType(httpClient, t)).ToList();

            foreach (var t in typesCollection)
            {
                var template = new TypeTemplate(t);
                string classText = template.TransformText();
                System.IO.File.WriteAllText(@$"{aidboxClasses}\GeneratedTypes\{t.EntityName}.cs", classText);
            }

            foreach (var r in resourceCollection)
            {
                var template = new ResourceTemplate(r);
                string classText = template.TransformText();
                System.IO.File.WriteAllText(@$"{aidboxClasses}\GeneratedResources\{r.EntityName}.cs", classText);
            }
        }

        public TypeDescription GetType(HttpClient httpClient, string entityName)
        {
            var td = new TypeDescription();
            td.EntityName = entityName;

            var request = new HttpRequestMessage(HttpMethod.Get, aidboxApiUrl + $"Attribute?entity={entityName}");
            var result = httpClient.SendAsync(request).Result;
            var text = result.Content.ReadAsStringAsync().Result;

            var attributes = new List<List<Tuple<string, JToken>>>();
            var items = (JArray)((JObject)JsonConvert.DeserializeObject(text))["entry"];
            foreach (JObject jobj in items)
            {
                var attr = new List<Tuple<string, JToken>>();

                foreach (JProperty kvp in jobj["resource"])
                {
                    attr.Add(new Tuple<string, JToken>(kvp.Name, kvp.Value));
                }

                attributes.Add(attr);
            }

            request = new HttpRequestMessage(HttpMethod.Get, aidboxApiUrl + $"Entity/{entityName}");
            result = httpClient.SendAsync(request).Result;
            text = result.Content.ReadAsStringAsync().Result;

            td.EntityType = ((JObject)JsonConvert.DeserializeObject(text))["type"].ToString();

            var descs = attributes.Select(l => new AttributeDescription()
            {
                Path = (l.First(l => l.Item1 == "path").Item2 as JArray).Select(p => (p as JValue).Value.ToString()).ToList(),
                Type = l.FirstOrDefault(l => l.Item1 == "type")?.Item2["id"].ToString(),
                Union = (l.FirstOrDefault(l => l.Item1 == "union")?.Item2 as JArray)?.Select(p => (p as JObject)["id"].ToString())?.ToList(),
                IsCollection = l.FirstOrDefault(l => l.Item1 == "isCollection") != null,
                IsRequired = l.FirstOrDefault(l => l.Item1 == "isRequired") != null,
                IsSummary = l.FirstOrDefault(l => l.Item1 == "isSummary") != null,
                Order = ((JValue)l.FirstOrDefault(l => l.Item1 == "order")?.Item2).ToNullableInt(),
                Refers = (l.FirstOrDefault(l => l.Item1 == "refers")?.Item2 as JArray)?.Select(p => (p as JValue).Value.ToString())?.ToList(),
                ValueSet = l.FirstOrDefault(l => l.Item1 == "valueSet")?.Item2["id"].ToString(),
            }
            ).ToList();

            var choiceOfDataTypesParents = descs.Where(d => d.Union != null).ToList();
            var choiceOfDataTypesNames = choiceOfDataTypesParents.Select(d => d.Path.First()).ToHashSet().ToList();
            var choiceOfDataTypes = descs.Where(d => d.Path.Any(p => choiceOfDataTypesNames.Contains(p))).ToList();

            var backboneElementsChilds = descs.Where(d => d.Path.Count > 1).ToList();
            var backboneElementsNames = backboneElementsChilds.Select(d => d.Path.First()).Except(choiceOfDataTypesParents.Select(d => d.Path.First())).ToHashSet().ToList();
            var backboneElements = descs.Where(d => d.Path.Any(p => backboneElementsNames.Contains(p))).ToList();

            var attrs = descs
                .Except(choiceOfDataTypes)
                .Except(backboneElements)
                .ToList();

            foreach (var item in choiceOfDataTypesParents)
            {
                var d = choiceOfDataTypes
                        .Where(d => (d.Path[0] == item.Path[0]) && d.Path.Count > 1)
                        .ToList();

                d.ForEach(d => d.Path = d.Path.Skip(1).ToList());

                attrs.Add(new AttributeDescription()
                {
                    Path = item.Path,
                    Type = $"{entityName}_{item.Path[0]}",
                    IsCollection = item.IsCollection,
                    IsRequired = item.IsRequired,
                    IsSummary = item.IsSummary,
                    Order = item.Order,
                    BackboneDescription = d
                });
            }

            var be = backboneElements.Where(d => d.Path.Count == 1).ToList();

            foreach (var item in be)
            {
                var d = backboneElements
                        .Where(d => (d.Path[0] == item.Path[0]) && d.Path.Count > 1)
                        .ToList();

                d.ForEach(d => d.Path = d.Path.Skip(1).ToList());

                attrs.Add(new AttributeDescription()
                {
                    Path = item.Path,
                    Type = $"{entityName}_{item.Path[0]}",
                    IsCollection = item.IsCollection,
                    IsRequired = item.IsRequired,
                    IsSummary = item.IsSummary,
                    Order = item.Order,
                    BackboneDescription = d
                });
            }

            td.Attributes = attrs;

            return td;
        }

        public List<string> GetTypesList(HttpClient httpClient, string type)
        {
            var result = new List<string>();

            var request = new HttpRequestMessage(HttpMethod.Get, aidboxApiUrl + $"Entity?.type={type}");
            var response = httpClient.SendAsync(request).Result;
            var text = response.Content.ReadAsStringAsync().Result;

            var obj = ((JObject)JsonConvert.DeserializeObject(text));
            var total = ((JValue)obj["total"]).ToNullableInt().Value;

            var pagesCount = total / 100 + 1;

            for (int page = 1; page <= pagesCount; page++)
            {
                request = new HttpRequestMessage(HttpMethod.Get, aidboxApiUrl + $"Entity?.type={type}&page={page}");
                text = httpClient.SendAsync(request).Result.Content.ReadAsStringAsync().Result;
                obj = ((JObject)JsonConvert.DeserializeObject(text));

                var items = (JArray)obj["entry"];
                foreach (JObject jobj in items)
                {
                    var name = ((JValue)jobj["resource"]["id"]).Value.ToString();
                    result.Add(name);
                }
            }

            return result;
        }
    }
}