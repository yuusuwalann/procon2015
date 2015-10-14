using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace Procon2015_2
{
    /* アプローチ方法
     *
    */

    /* 各定数情報 
     * AGI_FIELDS 
     *     空：0
     *     自：1
     *     壁：3
     *     障：5
     *     近1:-11
     *     近2:-13
     *
     * AGI_STONE
     *     空:0
     *     石:1
     *     近1:-7
     *
     */
    class Search
    {
        AdvanceGameInfo agi;
        int epsilonSize;   
        int problemEmptySize;
        int depthSize;
        int searchTimes;     /* サーチ回数 */
        string fileName;
        COMMON_MAP commonMap;

        const int EVAL_A = 0;   /* 評価関数(サイズ値あり) */
        const int EVAL_B = 1;   /* 評価関数(サイズ無関係) */
        const int EVAL_C = 2;   /* 評価関数(サイズ関係あり) */

        int INIT_SEARCH_IDX;  /* 初手最大探索IDX */
        int INIT_CHOISE_FOR_EVAL_NUM;   /* 初手保持個数 */
        int INIT_SEEDS_SAVE;             /* 初手種保存数 */

        int SEARCH_KIND_MAX;              /* 一つに対しx手分調査 */
        int CHOISE_FOR_EVAL_NUM;        /* 優秀種残し数 */

        int DEPTH_BIG_80;
        int DEPTH_BIG_50;
        int DEPTH_BIG_0;

        int DEPTH_MID_80;
        int DEPTH_MID_50;
        int DEPTH_MID_0;

        int DEPTH_MIN_80;
        int DEPTH_MIN_50;
        int DEPTH_MIN_0;


        const int MY_FIELD = 1;
        const int WALL = 3;
        const int OBJECT = 5;
        const int F_MAX_X = 34;
        const int NEIGHT1 = -18;
        const int NEIGHT2 = -27;
        const int NEIGHT3 = -36;
        const int NEIGHT4 = -45;
        const int NEIGHT5 = -54;
        const int NEIGHT6 = -63;
        const int NEIGHT7 = -72;
        const int NEIGHT8 = -81;

        const int PROG_SIZE_BIG = 3;
        const int PROG_SIZE_MID = 2;
        const int PROG_SIZE_SML = 1;

        int ANNEALING_RATE_0;
        int ANNEALING_RATE_10;
        int ANNEALING_RATE_20;
        int ANNEALING_RATE_30;
        int ANNEALING_RATE_40;
        int ANNEALING_RATE_50;
        int ANNEALING_RATE_60;
        int ANNEALING_RATE_70;
        int ANNEALING_RATE_80;
        int ANNEALING_RATE_90;
        int ANNEALING_RATE_100;

        int[] AnnealingRate;

        int[] smallOrderList;

        int RETRY_COUNT_EVAL = 20;
        int RETRY_COUNT_RAND = 5;

        Xorshift rand;
        Dictionary<int, StoreData> storeDataDic;
        ConcurrentQueue<int> searchQueId;
        ConcurrentQueue<int> nextQueId;
        Dictionary<int, StoreData> answerId;
        Dictionary<int, StoreData> saveStoreDataDic;

        /* 再探索 */
        Queue<List<StoreData>> RetrySearch;

        enum Kinds  /* Sorce:AGI_StoneKindsList */
        {
            ORIGIN = 0,
            R90, R180, R270, REV_ORG, REV_R90, REV_R180, REV_R270
        }

        public Search()
        {
            agi = null;
            commonMap = null;
            smallOrderList = null;
            rand = new Xorshift();
            storeDataDic = new Dictionary<int, StoreData>();
            searchQueId = new ConcurrentQueue<int>();
            nextQueId = new ConcurrentQueue<int>();
            answerId = new Dictionary<int, StoreData>();
            saveStoreDataDic = new Dictionary<int, StoreData>();
            problemEmptySize = 0;
            depthSize = 0;
            searchTimes = 0;
            AnnealingRate = new int[11] { ANNEALING_RATE_0,
                ANNEALING_RATE_10,
                ANNEALING_RATE_20,
                ANNEALING_RATE_30,
                ANNEALING_RATE_40,
                ANNEALING_RATE_50,
                ANNEALING_RATE_60,
                ANNEALING_RATE_70,
                ANNEALING_RATE_80,
                ANNEALING_RATE_90,
                ANNEALING_RATE_100 };
            setConstDataDefault();
            fileName = "default.txt";
            RetrySearch = new Queue<List<StoreData>>();
        }

        public Search(AdvanceGameInfo agi, COMMON_MAP map)
        {
            this.agi = agi;
            this.commonMap = map;
            rand = new Xorshift();
            storeDataDic = new Dictionary<int, StoreData>();
            searchQueId = new ConcurrentQueue<int>();
            nextQueId = new ConcurrentQueue<int>();
            answerId = new Dictionary<int, StoreData>();
            saveStoreDataDic = new Dictionary<int, StoreData>();
            problemEmptySize = agi.GetAgiFields().GetEmptySize();  /* 問題のEMPTYサイズ */
            depthSize = 0;
            searchTimes = 0;
            AnnealingRate = new int[11] { ANNEALING_RATE_0,
                ANNEALING_RATE_10,
                ANNEALING_RATE_20,
                ANNEALING_RATE_30,
                ANNEALING_RATE_40,
                ANNEALING_RATE_50,
                ANNEALING_RATE_60,
                ANNEALING_RATE_70,
                ANNEALING_RATE_80,
                ANNEALING_RATE_90,
                ANNEALING_RATE_100 };
            setConstData(map);
            this.fileName = map.FILENAME;
            smallOrderList = new int[agi.GetAgiStoneList().GetListSize()];
            RetrySearch = new Queue<List<StoreData>>();
        }

        public int SearchStart()
        {
            int retScore = 0;

            /* 準備 */
            preparation();

            /* 初手の探索 */
            step1();

            /* 初手の中から探索対象を決める */
            step2();

            /* 2番目以降の探索 */
            step3();

            /* 出力処理*/
            retScore = step4();

            /* 再探索処理 */
            int retryCnt = 1;
            setRetryConstData(commonMap);       /* 再探索用定数に更新 */
            while (RetrySearch.Count != 0)
            {
                Console.WriteLine("START_READ_CFG_FILE");
                commonMap.RetryCommonParam(commonMap.CFG_FILE);
                Console.WriteLine("END_READ_CFG_FILE");
                initialData();                        /* クリア */
                retryDataSet();                        /* リトライデータ設定 */
                step3();                             /* 探索 */
                if (answerId.Count != 0)
                {
                    retScore = step5(retScore, retryCnt);                        /* 再探索用出力処理 */
                }
                retryCnt++;
            }

            return retScore;
        }

        private void setConstData(COMMON_MAP map) 
        {
            INIT_SEARCH_IDX = map.INIT_SEARCH_IDX;  /* 初手最大探索IDX */
            INIT_CHOISE_FOR_EVAL_NUM = map.INIT_CHOISE_FOR_EVAL_NUM;   /* 初手保持個数 */
            INIT_SEEDS_SAVE = map.INIT_SEEDS_SAVE;             /* 初手種保存数 */

            SEARCH_KIND_MAX = map.SEARCH_KIND_MAX;              /* 一つに対しx手分調査 */
            CHOISE_FOR_EVAL_NUM = map.CHOISE_FOR_EVAL_NUM;        /* 優秀種残し数 */

            DEPTH_BIG_80 = map.DEPTH_BIG_80;
            DEPTH_BIG_50 = map.DEPTH_BIG_50;
            DEPTH_BIG_0 = map.DEPTH_BIG_0;

            DEPTH_MID_80 = map.DEPTH_MID_80;
            DEPTH_MID_50 = map.DEPTH_MID_50;
            DEPTH_MID_0 = map.DEPTH_MID_0;

            DEPTH_MIN_80 = map.DEPTH_MIN_80;
            DEPTH_MIN_50 = map.DEPTH_MIN_50;
            DEPTH_MIN_0 = map.DEPTH_MIN_0;

            ANNEALING_RATE_0 = map.ANNEALING_RATE_0;
            ANNEALING_RATE_10 = map.ANNEALING_RATE_10;
            ANNEALING_RATE_20 = map.ANNEALING_RATE_20;
            ANNEALING_RATE_30 = map.ANNEALING_RATE_30;
            ANNEALING_RATE_40 = map.ANNEALING_RATE_40;
            ANNEALING_RATE_50 = map.ANNEALING_RATE_50;
            ANNEALING_RATE_60 = map.ANNEALING_RATE_60;
            ANNEALING_RATE_70 = map.ANNEALING_RATE_70;
            ANNEALING_RATE_80 = map.ANNEALING_RATE_80;
            ANNEALING_RATE_90 = map.ANNEALING_RATE_90;
            ANNEALING_RATE_100 = map.ANNEALING_RATE_100;

            RETRY_COUNT_EVAL = map.RETRY_COUNT_EVAL;
            RETRY_COUNT_RAND = map.RETRY_COUNT_RAND;
        }

        private void setConstDataDefault()
        {
            INIT_SEARCH_IDX = 2;  /* 初手最大探索IDX */
            INIT_CHOISE_FOR_EVAL_NUM = 100;   /* 初手保持個数 */
            INIT_SEEDS_SAVE = 5;             /* 初手種保存数 */

            SEARCH_KIND_MAX = 2;              /* 一つに対しx手分調査 */
            CHOISE_FOR_EVAL_NUM = 80;        /* 優秀種残し数 */

            DEPTH_BIG_80 = 3;
            DEPTH_BIG_50 = 2;
            DEPTH_BIG_0 = 1;

            DEPTH_MID_80 = 3;
            DEPTH_MID_50 = 2;
            DEPTH_MID_0 = 1;

            DEPTH_MIN_80 = 3;
            DEPTH_MIN_50 = 2;
            DEPTH_MIN_0 = 1;

            ANNEALING_RATE_0 = 50;
            ANNEALING_RATE_10 = 40;
            ANNEALING_RATE_20 = 40;
            ANNEALING_RATE_30 = 30;
            ANNEALING_RATE_40 = 30;
            ANNEALING_RATE_50 = 20;
            ANNEALING_RATE_60 = 20;
            ANNEALING_RATE_70 = 10;
            ANNEALING_RATE_80 = 10;
            ANNEALING_RATE_90 = 5;
            ANNEALING_RATE_100 = 5;

            RETRY_COUNT_EVAL = 20;
            RETRY_COUNT_RAND = 5;
        }

        private void calcUpperLowerBound() 
        {
            /* MAP (ID, SIZE)作成 */
            Dictionary<int, int> tmpMap = new Dictionary<int, int>();
            for(int i=0; i < agi.GetAgiStoneList().GetListSize(); i++)
            {
                tmpMap.Add(i, agi.GetAgiStoneList().GetAgiStoneKindsList(i).GetStoneNumSize());
            }

            /* Initial */
            for(int i=0; i < smallOrderList.Length; i++)
            {
                smallOrderList[i] = -1;
            }

            /* 昇順ソート */
            var map = tmpMap.OrderBy((x) => x.Value); /* 昇順ソート */
            foreach (var data in map)
            {
                int id = data.Key;
                for(int i = id; i >= 0; i--)
                {
                    if(smallOrderList[i] != -1)
                    {
                        break;
                    }
                    smallOrderList[i] = data.Value;
                }
                if(id == (agi.GetAgiStoneList().GetListSize() - 1))
                {
                    break;
                }
            }
#if false
            int emptySize = agi.GetAgiFields().GetEmptySize();
            List<int> stoneSizeList = new List<int>();

            /* 全石のサイズを取得 */
            for (int i = 0; i < agi.GetAgiStoneList().GetListSize(); i++)
            {
                stoneSizeList.Add(agi.GetAgiStoneList().GetAgiStoneKindsList(i).GetStoneNumSize());
            }

            stoneSizeList.Sort();   /* 昇順ソート */

            int tmpSum = emptySize;
            int counter = 0;
            while (tmpSum >= 0)
            {
                if (counter < stoneSizeList.Count)
                {
                    tmpSum -= stoneSizeList[counter];
                }
                else
                {
                    break;
                }
                counter++;
            }
            upperBound = (counter - 1);

            stoneSizeList.Reverse();    /* 降順ソート */
            tmpSum = emptySize;
            counter = 0;
            while (tmpSum >= 0)
            {
                if (counter < stoneSizeList.Count)
                {
                    tmpSum -= stoneSizeList[counter];
                }
                else
                {
                    break;
                }
                counter++;
            }
            lowerBound = (counter - 1);
#endif
        }


        private int calcDepthSize(int myFieldSize)
        {
            /* 後半にいくつれにつれて深読みをする */
            /* 問題規模が大きい場合、読みの深さは加減する */
            int retDepth = 0;
            float progress = myFieldSize / problemEmptySize;

            if (progress > 0.8)  /* 80%完了 */
            {
                retDepth = DEPTH_BIG_80;
            }
            else if (progress > 0.5)
            {
                retDepth = DEPTH_BIG_50;
            }
            else
            {
                retDepth = DEPTH_BIG_0;
            }
            

            return retDepth;
        }

        private void preparation()
        {
            //createList();
            calcUpperLowerBound();  /* init:upperBound, lowerBound */
        }

        private void createList()
        {
            Dictionary<int, int> tmpMap = new Dictionary<int, int>();
            /* mapへ登録 (ID, size) */
            for (int i = 0; i < agi.GetAgiStoneList().GetListSize(); i++)
            {
                int id = agi.GetAgiStoneList().GetAgiStoneKindsList(i).GetStoneKindsListId();
                int size = agi.GetAgiStoneList().GetAgiStoneKindsList(i).GetStoneNumSize();
                tmpMap.Add(id, size);
            }
            /* mapソート */
            var map = tmpMap.OrderBy((x) => x.Value);
            List<int> sizeOrderList = new List<int>();

            foreach (var data in map)
            {
                sizeOrderList.Add(data.Key);
            }

            /* εを決める */
            int epsId = sizeOrderList[sizeOrderList.Count / 3]; //DEBUG
            if (tmpMap.ContainsKey(epsId) == true)
            {
                epsilonSize = tmpMap[epsId];
            }
            else
            {
                epsilonSize = sizeOrderList[0];
            }

            /* 各リストへ登録 (ID, size) */
            for (int i = 0; i < agi.GetAgiStoneList().GetListSize(); i++)
            {
                int id = agi.GetAgiStoneList().GetAgiStoneKindsList(i).GetStoneKindsListId();
                int size = agi.GetAgiStoneList().GetAgiStoneKindsList(i).GetStoneNumSize();
                if (size >= epsilonSize)
                {
                    /* ε以上の場合 */
                }
                else
                {
                    /* ε未満の場合 */
                }
            }

        }

        private void step1()
        {
            /* 初手処理 */
            /* 石1～石(INIT_SEARCH_IDX)まで全探索 */

            AGI_CalcFields calcF = new AGI_CalcFields(agi.GetAgiFields(), commonMap);
            storeRootFields(calcF); /* 親の登録 */

            for (int id = 0; id < INIT_SEARCH_IDX; id++)
            {
                if(id > agi.GetAgiStoneList().GetListSize())    /* 石全体の数よりオーバーした場合 */
                {
                    break;
                }

                for (int idx = 0; idx < calcF.GetAgiCalcFieldsSize(); idx++)    /* すべてのセルで確認 */
                {
                    for (int kind = 0; kind < (int)Kinds.REV_R270; kind++)      /* すべての回転・反転で確認 */
                    {
                        if (agi.GetAgiStoneList().GetAgiStoneKindsList(id).GetAGIStones(kind) == null)    /* 重複により削除された石 */
                        {
                            continue;
                        }
                        AGI_CalcFields resultF = placement(calcF, id, kind, idx, false);   /* 配置確認 */
                        if (resultF == null)  /* 配置失敗 */
                        {
                            continue;
                        }
                        else
                        {
                            /* ROOT_ID = 0 */
                            if (storeFields(resultF, 0, id, kind, idx, calcF, EVAL_A) == -1)  /* 書き込み失敗 */
                            {
                                continue;
                            }
                        }
                    }
                }

            }

        }

        private void step2()
        {
            Dictionary<int, float> tmpMap = new Dictionary<int, float>();
            Dictionary<int, StoreData> copyStoreDic = new Dictionary<int, StoreData>();
            List<StoreData> retryList = new List<StoreData>();
            int retryCnt = 0;

            /* map作成(ID, evalue) */
            for (int i = 1; i < storeDataDic.Count; i++)
            {
                if (storeDataDic.ContainsKey(i) == true)
                {
                    tmpMap.Add(storeDataDic[i].ID, storeDataDic[i].evalue);
                }
            }

            /* mapソート */
            var map = tmpMap.OrderByDescending((x) => x.Value); /* 降順ソート */
            int[] saveSeeds = new int[INIT_SEARCH_IDX];
            for (int i = 0; i < INIT_SEARCH_IDX; i++)
            {
                saveSeeds[i] = 0;
            }


            /* 評価値が優秀な種/評価値が優秀であり石ファイルが異なる種 */
            int count = 0;
            /* INIT_CHOISE_FOR_EVAL_NUM分Queueに格納 */
            foreach (var data in map)
            {
                int id = data.Key;

                if (count < INIT_CHOISE_FOR_EVAL_NUM)
                {
                    if (searchQueId.Contains(id) != true)
                    {
                        searchQueId.Enqueue(id);
                        if (copyStoreDic.ContainsKey(id) != true)
                        {
                            copyStoreDic.Add(id, storeDataDic[id]);
                        }
                        count++;
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    /* INIT_CHOISE_FOR_EVAL_NUM分Queueに格納 */
                    if (saveSeeds[storeDataDic[id].GetFinallyStoneId()] < INIT_SEEDS_SAVE)
                    {
                        if (searchQueId.Contains(id) != true)
                        {
                            searchQueId.Enqueue(id);
                            if (copyStoreDic.ContainsKey(id) != true)
                            {
                                copyStoreDic.Add(id, storeDataDic[id]);
                            }
                            saveSeeds[storeDataDic[id].GetFinallyStoneId()]++;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        ;
                    }     
                }

                /* リトライサーチ用 */
                if (retryCnt < RETRY_COUNT_EVAL)
                {
                    StoreData retrySt = new StoreData(storeDataDic[id], commonMap);
                    retryList.Add(retrySt);
                    retryCnt++;
                }

            }

            uint max = (uint)tmpMap.Count;

            /* リトライサーチ用 */
            for (retryCnt = 0; retryCnt < RETRY_COUNT_RAND; retryCnt++)
            {
                if (max != 0)
                {
                    int idx = (int)rand.NextMax(max - 1);
                    if (tmpMap.ContainsKey(idx) == true)
                    {
                        if (retryCnt < RETRY_COUNT_EVAL)
                        {
                            if (storeDataDic.ContainsKey(idx) == true)
                            {

                                StoreData retrySt = new StoreData(storeDataDic[idx], commonMap);
                                retryList.Add(retrySt);
                                retryCnt++;
                            }
                        }
                    }
                }
            }

            RetrySearch.Enqueue(retryList);

            /* 元に戻す */
            storeDataDic.Clear();   /* クリア */
            foreach (KeyValuePair<int, StoreData> pair in copyStoreDic)
            {
                storeDataDic.Add(pair.Key, pair.Value);
            }
        }

        private void step3()
        {
            /* 2手以降処理 */
            /* 当てはめ先は自陣の近似1 or 近似2セル */
            for (;;)
            {
                if (searchQueId.Count == 0)
                {
                    break;
                }
                int maxEvaId = 0;
                int maxEval = 0;
                searchQueId.TryPeek(out maxEvaId);
                if (storeDataDic.ContainsKey(maxEvaId) == true)
                {
                    maxEval = storeDataDic[maxEvaId].score;
                }

                depthSize = calcDepthSize(maxEval);
                for (int i = 0; i < depthSize; i++)
                {
                    queueSearchDepthForParallel();
                }
                searchTimes++;
                queueSearchForParallel();
                selectionQueue();

                if (searchQueId.Count != 0)
                {
                    int priId = -1;
                    float priEval;
                    int priScore;
                    
                    searchQueId.TryPeek(out priId);
                    if (priId != -1)
                    {
                        priEval = storeDataDic[priId].evalue;
                        priScore = storeDataDic[priId].score;

                        Console.WriteLine("id={0},eval={1}, score={2}", priId, priEval, priScore);
                    }
#if false
                    if (searchQueId.TryPeek(out id) == true)
                    {
                        var key = dic.FirstOrDefault(x => x.Value.Equals(id)).Key;          /* dicのValue(ID)よりFields(Key)を取得 */
                        if (key != null)
                        {
                            AGI_CalcFields debug = new AGI_CalcFields(key);
                            //debug.PrintAgiCalcFields();
                        }
                    }
#endif
                }
            }
        }

        private int step4()
        {
            int outputId = getOutputId();

            Output op = new Output(agi);
            StoreData st = answerId[outputId];
           // st.PrintStoreData();
            for (int i = 0; i < st.GetStoneInfoSize(); i++)
            {
                op.SetStone(st.GetStoneInfoId(i), st.GetStoneInfoKind(i), st.GetStoneInfoPlace(i));
            }
            /*
            var key = dic.FirstOrDefault(x => x.Value.Equals(outputId)).Key;
            if (key != null)
            {
                debug.PrintAgiCalcFields();
            }*/
            op.OutputExec(fileName);

            return st.score;
        }

        private int step5(int maxScore, int retryCnt)
        {
            int outputId = getOutputId();

            Output op = new Output(agi);
            StoreData st = answerId[outputId];
            if (st.score > maxScore)
            {
                for (int i = 0; i < st.GetStoneInfoSize(); i++)
                {
                    op.SetStone(st.GetStoneInfoId(i), st.GetStoneInfoKind(i), st.GetStoneInfoPlace(i));
                }
                fileName = commonMap.FILENAME + retryCnt.ToString() + ".txt";
                op.OutputExec(fileName);

                return st.score;
            }
            else
            {
                return maxScore;
            }
        
        }

        /* 探索(並列) */
        private void queueSearchForParallel()
        {
            int searchSize = searchQueId.Count;
            int[] queIdArray = searchQueId.ToArray();
            int[] searchField = new int[9] { MY_FIELD, NEIGHT1, NEIGHT2, NEIGHT3, NEIGHT4, NEIGHT5, NEIGHT6, NEIGHT7, NEIGHT8 };
            int answerSize = 9999999;
            Object thisLock = new Object();

            Parallel.For(0, searchSize, i =>
            {
                /// Console.WriteLine(searchQueId.Count);
                int targetId = queIdArray[i];                       /* キューからID取得 */
                                                                    // string key;

                // key = dic.FirstOrDefault(x => x.Value.Equals(targetId)).Key;          /* dicのValue(ID)よりFields(Key)を取得 */
                // if (key != null)
                //{
                //  string strFields = key.ToString();                          /* キーをStirngへ変換 */

                if (storeDataDic.ContainsKey(targetId) == true)
                {
                    StoreData tgtSD = storeDataDic[targetId];
                    AGI_CalcFields tgtAgiCF = new AGI_CalcFields(tgtSD.GetAgiCalcFields(), commonMap);    /* stringからAGI_CalcFieldsを復元 */
                    for (int idKind = tgtSD.lastAttackStone + 1; idKind <= tgtSD.lastAttackStone + SEARCH_KIND_MAX; idKind++)  /* SEARCH_KIND_MAX先分走査 */
                    {
                        /* 石の決定 */
                        if (idKind < agi.GetAgiStoneList().GetListSize()) /* 石最大オーバー */
                        {
                            bool setFlg = false;
                            for (int kind = 0; kind < (int)Kinds.REV_R270; kind++)
                            {
                                if (agi.GetAgiStoneList().GetAgiStoneKindsList(idKind).GetAGIStones(kind) == null)    /* 重複により削除された石 */
                                {
                                    continue;
                                }

                                int maxX = agi.GetAgiStoneList().GetAgiStoneKindsList(idKind).GetAGIStones(kind).GetXSize() - 2;
                                int maxY = agi.GetAgiStoneList().GetAgiStoneKindsList(idKind).GetAGIStones(kind).GetYSize() - 2;
                                int targetSize = Math.Max(maxX, maxY);
                                if (targetSize >= 8)
                                {
                                    targetSize = 8;
                                }
                                else if (targetSize <= 0)
                                {
                                    targetSize = 0;
                                }

                                for (int idx = 0; idx < tgtAgiCF.GetAgiCalcFieldsSize(); idx++)      /* 接地セル走査 */
                                {
                                    if ((tgtAgiCF.GetAgiCalcFieldNum(idx) == MY_FIELD) ||   /* 自フィールドまたは */

                                    ((tgtAgiCF.GetAgiCalcFieldNum(idx) < 0) && (tgtAgiCF.GetAgiCalcFieldNum(idx) >= searchField[targetSize])) || /* 近傍以内または */

                                    (((tgtAgiCF.GetAgiCalcFieldNum(idx) == WALL) || (tgtAgiCF.GetAgiCalcFieldNum(idx) == OBJECT)) &&  /* 壁または障害物であり、かつ */
                                    (
                                      ((tgtAgiCF.GetAgiCalcFieldNum(idx + 1) >= searchField[targetSize]) && (tgtAgiCF.GetAgiCalcFieldNum(idx + 1) < 0)) || /* 右隣りが近傍値 */
                                      ((tgtAgiCF.GetAgiCalcFieldNum(idx + F_MAX_X) >= searchField[targetSize]) && (tgtAgiCF.GetAgiCalcFieldNum(idx + F_MAX_X) < 0)) ||/* 下隣が近傍値 */
                                      ((tgtAgiCF.GetAgiCalcFieldNum(idx + (F_MAX_X + 1)) >= searchField[targetSize]) && (tgtAgiCF.GetAgiCalcFieldNum(idx + (F_MAX_X + 1)) < 0)) /* 右下が近傍値 */
                                    )
                                    )
                                    )

                                    {
                                        AGI_CalcFields resultF = placement(tgtAgiCF, idKind, kind, idx, true);   /* 配置確認 */
                                        if (resultF == null)  /* 配置失敗 */
                                        {
                                            continue;
                                        }
                                        else
                                        {
                                            int retId = storeFieldsForParallel(resultF, tgtSD.ID, idKind, kind, idx, tgtAgiCF, EVAL_C);
                                            if (retId == -1)  /* 書き込み失敗 */
                                            {
                                                continue;
                                            }
                                            else
                                            {
                                                setFlg = true;
                                                if (resultF.GetAgiCalcEmptySize() != 0)
                                                {
                                                    nextQueId.Enqueue(retId);
                                                }
                                                else
                                                {
                                                    lock (thisLock)
                                                    {
                                                        StoreData tmp;
                                                        if (saveStoreDataDic.ContainsKey(retId) == true)
                                                        {
                                                            tmp = new StoreData(saveStoreDataDic[retId], commonMap);

                                                            if (answerId.ContainsKey(retId) == true)
                                                            {

                                                                answerId[retId] = tmp;

                                                            }
                                                            else
                                                            {
                                                                answerId.Add(retId, tmp);
                                                            }

                                                        }
                                                        else
                                                        {
                                                            nextQueId.Enqueue(retId);
                                                        }
                                                    }
                                                }

                                            }
                                        }

                                    }

                                }
                            }

                            if (setFlg == false)
                            {
                                int retId = storeFieldsForParallelLast(tgtSD, idKind);
                                nextQueId.Enqueue(retId);
                            }
                        }
                        else
                        {
                            lock (thisLock)
                            {
                                if (answerSize < tgtSD.score)
                                {
                                    if (answerId.ContainsKey(targetId) == false)
                                    {
                                        answerId.Add(targetId, tgtSD);
                                    }
                                    answerSize = tgtSD.score;
                                }
                            }
                        }
                    }
                }
                else
                {
                    ;
                }

            });

            foreach (var key in saveStoreDataDic.Keys)
            {
                if (storeDataDic.ContainsKey(key) == true)
                {
                    if (saveStoreDataDic.ContainsKey(key) == true)
                    {
                        storeDataDic[key] = saveStoreDataDic[key];
                    }
                }
                else
                {
                    if (saveStoreDataDic.ContainsKey(key) == true)
                    {
                        storeDataDic.Add(key, saveStoreDataDic[key]);
                    }
                }
            }

            saveStoreDataDic.Clear();
            searchQueId = new ConcurrentQueue<int>();

        }

        /* 深度探索(並列) */
        private void queueSearchDepthForParallel()
        {
            int searchSize = searchQueId.Count;
            int[] queIdArray = searchQueId.ToArray();
            int[] searchField = new int[9] { MY_FIELD, NEIGHT1, NEIGHT2, NEIGHT3, NEIGHT4, NEIGHT5, NEIGHT6, NEIGHT7, NEIGHT8 };
            int answerSize = 0;
            Object thisLock = new Object();

            Parallel.For(0, searchSize, i =>
            {
                /// Console.WriteLine(searchQueId.Count);
                int targetId = queIdArray[i];                       /* キューからID取得 */
                                                                    // string key;

                // key = dic.FirstOrDefault(x => x.Value.Equals(targetId)).Key;          /* dicのValue(ID)よりFields(Key)を取得 */
                // if (key != null)
                // {
                //     string strFields = key.ToString();                          /* キーをStirngへ変換 */

                if (storeDataDic.ContainsKey(targetId) == true)
                {
                    StoreData tgtSD = storeDataDic[targetId];
                    AGI_CalcFields tgtAgiCF = new AGI_CalcFields(tgtSD.GetAgiCalcFields(), commonMap);    /* stringからAGI_CalcFieldsを復元 */

                    for (int idKind = (tgtSD.lastAttackStone + 1); idKind <= tgtSD.lastAttackStone + SEARCH_KIND_MAX; idKind++)  /* SEARCH_KIND_MAX先分走査 */
                    {
                        /* 石の決定 */
                        if (idKind < agi.GetAgiStoneList().GetListSize()) /* 石最大オーバー */
                        {


                            bool setFlg = false;
                            for (int kind = 0; kind < (int)Kinds.REV_R270; kind++)
                            {
                                if (agi.GetAgiStoneList().GetAgiStoneKindsList(idKind).GetAGIStones(kind) == null)    /* 重複により削除された石 */
                                {
                                    continue;
                                }

                                int maxX = agi.GetAgiStoneList().GetAgiStoneKindsList(idKind).GetAGIStones(kind).GetXSize() - 2;
                                int maxY = agi.GetAgiStoneList().GetAgiStoneKindsList(idKind).GetAGIStones(kind).GetYSize() - 2;
                                int targetSize = Math.Max(maxX, maxY);
                                if (targetSize >= 8)
                                {
                                    targetSize = 8;
                                }
                                else if (targetSize <= 0)
                                {
                                    targetSize = 0;
                                }

                                for (int idx = 0; idx < tgtAgiCF.GetAgiCalcFieldsSize(); idx++)      /* 接地セル走査 */
                                {
                                    if ((tgtAgiCF.GetAgiCalcFieldNum(idx) == MY_FIELD) ||   /* 自フィールドまたは */

                                    ((tgtAgiCF.GetAgiCalcFieldNum(idx) < 0) && (tgtAgiCF.GetAgiCalcFieldNum(idx) >= searchField[targetSize])) || /* 近傍以内または */

                                    (((tgtAgiCF.GetAgiCalcFieldNum(idx) == WALL) || (tgtAgiCF.GetAgiCalcFieldNum(idx) == OBJECT)) &&  /* 壁または障害物であり、かつ */
                                    (
                                      ((tgtAgiCF.GetAgiCalcFieldNum(idx + 1) >= searchField[targetSize]) && (tgtAgiCF.GetAgiCalcFieldNum(idx + 1) < 0)) || /* 右隣りが近傍値 */
                                      ((tgtAgiCF.GetAgiCalcFieldNum(idx + F_MAX_X) >= searchField[targetSize]) && (tgtAgiCF.GetAgiCalcFieldNum(idx + F_MAX_X) < 0)) ||/* 下隣が近傍値 */
                                      ((tgtAgiCF.GetAgiCalcFieldNum(idx + (F_MAX_X + 1)) >= searchField[targetSize]) && (tgtAgiCF.GetAgiCalcFieldNum(idx + (F_MAX_X + 1)) < 0)) /* 右下が近傍値 */
                                    )
                                    )
                                    )
                                    {
                                        AGI_CalcFields resultF = placement(tgtAgiCF, idKind, kind, idx, true);   /* 配置確認 */
                                        if (resultF == null)  /* 配置失敗 */
                                        {
                                            continue;
                                        }
                                        else
                                        {
                                            int retId = storeFieldsForParallel(resultF, tgtSD.ID, idKind, kind, idx, tgtAgiCF, EVAL_C);
                                            if (retId == -1)  /* 書き込み失敗 */
                                            {
                                                continue;
                                            }
                                            else
                                            {
                                                setFlg = true;

                                                if (resultF.GetAgiCalcEmptySize() != 0)
                                                {

                                                    float eval = resultF.GetAgiCalcEvalDepth(tgtAgiCF, smallOrderList[idKind]);
                                                    if (eval >= resultF.GetAgiCalcEvalDepthLimit())
                                                    {
                                                        searchQueId.Enqueue(retId);
                                                    }
                                                }
                                                else
                                                {
                                                    lock (thisLock)
                                                    {
                                                        StoreData tmp;
                                                        if (saveStoreDataDic.ContainsKey(retId) == true)
                                                        {
                                                            tmp = new StoreData(saveStoreDataDic[retId], commonMap);

                                                            if (answerId.ContainsKey(retId) == true)
                                                            {

                                                                answerId[retId] = tmp;

                                                            }
                                                            else
                                                            {
                                                                answerId.Add(retId, tmp);
                                                            }
                                                            
                                                        }
                                                        else
                                                        {
                                                            searchQueId.Enqueue(retId);
                                                        }
                                                    }

                                                }


                                            }
                                        }

                                    }

                                }
                            }

                            if (setFlg == false)
                            {
                                int retId = storeFieldsForParallelLast(tgtSD, idKind);
                                searchQueId.Enqueue(retId);
                            }
                        }
                        else
                        {
                            lock (thisLock)
                            {
                                if (answerSize < tgtSD.score)
                                {
                                    if (answerId.ContainsKey(targetId) == false)
                                    {
                                        answerId.Add(targetId, tgtSD);
                                    }
                                    answerSize = tgtSD.score;
                                }
                            }
                        }
                    }
                }
                else
                {
                    ;
                }
            });


            foreach (var key in saveStoreDataDic.Keys)
            {
                if (storeDataDic.ContainsKey(key) == true)
                {
                    if (saveStoreDataDic.ContainsKey(key) == true)
                    {
                        storeDataDic[key] = saveStoreDataDic[key];
                    }
                }
                else
                {
                    if (saveStoreDataDic.ContainsKey(key) == true)
                    {
                        storeDataDic.Add(key, saveStoreDataDic[key]);
                    }
                }
            }

            saveStoreDataDic.Clear();

        }

        /* StoreDataへ格納(並列) */
        private int storeFieldsForParallel(AGI_CalcFields agicF, int parentId, int stoneId, int kinds, int placeIdx, AGI_CalcFields beforeAgiF, int evalKind)
        {
            int retId = -1;

            /* 各種登録 */
            /* ID取得 */
            StoreData regist = new StoreData(true, agicF, commonMap);
            /* store登録 */
            /* stoneの登録(親のstoneを受け継ぐ) */
            if (storeDataDic.ContainsKey(parentId) == true)
            {
                for (int i = 0; i < storeDataDic[parentId].GetStoneInfoSize(); i++)
                {
                    regist.SetStoneInfo(storeDataDic[parentId].GetStoneInfoId(i),
                        storeDataDic[parentId].GetStoneInfoPlace(i), storeDataDic[parentId].GetStoneInfoKind(i));
                }
                int passStoneSize = stoneId - regist.GetFinallyStoneId();
                regist.SetStoneInfo(stoneId, placeIdx, kinds);
                regist.score = calcScore(agicF);
                switch (evalKind)
                {
                    case EVAL_A:
                        /* サイズが大きいほど有利 */
                        regist.evalue = calcEvalInitial(agicF, beforeAgiF, smallOrderList[stoneId]);
                        break;
                    case EVAL_B:
                        regist.evalue = calcEvalDepth(agicF, beforeAgiF, smallOrderList[stoneId]);
                        break;
                    case EVAL_C:
                        regist.evalue = calcEvalSizeKai(agicF, beforeAgiF, regist.score, passStoneSize, smallOrderList[stoneId]);
                        break;
                    default:
                        regist.evalue = calcEvalSizeKai(agicF, beforeAgiF, regist.score, passStoneSize, smallOrderList[stoneId]);
                        break;
                }

                for (int i = 0; i < storeDataDic[parentId].GetEvalueListSize(); i++)
                {
                    regist.SetEvalueList(storeDataDic[parentId].GetEvalueInfo(i));
                }

                regist.SetEvalueList(regist.evalue);

                regist.lastAttackStone = stoneId;

                Object thisLock = new Object();

                lock (thisLock)
                {
                    if (saveStoreDataDic.ContainsKey(regist.ID) != true)
                    {
                        try
                        {
                            saveStoreDataDic.Add(regist.ID, regist);
                        }
                        catch (System.ArgumentException)
                        {
                            saveStoreDataDic[regist.ID] = regist;
                        }
                    }
                    else
                    {
                        saveStoreDataDic[regist.ID] = regist;
                    }
                }
                retId = regist.ID;
            }
            return retId;
        }

        /* StoreDataへ格納(並列) */
        private int storeFieldsForParallelLast(StoreData sdt, int lastAttack)
        {
            int retId = -1;

            /* 各種登録 */
            /* ID取得 */
            StoreData regist = new StoreData(true, sdt.GetAgiCalcFields(), commonMap);
            /* store登録 */

            /* stoneの登録(親のstoneを受け継ぐ) */
            for (int i = 0; i < sdt.GetStoneInfoSize(); i++)
            {
                regist.SetStoneInfo(sdt.GetStoneInfoId(i),
                    sdt.GetStoneInfoPlace(i), sdt.GetStoneInfoKind(i));
            }

            regist.score = sdt.score;

            for (int i = 0; i < sdt.GetEvalueListSize(); i++)
            {
                regist.SetEvalueList(sdt.GetEvalueInfo(i));
            }

            regist.lastAttackStone = lastAttack;

            Object thisLock = new Object();

            lock (thisLock)
            {
                if (saveStoreDataDic.ContainsKey(regist.ID) != true)
                {
                    saveStoreDataDic[regist.ID] = regist;
                }
            }
            retId = regist.ID;
            return retId;
        }

        /* StoreDataへ格納 */
        private int storeFields(AGI_CalcFields agicF, int parentId, int stoneId, int kinds, int placeIdx, AGI_CalcFields beforeAgiF, int evalKind)
        {
            int retId = -1;

            /* 各種登録 */

            /* ID取得 */
            StoreData regist = new StoreData(true, agicF, commonMap);
            /* stoneの登録(親のstoneを受け継ぐ) */
            for (int i = 0; i < storeDataDic[parentId].GetStoneInfoSize(); i++)
            {
                regist.SetStoneInfo(storeDataDic[parentId].GetStoneInfoId(i),
                    storeDataDic[parentId].GetStoneInfoPlace(i), storeDataDic[parentId].GetStoneInfoKind(i));
            }
            int passSize = regist.GetFinallyStoneId() - stoneId;
            regist.SetStoneInfo(stoneId, placeIdx, kinds);
            regist.score = calcScore(agicF);
            if (evalKind == EVAL_A)
            {  /* サイズが大きいほど有利 */
                regist.evalue = calcEvalInitial(agicF, beforeAgiF, smallOrderList[stoneId]);
            }
            else if (evalKind == EVAL_B)
            {
                regist.evalue = calcEvalDepth(agicF, beforeAgiF, smallOrderList[stoneId]);
            }
            else
            {
                regist.evalue = calcEvalSizeKai(agicF, beforeAgiF, regist.score, passSize, smallOrderList[stoneId]);
            }
            regist.closed = agicF.closed;
            storeDataDic.Add(regist.ID, regist);
            regist.lastAttackStone = stoneId;
            retId = regist.ID;
            return retId;
        }

        /* 問題文をディクショナリに格納 */
        private bool storeRootFields(AGI_CalcFields agicF)
        {
            bool result = true;

            /* 各種登録 */
            /* ID取得 */
            StoreData regist = new StoreData(false, agicF, commonMap);
            regist.ID = 0;  /* ROOTのID */
            /* store登録 */
            regist.evalue = 0.0F;
            regist.score = calcScore(agicF);
            storeDataDic.Add(regist.ID, regist);

            return result;
        }

        /* サイズを含めた評価関数(EVAL_A) */
        private float calcEvalInitial(AGI_CalcFields agicF, AGI_CalcFields beforeAgicF, int closedDepthSize)
        {
            float eval = 0;

            eval = agicF.GetAgiCalcEvalInitial(beforeAgicF, closedDepthSize);

            return eval;

        }

        /* サイズに影響しない評価関数(EVAL_B) */
        private float calcEvalDepth(AGI_CalcFields agicF, AGI_CalcFields beforeAgicF, int closedDepthSize)
        {
            float eval = 0;

            eval = agicF.GetAgiCalcEvalDepth(beforeAgicF, closedDepthSize);

            return eval;

        }

        /* サイズを含めた評価関数評価関数(EVAL_C) */
        private float calcEvalSizeKai(AGI_CalcFields agicF, AGI_CalcFields beforeAgicF, int score, int passSize, int closedDepthSize)
        {
            float eval = 0;

            eval = agicF.GetAgiCalcEvalSizeKai(beforeAgicF, score, passSize, closedDepthSize);

            return eval;

        }

        /* スコアの計算 */
        private int calcScore(AGI_CalcFields agicF)
        {
            int score = 0;

            score = agicF.GetAgiCalcScore();

            return score;
        }

        /* 石の配置 NG:null */
        private AGI_CalcFields placement(AGI_CalcFields agicOrgF, int stoneId, int kinds, int placeIdx, bool connectCheck)
        {
            AGI_CalcFields retAgicF = new AGI_CalcFields(agicOrgF, commonMap);

            AGI_Stones setStone = new AGI_Stones(agi.GetAgiStoneList().GetAgiStoneKindsList(stoneId).GetAGIStones(kinds));
            /* 石の加算処理 */
            for (int i = 0; i < setStone.GetSize(); i++)
            {
                int idx = agiST2agiF(retAgicF.GetMaxX(), retAgicF.GetMaxY(), setStone.GetXSize(), setStone.GetYSize(), placeIdx, i);
                retAgicF.SetAgiCalcNum
                    (idx,
                    retAgicF.GetAgiCalcFieldNum(idx) + setStone.GetNum(i));
            }
            /* 判定 */
            bool connectFlg = false;    /* 自陣と接続するか判定 */

            for (int i = 0; i < retAgicF.GetAgiCalcFieldsSize(); i++)
            {
                int tmp = retAgicF.GetAgiCalcFieldNum(i);
                if ((tmp > 0) && ((tmp % 2) == 0))
                {
                    /* 正の偶数値があった場合、自・壁・障のいずれかと重なる */
                    return null;
                }
                if ((connectFlg == false) && (tmp == -6)) /* 近傍(-7) + 自陣(1) */
                {
                    connectFlg = true;
                }
            }

            if ((connectCheck == true) && (connectFlg != true))
            {
                return null;
            }

            /* 加算前(石設置済み)に戻す */
            retAgicF.GetReturnCalcFld();

            return retAgicF;
        }

        /* 座標変換 AGI_Stone ⇒ AGI_Fields */
        private int agiST2agiF(int fMaxX, int fMaxY, int sMaxX, int sMaxY, int agiAgLUIdx, int agiStIdx)
        {
            int retFIdx = 0;

            int agLUX = agiAgLUIdx % fMaxX; /* 頂点(F)からX座標(F)を得る */
            int agLUY = agiAgLUIdx / fMaxX; /* 頂点(F)からY座標(F)を得る */

            int stX = agiStIdx % sMaxX;     /* 変換IDX(S)からX座標(S)を得る */
            int stY = agiStIdx / sMaxX;     /* 変換IDX(S)からY座標(S)を得る */

            int ag_stX = stX + agLUX;       /* 頂点(F)+X座標(S)から変換X座標(F)を得る */
            int ag_stY = stY + agLUY;       /* 頂点(F)+Y座標(S)から変換Y座標(F)を得る */

            retFIdx = ag_stY * fMaxX + ag_stX;  /* X座標(F)とY座標(F)からIDX(F)を得る */

            return retFIdx;
        }

        public void PrintStoreDataAll()
        {
            Console.WriteLine("");
            Console.WriteLine("ID,配置回数,親ID,石ID,接地Index,向き,評価値,スコア");
            for (int i = 0; i < storeDataDic.Count; i++)
            {
                Console.WriteLine("{0},{1},{2}",
                    storeDataDic[i].ID.ToString(),
                    storeDataDic[i].evalue.ToString(),
                    storeDataDic[i].score.ToString());
            }
        }

        private string exchangeFieldsToString(AGI_CalcFields agicF)
        {
            string strRet = "";

            for (int i = 0; i < agicF.GetAgiCalcFieldsSize(); i++)
            {
                strRet += agicF.GetAgiCalcFieldNum(i).ToString();
            }

            return strRet;
        }

        private void selectionQueue()
        {
            Dictionary<int, float> tmpMap = new Dictionary<int, float>();
            List<int> tmpList = new List<int>();
            List<StoreData> retryList = new List<StoreData>();

            /* マップ作成 */
            int countId = nextQueId.Count;

            for (int i = 0; i < countId; i++)
            {
                int id;
                if (nextQueId.TryDequeue(out id) == false)
                {
                    continue;
                }
                if (storeDataDic.ContainsKey(id) == true)
                {
                    float eval = storeDataDic[id].GetEvalueListSum();
                    tmpList.Add(id);
                    if (tmpMap.ContainsKey(id) != true)
                    {
                        tmpMap.Add(id, eval);
                    }
                }
                else if (saveStoreDataDic.ContainsKey(id) == true)
                {
                    float eval = storeDataDic[id].GetEvalueListSum();
                    tmpList.Add(id);
                    if (tmpMap.ContainsKey(id) != true)
                    {
                        tmpMap.Add(id, eval);
                    }
                }
                else
                {
                    ;
                }

            }

            /* ソート */
            /* mapソート */
            var map = tmpMap.OrderByDescending((x) => x.Value); /* 降順ソート */

            /* 評価値が優秀な種 */
            int count = 0;
            int retryCnt = 0;
            float preEvalue = 0.0F; /* 前回の評価値 */

            Dictionary<int, StoreData> copyStoreDic = new Dictionary<int, StoreData>();

            /* CHOISE_FOR_EVAL_NUM分Queueに格納 */
            foreach (var data in map)
            {
                int id = data.Key;
                if (searchQueId.Contains(id) != true)
                {
                    if (count < CHOISE_FOR_EVAL_NUM)
                    {
                        searchQueId.Enqueue(id);
                        /* copy */
                        StoreData st = new StoreData(storeDataDic[id], commonMap);
                        if (preEvalue != st.evalue)
                        {
                            copyStoreDic.Add(id, st);
                            /* copyEnd */
                            count++;
                            preEvalue = st.evalue;
                        }
                    }
                    else
                    {
                        ;
                    }

                    /* リトライサーチ用 */
                    if (retryCnt < RETRY_COUNT_EVAL)
                    {
                        StoreData retrySt = new StoreData(storeDataDic[id], commonMap);
                        retryList.Add(retrySt);
                        retryCnt++;
                    }
                }
                if(count >= CHOISE_FOR_EVAL_NUM && retryCnt >= RETRY_COUNT_EVAL)
                {
                    break;
                }
            }

            /* 焼きなまし？分 */
            int maxScoreId;
            int Temperature;
            int maxScore;

            if(searchQueId.TryPeek(out maxScoreId)== true)
            {
                if (copyStoreDic.ContainsKey(maxScoreId) == true) {
                    maxScore = copyStoreDic[maxScoreId].score;
                }
                else
                {
                    maxScore = 0;
                }

                Temperature = (int)(((float) maxScore / (float)agi.GetAgiFields().GetEmptySize()) * 10);
            }
            else
            {
                Temperature = 0;
            }

            Array tmpAry = tmpMap.ToArray();
            uint max = (uint)tmpList.Count;

            if (Temperature < AnnealingRate.Length && max != 0) {
                for (int i = 0; i < AnnealingRate[Temperature]; i++)
                {
                    int idx = (int)rand.NextMax(max - 1);
                    int id = tmpList[idx];

                    if (searchQueId.Contains(id) != true)
                    {

                         searchQueId.Enqueue(id);
                         /* copy */
                         StoreData st = new StoreData(storeDataDic[id], commonMap);
                        if (preEvalue != st.evalue)
                        {
                            copyStoreDic.Add(id, st);
                            /* copyEnd */

                            preEvalue = st.evalue;
                        }

                    }
                }
            }

            /* リトライサーチ用 */
            for (retryCnt = 0; retryCnt < RETRY_COUNT_RAND; retryCnt++)
            {
                if (max != 0)
                {
                    int idx = (int)rand.NextMax(max - 1);
                    int id = tmpList[idx];
                    if (searchQueId.Contains(id) != true)
                    {
                        if (retryCnt < RETRY_COUNT_EVAL)
                        {
                            StoreData retrySt = new StoreData(storeDataDic[id], commonMap);
                            retryList.Add(retrySt);
                            retryCnt++;
                        }
                    }
                }
            }

            /* 元に戻す */
            storeDataDic.Clear();   /* クリア */
            tmpList.Clear();
            foreach (KeyValuePair<int, StoreData> pair in copyStoreDic)
            {
                storeDataDic.Add(pair.Key, pair.Value);
            }
            RetrySearch.Enqueue(retryList);
        }

        private int getOutputId()
        {
            /* スコアの一番高い結果を出力 */
            Dictionary<int, int> tmpMap = new Dictionary<int, int>();

            /* マップ作成 */

            foreach (int i in answerId.Keys)
            {
                if (answerId.ContainsKey(i) == true)
                {
                    if (answerId[i] != null)
                    {
                        int id = answerId[i].ID;
                        int score = answerId[i].score;

                        if (tmpMap.ContainsKey(id) == true)
                        {
                            if (tmpMap[id] < score)
                            {
                                tmpMap[id] = score;
                            }
                            else
                            {
                                ;
                            }
                        }
                        else
                        {
                            tmpMap.Add(id, score);
                        }
                    }
                }
            }

            /* ソート */
            /* mapソート */
            var map = tmpMap.OrderByDescending((x) => x.Value); /* 降順ソート */
            int outputId = -1;
            /* 一番Scoreの高い結果を出力　*/
            foreach (var data in map)
            {
                outputId = data.Key;
                break;
            }

            return outputId;
        }

        private string makeHashMd5(string str)
        {
            byte[] data = System.Text.Encoding.UTF8.GetBytes(str);
            System.Security.Cryptography.MD5CryptoServiceProvider md5 =
                 new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bs = md5.ComputeHash(data);
            //byte型配列を16進数の文字列に変換
            string result = BitConverter.ToString(bs).ToLower().Replace("-", "");

            return result;
        }

        private void initialData()
        {
            searchQueId = new ConcurrentQueue<int>();
            nextQueId = new ConcurrentQueue<int>();
            storeDataDic.Clear();
            answerId.Clear();
            saveStoreDataDic.Clear();

        }

        private void retryDataSet()
        {
            List<StoreData> retryData = new List<StoreData>();
            retryData = RetrySearch.Dequeue();
            for (int i=0; i < retryData.Count; i++)
            {
                searchQueId.Enqueue(retryData[i].ID);
                StoreData st = new StoreData(retryData[i], commonMap);
                if (storeDataDic.ContainsKey(st.ID) != true)
                {
                    storeDataDic.Add(st.ID, st);
                }
            }
        }

        private void setRetryConstData(COMMON_MAP map)
        {
            SEARCH_KIND_MAX = map.SEARCH_KIND_MAX;              /* 一つに対しx手分調査 */
            CHOISE_FOR_EVAL_NUM = map.CHOISE_FOR_EVAL_NUM;        /* 優秀種残し数 */

            DEPTH_BIG_80 = map.RETRY_DEPTH_BIG_80;
            DEPTH_BIG_50 = map.RETRY_DEPTH_BIG_50;
            DEPTH_BIG_0 = map.RETRY_DEPTH_BIG_0;

            DEPTH_MID_80 = map.RETRY_DEPTH_MID_80;
            DEPTH_MID_50 = map.RETRY_DEPTH_MID_50;
            DEPTH_MID_0 = map.RETRY_DEPTH_MID_0;

            DEPTH_MIN_80 = map.RETRY_DEPTH_MIN_80;
            DEPTH_MIN_50 = map.RETRY_DEPTH_MIN_50;
            DEPTH_MIN_0 = map.RETRY_DEPTH_MIN_0;

            ANNEALING_RATE_0 = map.RETRY_ANNEALING_RATE_0;
            ANNEALING_RATE_10 = map.RETRY_ANNEALING_RATE_10;
            ANNEALING_RATE_20 = map.RETRY_ANNEALING_RATE_20;
            ANNEALING_RATE_30 = map.RETRY_ANNEALING_RATE_30;
            ANNEALING_RATE_40 = map.RETRY_ANNEALING_RATE_40;
            ANNEALING_RATE_50 = map.RETRY_ANNEALING_RATE_50;
            ANNEALING_RATE_60 = map.RETRY_ANNEALING_RATE_60;
            ANNEALING_RATE_70 = map.RETRY_ANNEALING_RATE_70;
            ANNEALING_RATE_80 = map.RETRY_ANNEALING_RATE_80;
            ANNEALING_RATE_90 = map.RETRY_ANNEALING_RATE_90;
            ANNEALING_RATE_100 = map.RETRY_ANNEALING_RATE_100;

            map.CONNECT_MY_FIELD = map.RETRY_MY_FIELDS_RATE;
            map.CONNECT_WALL = map.RETRY_WALLS_RATE;
            map.CONNECT_OBJECT = map.RETRY_OBJECTS_RATE;
            map.RATE_CONNECT = map.RETRY_CONNECT_RATE;
            map.RATE_SCORE = map.RETRY_SCORE_RATE;

            map.DEPTH_MAX = map.RETRY_DEPTH_MAX;
            map.PASS_STONE_RATE = map.RETRY_PASS_STONE_RATE;
            map.FAILES_FIELD_RATE = map.RETRY_FAILES_FIELD_RATE;
            map.CLOSED_FIELD_RATE = map.RETRY_CLOSED_FIELD_RATE;

        }
    }
}
