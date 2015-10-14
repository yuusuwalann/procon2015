using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Procon2015_2
{
    public class Xorshift
    {
        // 内部メモリ
        private UInt32 x;
        private UInt32 y;
        private UInt32 z;
        private UInt32 w;

        public Xorshift() : this((UInt64)DateTime.Now.Ticks)
        {
        }

        public Xorshift(UInt64 seed)
        {
            SetSeed(seed);
        }

        /// <summary>
        /// シード値を設定
        /// </summary>
        /// <param name="seed">シード値</param>
        public void SetSeed(UInt64 seed)
        {
            // x,y,z,wがすべて0にならないようにする
            x = 521288629u;
            y = (UInt32)(seed >> 32) & 0xFFFFFFFF;
            z = (UInt32)(seed & 0xFFFFFFFF);
            w = x ^ z;
        }

        /// <summary>
        /// 乱数を取得
        /// </summary>
        /// <returns>乱数</returns>
        public UInt32 Next()
        {
            UInt32 t = x ^ (x << 11);
            x = y;
            y = z;
            z = w;
            w = (w ^ (w >> 19)) ^ (t ^ (t >> 8));
            return w;
        }

        public UInt32 NextMax(UInt32 max)
        {
            return ((Next())%(max+1));
        }


    }
}
