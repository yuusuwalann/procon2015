using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Procon2015_2
{
    class AGI_Stones
    {
        const int MAX_X = 10;
        const int MAX_Y = 10;
        const int COVER = -7;
        const int STONE = 1;
        const int NOTHING = 0;

        int xSize; /* 1～10 */
        int ySize; /* 1～10 */

        int LeftReduct;
        int UpReduct;
        int RightReduct;
        int DownReduct;

        int[] stone;
        
        public AGI_Stones()
        {
            stone = new int[MAX_X * MAX_Y];
            xSize = MAX_X;
            ySize = MAX_Y;
            LeftReduct = 0;
            UpReduct = 0;
            RightReduct = 0;
            DownReduct = 0;
        }

        public AGI_Stones(Stone st)
        {
            int max_x = 0;
            int min_x = 10;
            int max_y = 0;
            int min_y = 10;
            int initXPoint = -1;
            int initYPoint = -1;

            const int PGM_STONE_MAX_X = 8;
            const int PGM_STONE_MAX_Y = 8;

            /* 石のサイズ測定 */
            for(int i=0; i< PGM_STONE_MAX_X; i++)
            {
                for(int j=0; j< PGM_STONE_MAX_Y; j++)
                {
                    int x_idx = i * PGM_STONE_MAX_Y + j;
                    int y_idx = j * PGM_STONE_MAX_X + i;

                    int getX = st.GetStoneNum(x_idx);
                    int getY = st.GetStoneNum(y_idx);

                    if(getX == 1)
                    {
                        if (max_x < (x_idx % PGM_STONE_MAX_X))
                        {
                            max_x = (x_idx % PGM_STONE_MAX_X);
                        }

                        if(min_x > (x_idx % PGM_STONE_MAX_X))
                        {
                            initXPoint = j;
                            min_x = (x_idx % PGM_STONE_MAX_X);
                        }
                    }
                    if(getY == 1)
                    {
                        if(max_y < (y_idx / PGM_STONE_MAX_X))
                        {
                            max_y = (y_idx / PGM_STONE_MAX_X);
                        }

                        if(min_y > (y_idx / PGM_STONE_MAX_X))
                        {
                            initYPoint = j;
                            min_y = (y_idx / PGM_STONE_MAX_X);
                        }
                    }
                }
            }

            LeftReduct = min_x; /* 出力判定用 */
            UpReduct = min_y; /* 出力判定用 */
            RightReduct = (PGM_STONE_MAX_X - 1) - max_x;
            DownReduct = (PGM_STONE_MAX_Y - 1) - max_y;

            /* 石のサイズ＋接触判定分で動的確保 */
            xSize = (max_x - min_x + 1) + 2;    /* +1 はサイズのため(index5 - index3 = size3, +2は両端の接触判定分 */
            ySize = (max_y - min_y + 1) + 2;    /* ↑ */
            stone = new int[xSize * ySize];

            int initPoint = (initYPoint * PGM_STONE_MAX_X) + initXPoint;

            /* StoneからAGI_Stonesへ */
            int cnt = 0 ;
            for(int i=0; i < stone.Length; i++)
            {
                if ((i < xSize) || (i > (xSize * (ySize - 1))))
                {
                    stone[i] = NOTHING;
                }
                else if (((i % xSize) == 0) || ((i % xSize) == (xSize - 1)))
                {
                    stone[i] = NOTHING;
                }
                else
                {
                   
                    stone[i] = st.GetStoneNum(initPoint + cnt);
                    if(cnt < (max_x - min_x))
                    {
                        cnt++;
                    }
                    else
                    {
                        cnt = 0;
                        initPoint += PGM_STONE_MAX_X;
                    }
                }
                
            }

            /* 接触判定分を設定 */
            for(int i=0; i < stone.Length; i++)
            {
                if (stone[i] == STONE)
                {
                    /* 上下左右を接触セルとする */
                    /* 上 */
                    if (i - xSize >= 0)
                    {
                        if (stone[i - xSize] == NOTHING)
                        {
                            stone[i - xSize] = COVER;
                        }
                    }
                    /* 下 */
                    if(i + xSize < stone.Length)
                    {
                        if (stone[i + xSize] == NOTHING)
                        {
                            stone[i + xSize] = COVER;
                        }
                    }
                    /* 左 */
                    if(i - 1 >= 0)
                    {
                        if(stone[i-1] == NOTHING)
                        {
                            stone[i - 1] = COVER;
                        }
                    }

                    /* 右 */
                    if (i + 1 < stone.Length)
                    {
                        if (stone[i + 1] == NOTHING)
                        {
                            stone[i + 1] = COVER;
                        }
                    }
                }
            }

        }

        public AGI_Stones(AGI_Stones agiSt)
        {
            xSize = agiSt.xSize;
            ySize = agiSt.ySize;
            stone = new int[xSize * ySize];

            for(int i=0; i < stone.Length; i++)
            {
                stone[i] = agiSt.stone[i];
            } 

        }

        public AGI_Stones(int x, int y)
        {
            xSize = x;
            ySize = y;

            stone = new int[x * y];

            for (int i = 0; i < stone.Length; i++)
            {
                stone[i] = 0;
            }
        }

        public void PrintStone()
        {
            Console.WriteLine("");
            for(int i=0; i < stone.Length; i++)
            {
                if ((i % xSize) == 0)
                {
                    Console.WriteLine("");
                }
                Console.Write("{0, 2}",stone[i]);
            }
        }

        public void SetStoneNum(int idx, int num)
        {
            if (idx < stone.Length)
            {
                stone[idx] = num;
            }
        }

        public int GetXSize()
        {
            return xSize;
        }

        public int GetYSize()
        {
            return ySize;
        }

        public int GetSize()
        {
            return (xSize * ySize);
        }

        public int GetNum(int idx)
        {
            int ret = 0;
            if(idx < stone.Length)
            {
                ret = stone[idx];
            }

            return ret;
        }

        public int GetStoneNumSize()
        {
            int stoneCnt = 0;
            for(int i=0; i < stone.Length; i++)
            {
                if(stone[i] == STONE)
                {
                    stoneCnt++;
                }
            }
            return stoneCnt;
        }

        public int GetLeftReduct()
        {
            return LeftReduct;
        }

        public int GetUpReduct()
        {
            return UpReduct;
        }

        public int GetRightReduct()
        {
            return RightReduct;
        }

        public int GetDownReduct()
        {
            return DownReduct;
        }

        public void SetLeftReduct(int num)
        {
            LeftReduct = num;
        }
        public void SetUpReduct(int num)
        {
            UpReduct = num;
        }

        public void SetRightReduct(int num)
        {
            RightReduct = num;
        }

        public void SetDownReduct(int num)
        {
            DownReduct = num;
        }

    }
}
