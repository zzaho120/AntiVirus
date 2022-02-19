using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour
{
    private Button[] buttons;
    //public AudioClip buttonAudio;

    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        buttons = GetComponentsInChildren<Button>();
        foreach (var button in buttons)
        {
            button.onClick.AddListener(delegate { PlayButtonSound(); });
        }
    }

    private void PlayButtonSound()
    {
        //Debug.Log("Button Clicked");
        audioSource.Play();
        StartCoroutine(StopSound());
    }

    private IEnumerator StopSound()
    {
        yield return new WaitForSeconds(0.1f);
        audioSource.Stop();
    }
}
