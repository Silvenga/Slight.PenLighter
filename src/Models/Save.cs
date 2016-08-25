namespace SlightPenLighter.Models
{
    using System;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;

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
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, obj);
            }
        }

        public static Save DeserializeObject(string filename)
        {
            using (Stream stream = File.Open(filename, FileMode.Open, FileAccess.Read))
            {
                var formatter = new BinaryFormatter();
                var objectToSerialize = (Save) formatter.Deserialize(stream);
                return objectToSerialize;
            }
        }
    }
}