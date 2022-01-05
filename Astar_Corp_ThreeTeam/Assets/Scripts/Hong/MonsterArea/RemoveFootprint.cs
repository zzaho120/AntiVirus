using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveFootprint : MonoBehaviour
{
    private new SpriteRenderer renderer;
    private float fadeTime = 2f;

    private float timer = 0;

    private FootprintPool footprintPool;

    void Start()
    {
        //renderer = GetComponentInChildren<SpriteRenderer>();
        //StartCoroutine(Fadeout(fadeTime));

        footprintPool = GameObject.Find("FootprintPool").GetComponent<FootprintPool>();
        //StartCoroutine(ReturnObj());

        //Invoke("ReturnToPool", 0.8f);
        //Destroy(gameObject, fadeTime + 3f);
    }

    private void Update()
    {
        //Debug.Log(timer);
        if (gameObject.activeInHierarchy)
        {
            timer += Time.deltaTime;
        }

        if (timer >= 3f)
        {
            timer = 0;
            footprintPool.pools[0].Pool.Release(gameObject);
        }
    }

    //void ReturnToPool()
    //private IEnumerator ReturnObj()
    //{
    //    yield return new WaitForSeconds(3f);
    //
    //    footprintPool.pools[0].Pool.Release(gameObject);
    //    Debug.Log("Return");
    //}

    private IEnumerator Fadeout(float second)
    {
        yield return new WaitForSeconds(second);

        float fadeCount = renderer.color.a;
        while (fadeCount > 0f)
        {
            fadeCount -= 0.01f;
            yield return new WaitForSeconds(0.01f);
            renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, fadeCount);
        }
    }
}
