using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HairOverlayController : MonoBehaviour
{
	private Renderer rend;
	private Material mat;
	private float visibility = 0.0f;
	
    void Start() {
		rend = GetComponent<Renderer>();
		mat = rend.material;
		
		SetVisibility(0.0f);
		SetEnabled(false);
    }
	
	public void SetEnabled(bool _enabled){
		rend.enabled = _enabled;
	}

	public void SetVisibility(float _visibility) {
		if (!rend.enabled) {
			Debug.LogError(this + " is not enabled, call SetEnabled method");
			return;
		}
		
		visibility = Mathf.Clamp01(_visibility);
		
		mat.SetFloat("_AlphaCutoff", 1.0f - visibility * 0.5f);
	}
}
