using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class controlManager : MonoBehaviour
{ 
    public Text control1;
    public GameObject pad;
    public GameObject mouse;
    
    void Start()
    {
        if (PlayerPrefs.HasKey("pad"))
        {
            pad.gameObject.SetActive(true);
            mouse.gameObject.SetActive(false);
            
        }
        if (PlayerPrefs.HasKey("mouse"))
        {
            pad.gameObject.SetActive(false);
            mouse.gameObject.SetActive(true);
            

        }

        if (PlayerPrefs.HasKey("giro"))
        {
            pad.gameObject.SetActive(false);
            mouse.gameObject.SetActive(false);
            

        }
    }

    
}

