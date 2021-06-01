using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraManager : MonoBehaviour
{
    public GameObject camera1;
    public GameObject camera2;
    public GameObject camera3;
    private int Mcamera=0;

    private void Update()
    {
        if (Mcamera == 4) { Mcamera = 1; }

        if (Mcamera == 1)


        {
        camera1.SetActive(false);
        camera2.SetActive(true);
        camera3.SetActive(false);
        }


        if (Mcamera == 2)

        {
            camera2.SetActive(false);
            camera3.SetActive(true);
            camera1.SetActive(false);
        }


        if (Mcamera == 3)


        {
            camera3.SetActive(false);
            camera1.SetActive(true);
            camera2.SetActive(false);
        }
        

    }

    public void changeCamera()
    {
        Mcamera += 1;
    }
}
