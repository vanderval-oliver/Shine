using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    public GameObject SettingsMenu; // The settings Menu (desactivated by default)
    public GameObject HighScore; // The highscore (displayed in the settings menu)

    public GameObject load;
    public GameObject obstacles;
    bool pressed = false;

    private bool GameLaunched = false; // bool to know if the player has clicked on play

    Text hscoretext; // Highscore.

    private Quaternion calibrationQuaternion; // The quaternion used to calibrate our vector direction.

    void AccCalibration()
    {
        // Here we will take the acceleration capted in the start of the game and make it the default
        // acceleration for the rest of the game, unless the player want to calibrate it again.

        Vector3 accelerationSnapshot = Input.acceleration; // Get acceleration input

        Quaternion rotateQuaternion = Quaternion.FromToRotation(new Vector3(0.0f, 0.0f, -1.0f), accelerationSnapshot);
        calibrationQuaternion = Quaternion.Inverse(rotateQuaternion);
        // We then create a quaternion capable of calibration our future vector.

        //We save the Quaternion in PlayerPref. Considering you can only store floats and int in PlayerPref. We have
        // to store each part of the quaternion manually.
        PlayerPrefs.SetFloat("quadx", calibrationQuaternion.x);
        PlayerPrefs.SetFloat("quady", calibrationQuaternion.y);
        PlayerPrefs.SetFloat("quadz", calibrationQuaternion.z);
        PlayerPrefs.SetFloat("quadw", calibrationQuaternion.w);
    }

    void Start()
    {
        Time.timeScale = 1.0f; //Set Game speed to normal.

        // Then get the Highscore text component and update it.
        hscoretext = HighScore.GetComponent<Text>();
        hscoretext.text = "High score : " + PlayerPrefs.GetInt("HighScore");

        // Here we calibrate orientation for the first time.
        AccCalibration();


    }

    // These 4 function are UI related



    public void LaunchGame()
    {
        GameLaunched = true;
    }

    public void ShowParam()
    {
        if (SettingsMenu.activeSelf) { SettingsMenu.SetActive(false); } else { SettingsMenu.SetActive(true); }
       
    }

    public void SetAcc()
    {
        //Recalibrate the acceleration
        AccCalibration();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

        
    


   float fog;
        // Considering our fog is linear, the fog is "full" at a certain distance. (which is fogEndDistance) 

        if (GameLaunched)
        {

            // If the player has cliked on the play button, the fog will get thicker and thicker
            // When the fog will be the "thickest", the game will start.
            fog = RenderSettings.fogEndDistance;

            if (fog <= 10.0f)

            { obstacles.SetActive(false); }

            if (fog <= 7.0f)
            //SceneManager.LoadScene("Flaw");
            { load.SetActive(true); }


            else
                fog -= 100.0f * Time.deltaTime;
            // The "fogenddistance" is equal to 150 at the start of the game.
            //The line above is made to make it equal to 0 in 1 second.
            RenderSettings.fogEndDistance = fog; // aplly the fog.
        }
    }
}
