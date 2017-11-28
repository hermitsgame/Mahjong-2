using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 役の種類を定義
enum Yaku
{
    // 1
    RIICHI,
    HAKU,HATU,CHUN,CHANFON,MENFON,
    TANYAO,
    PINFU,
    MENZEN_TUMO,
    IPPATU,
    I_PE_KO,
    HAITEI_RON,
    HAITEI_TUMO,
    RINSHAN,
    CHANKAN,
    DOUBLE_RIICHI,
    //2
    TOITOI,
    SANSHOKU_DOUJUN,
    SANSHOKU_DOUKOU,
    CHITOITU,
    IKKI_TUUKAN,
    CHANTA,
    SANANKO,
    SHOUSANGEN,
    HONROUTOU,
    SANKANTU,
    //3
    JUNNCHAN,
    HONITU,
    RYANPEKO,
    //6
    CHINITU,

    // 役満
    SUANKO,
    SUANKO_TANKI,
    KOKUSHIMUSOU,
    DAISANGEN,
    SHOSUSHI,
    DAISUSHI,
    TUISO,
    CHINROTO,
    CHIHO,
    RYUISO,
    CHUREN,
    JUNSEI_CHUREN,
    SUKANTU,
    TENHO
}
// 役判定の処理を定義するクラス
public partial class Mahjong : MonoBehaviour {

    // 上がった役を格納していく
    List<Yaku> yaku = new List<Yaku>();

    // 役満系
    #region
    /// <summary>
    /// 国士以外の役満系の役があるかどうかを判定
    /// </summary>
    /// <returns></returns>
    bool CheckYakuman()
    {
        print("checkyakuman");
        CheckSuanko();
        CheckDaisangen();
        CheckRyuiso();
        CheckTuiso();
        CheckChinroto();
        //未実装 CheckSukantu();
        CheckShosushi();
        CheckDaisushi();
        CheckChuren();
        yaku.ForEach(item => print(string.Format("Yaku :{0} \n", item)));
        return (yaku.Count > 0) ?true:false;
    }
   
    // 四暗刻の判定
    void CheckSuanko()
    {
        if (isNaki())
            return;
        if (cntPungs == 4)
        {
            // 単騎待ちの場合
            if (income == head)
            {
                this.yaku.Add(Yaku.SUANKO_TANKI);
            }
            else
            {
                this.yaku.Add(Yaku.SUANKO);
            }
        }
    }

    // 国士の判定
    bool CheckKokushi()
    {
        for (int i = 0; i < tmp.Length; i++)
        {
            switch (i)
            {
                case 1:
                case 9:
                case 11:
                case 19:
                case 21:
                case 29:
                case 31:
                case 32:
                case 33:
                case 34:
                case 35:
                case 36:
                case 37:
                    if (tmp[i] == 2)
                    {
                        tmp[i] -= 2;
                    }
                    else if (tmp[i] == 1)
                    {
                        tmp[i]--;
                    }
                    break;
                default:
                    if (tmp[i] != 0)
                        return false;
                    break;
            }
        }
        // 19字牌のみで手持ち牌を使い切ったらtrue
        if (CheckUseOut())
        {
            yaku.Add(Yaku.KOKUSHIMUSOU);
            return true;
        }
        return false;
    }

    void CheckDaisangen()
    {
        int cnt = 0;
        for (int i = 0; i < Pungs.Length; i++)
        {
            // 三元牌の刻子の数をカウント
            if (CheckWhichEver(Pungs[i], 35, 36, 37))
                cnt++;
        }
        if (cnt > 2)
            yaku.Add(Yaku.DAISANGEN);
    }

    void CheckRyuiso()
    {
        // 刻子、順子、頭が全て緑か判定
        if (!CheckWhichEver(head, 22, 23, 24, 26, 28, 36))
            return;
        for (int i = 0; i < Pungs.Length; i++)
        {
            if (Pungs[i] != 0 && !CheckWhichEver(Pungs[i], 22, 23, 24, 26, 28, 36))
                return;
            if (Chows[i] != 0 && (!CheckWhichEver(Chows[i], 22, 23, 24, 26, 28, 36)
                || !CheckWhichEver(Chows[i] + 1, 22, 23, 24, 26, 28, 36)
                || !CheckWhichEver(Chows[i] + 2, 22, 23, 24, 26, 28, 36)))
                return;
        }

        yaku.Add(Yaku.RYUISO);
    }

