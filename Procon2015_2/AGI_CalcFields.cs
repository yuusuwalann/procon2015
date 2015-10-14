using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Procon2015_2
{
    class AGI_CalcFields
    {
        const int F_MAX_X = 34; /* 問題最大32 壁2 */
        const int F_MAX_Y = 34; /* ↑ */

        const int EMPUTY = 0;
        const int MY_FIELD = 1;
        const int WALL = 3;
        const int OBJECT = 5;
        const int NEIGHT1 = -18;
        const int NEIGHT2 = -27;
        const int NEIGHT3 = -36;
        const int NEIGHT4 = -45;
        const int NEIGHT5 = -54;
        const int NEIGHT6 = -63;
        const int NEIGHT7 = -72;
        const int NEIGHT8 = -81;

        int SCORE_MAX;    /* 3辺以上接地 */
        int SCORE_MID;    /* 2辺接地 */
        int SCORE_MIN;    /* 1辺接地 */
        int NON_SCORE;    /* 0辺接地 */

        int CONNECT_MY_FIELD; /* 自陣と接触 */
        int CONNECT_WALL;     /* 壁と接触 */
        int CONNECT_OBJECT;   /* 障害物と接触 */

        float RATE_CONNECT;
        float RATE_SCORE;

        int DEPTH_MAX;
        int PASS_STONE_RATE;
        int FAILES_FIELD_RATE;
        int CLOSED_FIELD_RATE;

        int[] fields;
        public bool closed
        {
            get; set;
        }

        public AGI_CalcFields()
        {
            fields = new int[F_MAX_X * F_MAX_Y];
            for (int i = 0; i < fields.Length; i++)
            {
                fields[i] = 0;
            }
            closed = false;
            setConstDataDef();
        }

        public AGI_CalcFields(AGI_Fields agiF, COMMON_MAP map)
        {
            setConstData(map);
            fields = new int[F_MAX_X * F_MAX_Y];

            for (int i = 0; i < fields.Length; i++)
            {
                fields[i] = agiF.GetAgiFieldsNum(i);
            }

            closed = false;

            addNeighborInfo();
        }

        public AGI_CalcFields(AGI_CalcFields agic, COMMON_MAP map)
        {
            setConstData(map);
            fields = new int[F_MAX_X * F_MAX_Y];

            agic.fields.CopyTo(fields, 0);

            closed = agic.closed;
        }

        private void setConstDataDef()
        {
            SCORE_MAX = 5;    /* 3辺以上接地 */
            SCORE_MID = 2;    /* 2辺接地 */
            SCORE_MIN = 1;    /* 1辺接地 */
            NON_SCORE = 0;    /* 0辺接地 */

            CONNECT_MY_FIELD = 3; /* 自陣と接触 */
            CONNECT_WALL = 2;     /* 壁と接触 */
            CONNECT_OBJECT = 1;   /* 障害物と接触 */

            RATE_CONNECT = 0.8F;
            RATE_SCORE = 0.2F;

            DEPTH_MAX = 3;
            PASS_STONE_RATE = 3;
            FAILES_FIELD_RATE = 5;
            CLOSED_FIELD_RATE = 3;
        }


        private void setConstData(COMMON_MAP map)
        {
            SCORE_MAX = map.SCORE_MAX;    /* 3辺以上接地 */
            SCORE_MID = map.SCORE_MID;    /* 2辺接地 */
            SCORE_MIN = map.SCORE_MIN;    /* 1辺接地 */
            NON_SCORE = 0;    /* 0辺接地 */

            CONNECT_MY_FIELD = map.CONNECT_MY_FIELD; /* 自陣と接触 */
            CONNECT_WALL = map.CONNECT_WALL;     /* 壁と接触 */
            CONNECT_OBJECT = map.CONNECT_OBJECT;   /* 障害物と接触 */

            DEPTH_MAX = map.DEPTH_MAX;
            PASS_STONE_RATE = map.PASS_STONE_RATE;
            FAILES_FIELD_RATE = map.FAILES_FIELD_RATE;
            CLOSED_FIELD_RATE = map.CLOSED_FIELD_RATE;
            
            RATE_CONNECT = ((float)map.RATE_CONNECT / 100F);
            RATE_SCORE = ((float)map.RATE_SCORE / 100F);
        }

#if false
        public AGI_CalcFields(string strFields)
        {
            fields = new int[F_MAX_X * F_MAX_Y];
            int counter = 0;
            bool[] preMinusFlg = new bool[10]; /* 0:-, 1:1x, 2:2x... */
            closed = false;

            if (strFields.Length >= fields.Length)
            {
                for (int i = 0; i < strFields.Length; i++)
                {
                    string s = strFields.Substring(i, 1);   /* i番目から一文字取得 */

                    if (s == "-")
                    {
                        preMinusFlg[0] = true;
                    }
                    else
                    {
                        int s_i = int.Parse(s);
                        switch (s_i)
                        {
                            case 0:
                                fields[counter] = EMPUTY;
                                counter++;
                                break;
                            case 1:
                                if (preMinusFlg[0] == false)
                                {
                                    fields[counter] = MY_FIELD; /* 1 */
                                    counter++;
                                }
                                else
                                {
                                    if (preMinusFlg[8] == true)
                                    {
                                        fields[counter] = NEIGHT8;  /* -81 */
                                        preMinusFlg[8] = false;
                                        preMinusFlg[0] = false;
                                        counter++;
                                    }
                                    else if (preMinusFlg[1] == false)
                                    {
                                        preMinusFlg[1] = true;      /* -1x */
                                    }
                                }
                                break;
                            case 2:
                                if(preMinusFlg[0] == false)
                                {
                                    break;
                                }
                                else
                                {
                                    if (preMinusFlg[7] == true)
                                    {
                                        fields[counter] = NEIGHT7; /* -72 */
                                        preMinusFlg[7] = false;
                                        preMinusFlg[0] = false;
                                        counter++;
                                    }else if(preMinusFlg[2] == false)
                                    {
                                        preMinusFlg[2] = true;
                                    }
                                    else
                                    {
                                        ;
                                    }
                                }
                                break;
                            case 3:
                                if (preMinusFlg[0] == false)
                                {
                                    fields[counter] = WALL; /* 3 */
                                    counter++;
                                }
                                else
                                {
                                    if(preMinusFlg[6] == true)
                                    {
                                        fields[counter] = NEIGHT6; /* -63 */
                                        preMinusFlg[6] = false;
                                        preMinusFlg[0] = false;
                                        counter++;
                                    }else if(preMinusFlg[3] == false)
                                    {
                                        preMinusFlg[3] = true;
                                    }
                                    else
                                    {
                                        ;
                                    }
                                }
                                break;
                            case 4:
                                if (preMinusFlg[5] == true)
                                {
                                    fields[counter] = NEIGHT5; /* -54 */
                                    preMinusFlg[5] = false;
                                    preMinusFlg[0] = false;
                                    counter++;
                                }
                                else if (preMinusFlg[4] == false)
                                {
                                    preMinusFlg[4] = true;
                                }
                                else
                                {
                                    ;
                                }
   
                                break;
                            case 5:
                                if (preMinusFlg[0] == false)
                                {
                                    fields[counter] = OBJECT; /* 5 */
                                    counter++;
                                }
                                else if (preMinusFlg[4] == true)
                                {
                                    fields[counter] = NEIGHT4; /* -45 */
                                    preMinusFlg[4] = false;
                                    preMinusFlg[0] = false;
                                    counter++;
                                }
                                else if (preMinusFlg[5] == false)
                                {
                                    preMinusFlg[5] = true;
                                }
                                else
                                {
                                    ;
                                }
                                break;
                            case 6:
                                if (preMinusFlg[3] == true)
                                {
                                    fields[counter] = NEIGHT3; /* -36 */
                                    preMinusFlg[3] = false;
                                    preMinusFlg[0] = false;
                                    counter++;
                                }
                                else if (preMinusFlg[6] == false)
                                {
                                    preMinusFlg[6] = true;
                                }
                                else
                                {
                                    ;
                                }
                                
                                break;
                            case 7:

                                if (preMinusFlg[2] == true)
                                {
                                    fields[counter] = NEIGHT2; /* -27 */
                                    preMinusFlg[2] = false;
                                    preMinusFlg[0] = false;
                                    counter++;
                                }
                                else if (preMinusFlg[7] == false)
                                {
                                    preMinusFlg[7] = true;
                                }
                                else
                                {
                                    ;
                                }
                                break;
                            case 8:
                                if (preMinusFlg[1] == true)
                                {
                                    fields[counter] = NEIGHT1; /* -18 */
                                    preMinusFlg[1] = false;
                                    preMinusFlg[0] = false;
                                    counter++;
                                }
                                else if (preMinusFlg[8] == false)
                                {
                                    preMinusFlg[8] = true;
                                }
                                else
                                {
                                    ;
                                }
                                break;
                            case 9:
                                break;
                            default:
                                break;
                        }
                    }
                }

            }
        }
#endif
        private void addNeighborInfo()
        {
            /* STEP1:自陣との接触情報 */

            int[] connectTarget = new int[8] { MY_FIELD, NEIGHT1, NEIGHT2, NEIGHT3, NEIGHT4, NEIGHT5, NEIGHT6, NEIGHT7 };
            int[] setInfo = new int[8] { NEIGHT1, NEIGHT2, NEIGHT3, NEIGHT4, NEIGHT5, NEIGHT6, NEIGHT7, NEIGHT8 };
            /* STEP1 */

            for (int tgt = 0; tgt < connectTarget.Length; tgt++)
            {
                for (int i = 0; i < fields.Length; i++)
                {
                    if ((fields[i] == connectTarget[tgt]))
                    {
                        /* 上 */
                        if (i - F_MAX_X >= 0)
                        {
                            if (fields[i - F_MAX_X] == EMPUTY)
                            {
                                fields[i - F_MAX_X] = setInfo[tgt];
                            }
                        }
                        if (tgt < 1) /* サーチでの配置対象として自陣の-2以下は配置不可 (左上セル指定で配置するため) */
                        {
                            /* 下 */
                            if (i + F_MAX_X < fields.Length)
                            {
                                if (fields[i + F_MAX_X] == EMPUTY)
                                {
                                    fields[i + F_MAX_X] = setInfo[tgt];
                                }
                            }
                        }
                        /* 左 */
                        if (i - 1 >= 0)
                        {
                            if (fields[i - 1] == EMPUTY)
                            {
                                fields[i - 1] = setInfo[tgt];
                            }
                        }
                        if (tgt < 1) /* サーチでの配置対象として自陣の-2以下は配置不可 (左上セル指定で配置するため) */
                        {
                            if (i + 1 < fields.Length)
                            {
                                if (fields[i + 1] == EMPUTY)
                                {
                                    fields[i + 1] = setInfo[tgt];
                                }
                            }
                        }
                    }
                }
            }
        }

        public void PrintAgiCalcFields()
        {
            Console.WriteLine("");
            for (int i = 0; i < fields.Length; i++)
            {
                if ((i % F_MAX_X) == 0)
                {
                    Console.WriteLine("");
                }
                Console.Write("{0, 3},", fields[i]);
            }
        }

        public int GetAgiCalcFieldsSize()
        {
            return fields.Length;
        }

        public int GetAgiCalcFieldNum(int idx)
        {
            if (idx < fields.Length)
            {
                return fields[idx];

            }
            return 99;
        }

        public int GetAgiCalcScore()
        {
            int retScore = 0;

            for (int i = 0; i < fields.Length; i++)
            {
                if (fields[i] == MY_FIELD)
                {
                    retScore++;
                }
            }
            return retScore;
        }

        /* 評価値に石のサイズが影響しない評価関数 Initial */
        public float GetAgiCalcEvalInitial(AGI_CalcFields beforeAgiF, int depthSize)
        {
            float retEval = 0;
            int stoneCnt = 0;
            int depth = DEPTH_MAX;
            int[] closed = new int[4] { 0, 0, 0, 0 };
            int closedFieldsSize = 0;
            int falesFieldsSize = 0;

            for (int i = 0; i < fields.Length; i++)
            {
                int cntEdge = 0;    /* スコア対象の辺の数をカウント */
                if (beforeAgiF.GetAgiCalcFieldNum(i) != MY_FIELD && fields[i] == MY_FIELD)    /* 前回から今回で自陣に変化した場合 */
                {
                    stoneCnt++;
                    /* 上 */
                    if (i - F_MAX_X >= 0)
                    {
                        /* エッジ接続チェック */
                        if (fields[i - F_MAX_X] == MY_FIELD || fields[i - F_MAX_X] == OBJECT || fields[i - F_MAX_X] == WALL)
                        {
                            cntEdge++;
                        }

                        /* 閉路チェック */
                        closed[0] = checkClosedCell2(i - F_MAX_X, depth);
                    }
                    /* 下 */
                    if (i + F_MAX_X < fields.Length)
                    {
                        if (fields[i + F_MAX_X] == MY_FIELD || fields[i + F_MAX_X] == OBJECT || fields[i + F_MAX_X] == WALL)
                        {
                            cntEdge++;
                        }
                        closed[1] = checkClosedCell2(i + F_MAX_X, depth);
                    }
                    /* 左 */
                    if (i - 1 >= 0)
                    {
                        if (fields[i - 1] == MY_FIELD || fields[i - 1] == OBJECT || fields[i - 1] == WALL)
                        {
                            cntEdge++;
                        }
                        closed[2] = checkClosedCell2(i - 1, depth);
                    }
                    /* 右 */
                    if (i + 1 < fields.Length)
                    {
                        if (fields[i + 1] == MY_FIELD || fields[i + 1] == OBJECT || fields[i + 1] == WALL)
                        {
                            cntEdge++;
                        }
                        closed[3] = checkClosedCell2(i + 1, depth);
                    }


                    /* 閉路のマス数以下の石がすでにない場合 */
                    for (int k = 0; k < closed.Length; k++)
                    {
                        if ((closed[k] != 0) && (closed[k] < depthSize))
                        {
                            falesFieldsSize++;
                        }
                    }

                    /* 規定以下の閉路数 */

                    for (int k = 0; k < closed.Length; k++)
                    {
                        if ((closed[k] != 0) && (closed[k] < DEPTH_MAX))
                        {
                            closedFieldsSize++;
                        }

                        if ((closed[k] != 0) && (closed[k] < (DEPTH_MAX / 2)))
                        {
                            closedFieldsSize++;
                        }
                    }
                }

                if (cntEdge >= 3)
                {
                    retEval += SCORE_MAX;
                }
                else if (cntEdge == 2)
                {
                    retEval += SCORE_MID;
                }
                else if (cntEdge == 1)
                {
                    retEval += SCORE_MIN;
                }
                else
                {
                    retEval += NON_SCORE;
                }
            }

            retEval = retEval / (float)stoneCnt;
            retEval = retEval - (FAILES_FIELD_RATE * falesFieldsSize) - (CLOSED_FIELD_RATE * closedFieldsSize);


            return (retEval);
        }

        /* 評価値に石のサイズが影響しない評価関数 Depth */
        public float GetAgiCalcEvalDepth(AGI_CalcFields beforeAgiF, int depthSize)
        {
            float retEval = 0.0F;
            int stoneCnt = 0;
            int depth = DEPTH_MAX;
            int closedFieldsSize = 0;
            int falesFieldsSize = 0;
            int[] closed = new int[4] { 0, 0, 0, 0 };

            for (int i = 0; i < fields.Length; i++)
            {
                int cntmyFieldEdge = 0;    /* スコア対象の辺の数をカウント */
                int cntwallEdge = 0;
                int cntObjectEdge = 0;

                if (beforeAgiF.GetAgiCalcFieldNum(i) != MY_FIELD && fields[i] == MY_FIELD)    /* 前回から今回で自陣に変化した場合 */
                {
                    stoneCnt++;
                    /* 上 */
                    if (i - F_MAX_X >= 0)
                    {
                        /* エッジ接続チェック */
                        switch (fields[i - F_MAX_X])
                        {
                            case MY_FIELD:
                                if (beforeAgiF.GetAgiCalcFieldNum(i - F_MAX_X) != MY_FIELD && fields[i - F_MAX_X] == MY_FIELD)
                                {
                                    cntmyFieldEdge++;
                                }
                                break;
                            case WALL:
                                cntwallEdge++;
                                break;
                            case OBJECT:
                                cntObjectEdge++;
                                break;
                            default:
                                break;
                        }

                        /* 閉路チェック */
                        closed[0] = checkClosedCell2(i - F_MAX_X, depth);
       
                    }
                    /* 下 */
                    if (i + F_MAX_X < fields.Length)
                    {
                        switch (fields[i + F_MAX_X])
                        {
                            case MY_FIELD:
                                if (beforeAgiF.GetAgiCalcFieldNum(i + F_MAX_X) != MY_FIELD && fields[i + F_MAX_X] == MY_FIELD)
                                {
                                    cntmyFieldEdge++;
                                }
                                break;
                            case WALL:
                                cntwallEdge++;
                                break;
                            case OBJECT:
                                cntObjectEdge++;
                                break;
                            default:
                                break;
                        }
                        closed[1] = checkClosedCell2(i + F_MAX_X, depth);
         
                    }
                    /* 左 */
                    if (i - 1 >= 0)
                    {
                        switch (fields[i - 1])
                        {
                            case MY_FIELD:
                                if (beforeAgiF.GetAgiCalcFieldNum(i - 1) != MY_FIELD && fields[i - 1] == MY_FIELD)
                                {
                                    cntmyFieldEdge++;
                                }
                                break;
                            case WALL:
                                cntwallEdge++;
                                break;
                            case OBJECT:
                                cntObjectEdge++;
                                break;
                            default:
                                break;
                        }
                        closed[2] = checkClosedCell2(i - 1, depth);
                    }
                    /* 右 */
                    if (i + 1 < fields.Length)
                    {
                        switch (fields[i + 1])
                        {
                            case MY_FIELD:
                                if (beforeAgiF.GetAgiCalcFieldNum(i + 1) != MY_FIELD && fields[i + 1] == MY_FIELD)
                                {
                                    cntmyFieldEdge++;
                                }
                                break;
                            case WALL:
                                cntwallEdge++;
                                break;
                            case OBJECT:
                                cntObjectEdge++;
                                break;
                            default:
                                break;
                        }
                        closed[3] = checkClosedCell2(i + 1, depth);
                    }

                    /* 閉路のマス数以下の石がすでにない場合 */
                    for (int k = 0; k < closed.Length; k++)
                    {
                        if ((closed[k] != 0) && (closed[k] < depthSize))
                        {
                            falesFieldsSize++;
                        }
                    }

                    /* 規定以下の閉路数 */

                    for (int k = 0; k < closed.Length; k++)
                    {
                        if ((closed[k] != 0) && (closed[k] < DEPTH_MAX))
                        {
                            closedFieldsSize++;
                        }

                        if ((closed[k] != 0) && (closed[k] < (DEPTH_MAX / 2)))
                        {
                            closedFieldsSize++;
                        }
                    }

                }

                retEval += (cntObjectEdge * 3) + (cntwallEdge * 2) + (cntmyFieldEdge * 1);
            }

            retEval = (retEval / stoneCnt);
            retEval = retEval - (FAILES_FIELD_RATE * falesFieldsSize) - (CLOSED_FIELD_RATE * closedFieldsSize);

            return ((float)retEval);
        }

        public float GetAgiCalcEvalDepthLimit()
        {
            return 4.0F;
        }

        /* 評価値に石のサイズが影響する評価関数 Normal*/
        public float GetAgiCalcEvalSizeKai(AGI_CalcFields beforeAgiF, int score, int passSize, int depthSize)
        {
            float retEval = 0.0F;
            int stoneCnt = 0;
            int depth = DEPTH_MAX;
            int[] closed = new int[4] { 0, 0, 0, 0 };
            int closedFieldsSize = 0;
            int falesFieldsSize = 0;

            for (int i = 0; i < fields.Length; i++)
            {
                int cntmyFieldEdge = 0;    /* スコア対象の辺の数をカウント */
                int cntwallEdge = 0;
                int cntObjectEdge = 0;
                if (beforeAgiF.GetAgiCalcFieldNum(i) != MY_FIELD && fields[i] == MY_FIELD)    /* 前回から今回で自陣に変化した場合 */
                {
                    stoneCnt++;
                    /* 上 */
                    if (i - F_MAX_X >= 0)
                    {
                        /* エッジ接続チェック */
                        switch (fields[i - F_MAX_X])
                        {
                            case MY_FIELD:
                                if (beforeAgiF.GetAgiCalcFieldNum(i - F_MAX_X) == MY_FIELD && fields[i - F_MAX_X] == MY_FIELD)
                                {
                                    cntmyFieldEdge++;
                                }
                                break;
                            case WALL:
                                cntwallEdge++;
                                break;
                            case OBJECT:
                                cntObjectEdge++;
                                break;
                            default:
                                break;
                        }

                        /* 閉路チェック */
                        closed[0] = checkClosedCell2(i - F_MAX_X, depth);
                       

                    }
                    /* 下 */
                    if (i + F_MAX_X < fields.Length)
                    {
                        switch (fields[i + F_MAX_X])
                        {
                            case MY_FIELD:
                                if (beforeAgiF.GetAgiCalcFieldNum(i + F_MAX_X) == MY_FIELD && fields[i + F_MAX_X] == MY_FIELD)
                                {
                                    cntmyFieldEdge++;
                                }
                                break;
                            case WALL:
                                cntwallEdge++;
                                break;
                            case OBJECT:
                                cntObjectEdge++;
                                break;
                            default:
                                break;
                        }
                        closed[1] = checkClosedCell2(i + F_MAX_X, depth);
                       
                    }
                    /* 左 */
                    if (i - 1 >= 0)
                    {
                        switch (fields[i - 1])
                        {
                            case MY_FIELD:
                                if (beforeAgiF.GetAgiCalcFieldNum(i - 1) == MY_FIELD && fields[i - 1] == MY_FIELD)
                                {
                                    cntmyFieldEdge++;
                                }
                                break;
                            case WALL:
                                cntwallEdge++;
                                break;
                            case OBJECT:
                                cntObjectEdge++;
                                break;
                            default:
                                break;
                        }
                        closed[2] = checkClosedCell2(i - 1, depth);
                        
                    }
                    /* 右 */
                    if (i + 1 < fields.Length)
                    {
                        switch (fields[i + 1])
                        {
                            case MY_FIELD:
                                if (beforeAgiF.GetAgiCalcFieldNum(i + 1) == MY_FIELD && fields[i + 1] == MY_FIELD)
                                {
                                    cntmyFieldEdge++;
                                }
                                break;
                            case WALL:
                                cntwallEdge++;
                                break;
                            case OBJECT:
                                cntObjectEdge++;
                                break;
                            default:
                                break;
                        }
                        closed[3] = checkClosedCell2(i + 1, depth);

                    }


                    /* 閉路のマス数以下の石がすでにない場合 */
                    for (int k = 0; k < closed.Length; k++)
                    {
                        if ((closed[k] != 0) && (closed[k] < depthSize))
                        {
                            falesFieldsSize++;
                        }
                    }

                    /* 規定以下の閉路数 */

                    for (int k = 0; k < closed.Length; k++)
                    {
                        if ((closed[k] != 0) && (closed[k] < DEPTH_MAX))
                        {
                            closedFieldsSize++;
                        }

                        if ((closed[k] != 0) && (closed[k] < (DEPTH_MAX/2)))
                        {
                            closedFieldsSize++;
                        }
                    }
                }

                retEval += (cntObjectEdge * CONNECT_OBJECT) + (cntwallEdge * CONNECT_WALL) + (cntmyFieldEdge * CONNECT_MY_FIELD);
            }


            retEval = (retEval/ (float)stoneCnt) * RATE_CONNECT + (float)score * RATE_SCORE;    /* プラス要素 */
            retEval = retEval - (PASS_STONE_RATE * passSize) - (FAILES_FIELD_RATE * falesFieldsSize) - (CLOSED_FIELD_RATE * closedFieldsSize);


            return (retEval);
        }

        private bool checkClosedCell(int idx)
        {
            bool result = false;
            if ((fields[idx] <= NEIGHT1) || (fields[idx] == EMPUTY))
            {
                /* 上 */
                if (idx - F_MAX_X >= 0)
                {
                    if (fields[idx - F_MAX_X] == EMPUTY || fields[idx - F_MAX_X] <= NEIGHT1)
                    {
                        result = true;
                    }
                }
                /* 下 */
                if (idx + F_MAX_X < fields.Length)
                {
                    if (fields[idx + F_MAX_X] == EMPUTY || fields[idx + F_MAX_X] <= NEIGHT1)
                    {
                        result = true;
                    }
                }
                /* 左 */
                if (idx - 1 >= 0)
                {
                    if (fields[idx - 1] == EMPUTY || fields[idx - 1] <= NEIGHT1)
                    {
                        result = true;
                    }
                }

                /* 右 */
                if (idx + 1 < fields.Length)
                {
                    if (fields[idx + 1] == EMPUTY || fields[idx + 1] <= NEIGHT1)
                    {
                        result = true;
                    }
                }
            }
            else
            {
                result = true;
            }

            return result;

        }

        /* depth未満の閉路があった場合、その個数を返す */
        /* depth未満の閉路がない場合、0を返す */
        private int checkClosedCell2(int idx, int depth)
        {
            int result = 0;

            const int LEFT = 0;
            const int UP = 1;
            const int RIGHT = 2;
            const int DOWN = 3;
            
            /* 境界線追跡 */
            /* 前回境界点の位置より反時計回りで走査 */

            int[] dirctList = new int[4] { LEFT, UP, RIGHT, DOWN };
            int[,] directNextList = new int[4, 4] {
                { UP, LEFT, DOWN, RIGHT },
                {RIGHT, UP, LEFT, DOWN },
                {DOWN, RIGHT, UP, LEFT},
                {LEFT, DOWN, RIGHT, UP } };
            int reDirect = RIGHT;
            int startCell = idx;
            int searchCell = idx;
            List<int> flgList = new List<int>();

            if (fields[idx] <= EMPUTY)
            {
                flgList.Add(startCell);

                while (true)
                {
                    for (int i = 0; i < dirctList.Length; i++)
                    {
                        int targetId = direct2Idx(searchCell, directNextList[reDirect, i]);
                        if (targetId == -1)
                        {
                            continue;
                        }
                        if (fields[targetId] <= EMPUTY)
                        {
                            searchCell = targetId;
                            reDirect = directNextList[reDirect, i];
                            break;
                        }
                    }

                    if (startCell == searchCell)
                    {
                        /* 検索終了 */
                        result = flgList.Count;   //閉路と判定
                        break;
                    }

                    if (flgList.Contains(searchCell) == true)
                    {
                        continue;
                    }
                    else
                    {
                        flgList.Add(searchCell);
                        if (flgList.Count >= depth)
                        {
                            result = 0;  /* 閉路無し */
                            break;

                        }
                    }
                }
            }
            else
            {
                result = 0;  /* 閉路無し */
            }

            return result;
        }

        public void SetAgiCalcNum(int idx, int num)
        {
            if (idx < fields.Length)
            {
                fields[idx] = num;
            }
        }

        public void SetAgiCalcCopyFromA(AGI_CalcFields A)
        {
            for (int i = 0; i < fields.Length; i++)
            {
                this.fields[i] = A.fields[i];
            }
        }

        public int GetMaxX()
        {
            return F_MAX_X;
        }

        public int GetMaxY()
        {
            return F_MAX_Y;
        }

        /* Searchクラスでの加算結果を元に戻す関数 */
        public void GetReturnCalcFld()
        {
            /* 各定数情報 
             * AGI_FIELDS 
             *     空：0
             *     自：1
             *     障：5
             *
             * AGI_STONE
             *     空:0
             *     石:1
             *     近1:-7
             *
             */


            for (int i = 0; i < this.GetAgiCalcFieldsSize(); i++)
            {
                switch (fields[i])
                {
                    case 0:                         /* 空(F) + 空(S) */
                        break;                      /* 空 */
                    case 1:                         /* 空(F) + 石(S) or 自(F) + 空(S) */
                        fields[i] = MY_FIELD;       /* 自 */
                        break;
                    case 3:                         /* 壁(F) + 空(S) */
                        break;                      /* 壁 */
                    case 5:                         /* 障(F) + 空(S) */
                        break;                      /* 障 */

                    case -17:                       /* 近1(F) + 石(S) */
                    /* FALL THROW */
                    case -26:                       /* 近2(F) + 石(S) */
                    /* FALL THROW */
                    case -35:
                    /* FALL THROW */
                    case -44:
                    /* FALL THROW */
                    case -53:
                    /* FALL THROW */
                    case -62:
                    /* FALL THROW */
                    case -71:
                    /* FALL THROW */
                    case -80:
                        fields[i] = MY_FIELD;       /* 自 */
                        break;

                    case -7:                        /* 空(F) + 近1(S)*/
                        fields[i] = EMPUTY;         /* 空 */
                        break;
                    case -6:                        /* 自(F) + 近1(S)*/
                        fields[i] = MY_FIELD;       /* 自 */
                        break;
                    case -4:                        /* 壁(F) + 近1(S)*/
                        fields[i] = WALL;           /* 壁 */
                        break;
                    case -2:                        /* 障(F) + 近1(S)*/
                        fields[i] = OBJECT;         /* 障 */
                        break;
                    default:
                        fields[i] = EMPUTY;
                        break;
                }
            }

            addNeighborInfo();

        }

        /* 空セルの数を返す */
        public int GetAgiCalcEmptySize()
        {
            int emptySize = 0;

            for(int i=0; i < fields.Length; i++)
            {
                if(fields[i] == EMPUTY || (fields[i] < 0 && fields[i] >= NEIGHT8))
                {
                    emptySize++;
                }
            }
            return emptySize;
        }

        private int direct2Idx(int idx, int direct)
        {
            const int LEFT = 0;
            const int DOWN = 1;
            const int RIGHT = 2;
            const int UP = 3;

            int ret = -1;

            int x = idx % F_MAX_X;
            int y = idx / F_MAX_X;

            switch (direct)
            {
                case LEFT:
                    if (x > 0)
                    {
                        ret = idx - 1;
                    }
                    break;
                case RIGHT:
                    if (x < (F_MAX_X - 1))
                    {
                        ret = idx + 1;
                    }
                    break;
                case UP:
                    if (y > 0)
                    {
                        ret = idx - F_MAX_X;
                    }
                    break;
                case DOWN:
                    if (y < (F_MAX_X - 1))
                    {
                        ret = idx + F_MAX_X;
                    }
                    break;
                default:
                    break;
            }
            return ret;
        }
    }
}
