using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCheck : MonoBehaviour
{
    public GameObject canvas;  // �������͡Ѻ Canvas ���س��ͧ��è��Դ/�Դ
    private bool canvasActive = false;
    
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "object3")
        {
            if (gameObject.tag == "Male")
            {
                SetCanvasActive(true);
            }
            else if (gameObject.tag == "Female")
            {
                SetCanvasActive(true);
            }
            else if (gameObject.tag == "Monster")
            {
                SetCanvasActive(true);
            }
        }   
    }

    public void SetCanvasActive(bool isActive)
    {
        canvas.SetActive(isActive);
        canvasActive = isActive;
         
    }
}

