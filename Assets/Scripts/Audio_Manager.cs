using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio_Manager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip introMusic;
    public AudioClip ghostNormalMusic;
    // Start is called before the first frame update
    void Start()
    {
        PlayIntroMusic();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void PlayIntroMusic()
    {
        audioSource.clip = introMusic;
        audioSource.Play();

        Invoke("PlayGhostMusic", 10f);
    }

    private void PlayGhostMusic()
    {
        audioSource.Stop();
        audioSource.clip = ghostNormalMusic;
        audioSource.Play();
    }
}
