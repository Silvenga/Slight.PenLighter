using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SlightPenLighter.Models {

    [Serializable]
    public class Save {

        public byte A {
            get;
            set;
        }

        public byte R {
            get;
            set;
        }

        public byte G {
            get;
            set;
        }

        public byte B {
            get;
            set;
        }

        public double Size {
            get;
            set;
        }

        public static void SerializeObject(string filename, Save objectToSerialize) {

            using(Stream stream = File.Open(filename, FileMode.OpenOrCreate, FileAccess.Write)) {

                var bFormatter = new BinaryFormatter();
                bFormatter.Serialize(stream, objectToSerialize);
            }
        }

        public static Save DeserializeObject(string filename) {

            using(Stream stream = File.Open(filename, FileMode.Open, FileAccess.Read)) {

                var bFormatter = new BinaryFormatter();
                var objectToSerialize = (Save) bFormatter.Deserialize(stream);
                return objectToSerialize;
            }
        }

    }

}