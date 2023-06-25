using System.Collections;
using System.Collections.Generic;
using System.Collections;
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
    private GameObject scanPoint;

	private CorkboardController corkboardController;
	
    private GameObject paperObject;
	private GameObject scannedObj;
	private GameObject cloneObj;

    [SerializeField]
    private GameObject scannerObject;
    private Animator scannerAnimation;
    private OpenCloseSFX scanningSoundFX;

    public float waitTimer = 3f;

    bool nowScanning = false;

    public GameObject hairOverlay;
    HairOverlayController hairController;
    int scanCounter = 0;
	
	float volume = 0.0f;

    AudioSource scareSounds;
    public AudioClip[] spookySFX;

    public JumpScare jumpScareScript;


    void OverlayHair(){
		volume += 0.07f;
		scareSounds.volume = volume;
		
        switch(scanCounter){
            case 1:
                hairController.SetEnabled(true);
                hairController.SetVisibility(0);
                scareSounds.clip = spookySFX[scanCounter - 1 ];
                scareSounds.Play();
                break;

            case 2:
                hairController.SetVisibility(0.03f);
                scareSounds.clip = spookySFX[scanCounter - 1];
                scareSounds.Play();
                break;

            case 3:
                hairController.SetVisibility(0.06f);
                scareSounds.clip = spookySFX[scanCounter - 1];
                scareSounds.Play();
                break;

            case 4:
                hairController.SetVisibility(0.1f);
                scareSounds.clip = spookySFX[scanCounter - 1];
                scareSounds.Play();
				corkboardController.Release();
                break;
            case 5:
                hairController.SetVisibility(0.5f);
                scareSounds.clip = spookySFX[scanCounter - 1];
                scareSounds.Play();
                break;

            case 6:
                hairController.SetVisibility(1);
                scareSounds.clip = spookySFX[scanCounter - 1];
                scareSounds.Play();
                break;
            default:
                hairController.SetEnabled(false);
                scareSounds.clip = spookySFX[scanCounter - 1];
                jumpScareScript.ChildEnabled(true);
                scareSounds.Play();
                hairController.SetVisibility(0);
				
				StartCoroutine(EndGame());
				
                break;

        }
    }
	
	private IEnumerator EndGame() {
		yield return new WaitForSeconds(1.5f);
		
		Application.Quit();
	}

    void Start() {
        hairController = hairOverlay.GetComponent<HairOverlayController>();
        scannerAnimation = scannerObject.GetComponent<Animator>();
        scanningSoundFX = scannerObject.GetComponent<OpenCloseSFX>();
        scareSounds = gameObject.GetComponent<AudioSource>();
        jumpScareScript = jumpScareScript.GetComponent<JumpScare>();
		
		corkboardController = GameObject.Find("Corkboard").GetComponent<CorkboardController>();
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
			paperObject = paper.gameObject;
			
			if (paperObject.GetComponentInParent<Rigidbody>() != null)
				paperObject = paperObject.GetComponentInParent<Rigidbody>().gameObject;
            
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
            ++scanCounter; //HERE IS WHERE THE CODE I JUST IMPLMENTED
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
		
        // Debug.Log("how many did i run");
		
        yield return new WaitForSeconds(waitTimer);
		
        scannerAnimation.SetBool("Opened", true);
		
        if (cloneObj != null)
            DestroyImmediate(cloneObj);
		
        scannedObj = paperObject;
        cloneObj = Instantiate(scannedObj, scanPoint.transform.position, scanPoint.transform.rotation,  scanPoint.transform);
        Rigidbody spawnedObj = scanPoint.GetComponentInChildren<Rigidbody>();
        spawnedObj.isKinematic = false;
        spawnedObj.useGravity = false;
		
        StartCoroutine(ScanningCompleted());
    }

    //  ONCE SCANNING HAS BEEN COMPLETED AND THE SCANNER HAS OPENED FROM THE IENUMERATOR ABOVE 
    //  THIS WILL REMOVE THE PAPER TO ITS DESIGNATED LOCATION AND PLAY THE REMOVAL SFX
    IEnumerator ScanningCompleted(){
        yield return new WaitForSeconds(0.5f);
		
		OverlayHair();
        paperObject.layer = LayerMask.NameToLayer("Interactable");
        paperObject.tag = "Untagged";
		
		for (int i = 0; i < paperObject.transform.childCount; i++)
			paperObject.transform.GetChild(i).tag = "Untagged";
		
        paperObject.GetComponent<Rigidbody>().isKinematic = false;
        paperObject.GetComponentInChildren<BoxCollider>().enabled = true;
        paperObject.transform.parent = _Interactables.transform;
        paperObject.transform.localScale = new Vector3(paperObject.transform.localScale.x * 1.5f, paperObject.transform.localScale.y * 1.5f, paperObject.transform.localScale.z * 1.5f);
        paperObject.transform.position = newSpawnPoint.transform.position;
        paperObject.transform.rotation = newSpawnPoint.transform.rotation;
        scanningSoundFX.isScanning = false;
        scanningSoundFX.ScanningSound();
        // Debug.Log("Completed reposition and resizing");
        scannerObject.layer = LayerMask.NameToLayer("Animatable");
        // Debug.Log("Completed Scanning");
        paperObject = null;
        nowScanning = false;
		
        gameObject.GetComponent<BoxCollider>().enabled = true;
    }
}