    void CheckTuiso()
    {
        // 刻子、順子、頭が全て字牌か判定
        if (!CheckWhichEver(head, 31, 32, 33, 34, 35, 36, 37))
            return;
        for (int i = 0; i < Pungs.Length; i++)
        {
            if (Pungs[i] != 0 && !CheckWhichEver(Pungs[i], 31, 32, 33, 34, 35, 36, 37))
                return;
            if (Chows[i] != 0 && !CheckWhichEver(Chows[i], 31, 32, 33, 34, 35, 36, 37))
                return;
        }

        yaku.Add(Yaku.TUISO);
    }
    void CheckChinroto()
    {
        if (!CheckWhichEver(head, 1, 9, 11, 19, 21, 29))
            return;
        if (cntChows > 0)
            return;
        for (int i = 0; i < Pungs.Length; i++)
        {
            if (Pungs[i] != 0 && !CheckWhichEver(Pungs[i], 1, 9, 11, 19, 21, 29))
                return;
        }

        yaku.Add(Yaku.CHINROTO);
    }

    void CheckShosushi()
    {
        if (!CheckWhichEver(head, 31, 32, 33, 34))
            return;

        // 頭が31~34の牌の場合
        int cntP = 0;
        for (int i = 0; i < Pungs.Length; i++)
        {
            if (Pungs[i] != 0 && CheckWhichEver(Pungs[i], 31, 32, 33, 34))
                cntP++;
        }
        if (cntP == 3)
            yaku.Add(Yaku.SHOSUSHI);
    }

    void CheckDaisushi()
    {
        // 頭が31~34の牌の場合return
        if (CheckWhichEver(head, 31, 32, 33, 34))
            return;
        if (cntChows > 0)
            return;
        int cntP = 0;
        for (int i = 0; i < Pungs.Length; i++)
        {
            if (Pungs[i] != 0 && CheckWhichEver(Pungs[i], 31, 32, 33, 34))
                cntP++;
        }
        if (cntP == 4)
            yaku.Add(Yaku.DAISUSHI);
    }

    void CheckChuren()
    {
        if (isNaki())
            return;
        int cnt = 0;
        // 1の暗刻の種類をカウント
        for (int i = 0; i < 3; i++)
        {
            cnt += (Hand[1 + i * 10] > 3) ?
                1 : 0;
        }
        // 1の暗刻が2種類以上
        if (cnt > 1) return;

        // 調べる牌の種類
        int n = Pungs[0] / 10;
        for (int i = n*10 + 1; i <= n * 10 + 9; i++)
        {
            switch (i % 10)
            {
                case 1:
                case 9:
                    if (Hand[i] < 3)
                        return;
                    break;
                default:
                    if (Hand[i] < 1)
                        return;
                    break;
            }
        }
        income = head;
        if (head != income)
            yaku.Add(Yaku.CHUREN);
        else
            yaku.Add(Yaku.JUNSEI_CHUREN);
    }

    void CheckSukantu()
    {
        if (cntKan > 3)
            yaku.Add(Yaku.SUKANTU);
    }
    #endregion

    // 役満以外の役判定
    void CheckNormalYaku()
    {
        print("CheckNormalYaku");
        CheckOneColor();
        CheckHonroChanta();
        CheckShousangen();
        CheckDoubleChows();
        CheckToitoihou();
        CheckSanankou(); // ***
        CheckIttu();
        CheckSanshokuDoujun();
        CheckSanshokuDoukou();
        CheckPinfu();
        CheckTanyao();
        CheckFanpai();
        CheckMenzen();
        yaku.ForEach(item => print(string.Format("Yaku :{0} \n", item)));
    }

