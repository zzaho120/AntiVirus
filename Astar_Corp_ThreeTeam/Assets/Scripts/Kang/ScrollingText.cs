using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScrollingText : MonoBehaviour
{
    public float time;
    public TextMeshPro scrollingText;
    private Transform target;

    public void SetDamage(int damage, bool isCritical)
    {
        if (isCritical)
        {
            scrollingText.color = Color.yellow;
            scrollingText.text = $"{damage}!!";
        }

        else
        {
            scrollingText.color = Color.red;
            scrollingText.text = $"{damage}";
        }
        target = Camera.main.transform;
        StartCoroutine(CoScrolling());
    }

    public void SetRecovery(int recovery, Color color)
    {
        scrollingText.color = color;
        scrollingText.text = $"{recovery}";
        target = Camera.main.transform;
        StartCoroutine(CoScrolling());
    }
    public void SetMiss()
    {
        scrollingText.color = Color.red;
        scrollingText.text = $"MISS!";
        target = Camera.main.transform;
        StartCoroutine(CoScrolling());
    }

    public IEnumerator CoScrolling()
    {
        var timer = 0f;
        while (timer < time)
        {
            timer += Time.deltaTime;

            transform.position += Vector3.up * Time.deltaTime;

            transform.rotation = Quaternion.LookRotation(target.forward);

            yield return null;
        }

        var returnToPool = GetComponent<ReturnToPool>();
        returnToPool.Return();
    }
}
