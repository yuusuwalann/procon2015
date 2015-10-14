using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Procon2015_2
{
    class COMMON_MAP
    {
        /* Program.cs */
        public string CFG_FILE
        {
            get;
        }
        public string PROBLEM_FILE
        {
            get;
        }

        public string FILENAME
        {
            get;
        }

        /* Search.cs */
        public int INIT_SEARCH_IDX
        {
            get;
        }
        public int INIT_CHOISE_FOR_EVAL_NUM
        {
            get;
        }
        public int INIT_SEEDS_SAVE
        {
            get;
        }

        public int SEARCH_KIND_MAX
        {
            get;
        }
        public int CHOISE_FOR_EVAL_NUM
        {
            get;
        }

        public int DEPTH_BIG_80
        {
            get;
        }
        public int DEPTH_BIG_50
        {
            get;
        }
        public int DEPTH_BIG_0
        {
            get;
        }

        public int DEPTH_MID_80
        {
            get;
        }
        public int DEPTH_MID_50
        {
            get;
        }
        public int DEPTH_MID_0
        {
            get;
        }

        public int DEPTH_MIN_80
        {
            get;
        }
        public int DEPTH_MIN_50
        {
            get;
        }
        public int DEPTH_MIN_0
        {
            get;
        }

        public int ANNEALING_RATE_0
        {
            get;
        }
        public int ANNEALING_RATE_10
        {
            get;
        }
        public int ANNEALING_RATE_20
        {
            get;
        }
        public int ANNEALING_RATE_30
        {
            get;
        }
        public int ANNEALING_RATE_40
        {
            get;
        }
        public int ANNEALING_RATE_50
        {
            get;
        }
        public int ANNEALING_RATE_60
        {
            get;
        }
        public int ANNEALING_RATE_70
        {
            get;
        }
        public int ANNEALING_RATE_80
        {
            get;
        }
        public int ANNEALING_RATE_90
        {
            get;
        }
        public int ANNEALING_RATE_100
        {
            get;
        }

        /* AGI_CalcFields.cs */
        public int SCORE_MAX
        {
            get; set;
        }    /* 3辺以上接地 */
        public int SCORE_MID
        {
            get; set;
        }    /* 2辺接地 */
        public int SCORE_MIN
        {
            get; set;
        }    /* 1辺接地 */
        public int NON_SCORE
        {
            get; set;
        }    /* 0辺接地 */

        public int CONNECT_MY_FIELD
        {
            get; set;
        } /* 自陣と接触 */
        public int CONNECT_WALL
        {
            get; set;
        }     /* 壁と接触 */
        public int CONNECT_OBJECT
        {
            get; set;
        }   /* 障害物と接触 */

        public int RATE_CONNECT
        {
            get; set;
        }
        public int RATE_SCORE
        {
            get; set;
        }

        public int DEPTH_MAX
        {
            get; set;
        }
        public int PASS_STONE_RATE
        {
            get; set;
        }
        public int FAILES_FIELD_RATE
        {
            get; set;
        }
        public int CLOSED_FIELD_RATE
        {
            get; set;
        }

        public int RETRY_COUNT_EVAL
        {
            get; set;
        }

        public int RETRY_COUNT_RAND
        {
            get; set;
        }

        public int RETRY_SEARCH_KIND_MAX
        {
            get; set;
        }
        public int RETRY_CHOISE_FOR_EVAL_NUM
        {
            get; set;
        }
        public int RETRY_DEPTH_BIG_80
        {
            get; set;
        }
        public int RETRY_DEPTH_BIG_50
        {
            get; set;
        }
        public int RETRY_DEPTH_BIG_0
        {
            get; set;
        }
        public int RETRY_DEPTH_MID_80
        {
            get; set;
        }
        public int RETRY_DEPTH_MID_50
        {
            get; set;
        }
        public int RETRY_DEPTH_MID_0
        {
            get; set;
        }
        public int RETRY_DEPTH_MIN_80
        {
            get; set;
        }
        public int RETRY_DEPTH_MIN_50
        {
            get; set;
        }
        public int RETRY_DEPTH_MIN_0
        {
            get; set;
        }
        public int RETRY_ANNEALING_RATE_0
        {
            get; set;
        }
        public int RETRY_ANNEALING_RATE_10
        {
            get; set;
        }
        public int RETRY_ANNEALING_RATE_20
        {
            get; set;
        }
        public int RETRY_ANNEALING_RATE_30
        {
            get; set;
        }
        public int RETRY_ANNEALING_RATE_40
        {
            get; set;
        }
        public int RETRY_ANNEALING_RATE_50
        {
            get; set;
        }
        public int RETRY_ANNEALING_RATE_60
        {
            get; set;
        }
        public int RETRY_ANNEALING_RATE_70
        {
            get; set;
        }
        public int RETRY_ANNEALING_RATE_80
        {
            get; set;
        }
        public int RETRY_ANNEALING_RATE_90
        {
            get; set;
        }
        public int RETRY_ANNEALING_RATE_100
        {
            get; set;
        }

        public int RETRY_MY_FIELDS_RATE
        {
            get; set;
        }

        public int RETRY_WALLS_RATE
        {
            get; set;
        }
        public int RETRY_OBJECTS_RATE
        {
            get; set;
        }
        public int RETRY_CONNECT_RATE
        {
            get; set;
        }
        public int RETRY_SCORE_RATE
        {
            get; set;
        }

        public int RETRY_DEPTH_MAX
        {
            get; set;
        }
        public int RETRY_PASS_STONE_RATE
        {
            get; set;
        }
        public int RETRY_FAILES_FIELD_RATE
        {
            get; set;
        }
        public int RETRY_CLOSED_FIELD_RATE
        {
            get; set;
        }

        public COMMON_MAP()
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

           SCORE_MAX = 5;    /* 3辺以上接地 */
           SCORE_MID = 2;    /* 2辺接地 */
           SCORE_MIN = 1;    /* 1辺接地 */
           NON_SCORE = 0;    /* 0辺接地 */

           CONNECT_MY_FIELD = 3; /* 自陣と接触 */
           CONNECT_WALL = 2;     /* 壁と接触 */
           CONNECT_OBJECT = 1;   /* 障害物と接触 */

            RATE_CONNECT = 80;
            RATE_SCORE = 20;

            DEPTH_MAX = 3;
            PASS_STONE_RATE = 5;
            FAILES_FIELD_RATE = 10;
            CLOSED_FIELD_RATE = 3;

            RETRY_COUNT_EVAL = 20;
            RETRY_COUNT_RAND = 5;

            RETRY_SEARCH_KIND_MAX = 3;
            RETRY_CHOISE_FOR_EVAL_NUM = 40;
            RETRY_DEPTH_BIG_80 = 4;
            RETRY_DEPTH_BIG_50 = 3;
            RETRY_DEPTH_BIG_0 = 2;
            RETRY_DEPTH_MID_80 = 4;
            RETRY_DEPTH_MID_50 = 3;
            RETRY_DEPTH_MID_0 = 2;
            RETRY_DEPTH_MIN_80 = 4;
            RETRY_DEPTH_MIN_50 = 3;
            RETRY_DEPTH_MIN_0 = 2;
            RETRY_ANNEALING_RATE_0 = 60;
            RETRY_ANNEALING_RATE_10 = 60;
            RETRY_ANNEALING_RATE_20 = 50;
            RETRY_ANNEALING_RATE_30 = 50;
            RETRY_ANNEALING_RATE_40 = 40;
            RETRY_ANNEALING_RATE_50 = 40;
            RETRY_ANNEALING_RATE_60 = 30;
            RETRY_ANNEALING_RATE_70 = 30;
            RETRY_ANNEALING_RATE_80 = 20;
            RETRY_ANNEALING_RATE_90 = 20;
            RETRY_ANNEALING_RATE_100 = 10;
            RETRY_MY_FIELDS_RATE = 1;
            RETRY_WALLS_RATE = 2;
            RETRY_OBJECTS_RATE = 3;
            RETRY_CONNECT_RATE = 95;
            RETRY_SCORE_RATE = 5;

            RETRY_DEPTH_MAX = 1;
            RETRY_PASS_STONE_RATE = 5;
            RETRY_FAILES_FIELD_RATE = 5;
            RETRY_CLOSED_FIELD_RATE = 6;

            Console.WriteLine("MAP_FILE_OPEN_FAILE");
        }

        public COMMON_MAP(string fileName)
        {
            string line = "";
            System.IO.StreamReader file = new System.IO.StreamReader(fileName);

            line = file.ReadLine(); /* [CFG_FILE] */
            CFG_FILE = file.ReadLine();

            line = file.ReadLine(); /* [Problem] */
            PROBLEM_FILE = file.ReadLine();

            line = file.ReadLine(); /* [FileName] */
            FILENAME = file.ReadLine();

            line = file.ReadLine(); /* [初手最大探索インデックス] */
            INIT_SEARCH_IDX = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [初手保持数] */
            INIT_CHOISE_FOR_EVAL_NUM = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [初手種保存数] */
            INIT_SEEDS_SAVE = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [通常パス可能数] */
            SEARCH_KIND_MAX = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [通常種保存数] */
            CHOISE_FOR_EVAL_NUM = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [深さ(BIG)80%] */
            DEPTH_BIG_80 = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [深さ(BIG)50%] */
            DEPTH_BIG_50 = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [深さ(BIG)以外] */
            DEPTH_BIG_0 = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [深さ(MID)80%] */
            DEPTH_MID_80 = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [深さ(MID)50%] */
            DEPTH_MID_50 = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [深さ(MID)以外] */
            DEPTH_MID_0 = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [深さ(MIN)80%] */
            DEPTH_MIN_80 = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [深さ(MIN)50%] */
            DEPTH_MIN_50 = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [深さ(MIN)以外] */
            DEPTH_MIN_0 = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [焼きなまし0%] */
            ANNEALING_RATE_0 = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [焼きなまし10%] */
            ANNEALING_RATE_10 = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [焼きなまし20%] */
            ANNEALING_RATE_20 = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [焼きなまし30%] */
            ANNEALING_RATE_30 = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [焼きなまし40%] */
            ANNEALING_RATE_40 = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [焼きなまし50%] */
            ANNEALING_RATE_50 = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [焼きなまし60%] */
            ANNEALING_RATE_60 = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [焼きなまし70%] */
            ANNEALING_RATE_70 = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [焼きなまし80%] */
            ANNEALING_RATE_80 = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [焼きなまし90%] */
            ANNEALING_RATE_90 = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [焼きなまし100%] */
            ANNEALING_RATE_100 = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [評価関数:スコアMAX(Initial)] */
            SCORE_MAX = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [評価関数：スコアMID(Initial)] */
            SCORE_MID = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [評価関数：スコアMIN(Initial)] */
            SCORE_MIN = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [評価関数：自陣と接触] */
            CONNECT_MY_FIELD = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [評価関数：壁と接触] */
            CONNECT_WALL = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [評価関数：障害物と接触] */
            CONNECT_OBJECT = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [評価関数：接触レート] */
            RATE_CONNECT = int.Parse(file.ReadLine());
            
            line = file.ReadLine(); /* [評価関数：サイズレート] */
            RATE_SCORE = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [評価関数：閉路判定数] */
            DEPTH_MAX = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [評価関数：パス石レート] */
            PASS_STONE_RATE = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [評価関数：配置不可フィールドレート] */
            FAILES_FIELD_RATE = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [評価関数：閉路レート] */
            CLOSED_FIELD_RATE = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [再探索：高評価個数] */
            RETRY_COUNT_EVAL = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [再探索：ランダム個数] */
            RETRY_COUNT_RAND = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [再探索：通常パス可能数] */
            RETRY_SEARCH_KIND_MAX = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [再探索：通常種保存数] */
            RETRY_CHOISE_FOR_EVAL_NUM = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [再探索：深さ(BIG)80%] */
            RETRY_DEPTH_BIG_80 = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [再探索：深さ(BIG)50%] */
            RETRY_DEPTH_BIG_50 = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [再探索：深さ(BIG)以外] */
            RETRY_DEPTH_BIG_0 = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [再探索：深さ(MID)80%] */
            RETRY_DEPTH_MID_80 = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [再探索：深さ(MID)50%] */
            RETRY_DEPTH_MID_50 = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [再探索：深さ(MID)以外] */
            RETRY_DEPTH_MID_0 = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [再探索：深さ(MIN)80%] */
            RETRY_DEPTH_MIN_80 = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [再探索：深さ(MIN)50%] */
            RETRY_DEPTH_MIN_50 = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [再探索：深さ(MIN)以外] */
            RETRY_DEPTH_MIN_0 = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [再探索：焼きなまし0%] */
            RETRY_ANNEALING_RATE_0 = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [再探索：焼きなまし10%] */
            RETRY_ANNEALING_RATE_10 = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [再探索：焼きなまし20%] */
            RETRY_ANNEALING_RATE_20 = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [再探索：焼きなまし30%] */
            RETRY_ANNEALING_RATE_30 = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [再探索：焼きなまし40%] */
            RETRY_ANNEALING_RATE_40 = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [再探索：焼きなまし50%] */
            RETRY_ANNEALING_RATE_50 = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [再探索：焼きなまし60%] */
            RETRY_ANNEALING_RATE_60 = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [再探索：焼きなまし70%] */
            RETRY_ANNEALING_RATE_70 = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [再探索：焼きなまし80%] */
            RETRY_ANNEALING_RATE_80 = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [再探索：焼きなまし90%] */
            RETRY_ANNEALING_RATE_90 = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [再探索：焼きなまし100%] */
            RETRY_ANNEALING_RATE_100 = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [再探索：MYFIELDS] */
            RETRY_MY_FIELDS_RATE = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [再探索：WALLS] */
            RETRY_WALLS_RATE = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [再探索：OBJECTS] */
            RETRY_OBJECTS_RATE = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [再探索：CONNECT] */
            RETRY_CONNECT_RATE = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [再探索：SCORE] */
            RETRY_SCORE_RATE = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [再探索：閉路判定数] */
            RETRY_DEPTH_MAX = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [再探索：パス石レート] */
            RETRY_PASS_STONE_RATE = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [再探索：配置不可フィールドレート] */
            RETRY_FAILES_FIELD_RATE = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [再探索：閉路レート] */
            RETRY_CLOSED_FIELD_RATE = int.Parse(file.ReadLine());
        }

        public void RetryCommonParam(string fileName)
        {
            string line = "";
            System.IO.StreamReader file = new System.IO.StreamReader(fileName);

            line = file.ReadLine(); /* [CFG_FILE] */
            line = file.ReadLine(); /* [Problem] */
            line = file.ReadLine(); /* [FileName] */
            line = file.ReadLine(); /* [初手最大探索インデックス] */
            line = file.ReadLine(); /* [初手保持数] */
            line = file.ReadLine(); /* [初手種保存数] */
            line = file.ReadLine(); /* [通常パス可能数] */
            line = file.ReadLine(); /* [通常種保存数] */
            line = file.ReadLine(); /* [深さ(BIG)80%] */
            line = file.ReadLine(); /* [深さ(BIG)50%] */
            line = file.ReadLine(); /* [深さ(BIG)以外] */
            line = file.ReadLine(); /* [深さ(MID)80%] */
            line = file.ReadLine(); /* [深さ(MID)50%] */
            line = file.ReadLine(); /* [深さ(MID)以外] */
            line = file.ReadLine(); /* [深さ(MIN)80%] */
            line = file.ReadLine(); /* [深さ(MIN)50%] */
            line = file.ReadLine(); /* [深さ(MIN)以外] */
            line = file.ReadLine(); /* [焼きなまし0%] */
            line = file.ReadLine(); /* [焼きなまし10%] */
            line = file.ReadLine(); /* [焼きなまし20%] */
            line = file.ReadLine(); /* [焼きなまし30%] */
            line = file.ReadLine(); /* [焼きなまし40%] */
            line = file.ReadLine(); /* [焼きなまし50%] */
            line = file.ReadLine(); /* [焼きなまし60%] */
            line = file.ReadLine(); /* [焼きなまし70%] */
            line = file.ReadLine(); /* [焼きなまし80%] */
            line = file.ReadLine(); /* [焼きなまし90%] */
            line = file.ReadLine(); /* [焼きなまし100%] */

            line = file.ReadLine(); /* [評価関数:スコアMAX(Initial)] */
            SCORE_MAX = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [評価関数：スコアMID(Initial)] */
            SCORE_MID = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [評価関数：スコアMIN(Initial)] */
            SCORE_MIN = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [評価関数：自陣と接触] */
            CONNECT_MY_FIELD = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [評価関数：壁と接触] */
            CONNECT_WALL = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [評価関数：障害物と接触] */
            CONNECT_OBJECT = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [評価関数：接触レート] */
            RATE_CONNECT = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [評価関数：サイズレート] */
            RATE_SCORE = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [評価関数：閉路判定数] */
            DEPTH_MAX = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [評価関数：パス石レート] */
            PASS_STONE_RATE = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [評価関数：配置不可フィールドレート] */
            FAILES_FIELD_RATE = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [評価関数：閉路レート] */
            CLOSED_FIELD_RATE = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [再探索：高評価個数] */
            RETRY_COUNT_EVAL = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [再探索：ランダム個数] */
            RETRY_COUNT_RAND = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [再探索：通常パス可能数] */
            RETRY_SEARCH_KIND_MAX = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [再探索：通常種保存数] */
            RETRY_CHOISE_FOR_EVAL_NUM = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [再探索：深さ(BIG)80%] */
            RETRY_DEPTH_BIG_80 = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [再探索：深さ(BIG)50%] */
            RETRY_DEPTH_BIG_50 = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [再探索：深さ(BIG)以外] */
            RETRY_DEPTH_BIG_0 = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [再探索：深さ(MID)80%] */
            RETRY_DEPTH_MID_80 = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [再探索：深さ(MID)50%] */
            RETRY_DEPTH_MID_50 = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [再探索：深さ(MID)以外] */
            RETRY_DEPTH_MID_0 = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [再探索：深さ(MIN)80%] */
            RETRY_DEPTH_MIN_80 = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [再探索：深さ(MIN)50%] */
            RETRY_DEPTH_MIN_50 = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [再探索：深さ(MIN)以外] */
            RETRY_DEPTH_MIN_0 = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [再探索：焼きなまし0%] */
            RETRY_ANNEALING_RATE_0 = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [再探索：焼きなまし10%] */
            RETRY_ANNEALING_RATE_10 = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [再探索：焼きなまし20%] */
            RETRY_ANNEALING_RATE_20 = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [再探索：焼きなまし30%] */
            RETRY_ANNEALING_RATE_30 = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [再探索：焼きなまし40%] */
            RETRY_ANNEALING_RATE_40 = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [再探索：焼きなまし50%] */
            RETRY_ANNEALING_RATE_50 = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [再探索：焼きなまし60%] */
            RETRY_ANNEALING_RATE_60 = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [再探索：焼きなまし70%] */
            RETRY_ANNEALING_RATE_70 = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [再探索：焼きなまし80%] */
            RETRY_ANNEALING_RATE_80 = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [再探索：焼きなまし90%] */
            RETRY_ANNEALING_RATE_90 = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [再探索：焼きなまし100%] */
            RETRY_ANNEALING_RATE_100 = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [再探索：MYFIELDS] */
            RETRY_MY_FIELDS_RATE = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [再探索：WALLS] */
            RETRY_WALLS_RATE = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [再探索：OBJECTS] */
            RETRY_OBJECTS_RATE = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [再探索：CONNECT] */
            RETRY_CONNECT_RATE = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [再探索：SCORE] */
            RETRY_SCORE_RATE = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [再探索：閉路判定数] */
            RETRY_DEPTH_MAX = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [再探索：パス石レート] */
            RETRY_PASS_STONE_RATE = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [再探索：配置不可フィールドレート] */
            RETRY_FAILES_FIELD_RATE = int.Parse(file.ReadLine());

            line = file.ReadLine(); /* [再探索：閉路レート] */
            RETRY_CLOSED_FIELD_RATE = int.Parse(file.ReadLine());

        }

    }
}
