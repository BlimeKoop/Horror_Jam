using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorkboardController : MonoBehaviour
{
    private Rigidbody[] rigidbodies;
	AudioSource audioSource;
	
	public AudioClip soundClip;
	
	private void Start() {
		audioSource = gameObject.AddComponent<AudioSource>();
		audioSource.volume = 0.4f;
		audioSource.spatialBlend = 1f;
		audioSource.pitch = 0.88f;
		
		audioSource.clip = soundClip;
	}
	
	public void Release() {
		rigidbodies = GetComponentsInChildren<Rigidbody>();
		
		audioSource.Play();
		
		foreach(Rigidbody r in rigidbodies) {
			Vector3 dir = r.transform.forward;
			
			dir = Vector3.Lerp(dir, -r.transform.right, Random.Range(0f, 1f)).normalized;
			dir = Vector3.Lerp(dir, r.transform.up, 0.4f).normalized;
			
			r.constraints = RigidbodyConstraints.None;
			r.AddForce(dir * Random.Range(3f, 7f), ForceMode.Impulse);
		}
	}
}
