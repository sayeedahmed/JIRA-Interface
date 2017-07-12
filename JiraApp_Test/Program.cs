using System;
using System.Text;
using System.IO;
using System.Net;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Extensions.MonoHttp;

namespace JiraApp_Test
{
    class Program
    {
        public static string GetEncodedCredentials(string username, string password)
        {
            string mergedCredentials = string.Format("{0}:{1}", username, password);
            byte[] byteCredentials = UTF8Encoding.UTF8.GetBytes(mergedCredentials);
            return Convert.ToBase64String(byteCredentials);
        }

        static void Main(string[] args)
        {
            Console.Write("Provide jql: ");
            string jql = Console.ReadLine();
            string username = "************";
            string password = "************";

            string base64Credentials = Program.GetEncodedCredentials(username, password);

            var client = new RestClient("https://jira.rsi.lexisnexis.com/rest/issueNav/1/issueTable");
            var request = new RestRequest(Method.POST);

            string encodedstring = HttpUtility.UrlEncode(jql);
            encodedstring = "startIndex=0&jql=" + encodedstring + "&layoutKey=split-view";

            var body = encodedstring;

            request.AddHeader("x-atlassian-token", "no-check");

            request.AddHeader("authorization", "Basic" + " " + base64Credentials);
            request.AddParameter("text/plain", body, ParameterType.RequestBody);


            IRestResponse response = client.Execute(request);

            var output = JsonConvert.DeserializeObject(response.Content);
            Console.WriteLine(output.ToString());

            var serializer = new JavaScriptSerializer();
            var jsonObject = serializer.DeserializeObject(response.Content);

            Console.WriteLine(jsonObject.ToString());
            Console.Read();
        }
    }
}
