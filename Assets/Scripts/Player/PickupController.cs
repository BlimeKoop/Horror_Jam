using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.Mathematics;
using Unity.VisualScripting;
// using UnityEditor.Rendering.Utilities;
// using UnityEditorInternal;
using UnityEngine;

public class PickupController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] Transform holdPoint;
    private GameObject heldObject;
    private Rigidbody heldObjectRigidBody;

    [SerializeField] GameObject interactableSystem;

    [Header("Physics")]
    [SerializeField] private float pickupRange = 4.0f;
    [SerializeField] private float pickupForce = 130f;
    [SerializeField] private float rotationSpeed = 3f;

    private Animator objectAnimator;

    //has the object been rotated
    bool rotatedObject = false;

    bool scannerPrevention = false;

    //the time between rotation
    bool rotatingObj = false;

    private void Update()
    {
    //  THIS IS HOW WE PICK UP, MOVE AND DROP AN OBJECT THAT HAS THE CORRECT LAYER
    // WE FIRSTLY CHECK THAT WE ARE CURRENTLY NOT HOLDING AN OBJECT CALLED BELOW
        if (Input.GetMouseButtonDown(0))
        {
            if (heldObject == null)
            {
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward), out hit, pickupRange)){
                    rotatedObject = false;
                    PickupObject(hit.transform.gameObject);
                }
            } 
            else 
            {
                DropObject();
            }

        } 

        //  AFTER AN OBJECT HAS BEEN PICKED UP WE WILL ROTATED AN AXIS TO FACE THE PLAYER
        if (Input.GetMouseButtonDown(1) && rotatedObject == false){
            if (heldObject != null){
                //heldObject.transform.LookAt(Camera.main.transform);
                //rotationOffset = quaternion.identity;
                rotatedObject = true;
                rotatingObj = true;
                if (heldObject.GetComponent<AudioSource>() != null)
                    heldObject.GetComponent<ObjectSoundEffect>().PlayCustomSound(0);
            }
        }

        //  SIMILAR TO THE PICK UP MECHANIC HOWEVER INSTEAD IT PLAYS AN OBJECTS ANIMATION IF IT HAS ANIMATABLE LAYER CALLED BELOW
        if (Input.GetMouseButtonDown(0) && scannerPrevention == false)
        {
            RaycastHit animateHit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward), out animateHit, pickupRange))
            {
                ObjectAnimate(animateHit.transform.gameObject);
            }
        }

        

    }

    void FixedUpdate(){
        // HeldObjectPosition();

		if (heldObject != null)
		{
			MoveObject();
            scannerPrevention = true;
        }

        if (rotatingObj == true)
        {
			Quaternion targetRot = transform.rotation * Quaternion.AngleAxis(180f, Vector3.up);
			
            holdPoint.transform.rotation = Quaternion.Lerp(holdPoint.transform.rotation, targetRot, rotationSpeed * Time.fixedDeltaTime);
        
            if (Quaternion.Angle(holdPoint.transform.rotation, targetRot) < 1) {
                rotatingObj = false;
			}
        }
    }

    

    //  HERE IS HOW WE CONTROL THE ANIMATION OF ANOTHER OBJECT AFTER DETECTING ITS RAY.
    void ObjectAnimate(GameObject animateObject){
        if (animateObject.GetComponent<Animator>() && animateObject.layer == LayerMask.NameToLayer("Animatable"))
        {
            objectAnimator = animateObject.GetComponent<Animator>();
            animateObject.transform.GetComponent<OpenCloseSFX>().isOpening = !animateObject.transform.GetComponent<OpenCloseSFX>().isOpening;
            objectAnimator.SetBool("Opened",!objectAnimator.GetBool("Opened"));
        }
    }

    //  HERE IS HOW WE CONTROL THE OBJECTS POSITION 
    void MoveObject(){
        if (Vector3.Distance(heldObject.transform.position, holdPoint.position) > 0.1f) {
            Vector3 moveDirection = (holdPoint.position - heldObject.transform.position);
            heldObjectRigidBody.AddForce(moveDirection * pickupForce * 1.3f);
        }
		
		if (Quaternion.Angle(heldObject.transform.rotation, holdPoint.rotation) > 1f) {
			heldObjectRigidBody.MoveRotation(holdPoint.rotation);
		}
    }

    void PickupObject(GameObject pickObj)
    {
        if (pickObj.GetComponentInParent<Rigidbody>() != null) {
			pickObj = pickObj.GetComponentInParent<Rigidbody>().gameObject;
		}
		
        if (pickObj.GetComponent<Rigidbody>() && pickObj.layer == LayerMask.NameToLayer("Interactable"))
        {
            heldObjectRigidBody = pickObj.GetComponent<Rigidbody>();
            heldObjectRigidBody.useGravity = false;
            heldObjectRigidBody.drag = 10;
            heldObjectRigidBody.constraints = RigidbodyConstraints.FreezeRotation;
            heldObject = pickObj;
			
			holdPoint.transform.rotation = heldObject.transform.rotation;
        }
    }

/*
    void HeldObjectPosition()
    {
        if(heldObject != null){
            heldObjectRigidBody.position = holdPoint.transform.position;
            heldObjectRigidBody.rotation = transform.rotation * Quaternion.AngleAxis(offsetDeg, offsetAxis);
        }
    }
*/

    void DropObject()
    {
       heldObjectRigidBody.useGravity = true;
       heldObjectRigidBody.drag = 1;
       heldObjectRigidBody.constraints = RigidbodyConstraints.None;

       heldObjectRigidBody.transform.parent = interactableSystem.transform;
       heldObject = null;
       heldObjectRigidBody = null;

        StartCoroutine(ScannerAnimationStop());

    }

    IEnumerator ScannerAnimationStop(){

        yield return new WaitForSeconds(0.5f);
        scannerPrevention = false;

    }

}
