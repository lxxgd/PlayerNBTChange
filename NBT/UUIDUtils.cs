using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace NBT
{
    internal class UUIDUtils
    {
        public static string PlayerNameToUUID(string name)
        {
            string url = "https://api.mojang.com/users/profiles/minecraft/" + name;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            using HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using System.IO.Stream stream = response.GetResponseStream();
            using System.IO.StreamReader reader = new System.IO.StreamReader(stream);
            string json = reader.ReadToEnd();
            Console.WriteLine(json);
            int start = json.IndexOf("\"id\" : \"")+8;
            int end = json.IndexOf("\"", start);
            string formattedUUID = json.Substring(start, end - start);
            return formattedUUID;
        }
    }
}
