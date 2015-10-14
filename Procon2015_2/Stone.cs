using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Procon2015_2
{
    class Stone
    {
        const int MAX_X = 8;
        const int MAX_Y = 8;

        int[] stone;

        public Stone()
        {
            stone = new int[MAX_X* MAX_Y];
            for (int i = 0; i < stone.Length; i++)
            {
                stone[i] = 0;

            }
        }

        public void SetStone(int cnt, int num)
        {
            if (cnt < stone.Length)
            {
                stone[cnt] = num;
            }
        }

        public void PrintStone()
        {

            for (int i = 0; i < MAX_X; i++)
            {
                for (int j = 0; j < MAX_Y; j++)
                {
                    Console.Write(stone[i * MAX_X + j]);
                }
                Console.WriteLine("");
            }
            Console.WriteLine("");
        }

        public int[] GetStonesMap()
        {
                return stone;
        }

        public int GetStoneNum(int i)
        {
            int ret = 0;
            if(i < stone.Length)
            {
                ret = stone[i];
            }
            return ret;
        }

        public int GetStoneLength()
        {
            return stone.Length;
        }
    }
}
