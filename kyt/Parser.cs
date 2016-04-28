using System;
using System.Text;
using System.Web.Script.Serialization;

namespace kyt
{
    class Parser
    {
 
        private static readonly Lazy<Parser> instance = new Lazy<Parser>(() => new Parser());

        private Parser() { }

        public static Parser Instance {
            get
            {
                return instance.Value;
            }
        }

        public byte[] GetBytes(string s)
        {
            s = s.Replace(" ", "");
            byte[] buffer = new byte[s.Length / 2];
            for (int i = 0; i < s.Length; i += 2)
                buffer[i / 2] = (byte)Convert.ToByte(s.Substring(i, 2), 16);
            return buffer;
        }

        public string GetString(byte[] data)
        {
            StringBuilder sb = new StringBuilder(data.Length * 3);
            foreach (byte b in data)
                sb.Append(Convert.ToString(b, 16).PadLeft(2, '0'));
            return sb.ToString().ToUpper();
        }

        public dynamic GetJSON(string str) {
            var serializer = new JavaScriptSerializer();
            serializer.RegisterConverters(new[] { new DynamicJsonConverter() });
            return serializer.Deserialize(str, typeof(object));
        }

}
}
