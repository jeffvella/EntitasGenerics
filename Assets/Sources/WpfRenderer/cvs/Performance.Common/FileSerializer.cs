using System.IO;

//using Newtonsoft.Json;

namespace Performance.Common
{
    public static class FileSerializer
    {
        //private static JsonSerializer _serializer;

        static FileSerializer()
        {
            //_serializer = new JsonSerializer();
        }

        public static void Serialize<T>(T obj, string path) where T : class
        {
            //var json = JsonConvert.SerializeObject(obj);
            var json = JsonSerializer.Serialize<T>(obj);
            File.WriteAllText(path, json);
        }

        public static T Deserialize<T>(string path) where T : class
        {
            if (File.Exists(path))
            {
                var json = File.ReadAllText(path);
                return JsonSerializer.Deserialize<T>(json);
                //return JsonConvert.DeserializeObject<T>(json);
            }
            return default;
        }

        public static bool TryDeserialize<T>(out T instance, string path) where T : class
        {            
            if (File.Exists(path))
            {
                var json = File.ReadAllText(path);
                instance = JsonSerializer.Deserialize<T>(json);
                //instance = JsonConvert.DeserializeObject<T>(json);
                return true;
            }
            instance = default;
            return false;
        }
    }

}



