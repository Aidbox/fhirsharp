using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Aidbox.RestClient;
using Aidbox.RestClient.Models;

namespace TestConsoleApp
{
    class ProgramTest
    {
        private static HttpClient httpClient = new HttpClient();

        public static void Main(string[] args)
        {
            m1();
        }

        private static void m1()
        {
            /*
            var client = new AidboxRestClient(new AidboxRestClientSettings
            {
                BaseUrl = "http://{address}:{port}/",
                IsWithoutAuthentication = false,
                AuthenticationSettings = new AidboxAuthenticationSettings
                {
                    GrantType = "",
                    ClientId = "",
                    ClientSecret = "",
                    Username = "",
                    Password = ""
                }
            });

            var newPatient = new Aidbox.Model.Patient
            {
                inn = "Test rest client",
                birthDate = new DateTime(1990, 4, 13)
            };
            var id = client.CreateResource(newPatient);


            var getPat = client.GetResource<Aidbox.Model.Patient>(id);

            var returnedPatient = client.GetResource<Aidbox.Model.Patient>(id);
            returnedPatient.inn = "Updated test rest client";
            returnedPatient.name = new List<Aidbox.Model.HumanName>
            {
                new Aidbox.Model.HumanName() { text = "new name" }
            };
            returnedPatient.multipleBirth = new Aidbox.Model.Patient.Patient_multipleBirth { boolean = true };
            client.UpdateResource(returnedPatient, id);

            returnedPatient = client.GetResource<Aidbox.Model.Patient>(id);
            returnedPatient.multipleBirth = new Aidbox.Model.Patient.Patient_multipleBirth { integer = 8 };
            client.UpdateResource(returnedPatient, id);

            returnedPatient = client.GetResource<Aidbox.Model.Patient>(id);
            returnedPatient.contact = new List<Aidbox.Model.Patient.Patient_contact> {
                new  Aidbox.Model.Patient.Patient_contact
                {
                    address = new Aidbox.Model.Address { city = "city", state = "state" },
                    name = new Aidbox.Model.HumanName() { text = "contact name" },
                    gender = "male",
                    period = new Aidbox.Model.Period { start = new DateTime(2020, 01, 01), end = new DateTime(2021, 01, 01) },
                    relationship = new List<Aidbox.Model.CodeableConcept> { new Aidbox.Model.CodeableConcept { text = "father" } }
                }
            };
            client.UpdateResource(returnedPatient, id);

            client.DeleteResource<Aidbox.Model.Patient>(id);
            */
        }
    }
}
