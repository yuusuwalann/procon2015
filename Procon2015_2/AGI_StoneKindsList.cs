using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Procon2015_2
{
    class AGI_StoneKindsList
    {
        List<AGI_Stones> list;
        int ID;

        public static int idCounter = 0;

        enum Kinds
        {
            ORIGIN = 0,
            R90, R180, R270, REV_ORG, REV_R90, REV_R180, REV_R270
        }

        /* 静的コンストラクタ */
        static AGI_StoneKindsList()
        {
            AGI_StoneKindsList.idCounter = 0;
        }

        public AGI_StoneKindsList()
        {
            list = new List<AGI_Stones>();

            ID = idCounter;
            AGI_StoneKindsList.idCounter++;

            for (int i=0; i < (int)Kinds.REV_R270; i++)
            {
                AGI_Stones tmp = new AGI_Stones();
                list.Add(tmp);
            }
        }

        public AGI_StoneKindsList(Stone orgSt)
        {
            list = new List<AGI_Stones>();
            ID = idCounter;
            AGI_StoneKindsList.idCounter++;
            AGI_Stones tmp = new AGI_Stones(orgSt);
            list.Add(tmp);
            //tmp.PrintStone();
            for(int i = (int)Kinds.R90; i <= (int)Kinds.REV_R270; i++)
            {
                if (i == (int)Kinds.REV_ORG)
                {
                    tmp = createKinds(list[(int)Kinds.ORIGIN], i, list[(int)Kinds.ORIGIN]);
                }
                else
                {
                    tmp = createKinds(list[i - 1], i, list[(int)Kinds.ORIGIN]);
                }
                list.Add(tmp);
           //     tmp.PrintStone();
            }

            /* 重複する石の削除 */
            /* 回転・反転により石の形が重複した場合、片方の石を破壊(null)する */
            Dictionary<String, int> overlapCheck = new Dictionary<String, int>();
            for(int i = (int)Kinds.ORIGIN; i <= (int)Kinds.REV_R270; i++)
            {
                String str = "";
                for(int j=0; j < list[i].GetSize(); j++)
                {
                    str += list[i].GetNum(j).ToString();
                }

                if(overlapCheck.ContainsKey(str) == true) /* すでに格納済みの形 */
                {
                    list[i] = null;
                }
                else
                {
                    overlapCheck.Add(str, 1);
                }

            }

        }

        private AGI_Stones createKinds(AGI_Stones target, int k, AGI_Stones org)
        {
            AGI_Stones retStone;
            int[] ret = new int[] { };
            int r = 0;
            int l = 0;
            int u = 0;
            int d = 0;

            switch (k)
            {
                case (int)Kinds.R90:
                    /* xサイズとyサイズを反転 */
                    retStone = new AGI_Stones(target.GetYSize(), target.GetXSize());
                    ret = r90(target);
                    r = org.GetUpReduct();
                    d = org.GetRightReduct();
                    l = org.GetDownReduct();
                    u = org.GetLeftReduct();

                    break;
                case (int)Kinds.R180:
                    /* xサイズとyサイズを反転 */
                    retStone = new AGI_Stones(target.GetYSize(), target.GetXSize());
                    ret = r90(target);
                    r = org.GetLeftReduct();
                    d = org.GetUpReduct();
                    l = org.GetRightReduct();
                    u = org.GetDownReduct();

                    break;
                case (int)Kinds.R270:
                    /* xサイズとyサイズを反転 */
                    retStone = new AGI_Stones(target.GetYSize(), target.GetXSize());
                    ret = r90(target);
                    r = org.GetDownReduct();
                    d = org.GetLeftReduct();
                    l = org.GetUpReduct();
                    u = org.GetRightReduct();
                    break;
                case (int)Kinds.REV_ORG:
                    /* xサイズとyサイズはそのまま */
                    retStone = new AGI_Stones(target.GetXSize(), target.GetYSize());
                    ret = rev(target);
                    r = org.GetLeftReduct();
                    d = org.GetDownReduct();
                    l = org.GetRightReduct();
                    u = org.GetUpReduct();
                    break;
                case (int)Kinds.REV_R90:
                    /* xサイズとyサイズを反転 */
                    ret = r90(target);
                    retStone = new AGI_Stones(target.GetYSize(), target.GetXSize());
                    r = org.GetUpReduct();
                    d = org.GetLeftReduct();
                    l = org.GetDownReduct();
                    u = org.GetRightReduct();
                    break;
                case (int)Kinds.REV_R180:
                    /* xサイズとyサイズを反転 */
                    ret = r90(target);
                    retStone = new AGI_Stones(target.GetYSize(), target.GetXSize());
                    r = org.GetRightReduct();
                    d = org.GetUpReduct();
                    l = org.GetLeftReduct();
                    u = org.GetDownReduct();
                    break;
                case (int)Kinds.REV_R270:
                    /* xサイズとyサイズを反転 */
                    ret = r90(target);
                    retStone = new AGI_Stones(target.GetYSize(), target.GetXSize());
                    r = org.GetDownReduct();
                    d = org.GetRightReduct();
                    l = org.GetUpReduct();
                    u = org.GetLeftReduct();
                    break;
                default:
                    retStone = null;
                    break;
            }

            for (int i = 0; i < ret.Length; i++)
            {
                retStone.SetStoneNum(i, ret[i]);
            }
            retStone.SetRightReduct(r);
            retStone.SetLeftReduct(l);
            retStone.SetUpReduct(u);
            retStone.SetDownReduct(d);

            return retStone;
        }

        private int[] r90(AGI_Stones agiSt)
        {
            int[] ret = new int[agiSt.GetXSize() * agiSt.GetYSize()];

            for (int i = 0; i < agiSt.GetXSize(); i++)
            {
                for (int j = 0; j < agiSt.GetYSize(); j++)
                {
                    ret[((agiSt.GetYSize() - 1) - j) + i * agiSt.GetYSize()] = agiSt.GetNum(j * agiSt.GetXSize() + i);
                }
            }

            return ret;
        }

        private int[] rev(AGI_Stones agiSt)
        {
            int[] ret = new int[agiSt.GetXSize() * agiSt.GetYSize()];
            for (int i = 0; i < agiSt.GetYSize(); i++)
            {
                int[] array = new int[agiSt.GetXSize()];
                for (int j = 0; j < agiSt.GetXSize(); j++)
                {
                    array[j] = agiSt.GetNum(i * agiSt.GetXSize() + j);
                }
                Array.Reverse(array);
                for (int j = 0; j < agiSt.GetXSize(); j++)
                {
                    ret[i * agiSt.GetXSize() + j] = array[j];
                }
            }
            return ret;
        }

        public AGI_Stones GetAGIStones(int kind)
        {
            if(kind <= (int)Kinds.REV_R270)
            {
                return list[kind];
            }
            return null;
        }

        public int GetStoneNumSize()
        {
            return list[(int)Kinds.ORIGIN].GetStoneNumSize();

        }

        public int GetStoneKindsListId()
        {
            return ID;
        }
    }
}
