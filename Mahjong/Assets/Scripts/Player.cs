using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum PlayerState
{
    TUMO,
    CUT_TILE,
    ARRANGE,
    END_TURN
}
public class Player : Mahjong {

    MahjongTileManager tileManager;

    [SerializeField]
    private Text playsideText;

    [SerializeField]
    private List<GameObject> HandObj = new List<GameObject>();

    private Transform handField;

    private PlayerState state = PlayerState.TUMO;

    // Use this for initialization
    void Start () {
        // ランダムな牌を取得
        //this.Hand = tileManager.
        // 牌を生成する
        //tileManager.Generate(this.serialHand, this.transform);

        // 生成した牌を判別しやすい形に
        income = 3;

        // 手持ち牌の面子チェック
        //TumoCheck();
    }
	
	// Update is called once per frame
	void Update () {
        if (GameController.GameStateProp == State.GAME)
        {
            switch (this.state)
            {
                case PlayerState.TUMO:
                    this.ArrangeHand();
                    this.state = PlayerState.CUT_TILE;
                    break;
                case PlayerState.CUT_TILE:
                    if (Input.GetMouseButtonDown(0))
                    {

                    }
                    break;
            }
        }
		
	}

    public void Init()
    {
        tileManager = GameObject.Find("FieldManager").GetComponent<MahjongTileManager>();
        this.handField = this.transform.Find("Hand");
    }

    /// <summary>
    /// 牌を1枚ツモってくる
    /// </summary>
    /// <param name="hai">取得する牌</param>
    public void TumoHai()
    {
        GameObject hai = tileManager.GetTileObj();
        int type = (int)hai.GetComponent<Tile>().type;
        //print(string.Format("hand type{0} : num{1}", type,Hand[type]));
        this.Hand[type]++;

        this.HandObj.Add(hai);
        this.AdjustHaipos(hai.transform, HandObj.Count);

        this.income = type;
    }
    // 複数枚の牌をツモってくる
    public void TumoHai(int num)
    {
        for (int i = 0; i < num; i++)
            this.TumoHai();
    }

    private void AdjustHaipos(Transform hai,int num)
    {
        hai.SetParent(this.handField);
        Vector3 pos = Vector3.zero;
        pos.x = tileManager.GetTileSize.x * (num - 14);
        if (num == 14) pos.x = tileManager.GetTileSize.x;
        hai.localPosition = pos;

        Vector3 rot = this.transform.eulerAngles;
        rot.x = -90;
        hai.eulerAngles = rot;
    }
    // リーパイする
    private void ArrangeHand()
    {
        HandObj.Sort((a, b) =>
            (int)a.GetComponent<Tile>().type - (int)b.GetComponent<Tile>().type);

        for (int i = 0; i < HandObj.Count; i++)
        {
            AdjustHaipos(HandObj[i].transform, i);
        }
    }

    /// <summary>
    /// 選択した牌を切る動作を行う
    /// </summary>
    private void SelectTile()
    {
    }

    public PlaySide PlaysideProp
    {
        get { return this.playSide; }
        set
        {
            this.playSide = value;
            switch (this.playSide)
            {
                case PlaySide.TON:
                    this.playsideText.text = "東";
                    break;
                case PlaySide.NAN:
                    this.playsideText.text = "南";
                    break;
                case PlaySide.XIA:
                    this.playsideText.text = "西";
                    break;
                case PlaySide.PEI:
                    this.playsideText.text = "北";
                    break;
                default:
                    Debug.LogError("Error Playsideprop");
                    break;
            }
        }
    }
}
