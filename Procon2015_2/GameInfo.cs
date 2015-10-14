using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Procon2015_2
{
    class GameInfo
    {
        Fields pFields;
        int stoneNum;
        const int EMPTY = 0;
        StoneList list;

        public GameInfo()
        {
            pFields = new Fields();
            stoneNum = 0;
            list = new StoneList();
        }

        public void GetProblem(COMMON_MAP map)
        {
            string line = "";
            //FILE INPUT
            //FILELDS
            System.IO.StreamReader file = new System.IO.StreamReader(map.PROBLEM_FILE);
            for (int lineCnt = 0; lineCnt < 32; lineCnt++)
            {
                line = file.ReadLine();
                for (int i = 0; i < line.Length; i++)
                {
                    String tmp = line.Substring(i, 1);
                    pFields.SetField(lineCnt * 32 + i, int.Parse(tmp));
                }
            }


            //pFields.PrintFields();

            line = file.ReadLine(); //空列読み込み
            stoneNum = int.Parse(file.ReadLine()); //石の数読み込み

            for (int lineCnt = 0; lineCnt < stoneNum; lineCnt++)
            {
                Stone tmpStn = new Stone();
                for (int i = 0; i < 8; i++)
                {
                    line = file.ReadLine();
                    for (int j = 0; j < 8; j++)
                    {
                        String tmpSt = line.Substring(j, 1);
                        tmpStn.SetStone(i * 8 + j, int.Parse(tmpSt));
                    }
                }
                line = file.ReadLine(); //空列読み
                list.AddStoneList(tmpStn);
            }
        }

        public Fields GetGameInfoFields()
        {
            return pFields;
        }

        public Stone GetGameInfoStone(int no)
        {
            Stone ret = null;
            if (no <= list.GetStoneListCount())
            {
                ret = list.GetStone(no);
            }
            return ret;
        }

        public int GetGameInfoStoneListSize()
        {
            return list.GetStoneListCount();
        }

        public int GetGameInfoMaxScore()
        {
            int ret = 0;

            for (int i = 0; i < pFields.GetFieldsSize(); i++)
            {
                if (pFields.GetFieldsInfo(i) == EMPTY)
                {
                    ret++;
                }
            }

            return ret;
        }

        public StoneList GetStoneList()
        {
            return list;
        }
    }
}
