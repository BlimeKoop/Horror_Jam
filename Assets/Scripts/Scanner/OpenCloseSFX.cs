using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCloseSFX : MonoBehaviour
{
    AudioSource scannerSFX;

    [SerializeField]
    AudioClip []soundFX;

    public bool isOpening = false;


    void Start(){
        scannerSFX = GetComponent<AudioSource>();
    }

    public void PlaySound(){
        switch(isOpening){
            case true:
            //Open SFX
                scannerSFX.clip = soundFX[0];
                scannerSFX.Play();
            break;
            case false:
            //Close SFX
            scannerSFX.clip = soundFX[1];
                scannerSFX.Play();
            break;
        }
    }
}