    #region
    // ホンイツ・チンイツの判定
    void CheckOneColor()
    {
        bool isHonitu = false;
        int color = -1;
        for (int i = 1; i < 30; i++)
        {
            if (Hand[i] > 0) {
                if (color >= 0 && i / 10 != color) return;
                else if (color < 0) color = i / 10;
            }
        }
        for (int i = 30; i < Hand.Length; i++)
        {
            if (Hand[i] > 0) isHonitu = true;
        }
        if (isHonitu) yaku.Add(Yaku.HONITU);
        else yaku.Add(Yaku.CHINITU);
    }

    // ホンロー・チャンタ系の判定
    void CheckHonroChanta()
    {
        if (CheckWhichEver(head, 1, 9, 11, 19, 21, 29, 31, 32, 33, 34, 35, 36, 37))
        {
            bool isHonro = true,isJunchan = true;
            int cnt = 0;
            if (head > 30) isJunchan = false;
            if (cntPungs < 4) isHonro = false;
            for (int i = 0; i < cntPungs; i++)
            {
                cnt += (CheckWhichEver(Pungs[i], 1, 9, 11, 19, 21, 29, 31, 32, 33, 34, 35, 36, 37)) ? 1 : 0;
                if (Pungs[i] > 30) isJunchan = false;
            }
            if (!isHonro)
            {
                for (int i = 0; i < cntChows; i++)
                {
                    cnt += (CheckWhichEver(Chows[i], 1, 7, 11, 17, 21, 27)) ? 1 : 0;
                }
            }
            if (cnt == 4)
            {
                if (isHonro) yaku.Add(Yaku.HONROUTOU);
                else if (isJunchan) yaku.Add(Yaku.JUNNCHAN);
                else yaku.Add(Yaku.CHANTA);
            }
        }
    }

    void CheckShousangen()
    {
        if (!CheckWhichEver(head,35,36,37)) return;
        int cnt = 0;// 三元牌の暗刻の数
        for (int i = 0; i < cntPungs; i++)
        {
            cnt += (CheckWhichEver(Pungs[i], 35, 36, 37)) ? 1 : 0;
        }
        if (cnt == 2) yaku.Add(Yaku.SHOUSANGEN);
    }

    // 一盃口,二盃口の判定
    void CheckDoubleChows()
    {
        // chow 0~29
        int[] chowCnt = new int[30];
        int pairChows = 0;
        for (int i = 0; i < cntChows; i++)
        {
            chowCnt[Chows[i]]++;
        }
        for (int i = 0; i < chowCnt.Length; i++)
        {
            if (chowCnt[i] > 1) pairChows++;
        }
        switch (pairChows)
        {
            case 1:
                yaku.Add(Yaku.I_PE_KO);
                break;
            case 2:
                yaku.Add(Yaku.RYANPEKO);
                break;
            default:
                return;
        }
    }

    // 対々和の判定
    void CheckToitoihou()
    {
        if (cntPungs == 4) yaku.Add(Yaku.TOITOI);
    }

    // 一気通貫の判定
    void CheckIttu()
    {
        if (CheckWhichEver(1, Chows[0], Chows[1], Chows[2], Chows[3])
            && CheckWhichEver(4, Chows[0], Chows[1], Chows[2], Chows[3])
            && CheckWhichEver(7, Chows[0], Chows[1], Chows[2], Chows[3]))
        {
            yaku.Add(Yaku.IKKI_TUUKAN);
        }
        else if (CheckWhichEver(11, Chows[0], Chows[1], Chows[2], Chows[3])
            && CheckWhichEver(14, Chows[0], Chows[1], Chows[2], Chows[3])
            && CheckWhichEver(17, Chows[0], Chows[1], Chows[2], Chows[3]))
        {
            yaku.Add(Yaku.IKKI_TUUKAN);
        }
        else if (CheckWhichEver(21, Chows[0], Chows[1], Chows[2], Chows[3])
            && CheckWhichEver(24, Chows[0], Chows[1], Chows[2], Chows[3])
            && CheckWhichEver(27, Chows[0], Chows[1], Chows[2], Chows[3]))
        {
            yaku.Add(Yaku.IKKI_TUUKAN);
        }
    }
    // 三暗刻の判定
    void CheckSanankou()
    {
        if (cntPon > 1 || cntChi > 1)
            return;

        int cnt = cntPungs;// 暗刻の数
    }

