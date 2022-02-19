using UnityEngine;

public class WorldAudioMgr : MonoBehaviour
{
    public AudioClip truckAudio;

    AudioSource audioSource;

    private PlayerController playerController;


    public void Init()
    {
        playerController = NonBattleMgr.Instance.playerController;

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = truckAudio;
    }



    public void AudioUpdate()
    {
        if (playerController.agent.velocity.magnitude > 0f)
        {
            if (audioSource.isPlaying) return;

            audioSource.Play();
        }
        else
        {
            audioSource.Stop();
        }
    }
}
