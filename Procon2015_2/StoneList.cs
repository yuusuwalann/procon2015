using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Procon2015_2
{
    class StoneList
    {
        List<Stone> list;

        public StoneList()
        {
            list = new List<Stone>();
        }

        public void AddStoneList(Stone s)
        {
            list.Add(s);
        }

        public Stone GetStone(int idx)
        {
            Stone ret = new Stone();
            if(list.Count > idx)
            {
                ret = list[idx];
            }
            else
            {
                ret = null;
            }
            return ret;
        }

        public int GetStoneListCount()
        {
            return list.Count;
        }
    }
}
