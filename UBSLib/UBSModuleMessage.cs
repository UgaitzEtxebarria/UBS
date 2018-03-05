
using System.Text;

namespace UBSLib
{
    public class UBSMessage
    {
        //////////////////////////////////////////////////////////////
        public enum UBSContentType { Text, Binary }

        public string DestinyId { get; set; }
        public string OriginId { get; set; }
        public byte[] Data { get; set; }
        public UBSContentType ContentType { get; set; }

        private ASCIIEncoding ascii_encoding = new ASCIIEncoding();
        private Encoding enc = new UTF8Encoding(true, true);

        //////////////////////////////////////////////////////////////
        public UBSMessage(string destiny, string data)
        {
            DestinyId = destiny;
            OriginId = "undefined";
            Data = enc.GetBytes(data);
            ContentType = UBSContentType.Text;
        }

        //////////////////////////////////////////////////////////////
        public UBSMessage(string destiny, byte[] data)
        {
            DestinyId = destiny;
            OriginId = "undefined";
            Data = data;
            ContentType = UBSContentType.Binary;
        }

        //////////////////////////////////////////////////////////////
        public override string ToString()
        {
            return enc.GetString(Data);
        }

        //////////////////////////////////////////////////////////////
        public byte[] ToByteArray()
        {
            return Data;
        }

        //////////////////////////////////////////////////////////////
    }
}
