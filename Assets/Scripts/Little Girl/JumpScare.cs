using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpScare : MonoBehaviour
{
    public GameObject childObject;




    public void ChildEnabled(bool isEnabled){
        childObject.SetActive(isEnabled);
    }
}
