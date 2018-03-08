using System.Runtime.Serialization;

namespace UBSLib
{
    [DataContractAttribute()]
    public class ModuleInfoLocal : ModuleInfo
    {
        ///////////////////////////////////////////////////////////
        [DataMember()]
        public string Filename { get; set; }
        [DataMember()]
        public string ModuleType { get; set; }

        ///////////////////////////////////////////////////////////
        public ModuleInfoLocal()
            : base("undefined", "undefined")
        {
            Filename = "undefined";
            ModuleType = typeof(UBSModule).FullName;
        }
    }
}
