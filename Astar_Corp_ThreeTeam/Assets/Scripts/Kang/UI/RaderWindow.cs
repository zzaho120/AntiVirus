using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaderWindow : GenericWindow
{
    public RectTransform raderImage;
    public List<Image> raderLevels;
    public static readonly float maxTime = 3f;
    public float alphaSpeed;
    public override void Open()
    {
        base.Open();
        raderImage.anchoredPosition = new Vector2(0, 0);
        raderImage.rotation = Quaternion.identity;
        raderImage.localScale = Vector3.one;
    }

    public override void Close()
    {
        base.Close();
    }

    public void StartRader(Vector3 player, Vector3 monster, int level)
    {
        var dir = (monster - player).normalized;
        var newPos = new Vector2(dir.x, dir.z);
        var rot = Quaternion.FromToRotation(raderImage.up, newPos);

        rot.x = 0;
        rot.y = 0;

        raderImage.rotation = rot;

        raderImage.position = Camera.main.WorldToScreenPoint(player);
        raderImage.anchoredPosition += newPos * 200;

        StartCoroutine(CoAlphaRader(level));
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

        CameraController.Instance.SetFollowObject(null);
        Close();
    }
}
