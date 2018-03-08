using System.Windows.Forms;

namespace UBSLib
{
    public class UBSBackgroundModule : UBSModule
    {

        //////////////////////////////////////////////////////////////
        public Form WindowForm { get; set; }

        //////////////////////////////////////////////////////////////
        public UBSBackgroundModule(string _id)
            : base(_id)
        {
        }
    }
}
