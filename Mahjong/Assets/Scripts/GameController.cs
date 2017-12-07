using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    HAIPAI,
    GAME,
    RESULT,
}
public class GameController : MonoBehaviour {

    private static State state = State.HAIPAI;
    [SerializeField]
    private float HaipaiInterval = 0.2f;
    [SerializeField]
    private Player[] players;
    private Camera[] cameras = new Camera[4];


    [SerializeField]
    private int PlayerNum = 4;

    public static int ParentPlayer;
    public static int InitTumoPos; // 最初に牌をツモる位置

    private MahjongTileManager tileManager;

    // Use this for initialization
    void Start() {
        state = State.HAIPAI;
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
        StartCoroutine(Haipai());
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
            this.players[(sum - 1 + i) % PlayerNum].Init();
            if (PlaySide.TON == (PlaySide)side)ParentPlayer = (sum - 1 + i) % PlayerNum;
        }
        for (int i = 0; i < cameras.Length; i++)
            this.cameras[i] = this.players[i].GetCamera;

        SwitchCamera(ParentPlayer);
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
    IEnumerator Haipai()
    {
        for (int cnt = 0; cnt < 3; cnt++)
        {
            for (int i = 0; i < players.Length; i++)
            {
                this.players[(ParentPlayer + i) % players.Length].TumoHai(4);
                yield return new WaitForSeconds(HaipaiInterval);
            }
        }
        for (int i = 0; i < players.Length; i++)
        {
            this.players[(ParentPlayer + i) % players.Length].TumoHai();
            yield return new WaitForSeconds(HaipaiInterval);
        }
        for (int i = 0; i < players.Length; i++)
        {
            this.players[(ParentPlayer + i) % players.Length].ArrangeHand();
        }
        state = State.GAME;
    }

    public void SwitchCamera(int id)
    {
        foreach (Camera c in cameras)
            c.enabled = false;
        cameras[id].enabled = true;
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

    public static State GameStateProp
    {
        get { return state; }
        set { state = value; }
    }
}
