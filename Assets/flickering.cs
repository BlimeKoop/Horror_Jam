using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flickering : MonoBehaviour
{
    bool isFlickering = false;
    float timeDelay = 3f;
    public float minLength = 0.01f;
    public float maxLength = 1f;

    public AudioClip flickerSFX;
    
    AudioSource lightEffect;

    Light objLight;

    void Start()
    {
        objLight = GetComponent<Light>();
        lightEffect = gameObject.GetComponent<AudioSource>();
        lightEffect.clip = flickerSFX;
        if (isFlickering == false)
        {
            StartCoroutine(FlickeringLight());
        }
    }
        IEnumerator FlickeringLight(){
        isFlickering = true;
        lightEffect.Play();
        objLight.enabled = false;
        timeDelay = Random.Range(minLength, maxLength);
        yield return new WaitForSeconds(timeDelay);
        lightEffect.Stop();
        objLight.enabled = true;
        timeDelay = Random.Range(minLength, maxLength);
        yield return new WaitForSeconds(timeDelay);
        isFlickering = false;
        StartCoroutine(FlickeringLight());
    }
}