    // 三色同順の判定
    void CheckSanshokuDoujun()
    {
        if (cntChows < 3) return;
        // 先頭2つを調べる
        for (int i = 0; i < 2; i++)
        {
            int num = Chows[i] % 10;
            if (CheckWhichEver(num, Chows[0], Chows[1], Chows[2], Chows[3])
                && CheckWhichEver(num + 10, Chows[0], Chows[1], Chows[2], Chows[3])
                && CheckWhichEver(num + 20, Chows[0], Chows[1], Chows[2], Chows[3]))
            {
                yaku.Add(Yaku.SANSHOKU_DOUJUN);
                return;
            }
        }
    }

    // 三色同刻の判定
    void CheckSanshokuDoukou()
    {
        if (cntPungs < 3) return;
        // 先頭2つを調べる
        for (int i = 0; i < 2; i++)
        {
            int num = Pungs[i] % 10;
            if (CheckWhichEver(num, Pungs[0], Pungs[1], Pungs[2], Pungs[3])
                && CheckWhichEver(num + 10, Pungs[0], Pungs[1], Pungs[2], Pungs[3])
                && CheckWhichEver(num + 20, Pungs[0], Pungs[1], Pungs[2], Pungs[3]))
            {
                yaku.Add(Yaku.SANSHOKU_DOUKOU);
                return;
            }
        }
    }

    // 平和の判定
    void CheckPinfu()
    {
        if (!isChi() && !CheckWhichEver(head,Chanfon(),Menfon(),35,36,37))
        {
            print(income);
            for (int i = 0; i < 4; i++)
            {
                // ペンチャン待ちの場合
                if ((Chows[i] % 10 == 1 && income == Chows[i] + 2) || (Chows[i] % 10 == 7 && income == Chows[i]))
                {
                    continue;
                } // カンチャン待ちの場合return
                else if ((income == Chows[i])||(income == Chows[i] + 2))
                {
                    yaku.Add(Yaku.PINFU);
                    return;
                }
            }
        }
    }
    // タンヤオの判定
    void CheckTanyao()
    {
        if (CheckWhichEver(head, 1, 9, 11, 19, 21, 29, 31, 32, 33, 34, 35, 36, 37))
            return;
        for (int i = 0; i < 4; i++)
        {
            if (CheckWhichEver(Pungs[i], 1, 9, 11, 19, 21, 29, 31, 32, 33, 34, 35, 36, 37)) return;
            if (CheckWhichEver(Chows[i], 1, 9, 11, 19, 21, 29, 31, 32, 33, 34, 35, 36, 37)) return;
        }
        yaku.Add(Yaku.TANYAO);
    }

    // 役牌の判定
    void CheckFanpai()
    {
        for (int i = 0; i < 4; i++)
        {
            if (CheckWhichEver(Pungs[i], 31, 32, 33, 34, 35, 36, 37))
            {
                if (Chanfon() == Pungs[i]) yaku.Add(Yaku.CHANFON);
                if (Menfon() == Pungs[i]) yaku.Add(Yaku.MENFON);
                switch (Pungs[i])
                {
                    case 35:
                        yaku.Add(Yaku.HAKU);
                        break;
                    case 36:
                        yaku.Add(Yaku.HATU);
                        break;
                    case 37:
                        yaku.Add(Yaku.CHUN);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    void CheckMenzen()
    {
        if (cntChi + cntKan + cntKan <= 0 && FieldManager.IsMyTurn(playSide))
            yaku.Add(Yaku.MENZEN_TUMO);
    }
    #endregion

    // チートイのf判定
    void Check7Pairs()
    {
        for (int i = 0; i < tmp.Length; i++)
        {
            if (tmp[i] == 2) tmp[i] -= 2;
        }
        if (CheckUseOut())
        {
            // 得点計算を行う
        }
    }

    bool isChi()
    {
        return (cntChi > 0) ? true : false;
    }

    bool isPon()
    {
        return (cntPon > 0) ? true : false;
    }
    // 鳴いたかどうか
    bool isNaki()
    {
        if (isChi() | isPon())
            return true;
        return false;
    }
}
