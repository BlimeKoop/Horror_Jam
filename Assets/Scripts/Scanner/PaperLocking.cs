using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PaperLocking : MonoBehaviour
{


    //If you are seeing this code for the love of god that is unholy bull shit right here
    //I'm sorry you have to see this MA MA MIA of a mistake I am not a programmer ;)
    [SerializeField]
    private GameObject lockingPoint;
    [SerializeField]
    private GameObject newSpawnPoint;
    [SerializeField]
    private GameObject _Interactables;

    [SerializeField]
    private GameObject scannerObject;
    private Animator scannerAnimation;
    private OpenCloseSFX scanningSoundFX;

    private GameObject paperObject;

    public float waitTimer = 3f;

    bool nowScanning = false;

    void Start(){
        scannerAnimation = scannerObject.GetComponent<Animator>();
        scanningSoundFX = scannerObject.GetComponent<OpenCloseSFX>();
    }

    void Update()
    {
        if (scannerAnimation.GetBool("Opened") == false && nowScanning == false){
            ScanningMode();
        }
    }

    void OnTriggerEnter(Collider paper){
        if (paper.CompareTag("Scannable"))
        {
            paperObject = paper.gameObject;
            paperObject.gameObject.layer = LayerMask.NameToLayer("Default");
            paperObject.GetComponent<Rigidbody>().isKinematic = true;
            paperObject.GetComponent<BoxCollider>().enabled = false;
            paperObject.transform.parent = lockingPoint.transform;
            paperObject.transform.localScale = new Vector3(paperObject.transform.localScale.x / 1.5f, paperObject.transform.localScale.y / 1.5f, paperObject.transform.localScale.z / 1.5f);
            paperObject.transform.position = lockingPoint.transform.position;
            paperObject.transform.rotation = lockingPoint.transform.rotation;
        }
    }

    void ScanningMode(){
        if (paperObject != null){
            scannerObject.layer = LayerMask.NameToLayer("Default");
            StartCoroutine(ScanningTimer());
        }
    }

    IEnumerator ScanningTimer(){
        nowScanning = true;
        yield return new WaitForSeconds(0.35f);
        scanningSoundFX.isScanning = true;
        scanningSoundFX.ScanningSound();
        Debug.Log("how many did i run");
        yield return new WaitForSeconds(waitTimer);
        ScanningCompletedSound();
    }

    //Here is an example of my coding probably better than yandere
    //I feel better about my self when I look at that code and you have to agree with me :)

    void ScanningCompletedSound(){
        scannerAnimation.SetBool("Opened", true);
        StartCoroutine(ScanningCompleted());
    }

    IEnumerator ScanningCompleted(){
        yield return new WaitForSeconds(0.5f);
        paperObject.layer = LayerMask.NameToLayer("Interactable");
        paperObject.GetComponent<Rigidbody>().isKinematic = false;
        paperObject.GetComponent<BoxCollider>().enabled = true;
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
    }
}
