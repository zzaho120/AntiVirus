using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaderWindow : GenericWindow
{
    public RectTransform raderImage;
    public List<Image> raderLevels;
    public float maxTime;
    public float alphaSpeed;
    public override void Open()
    {
        base.Open();
    }

    public override void Close()
    {
        base.Close();
    }

    public IEnumerator CoAlphaRader(int level)
    {
        var timer = 0f;
        var alpha = 0f;
        var alphaTrigger = false;
        while (timer < maxTime)
        {
            timer += Time.deltaTime;
            if (!alphaTrigger)
                alpha += Time.deltaTime * alphaSpeed;
            else
                alpha -= Time.deltaTime * alphaSpeed;

            if (alpha >= 1.0f)
                alphaTrigger = true;
            else if (alpha <= 0f)
                alphaTrigger = false;

            alpha = Mathf.Clamp(alpha, 0f, 1.0f);

            for (var idx = 0; idx < level; ++idx)
            {
                var color = raderLevels[idx].color; 
                raderLevels[idx].color = new Color(color.r, color.g, color.b, alpha);
            }

            yield return null;
        }

        Close();
    }

    public void StartRader(Vector3 player, Vector3 monster, int level)
    {
        var dir = player - monster;

        var rot = Quaternion.FromToRotation(raderImage.up, dir);

        raderImage.rotation = rot;

        var newPos = new Vector2(dir.x, dir.z).normalized;
        Debug.Log(newPos);
        raderImage.anchoredPosition -= newPos * 200;

        StartCoroutine(CoAlphaRader(level));
    }
}
