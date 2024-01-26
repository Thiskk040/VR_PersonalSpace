using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testcollisiokn : MonoBehaviour
{
    // Start is called before the first frame update
    public void OnTriggerEnter(Collider other)
    {
       if(other.gameObject.tag == "Enemy")
        {
            Debug.Log("collsion jaaaaaa");
        }
    }
}
