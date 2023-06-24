using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildController : MonoBehaviour
{
	private Renderer[] rends;
	private Collider col;
	
    void Start() {
        rends = GetComponentsInChildren<Renderer>();
		col = GetComponentInChildren<Collider>();
		
		col.enabled = false;
		
		foreach (Renderer rend in rends)
			rend.enabled = false;
    }

	public void JumpScare() {
		col.enabled = true;
		
		foreach (Renderer rend in rends)
			rend.enabled = true;
    }
}
