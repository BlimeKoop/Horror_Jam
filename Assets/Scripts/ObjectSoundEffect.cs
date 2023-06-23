using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectSoundEffect : MonoBehaviour
{
    [SerializeField] private AudioClip[] customSound;
    AudioSource playCustomSound;
    //int customCounter;

    void Start()
    {
        playCustomSound = GetComponent<AudioSource>();
        //customCounter = customSound.Count();
    }

    public void PlayCustomSound(int customCounter)
    {
        switch (customCounter)
        {
            case 0:
                playCustomSound.clip = customSound[0];
                playCustomSound.Play();
                break;
                case 1:
                playCustomSound.clip = customSound[1];
                playCustomSound.Play();
                break;
                

        }
    }
}
