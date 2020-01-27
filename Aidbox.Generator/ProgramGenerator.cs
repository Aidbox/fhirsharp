using System;
using System.Linq;

namespace Aidbox.Generator
{
    public class ProgramGenerator
    {
        static void Main(string[] args)
        {
            string address, username, password, clientid, clientsecret;

            if (args == null)
                throw new ArgumentException("server parameters expected");

            if (args.Any(a => a.StartsWith("address=")))
                address = args.First(f => f.StartsWith("address=")).Replace("address=", string.Empty);
            else
                throw new ArgumentException("server address (like 'http://127.0.0.1:8888') parameter expected");

            if (args.Any(a => a.StartsWith("username=")))
                username = args.First(f => f.StartsWith("username=")).Replace("username=", string.Empty);
            else
                throw new ArgumentException("username parameter expected");

            if (args.Any(a => a.StartsWith("password=")))
                password = args.First(f => f.StartsWith("password=")).Replace("password=", string.Empty);
            else
                throw new ArgumentException("password parameter expected");

            if (args.Any(a => a.StartsWith("clientid=")))
                clientid = args.First(f => f.StartsWith("clientid=")).Replace("clientid=", string.Empty);
            else
                throw new ArgumentException("clientid parameter expected");

            if (args.Any(a => a.StartsWith("clientsecret=")))
                clientsecret = args.First(f => f.StartsWith("clientsecret=")).Replace("clientsecret=", string.Empty);
            else
                throw new ArgumentException("clientsecret parameter expected");

            var client = new AidboxSdkGenerator(address, username, password, clientid, clientsecret);

            client.MakeClasses();
        }
    }
}