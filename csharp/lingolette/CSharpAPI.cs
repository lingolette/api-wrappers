using System;
using System.Net;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace lingolette
{
    public class CSharpAPI
    {
        protected WebRequest request;

        protected string username;
        protected string secret;
        protected string apiHost = "lingolette.com";
        protected string apiVersion = "1";

        public CSharpAPI (string username, string secret)
        {
            if (username == null || secret == null)
            {
                throw new System.Exception ("Both username and secret are required.");
            }

            this.username = username;
            this.secret = secret;
        }

        private static string RandomString(int length)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[32];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new String(stringChars);

            return finalString;
        }

        public string Exec(string endpoint, string method, string jsonString = "{}")
        {
            if (endpoint == null || endpoint.Length == 0)
            {
                throw new System.Exception ("No endpoint specified");
            }

            // setup connection to endpoint
            request = WebRequest.Create("https://" + apiHost + "/api/" + endpoint);

            // compute HMAC
            var enc = Encoding.ASCII;
            HMACSHA256 hmac = new HMACSHA256(enc.GetBytes(secret));
            hmac.Initialize();

            var salt = RandomString(32);
            byte[] buffer = enc.GetBytes(salt);
            var hash = BitConverter.ToString(hmac.ComputeHash(buffer)).Replace("-", "").ToLower();

            request.Method = "POST";
              request.Headers ["x-api-version"] = apiVersion;
              request.Headers ["x-random"] = salt;
              request.Headers ["x-auth-id"] = username;
              request.Headers ["x-auth-key"] = hash;

            byte[] byteArray = Encoding.UTF8.GetBytes ("{\"method\":\"" + method + "\",\"data\":" + jsonString + "}");

            // Set the ContentLength property of the WebRequest.
            request.ContentLength = byteArray.Length;

            // Write data to the Stream
            Stream dataStream = request.GetRequestStream ();
            dataStream.Write (byteArray, 0, byteArray.Length);
            dataStream.Close ();

            try
            {
                // Get the response.
                WebResponse response = request.GetResponse ();

                // Get the stream content and read it
                dataStream = response.GetResponseStream ();
                StreamReader reader = new StreamReader (dataStream);

                // Read the content.
                string responseFromServer = reader.ReadToEnd ();

                // Clean up
                reader.Close ();
                dataStream.Close ();
                response.Close ();

                return responseFromServer;
            }
            catch (WebException webExcp)
            {
                HttpWebResponse httpResponse = (HttpWebResponse)webExcp.Response;

                using (StreamReader Reader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    Console.WriteLine (Reader.ReadToEnd());
                }

                return (int)httpResponse.StatusCode + " - " + httpResponse.StatusCode;
            }
        }
    }
}
