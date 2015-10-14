using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Procon2015_2
{
    class AGI_Fields
    {
        const int F_MAX_X = 34; /* 問題最大32 壁2 */
        const int F_MAX_Y = 34; /* ↑ */

        const int EMPUTY = 0;
        const int MY_FIELD = 1;
        const int WALL = 3;
        const int OBJECT = 5;

        int[] fields;

        public AGI_Fields()
        {
            fields = new int[F_MAX_X * F_MAX_Y];

            /* 初期化 */
            for(int i=0; i < fields.Length; i++)
            {
                if (i < F_MAX_X)
                {
                    fields[i] = WALL;
                }else if(i > (F_MAX_X * (F_MAX_Y - 1))){
                    fields[i] = WALL;
                }else if (((i % F_MAX_X) == 0) || ((i % F_MAX_X) == (F_MAX_X-1)))
                {
                    fields[i] = WALL;
                }
                else
                {
                    fields[i] = EMPUTY;
                }
            }
        }

        public AGI_Fields(Fields gi_fields)
        {
            fields = new int[F_MAX_X * F_MAX_Y];

            /* 初期化 */
            int cnt = 0;
            for (int i = 0 ; i < fields.Length; i++)
            {
                if (i < F_MAX_X)
                {
                    fields[i] = WALL;
                }
                else if (i > (F_MAX_X * (F_MAX_Y - 1)))
                {
                    fields[i] = WALL;
                }
                else if (((i % F_MAX_X) == 0) || ((i % F_MAX_X) == (F_MAX_X - 1)))
                {
                    fields[i] = WALL;
                }
                else
                {
                    if (gi_fields.GetFieldsInfo(cnt) == 0)
                    {
                        fields[i] = EMPUTY;
                    }
                    else
                    {
                        fields[i] = OBJECT;
                    }
                    cnt++;
                }
            }


        }

        public void PrintAgiFields()
        {
            for(int i=0; i<fields.Length; i++)
            {
                if ((i % F_MAX_X) == 0)
                {
                    Console.WriteLine("");
                }
                Console.Write(fields[i]);
            }
        }

        public int GetEmptySize()
        {
            int emptyCnt = 0;

            for(int i=0; i < fields.Length; i++)
            {
                if(fields[i] == EMPUTY)
                {
                    emptyCnt++;
                }
            }

            return emptyCnt;
        }

        public int GetAgiFieldsNum(int idx)
        {
            if(idx < fields.Length)
            {
                return fields[idx];
            }
            return -1;
        }

        public int GetMaxX()
        {
            return F_MAX_X;
        }

        public int GetMaxY()
        {
            return F_MAX_Y;
        }
    }
}
