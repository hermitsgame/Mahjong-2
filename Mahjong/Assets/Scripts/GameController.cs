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
        DecideInitalTumoPos();
        tileManager.InitTileManager();
    }

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
        }
    }
    // サイコロを振り、最初の牌を取る位置を決定する
    private void DecideInitalTumoPos()
    {
        int dice1 = Random.Range(1, 7);
        int dice2 = Random.Range(1, 7);
        int sum = dice1 + dice2;int yama = (sum - 1) % PlayerNum;int pos = yama * 34 + sum * 2;

        print(string.Format("yama {0} sum {1} pos {2}", yama, sum, pos));
        if (yama == 1) yama = 3; else if (yama == 3) yama = 1;
    }
}
