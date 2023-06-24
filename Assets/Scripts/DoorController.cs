using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
	private Animator animator;
	private AudioSource audioSource;
	
	[SerializeField]
	private AudioClip openSound;
	[SerializeField]
	private AudioClip closeSound;
	
	private void Start() {
		animator = GetComponent<Animator>();
		audioSource = GetComponent<AudioSource>();
	}
	
    private void OnTriggerEnter() {
		animator.SetBool("Opened", true);
	}

    private void OnTriggerExit() {
		animator.SetBool("Opened", false);
	}
	
	public void PlayOpenSound() {
		audioSource.clip = openSound;
		audioSource.Play();
	}
	
	public void PlayCloseSound() {
		audioSource.clip = closeSound;
		audioSource.Play();
	}
}

