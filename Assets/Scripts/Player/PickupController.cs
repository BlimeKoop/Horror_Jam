using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEditorInternal;
using UnityEngine;

public class PickupController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] Transform holdPoint;
    private GameObject heldObject;
    private Rigidbody heldObjectRigidBody;

    [Header("Physics")]
    [SerializeField] private float pickupRange = 4.0f;
    [SerializeField] private float pickupForce = 130f;

    private Animator objectAnimator;

    bool rotatedObject = false;

    private void Update()
    {

        #region Interact to pickup
        if (Input.GetMouseButtonDown(0))
        {
            if (heldObject == null)
            {
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward), out hit, pickupRange)){
                    //Here is how you pickk up the object
                    rotatedObject = false;
                    PickupObject(hit.transform.gameObject);

                }
            } 
            else 
            {
                //here you drop the object
                DropObject();
            }

        } 
            if (heldObject != null)
            {
            //Move Object here
            MoveObject();
            }


        if (Input.GetMouseButtonDown(1) && rotatedObject == false){
            if (heldObject != null){
                // rotate the object to face the player
                //heldObject.transform.Rotate(Camera.main.transform.right * -75, Space.World);
                heldObject.transform.LookAt(Camera.main.transform);
                rotatedObject = true;
                //heldObject.transform.position = nVector3(0.75f, 0f, 0f);

            }
        }
        #endregion

        #region Interact to animate

        if (Input.GetMouseButtonDown(0) && heldObject == null)
        {
            RaycastHit animateHit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.TransformDirection(Vector3.forward), out animateHit, pickupRange))
            {
                ObjectAnimate(animateHit.transform.gameObject);
            }
        }
        #endregion

    }

    void ObjectAnimate(GameObject animateObject){
        if (animateObject.GetComponent<Animator>() && animateObject.layer == LayerMask.NameToLayer("Animatable"))
        {
            objectAnimator = animateObject.GetComponent<Animator>();
            animateObject.transform.GetComponent<OpenCloseSFX>().isOpening = !animateObject.transform.GetComponent<OpenCloseSFX>().isOpening;
            objectAnimator.SetBool("Opened",!objectAnimator.GetBool("Opened"));
        }
    }

    void MoveObject(){
        if (Vector3.Distance(heldObject.transform.position, holdPoint.position) > 0.1f)
        {
            Vector3 moveDirection = (holdPoint.position - heldObject.transform.position);
            heldObjectRigidBody.AddForce(moveDirection * pickupForce);
        }
    }

    void PickupObject(GameObject pickObj)
    {
        if (pickObj.GetComponent<Rigidbody>() && pickObj.layer == LayerMask.NameToLayer("Interactable"))
        {
            heldObjectRigidBody = pickObj.GetComponent<Rigidbody>();
            heldObjectRigidBody.useGravity = false;
            heldObjectRigidBody.drag = 10;
            heldObjectRigidBody.constraints = RigidbodyConstraints.FreezeRotation;

            heldObjectRigidBody.transform.parent = holdPoint;
            heldObject = pickObj;

        }
    }

    void DropObject()
    {
       heldObjectRigidBody.useGravity = true;
       heldObjectRigidBody.drag = 1;
       heldObjectRigidBody.constraints = RigidbodyConstraints.None;

       heldObjectRigidBody.transform.parent = null;
        heldObject = null;

    }

}
