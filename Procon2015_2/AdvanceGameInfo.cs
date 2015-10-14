using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Procon2015_2
{
    class AdvanceGameInfo
    {
        AGI_Fields agiFields;
        AGI_StoneList agiStoneList;

        public AdvanceGameInfo()
        {
            agiFields = new AGI_Fields();
            agiStoneList = new AGI_StoneList();
        //    agiFields.PrintAgiFields();
        }

        public AdvanceGameInfo(GameInfo gi)
        {
            agiFields = new AGI_Fields(gi.GetGameInfoFields());
            agiStoneList = new AGI_StoneList(gi.GetStoneList());
        //    agiFields.PrintAgiFields();
           
        }

        public AGI_Fields GetAgiFields()
        {
            return agiFields;
        }

        public AGI_StoneList GetAgiStoneList()
        {
            return agiStoneList;
        }
    }
}
