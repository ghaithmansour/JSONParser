using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using JSONParser;
namespace JSONParser
{
    public static class Driver
    {
        private readonly static string hardcodedTest = "{\r\n  \"key\": \"value\",\r\n  \"key-n\": 101,\r\n  \"key-o\": {},\r\n  \"key-l\": []\r\n}";
        public static void Main(string? filename)
        {
            string json = "";
            JsonParser jsonParser = new JsonParser();

            if (filename != null)
                try
                {
                    json = File.ReadAllText(filename);
                    //Console.WriteLine("File content:");
                    //Console.WriteLine(json);
                }
                catch (IOException)
                {
                    //Console.WriteLine("An error occurred while reading the file:");
                    //Console.WriteLine(e.Message);
                    throw;
                }
            else
                json = hardcodedTest;

            // Parse the JSON string
            var jsonDocument = jsonParser.Parse(json);
            //Console.WriteLine("processing File: {0}", filename);

            // Access properties
            //foreach (var item in jsonDocument)
            //{
            //    Console.WriteLine(
            //        item.Key + ": " +
            //        (item.Value != null ?
            //        (item.Value.ToString() + " (" + item.Value.GetType().Name + ")") :
            //        "null"));
            //}
            Console.WriteLine("successful parsing of file {0}", filename);
        }
    }
}
