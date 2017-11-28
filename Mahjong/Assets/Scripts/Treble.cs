using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treble : MonoBehaviour {

    [SerializeField]
    private float time;

    [SerializeField]
    private Vector3 maxScale;
    [SerializeField]
    private Vector3 minScale;
    Coroutine coro;
    Transform field;

	// Use this for initialization
	void Start () {
        field = this.GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.S)) {
            print("push S");
            if (this.coro == null)
            {
                print("StartCoroutine");
                this.coro = StartCoroutine(this.Tremble());
            }
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            print("push P");
            if (this.coro == null)
            {
                print("StopCoroutine");
                StopCoroutine(this.coro);
                this.coro = null;
            }
        }
    }

    IEnumerator Tremble()
    {
        float timer = 0;
        for (; timer < time;)
        {
            field.localScale = Vector3.Lerp(minScale, maxScale, timer / time);
            timer += Time.deltaTime;
            yield return null;
        }
        timer = 0;
        Vector3 min = this.minScale;
        min.y *= -1;
        Vector3 max = this.maxScale;
        max.y *= -1;
        for (; timer < time;)
        {
            field.localScale = Vector3.Lerp(min, max, timer / time);
            timer += Time.deltaTime;
            yield return null;
        }
        this.coro = StartCoroutine(Tremble());
    }
}
