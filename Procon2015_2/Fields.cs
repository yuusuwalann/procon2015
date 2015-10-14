using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Procon2015_2
{
    class Fields
    {
        const int MAX_X = 32; /* 縦・横 壁を設ける */
        const int MAX_Y = 32;

        int[] fieldInfo = new int[MAX_X * MAX_Y];


        public Fields()
        {
            for (int i = 0; i < fieldInfo.Length; i++)
            {
                fieldInfo[i] = 0;
            }
        }

        public void SetField(int cnt, int num)
        {
            if (cnt < fieldInfo.Length)
            {
                fieldInfo[cnt] = num;
            }
        }

        public void PrintFields()
        {
            for (int i = 0; i < MAX_X; i++)
            {
                for (int j = 0; j < MAX_Y; j++)
                {
                    Console.Write(fieldInfo[i * MAX_X + j]);
                }
                Console.WriteLine("");
            }
            Console.WriteLine("");
        }

        public Fields StoneSetExists(Stone stone, int x, int y)
        {
            Fields retFld = new Fields();


            return retFld;
        }

        private int indexToX(int idx)
        {
            return (idx % MAX_X);
        }

        private int indexToY(int idx)
        {
            return (idx / MAX_X);
        }

        public int GetFieldsSize()
        {
            return fieldInfo.Length;
        }

        public int GetFieldsInfo(int idx)
        {
            if (fieldInfo.Length <= idx)
            {
                return -1;
            }
            else
            {
                return fieldInfo[idx];
            }
        }

        public void GetCopyFromA(Fields A)
        {
            for (int i = 0; i < this.fieldInfo.Length; i++)
            {
                this.fieldInfo[i] = A.fieldInfo[i];
            }
        }

        /* フィールドの空の数を返す */
        public int GetEmputySize()
        {
            int size = 0;

            for(int i=0; i < fieldInfo.Length; i++)
            {
                if(fieldInfo[i] == 0)
                {
                    size++;
                }
            }

            return size;
        }
    }
}
