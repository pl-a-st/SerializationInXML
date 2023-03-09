using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;

namespace SerializationInXML {
    class Program {
        static void Main(string[] args) {
            List<Table> tables = Tables.ListTables;
            SerializerResult serializerResult = SerialXML.SerializationInXML(tables, "Tables.xml");
        }
    }
    [Serializable]
    public class Table {
        public int Height;
        public int Width;
        public int Angle;
        
        public Table(int height, int with, int angle) {
            Height = height;
            Width = with;
            Angle = angle;
        }
        public Table() {

        }
       
    }

    public static class Tables {
        public static List<Table> ListTables = new List<Table>();

        static Tables() {
            if (!File.Exists("Tables.xml")) {
                for (int i = 1; i < 6; i++) {
                    ListTables.Add(new Table(i, i, i));
                }
                return;
            }
            SerializerResult result = SerialXML.DeSerializationInXML(out ListTables, "Tables.xml");
        }
    }
    public enum SerializerResult {
        Ok,
        Fault
    }
    public static class SerialXML {
        public static SerializerResult SerializationInXML<Type>(Type serObj, string path) {
            try {
                XmlSerializer serializer = new XmlSerializer(serObj.GetType());
                using (FileStream stream = new FileStream(path, FileMode.Create)) {
                    serializer.Serialize(stream, serObj);
                }
            }
            catch (Exception ex) {
                return SerializerResult.Fault;
            }
            return SerializerResult.Ok;
        }
        public static SerializerResult DeSerializationInXML<Type>(out Type serObj, string path) {
            serObj = default(Type);
            try {
                XmlSerializer serializer = new XmlSerializer(typeof(Type));
                using (FileStream stream = new FileStream(path, FileMode.Open)) {
                    serObj = (Type)serializer.Deserialize(stream);
                }
            }
            catch (Exception ex) {
                return SerializerResult.Fault;
            }
            return SerializerResult.Ok;
        }
    }
}
