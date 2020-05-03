using System;
using System.IO;
using Newtonsoft.Json;

namespace SlightPenLighter.Models
{
    [Serializable]
    public class Save
    {
        public byte A { get; set; }

        public byte R { get; set; }

        public byte G { get; set; }

        public byte B { get; set; }

        public double Size { get; set; }

        public static void SerializeObject(string filename, Save obj)
        {
            using (Stream stream = File.Open(filename, FileMode.OpenOrCreate, FileAccess.Write))
            using (var streamWriter = new StreamWriter(stream))
            using (var jsonWriter = new JsonTextWriter(streamWriter))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(jsonWriter, obj);
            }
        }

        public static Save DeserializeObject(string filename)
        {
            using (var stream = File.Open(filename, FileMode.Open, FileAccess.Read))
            using (var streamReader = new StreamReader(stream))
            using (var jsonReader = new JsonTextReader(streamReader))
            {
                var serializer = new JsonSerializer();
                var save = serializer.Deserialize<Save>(jsonReader);
                return save;
            }
        }
    }
}