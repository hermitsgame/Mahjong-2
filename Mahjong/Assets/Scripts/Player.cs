using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Mahjong {

    MahjongTileManager tileManager;

    [SerializeField]
    private Text playsideText;

    [SerializeField]
    private List<GameObject> HandObj = new List<GameObject>();
    
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
		
	}

    public void Init()
    {
        tileManager = GameObject.Find("FieldManager").GetComponent<MahjongTileManager>();
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
        hai.SetParent(this.transform);
        Vector3 pos = Vector3.zero;
        pos.x = tileManager.GetTileSize.x * (num - 14);
        hai.localPosition = pos;

        Vector3 rot = this.transform.eulerAngles;
        rot.x = -90;
        hai.eulerAngles = rot;
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
