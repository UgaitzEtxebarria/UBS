using System.Runtime.Serialization;

namespace UBSLib
{
    [DataContractAttribute()]
    public class ModuleInfoRemote : ModuleInfo
    {
        ///////////////////////////////////////////////////////////
        [DataMember()]
        public string Filename { get; set; }
        [DataMember()]
        public string ModuleType { get; set; }

        [DataMember()]
        public string Ip { get; set; }

        ///////////////////////////////////////////////////////////
        public ModuleInfoRemote()
            : base("undefined", "undefined")
        {
            Filename = "undefined";
            ModuleType = typeof(UBSModule).FullName;
            Ip = "undefined";
        }
    }
}
