using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Procon2015_2
{
    /* 配置用データ */
    class StoreData
    {
        /* 配置データID */
        public int ID
        {
            get; set;
        }
        /* 配置リスト */
        AGI_CalcFields agicF;

        /* 今までに接地してきた石のリスト */
        private List<int> stoneIdList;

        /* 今までに接地してきた石の頂点リスト*/
        private List<int> stonePlaceList;

        /* 今までに接地してきた石の種別リスト */
        private List<int> stoneKindsList;

        /* 今までの配置に対する評価値リスト */
        private List<float> evalueList;

        /* 評価値 */
        public float evalue
        {
            get; set;
        }

        /* スコア */
        public int score
        {
            get; set;
        }

        public bool closed
        {
            get; set;
        }

        public int lastAttackStone
        {
            get; set;
        }

        public int ROOT_PARENT_ID = 0;

        public static int idCounter = 1;

        /* 静的コンストラクタ */
        static StoreData()
        {
            StoreData.idCounter = 1;
        }

        StoreData()
        {
            ID = idCounter;
            StoreData.idCounter++;
            lastAttackStone = 0;
            stoneIdList = new List<int>();
            stonePlaceList = new List<int>();
            stoneKindsList = new List<int>();
            evalueList = new List<float>();
            closed = false;
            agicF = null;
            lastAttackStone = 0;

            evalue = -1;
            score = -1;
        }

        public StoreData(bool idExists, AGI_CalcFields ag, COMMON_MAP map)
        {
            if (idExists == true)
            {
                ID = idCounter;
                StoreData.idCounter++;
            }
            else
            {
                ID = 0;
            }
            stoneIdList = new List<int>();
            stonePlaceList = new List<int>();
            stoneKindsList = new List<int>();
            evalueList = new List<float>();
            evalue = -1;
            score = -1;
            closed = false;
            agicF = new AGI_CalcFields(ag, map);
            lastAttackStone = 0;
        }

        public StoreData(StoreData st, COMMON_MAP map)
        {
            this.ID = st.ID;
            stoneIdList = new List<int>();
            stonePlaceList = new List<int>();
            stoneKindsList = new List<int>();
            evalueList = new List<float>();

            for(int i=0; i < st.stoneKindsList.Count; i++)
            {
                stoneIdList.Add(st.stoneIdList[i]);
                stonePlaceList.Add(st.stonePlaceList[i]);
                stoneKindsList.Add(st.stoneKindsList[i]);
            }
            for(int i=0; i < evalueList.Count; i++)
            {
                evalueList.Add(st.evalueList[i]);
            }
            evalue = st.evalue;
            score = st.score;
            closed = st.closed;
            agicF = new AGI_CalcFields(st.agicF, map);
            lastAttackStone = st.lastAttackStone;

        }

        public void SetStoneInfo(int stoneId, int placeIdx, int kinds)
        {
            stoneIdList.Add(stoneId);
            stonePlaceList.Add(placeIdx);
            stoneKindsList.Add(kinds);
        }

        public void SetEvalueList(float evalue)
        {
            evalueList.Add(evalue);
        }

        public int GetFinallyStoneId()
        {
            int ret = 0;
            if(stoneIdList.Count != 0)
            {
                ret = stoneIdList[stoneIdList.Count - 1];
            }
            else
            {
                ret = -1;
            }
            return ret;
        }

        public int GetStoneInfoSize()
        {
            return stoneIdList.Count;
        }

        public int GetStoneInfoKind(int idx)
        {
            if(idx < stoneKindsList.Count)
            {
                return stoneKindsList[idx];
            }
            return -1;
        }

        public int GetStoneInfoId(int idx)
        {
            if (idx < stoneIdList.Count)
            {
                return stoneIdList[idx];
            }
            return -1;
        }

        public int GetStoneInfoPlace(int idx)
        {
            if(idx < stonePlaceList.Count)
            {
                return stonePlaceList[idx];
            }
            return -1;
        }

        public float GetEvalueInfo(int idx)
        {
            if (idx < evalueList.Count)
            {
                return evalueList[idx];
            }

            return -1;
        }

        public float GetEvalueListSum()
        {
            float ret = 0.0F ; 

            for(int i=0; i < evalueList.Count; i++)
            {
                ret += evalueList[i];
            }

            return ret;
        }

        public int GetEvalueListSize()
        {
            return evalueList.Count;
        }

        public void PrintStoreData()
        {
            Console.WriteLine("");
            Console.WriteLine("ID:{0}", ID);
            Console.WriteLine("EVALUE:{0}", evalue);
            Console.WriteLine("SCORE:{0}", score);
            for (int i = 0; i < stoneIdList.Count; i++)
            {
                Console.WriteLine("i:{0},ID:{1},kinds:{2},place{3}", i, stoneIdList[i], stoneKindsList[i], stonePlaceList[i]);
            }
        }

        public AGI_CalcFields GetAgiCalcFields()
        {
            return agicF;
        }
    }
}
