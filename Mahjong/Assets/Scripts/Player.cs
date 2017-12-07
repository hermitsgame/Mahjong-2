using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum PlayerState
{
    TUMO,
    CUT_TILE,
    END_TURN
}
public class Player : Mahjong {

    MahjongTileManager tileManager;

    [SerializeField]
    private Text playsideText;

    [SerializeField]
    private List<GameObject> HandObj = new List<GameObject>();

    [SerializeField]
    private List<GameObject> TrashObj = new List<GameObject>();

    private Transform handField, trashField;

    private PlayerState state = PlayerState.TUMO;

    private Camera myCamera;

    [SerializeField]
    private float TumoTime = 0.1f;

    // Use this for initialization
    void Start() {
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
    void Update() {
        if (GameController.GameStateProp == State.GAME && FieldManager.IsMyTurn(this.PlaysideProp))
        {
            switch (this.state)
            {
                case PlayerState.TUMO:
                    this.TumoHai();
                    this.TumoCheck();
                    this.state = PlayerState.CUT_TILE;
                    break;
                case PlayerState.CUT_TILE:
                    if (Input.GetMouseButtonDown(0))
                    {
                        CutTile();
                    }
                    break;
                case PlayerState.END_TURN:
                    this.state = PlayerState.TUMO;
                    break;
                default:
                    break;
            }
        }

    }

    public void Init()
    {
        tileManager = GameObject.Find("FieldManager").GetComponent<MahjongTileManager>();
        this.handField = this.transform.Find("Hand");
        this.trashField = this.transform.Find("Trash");
        this.myCamera = this.transform.GetComponentInChildren<Camera>();
    }

    /// <summary>
    /// 牌を1枚ツモってくる
    /// </summary>
    /// <param name="hai">取得する牌</param>
    public void TumoHai()
    {
        GameObject hai = tileManager.GetTileObj();
        Tile tile = hai.GetComponent<Tile>();
        int type = (int)tile.type;
        tile.isHand = true;
        //print(string.Format("hand type{0} : num{1}", type,Hand[type]));
        this.Hand[type]++;

        this.HandObj.Add(hai);
        this.ToHandPos(hai.transform, HandObj.Count, TumoTime);

        this.income = type;

    }
    // 複数枚の牌をツモってくる
    public void TumoHai(int num)
    {
        for (int i = 0; i < num; i++)
            this.TumoHai();
    }

    public void TrashHai(GameObject hai)
    {
        Tile tile = hai.GetComponent<Tile>();
        tile.isHand = false;
        int type = (int)tile.type;
        this.Hand[type]--;
        this.HandObj.Remove(hai);
        
        this.TrashObj.Add(hai);
        /*
        TrashObj.ForEach((GameObject g) =>
        {
            print(g.GetComponent<Tile>().type);
        });
        */
    }

    void ToHandPos(Transform hai, int num, float duration)
    {
        hai.SetParent(this.handField);
        Vector3 destPos = Vector3.zero;
        destPos.x = tileManager.GetTileSize.x * (num - 14);
        if (num == 14) destPos.x = tileManager.GetTileSize.x;

        hai.localPosition = destPos;

        Vector3 destRot = Vector3.zero;
        destRot.x = -90;

        //StartCoroutine(Rotate(hai, destRot, duration));
        hai.localEulerAngles = Vector3.zero;
    }

    void ToTrashPos(Transform hai, float duration)
    {
        hai.SetParent(trashField);
        hai.localEulerAngles = Vector3.zero;
        int tileCnt = this.TrashObj.Count;
        int row = tileCnt / 6; int col = tileCnt % 6;

        this.TrashHai(hai.gameObject);

        Vector3 destPos = Vector3.zero;
        destPos.x = col * tileManager.GetTileSize.x;
        destPos.z = - row * tileManager.GetTileSize.z;

        StartCoroutine(Move(hai, destPos, duration));

        ArrangeHand();
    }

    IEnumerator Move(Transform hai, Vector3 dest, float duration)
    {
        float startTime = Time.time;
        Vector3 startPos = hai.localPosition;
        for (; Time.time - startTime < duration;)
        {
            hai.localPosition = Vector3.Lerp(startPos, dest, (Time.time - startTime) / duration);
            yield return null;
        }
        hai.localPosition = dest;
    }
    IEnumerator Rotate(Transform hai, Vector3 dest, float duration)
    {
        float startTime = Time.time;
        Vector3 startRot = Vector3.zero;
        for (; Time.time - startTime < duration;)
        {
            hai.localEulerAngles = Vector3.Lerp(startRot, dest, (Time.time - startTime) / duration);
            yield return null;
        }
        hai.localEulerAngles = dest;
    }
    // リーパイする
    public void ArrangeHand()
    {
        HandObj.Sort((a, b) =>
            (int)a.GetComponent<Tile>().type - (int)b.GetComponent<Tile>().type);

        for (int i = 0; i < HandObj.Count; i++)
        {
            ToHandPos(HandObj[i].transform, i,0f);
        }
    }

    /// <summary>
    /// 選択した牌を切る動作を行う
    /// </summary>
    private void CutTile()
    {
        Ray ray = myCamera.ScreenPointToRay(Input.mousePosition);

        int layerMask = 1 << 8;

        Debug.DrawRay(ray.origin, ray.direction * 20.0f, Color.red,2.0f);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 20.0f, layerMask))
        {
            if (hit.collider.CompareTag("Tile"))
            {
                print("Cut " + this.gameObject.name);
                Tile t = hit.collider.GetComponentInParent<Tile>();
                if (t.isHand)
                {
                    ToTrashPos(t.transform, 0.2f);

                    EndTurn();
                }
            }
        }
    }

    private void EndTurn()
    {
        FieldManager.PassMyTurn(this.playSide);
        GameController gc = GameObject.Find("GameController").GetComponent<GameController>();
        gc.SwitchCamera(gc.GetNextPlayerID(this.playSide));
        this.state = PlayerState.END_TURN;
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
    public Camera GetCamera
    {
        get { return this.myCamera; }
    }
}
