using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Procon2015_2
{
    class Output
    {
        enum Kinds
        {
            ORIGIN = 0,
            R90, R180, R270, REV_ORG, REV_R90, REV_R180, REV_R270
        }

        /* FORMAT 
         * H:表 T:裏
         * 0:ORG, 90:90°,180:180°,270:270°
         */
        
        AdvanceGameInfo agi;
        string [] stoneReverse;
        string [] stoneRotate;
        int[] stoneX;
        int[] stoneY;

        public Output()
        {
            agi = null;
            stoneReverse = null;
            stoneRotate = null;
            stoneX = null;
            stoneY = null;
        }

        public Output(AdvanceGameInfo agi)
        {
            this.agi = agi;
            int listSize = agi.GetAgiStoneList().GetListSize();
            stoneReverse = new string[listSize];
            stoneRotate = new string[listSize];
            stoneX = new int[listSize];
            stoneY = new int[listSize];
            for(int i=0; i < listSize; i++)
            {
                stoneReverse[i] = "";
                stoneRotate[i] = "";
                stoneX[i] = 0;
                stoneY[i] = 0;
            }
        }

        public void SetStone(int id, int kinds, int agiFldIdx)
        {
            if (id < stoneReverse.Length)
            {
                exchangeKinds(id, kinds);
                exchangeIdx(id, agiFldIdx,kinds);
            }
        }

        private void exchangeKinds(int id, int kinds)
        {
            switch (kinds)
            {
                case (int)Kinds.ORIGIN:
                    stoneReverse[id] = "H";     /* 表 */
                    stoneRotate[id] = "0";      /* 0°*/
                    break;
                case (int)Kinds.R90:
                    stoneReverse[id] = "H";     /* 表 */
                    stoneRotate[id] = "90";     /* 90°*/
                    break;
                case (int)Kinds.R180:
                    stoneReverse[id] = "H";     /* 表 */
                    stoneRotate[id] = "180";    /* 180°*/
                    break;
                case (int)Kinds.R270:
                    stoneReverse[id] = "H";     /* 表 */
                    stoneRotate[id] = "270";    /* 270°*/
                    break;
                case (int)Kinds.REV_ORG:
                    stoneReverse[id] = "T"; /* 裏 */
                    stoneRotate[id] = "0";      /* 0°*/
                    break;
                case (int)Kinds.REV_R90:
                    stoneReverse[id] = "T"; /* 裏 */
                    stoneRotate[id] = "90";     /* 90°*/
                    break;
                case (int)Kinds.REV_R180:
                    stoneReverse[id] = "T"; /* 裏 */
                    stoneRotate[id] = "180";    /* 180°*/
                    break;
                case (int)Kinds.REV_R270:
                    stoneReverse[id] = "T"; /* 裏 */
                    stoneRotate[id] = "270";    /* 270°*/
                    break;
                default:
                    break;
            
            }
        }

        private void exchangeIdx(int id, int idx, int kinds)
        {
            int leftReduct = agi.GetAgiStoneList().GetAgiStoneKindsList(id).GetAGIStones(kinds).GetLeftReduct();
            int upReduct = agi.GetAgiStoneList().GetAgiStoneKindsList(id).GetAGIStones(kinds).GetUpReduct();
            int AGIF_MAX_X = agi.GetAgiFields().GetMaxX();
            int AGIF_MAX_Y = agi.GetAgiFields().GetMaxY();

            AGI_Stones tmp = agi.GetAgiStoneList().GetAgiStoneKindsList(id).GetAGIStones(0);

            /* idx⇒(x, y)baseAGI_F */
            int tmpX = (idx % AGIF_MAX_X);
            int tmpY = (idx / AGIF_MAX_X);

            /* AGI_STONE(x, y)baseAGI_F⇒PRB_STONE(x, y)baseAGI_F */
            tmpX -= leftReduct;
            tmpY -= upReduct;

            stoneX[id] = tmpX;
            stoneY[id] = tmpY;

        }

        public void OutputExec(string fileName)
        {
            StreamWriter writer = new StreamWriter(fileName, false, Encoding.ASCII);
            for (int i = 0; i < stoneReverse.Length; i++)
            {
                if(stoneReverse[i] == "")
                {
                    writer.WriteLine("");
                }
                else
                {
                    writer.WriteLine("{0} {1} {2} {3}", stoneX[i].ToString(), stoneY[i].ToString(), stoneReverse[i], stoneRotate[i]);
                }
            }
            writer.Close();

        }

    }
}
