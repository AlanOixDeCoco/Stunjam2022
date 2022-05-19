using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Sounds
{
    DOOR,
    STARTUP,
    RADIO,
    MUSIC
}

public class WindshieldController : MonoBehaviour
{

    private AudioSource[] audioSource;
    public Animator wipersAnimator;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponents<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDoor()
    {
        audioSource[(int)Sounds.DOOR].Play();
    }

    void OnStartup()
    {
        audioSource[(int)Sounds.STARTUP].Play();
    }

    void OnRadio()
    {
        audioSource[(int)Sounds.RADIO].Play();
    }

    void OnMusic()
    {
        audioSource[(int)Sounds.MUSIC].Play();
    }

    void OnRoundStart()
    {
        wipersAnimator.SetTrigger("Start");
        GameManager.Instance.StartRound();
    }
}
