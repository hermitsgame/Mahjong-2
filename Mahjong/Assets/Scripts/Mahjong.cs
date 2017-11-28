using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Mahjong : MonoBehaviour
{

    // 自分の風
    [SerializeField]
    protected PlaySide playSide = PlaySide.TON;

    protected int[] Hand = new int[38];

    public int[] tmp = new int[38];

    public int[] Pungs = new int[4];// 刻子
    public int[] Chows = new int[4];// 順子
    public int[] exPungs = new int[4];// ポンした牌
    public int[] exChows = new int[4];// チーした牌
    public int income;// ツモった牌
    public int head, cntPungs, cntChows;// 雀頭、刻子のカウント、順子のカウント

    // ポン、チーの回数
    int cntPon, cntChi, cntKan;
    /// <summary>
    /// ポンができる場合にポンする
    /// </summary>
    /// <param name="extra">相手の捨て牌</param>
    void Pon(int extra)
    {
        // 相手の捨て牌をツモ牌として刻子を1つ判定する
        income = extra;
        exPungs[cntPon] = income;// ポンした牌を記録
        CheckPung(1);
        cntPon++;
    }
    /// <summary>
    /// チーができる場合にチーする
    /// </summary>
    /// <param name="extra">相手の捨て牌</param>
    void Chi(int extra)
    {
        // 相手の捨て牌をツモ牌として順子を1つ判定する
        income = extra;
        exChows[cntChi] = income;// チーした牌を記録
        CheckChow();
        cntChi++;
    }

    void Kan(int extra)
    {
        income = extra;
        exPungs[cntKan] = income;
        CheckChow();
        cntKan++;
        FieldManager.CurrentPlayer = playSide;
    }


    /*
    // 配られた牌をリスト形式の牌としてカウントする 使わないかも
    public void ToListable()
    {
        this.Hand = new int[38];
        for (int i = 0; i < serialHand.Length; i++)
        {
            Hand[serialHand[i]]++;
        }
        ResetTmp();//判定用の変数をリセット
    }
    */
    public void TumoCheck()
    {
        ResetTmp();
        // フリテンの判定
        // 国士無双の判定
        if (CheckKokushi())
        {
            print("kokushi");
            return;
        }
        // 通常の役判定
        for (int n = 0; n < tmp.Length; n++)
        {
            // 判定用の変数リセット
            ResetTmp();
            // 候補となる雀頭ごとに判定する
            if (tmp[n] >= 2)
            {
                tmp[n] -= 2; head = n;
                // 刻子→順子の順番に判定
                CheckPung(0);
                CheckChow();
                if (CheckUseOut())
                {
                    // 得点計算を行う
                    if (CheckYakuman())
                    {
                        print("yakuman found");
                        return;
                    }
                    CheckNormalYaku();
                    return;
                }
                ResetTmp();
                tmp[n] -= 2; head = n;
                // 順子→刻子の順番に判定
                CheckChow();
                CheckPung(0);
                if (CheckUseOut())
                {
                    // 得点計算を行う
                    if (CheckYakuman())
                    {
                        print("yakuman found");
                        return;
                    }
                    CheckNormalYaku();
                    return;
                }
                ResetTmp();
                tmp[n] -= 2; head = n;
                // 刻子1個→順子→刻子の順番に判定
                CheckPung(1);
                CheckChow();
                CheckPung(0);
                if (CheckUseOut())
                {
                    // 得点計算を行う
                    if (CheckYakuman())
                    {
                        print("yakuman found");
                        return;
                    }
                    CheckNormalYaku();
                    return;
                }
            }
        }
    }
    /// <summary>
    /// 刻子の判定
    /// </summary>
    /// <param name="num">指定した数の刻子を抽出 0以下なら取り出せるだけ抽出</param>
    void CheckPung(int num)
    {
        for (int i = 0; i < tmp.Length; i++)
        {
            if (tmp[i] >= 3)
            {
                tmp[i] -= 3;
                Pungs[cntPungs++] = i;
                if (num > 0 && cntPungs == num) return;     // 一個だけ抽出
            }
        }
    }

    // 順子の判定
    void CheckChow()
    {
        // 字牌はチェックしない
        for (int i = 0; i < tmp.Length - 7; i++)
        {
            // 8以上は順子になりえない
            if (i % 10 > 7) continue;
            for (; tmp[i] > 0 && tmp[i + 1] > 0 && tmp[i + 2] > 0;)
            {
                tmp[i]--; tmp[i + 1]--; tmp[i + 2]--;
                Chows[cntChows++] = i;
            }
        }
    }
    // 全部使い切ってたらtrueを返す関数
    bool CheckUseOut()
    {
        for (int i = 0; i < tmp.Length; i++) if (tmp[i] > 0) return false;
        return true;
    }
    /// <summary>
    /// valueに一致するものがcheckvalにあればtrueを返す
    /// </summary>
    /// <param name="value"></param>
    /// <param name="checkval"></param>
    /// <returns></returns>
    bool CheckWhichEver(int value, params int[] checkval)
    {
        for (int i = 0; i < checkval.Length; i++)
        {
            if (value == checkval[i])
                return true;
        }
        return false;
    }
    // 判定用の変数リセット
    public void ResetTmp()
    {
        Hand.CopyTo(tmp, 0);
        this.Pungs = new int[4];
        this.Chows = new int[4];
        cntChows = cntPungs = head = 0;
    }
    // 場風を取得
    int Chanfon()
    {
        return (byte)FieldManager.FieldSide;
    }
    // 自風を取得
    int Menfon()
    {
        return (byte)playSide;
    }
    // tmp手牌確認用関数
    public void ShowTmp()
    {
        for (int i = 0; i < tmp.Length; i++) print("tmp " + i + " : " + tmp[i]);
    }

    public void ShowMentu()
    {
        for (int i = 0; i < Pungs.Length; i++) print("pungs " + i + " : " + tmp[i]);
        for (int i = 0; i < Chows.Length; i++) print("chows" + i + " : " + tmp[i]);
    }

}
