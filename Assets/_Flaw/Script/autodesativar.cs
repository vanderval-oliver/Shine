using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class autodesativar : MonoBehaviour
{

    public int tempo = 0;
    public int start = 0;
    public GameObject text;
    // Start is called before the first frame update
    void Start()
    {
        start = 1;  
    }

    // Update is called once per frame
    void Update()
    {
        if (start == 1)
        {

            tempo += 1;
          
            if (tempo == 70)
            {
                start = 0;
                tempo = 0;
            text.gameObject.SetActive(false);

            }
        }
    }
}
