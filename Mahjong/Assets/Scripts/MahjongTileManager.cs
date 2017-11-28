using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType:byte
{
    M1 = 1,M2,M3,M4,M5,M6,M7,M8,M9,
    P1 = 11,P2,P3,P4,P5,P6,P7,P8,P9,
    S1 = 21,S2,S3,S4,S5,S6,S7,S8,S9,
    TON = 31,NAN = 32,XIA = 33,PEI = 34,
    HAKU = 35,HATU = 36,CHUN = 37
}
public class Tile:MonoBehaviour
{
    public TileType type;
    public bool isAkadora = false;
}
public class MahjongTileManager : MonoBehaviour{

    // 生成する牌のGameObject
    [SerializeField]
    GameObject tilePrefab;
    // 牌のテクスチャ
    [SerializeField]
    Material[] tileMaterials;
    Vector3 tileSize;
    
    int[] allTiles = new int[136];
    List<GameObject> AllTileObj = new List<GameObject>();
    // 牌山の一番左上の牌の座標を格納
    [SerializeField]
    Vector3 YamaHaipos;

    Transform[] tileField;

    public int CurrentTumoTile = 0;

    void Start()
    {

    }

    // 牌や山、手持ちの牌などの初期化処理
    public void InitTileManager()
    {
        tileSize = tilePrefab.transform.GetChild(0).GetComponent<Renderer>().bounds.size;
        InitTiles();
        SetInitialTiles();
    }
    // 山の牌を初期化する
    private void InitTiles()
    {
        int[] tmp = new int[136];
        int cnt = 0;
        for (int i = 1; i < 38; i++)
        {
            if (i % 10 != 0)
                for (int j = 0; j < 4; j++) {
                    tmp[cnt++] = i;
                }
        }
        // 牌をシャッフルする
        allTiles = Shuffle<int>(tmp);
    }
    // 牌山のObject生成
    private void SetInitialTiles()
    {
        int n = 0;
        Vector3 offset = Vector3.zero;
        Vector3 angle = Vector3.zero;
        angle.x = 180.0f;
        for (int i = 0; i < 4; i++)
        {
            for (int col = 0; col < 17; col++)
            {
                offset.x = - col * tileSize.x;offset.y = 0;
                AllTileObj[n] = InstantiateTile(tilePrefab, allTiles[n], YamaHaipos + offset,angle, tileField[i]);
                n++;
                offset.y = -tileSize.y;
                AllTileObj[n] = InstantiateTile(tilePrefab, allTiles[n], YamaHaipos + offset,angle, tileField[i]);
                n++;
            }
        }
    }

    /*
    public int[] GetInitialHand()
    {
        int[] hand = new int[14];
        for(int i = 0;i < hand.Length-1;i++)

    }*/


    // 指定した手持ち牌を生成する
    public void InitTile(int[] distributed,Transform playerPos)
    {
        System.Array.Sort(distributed);
        Vector3 initPos = new Vector3(-distributed.Length/ 2 * tileSize.x, 0,0);
        for (int i = 0; i < distributed.Length; i++)
        {
            /*
            GameObject g = InstantiateTile(tilePrefab, distributed[i], playerPos) as GameObject;
            g.transform.localPosition = initPos + new Vector3(tileSize.x * i, 0, 0);
            g.transform.localEulerAngles = new Vector3(-90, 0, 0);
            */
        }
    }

    private T[] Shuffle<T>(T[] array)
    {
        var length = array.Length;
        var result = new T[length];
        Array.Copy(array, result, length);

        var random = new System.Random();
        int n = length;
        while (1 < n)
        {
            n--;
            int k = random.Next(n + 1);
            var tmp = result[k];
            result[k] = result[n];
            result[n] = tmp;
        }
        return result;
    }

    // 指定した座標、回転位置、フィールドに指定したIDの牌を生成する関数
    private GameObject InstantiateTile(GameObject tile,int id,Vector3 pos,Vector3 rot,Transform field)
    {
        GameObject obj = Instantiate(tile, field) as GameObject;
        obj.transform.localPosition = pos;
        obj.transform.localEulerAngles = rot;
        Renderer rend = obj.transform.GetChild(0).GetComponent<Renderer>();
        Material[] mats = rend.materials;
        mats[2] = new Material(this.tileMaterials[id]);
        rend.materials = mats;
        obj.name = id.ToString();
        obj.AddComponent<Tile>().type = (TileType)id;
        return obj;
    }
}
