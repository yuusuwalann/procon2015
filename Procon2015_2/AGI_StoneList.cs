using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Procon2015_2
{
    class AGI_StoneList
    {
        List<AGI_StoneKindsList> list;

        public AGI_StoneList()
        {
            list = new List<AGI_StoneKindsList>();
        }

        public AGI_StoneList(StoneList giList)
        {
            list = new List<AGI_StoneKindsList>();

            for(int i=0; i < giList.GetStoneListCount(); i++)
            {
                AGI_StoneKindsList kTmp = new AGI_StoneKindsList(giList.GetStone(i));
                this.list.Add(kTmp);
            }
        }

        public int GetListSize()
        {
            return list.Count;
        }

        public AGI_StoneKindsList GetAgiStoneKindsList(int idx)
        {
            if(idx < list.Count)
            {
                return list[idx];
            }
            return null;
        }


    }
}
