using System.Windows.Forms;

namespace UBSLib
{
    public class UBSVisualModule : UBSModule
    {

        //////////////////////////////////////////////////////////////
        public Form WindowForm { get; set; }

        //////////////////////////////////////////////////////////////
        public UBSVisualModule(string _id)
            : base(_id)
        {
        }
    }
}
