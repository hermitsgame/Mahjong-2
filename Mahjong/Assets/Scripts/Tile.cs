using UnityEngine;
using System.Collections;

public class Tile : UnityEngine.MonoBehaviour
{
    public TileType type;
    public bool isAkadora = false;
    public bool isHand = false;
    public PlaySide tileOwner;

    public IEnumerator Move(Vector3 dest, float duration)
    {
        float startTime = Time.time;
        Vector3 startPos = this.transform.localPosition;
        for (; Time.time - startTime < duration;)
        {
            this.transform.localPosition = Vector3.Lerp(startPos, dest, (Time.time - startTime) / duration);
            yield return null;
        }
        this.transform.localPosition = dest;
    }
    public IEnumerator Rotate(Vector3 dest, float duration)
    {
        float startTime = Time.time;
        Vector3 startRot = Vector3.zero;
        for (; Time.time - startTime < duration;)
        {
            this.transform.localEulerAngles = Vector3.Lerp(startRot, dest, (Time.time - startTime) / duration);
            yield return null;
        }
        this.transform.localEulerAngles = dest;
    }
}