using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class setcontrols : MonoBehaviour
{
     public Text control1;

    public void setpad()
    {
        PlayerPrefs.SetString("pad", control1.text);
        PlayerPrefs.DeleteKey("mouse");
        PlayerPrefs.DeleteKey("giro");
  

    }

    public void setmouse()
    {
        PlayerPrefs.SetString("mouse", control1.text);
        PlayerPrefs.DeleteKey("pad");
        PlayerPrefs.DeleteKey("giro");


    }
    public void setgiroscopio()
    {
        PlayerPrefs.SetString("giro", control1.text);
        PlayerPrefs.DeleteKey("mouse");
        PlayerPrefs.DeleteKey("pad");


    }
}
