using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering;

public class PaperLocking : MonoBehaviour
{


    //If you are seeing this code for the love of god that is unholy bull shit right here
    //I'm sorry you have to see this MA MA MIA of a mistake I am not a programmer ;)

    //Here is an example of my coding probably better than yandere
    //I feel better about my self when I look at that code and you have to agree with me :)
    [SerializeField]
    private GameObject lockingPoint;
    [SerializeField]
    private GameObject newSpawnPoint;
    [SerializeField]
    private GameObject _Interactables;
    [SerializeField]
    private GameObject monitorPoint;

    [SerializeField]
    private GameObject scannerObject;
    private Animator scannerAnimation;
    private OpenCloseSFX scanningSoundFX;

    private GameObject paperObject;

    private GameObject scannedObj;

    public float waitTimer = 3f;

    private GameObject cloneObj;

    bool nowScanning = false;

    void Start(){
        scannerAnimation = scannerObject.GetComponent<Animator>();
        scanningSoundFX = scannerObject.GetComponent<OpenCloseSFX>();
    }

    void Update()
    {
        // IF THE SCANNER ANIMATION IS CLOSED AND THE SCANNER IS NOT OPERATING THEN IT WILL BEGIN THE SCANNING OF THE PAPER
        if (scannerAnimation.GetBool("Opened") == false && nowScanning == false){
            ScanningMode();
        }
    }

    void OnTriggerEnter(Collider paper){
    //  CHECKING IF THE OBJECT THAT HAS COLLIDED TAG IS SCANNABLE I.E. PAPER
        if (paper.CompareTag("Scannable"))
        {
<<<<<<< HEAD
            gameObject.GetComponent<BoxCollider>().enabled = false;
            paperObject = paper.gameObject;
=======
			paperObject = paper.gameObject;
			
			if (paperObject.GetComponentInParent<Rigidbody>() != null)
				paperObject = paperObject.GetComponentInParent<Rigidbody>().gameObject;
            
>>>>>>> main
            paperObject.gameObject.layer = LayerMask.NameToLayer("Default");
            paperObject.GetComponent<Rigidbody>().isKinematic = true;
            paperObject.GetComponentInChildren<BoxCollider>().enabled = false;
            paperObject.transform.parent = lockingPoint.transform;
            paperObject.transform.localScale = new Vector3(paperObject.transform.localScale.x / 1.5f, paperObject.transform.localScale.y / 1.5f, paperObject.transform.localScale.z / 1.5f);
            paperObject.transform.position = lockingPoint.transform.position;
            paperObject.transform.rotation = lockingPoint.transform.rotation;
        }
    }

    void ScanningMode(){
    //  IF THE SCANNER IS CLOSED AND IT IS NOT CURRENTLY SCANNING THIS WILL MAKE THE SCANNER NOT INTERACTABLE/ANIMATABLE
        if (paperObject != null){
            scannerObject.layer = LayerMask.NameToLayer("Default");
            StartCoroutine(ScanningTimer());
        }
    }

    // HERE IS THE TIMER CONTROL FOR THE SCANNING MECHANIC, ONCE THE SCANNER CLOSES AND HAS PLAYED THE CLOSED SFX
    // IT WILL BEGIN THE SCANNING SFX, AFTER WAITTIMER IT WILL HAVE COMPLETED THE SCANNING AND START A NEW COROUTINE
    IEnumerator ScanningTimer(){
        nowScanning = true;
        yield return new WaitForSeconds(0.35f);
        scanningSoundFX.isScanning = true;
        scanningSoundFX.ScanningSound();
        Debug.Log("how many did i run");
        yield return new WaitForSeconds(waitTimer);
        scannerAnimation.SetBool("Opened", true);
        if (cloneObj != null)
            DestroyImmediate(cloneObj);
        scannedObj = paperObject;
        cloneObj = Instantiate(scannedObj, monitorPoint.transform.position, monitorPoint.transform.rotation,  monitorPoint.transform);
        Rigidbody spawnedObj = monitorPoint.GetComponentInChildren<Rigidbody>();
        spawnedObj.isKinematic = false;
        spawnedObj.useGravity = false;
        StartCoroutine(ScanningCompleted());
    }

    //  ONCE SCANNING HAS BEEN COMPLETED AND THE SCANNER HAS OPENED FROM THE IENUMERATOR ABOVE 
    //  THIS WILL REMOVE THE PAPER TO ITS DESIGNATED LOCATION AND PLAY THE REMOVAL SFX
    IEnumerator ScanningCompleted(){
        yield return new WaitForSeconds(0.5f);
        paperObject.layer = LayerMask.NameToLayer("Interactable");
        paperObject.tag = "Untagged";
        paperObject.GetComponent<Rigidbody>().isKinematic = false;
        paperObject.GetComponentInChildren<BoxCollider>().enabled = true;
        paperObject.transform.parent = _Interactables.transform;
        paperObject.transform.localScale = new Vector3(paperObject.transform.localScale.x * 1.5f, paperObject.transform.localScale.y * 1.5f, paperObject.transform.localScale.z * 1.5f);
        paperObject.transform.position = newSpawnPoint.transform.position;
        paperObject.transform.rotation = newSpawnPoint.transform.rotation;
        scanningSoundFX.isScanning = false;
        scanningSoundFX.ScanningSound();
        Debug.Log("Completed reposition and resizing");
        scannerObject.layer = LayerMask.NameToLayer("Animatable");
        Debug.Log("Completed Scanning");
        paperObject = null;
        nowScanning = false;
        gameObject.GetComponent<BoxCollider>().enabled = true;
    }
}
