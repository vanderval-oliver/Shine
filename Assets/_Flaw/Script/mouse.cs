using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouse : MonoBehaviour
{

    private Transform target;
    

    
    void Update()
    {

        this.transform.position = Input.mousePosition;

    }
}
