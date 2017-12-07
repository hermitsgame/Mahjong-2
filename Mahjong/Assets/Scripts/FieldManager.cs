using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PlaySide : byte
{
    TON = 31, NAN=32, XIA=33, PEI=34
}
// 場の状態を管理する
public class FieldManager :MonoBehaviour{
    
    // 場の風
    static PlaySide fieldSide = PlaySide.TON;

    // 現在の手番の人の風
    static PlaySide currentPlayer = PlaySide.TON;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    /// <summary>
    /// 次のプレイヤーの順番にする
    /// </summary>
    /// <param name="player"></param>
    public static void PassMyTurn(PlaySide player)
    {
        int next = (byte)player;
        next++;
        if (next == 35) next = 31;
        CurrentPlayer = (PlaySide)next;
    }
    /// <summary>
    /// プレイヤーの手番かどうかを返す
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public static bool IsMyTurn(PlaySide player)
    {
        return (player == FieldManager.currentPlayer)?true:false;
    }

    public static PlaySide FieldSide
    {
        get { return fieldSide; }
    }
    public static PlaySide CurrentPlayer
    {
        get { return currentPlayer; }
        set { currentPlayer = value; }
    }
}
