using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum State
{
    HAIPAI,
    GAME,
    RESULT,
}
public class GameController : MonoBehaviour {

    private State state = State.HAIPAI;
    [SerializeField]
    private Player[] players;

    [SerializeField]
    private int PlayerNum = 4;

    public static int ParentPlayer;
    public static int InitTumoPos; // 最初に牌をツモる位置

    private MahjongTileManager tileManager;

    // Use this for initialization
    void Start() {
        tileManager = GameObject.Find("FieldManager").GetComponent<MahjongTileManager>();
        InitGame();

    }

    // Update is called once per frame
    void Update() {

    }
    private void InitGame()
    {
        DecidePlaySide();
        tileManager.InitTileManager();
        DecideInitalTumoPos();
    }

    // 各プレイヤーの風を決定、プレイ順に並び替え
    private void DecidePlaySide()
    {
        int dice1 = Random.Range(1, 7);
        int dice2 = Random.Range(1, 7);
        int sum = dice1 + dice2;
        // 出目に応じてプレイサイドを決定
        for (int i = 0; i < PlayerNum; i++)
        {
            int side = (i + 1) + 30;
            this.players[(sum - 1 + i) % PlayerNum].PlaysideProp = (PlaySide)side;
            if (PlaySide.TON == (PlaySide)side)ParentPlayer = (sum - 1 + i) % PlayerNum;
        }
        print("parent " + ParentPlayer);
    }
    // サイコロを振り、最初の牌を取る位置を決定、牌の順番を並び替え
    private void DecideInitalTumoPos()
    {
        int dice1 = Random.Range(1, 7);
        int dice2 = Random.Range(1, 7);
        int sum = dice1 + dice2;int yama = (ParentPlayer + sum - 1) % PlayerNum;

        print(string.Format("yama {0} sum {1}", yama, sum));
        if (yama == 1) yama = 3; else if (yama == 3) yama = 1;
        InitTumoPos = yama * 34 + sum * 2;
        print("pos " + InitTumoPos);
    }

    // utility
    /// <summary>
    /// indexで指定した要素を先頭要素として並べ替える
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <param name="index"></param>
    private void ArrangeArray<T>(T[] array,int index)
    {
        T[] tmp = new T[array.Length];
        array.CopyTo(tmp, 0);
        int len = array.Length;
        for (int i = 0; i < len - index ; i++)
        {
            array[i] = tmp[index + i];
        }
        for (int j = 0; j < index; j++)
        {
            array[len - index + j] = tmp[j];
        }
    }
}
