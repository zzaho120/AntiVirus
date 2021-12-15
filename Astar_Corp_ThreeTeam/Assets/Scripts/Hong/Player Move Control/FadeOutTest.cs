using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeOutTest : MonoBehaviour
{
    private Image image;
    private float fadeTime = 2f;

    private void Awake()
    {
        image = GetComponent<Image>();
        gameObject.SetActive(false);
    }

    public void StartFade()
    {
        Debug.Log("Start Fade");
        gameObject.SetActive(true);
        StartCoroutine(FadeInOut());
    }

    private IEnumerator Fade(float start, float end)
    {
        float currentTime = 0.0f;
        float percent = 0.0f;

        while (currentTime < 2f)
        {
            currentTime += Time.deltaTime;
            percent = currentTime / fadeTime;           // 나누기 안해주면 무조건 1초동안 실행됨

            Color color = image.color;
            color.a = Mathf.Lerp(start, end, percent);
            image.color = color;

            yield return null;
        }
    }

    private IEnumerator FadeInOut()
    {
        while (true)
        {
            yield return StartCoroutine(Fade(0, 1f));       // Fade Out
            //yield return StartCoroutine(Fade(1, 0));        // Fade In
            break;
        }
    }
}
