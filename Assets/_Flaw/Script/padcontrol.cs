using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class padcontrol: MonoBehaviour
{

    public float speed = 20.0f; // Speed of the player


    // UI related GameObject.
    public GameObject PauseMenuobj;
    public GameObject GameOverMenu;
    public GameObject HighScoreObj;
    public GameObject ScoreObj;
    public GameObject pause;
    int tocando = 0;
    int giroscopio = 0;
    public InterstitialAdsScript ads;
    // UI related text.
    Text HscoreText;
    Text ScoreText;

    // Two global float to smooth movement and rotation.
    private float zrotation = 0.0f;
    private float xrotation = 0.0f;

    private int score = 0; // The score of the player
    private bool alive = true; // Has your player hit another object ?
    private bool Paused = false; // Is the game paused ?
    private bool GameLost = false; // Is the Game lost ?

    private Quaternion calibration; // The quaternion used to calibrate our vector direction.

    Vector3 dir;
    public GameObject showbtn;
    public GameObject failedReward;
    int scoresave;



    void Start()
    {
        if (PlayerPrefs.HasKey("savedspeed")) { speed = PlayerPrefs.GetFloat("savedspeed"); PlayerPrefs.DeleteKey("savedspeed"); }
    
        if (PlayerPrefs.HasKey("savedScore")) { scoresave = PlayerPrefs.GetInt("savedScore"); PlayerPrefs.DeleteKey ("savedScore"); }

        calib();
       
        // When the Game start, this script will make up the calibration quaternion with previously saved value.
        // Same code can be seen in the Cameramovement script played in the intro scene.
      //  calibration = new Quaternion(PlayerPrefs.GetFloat("quadx"), PlayerPrefs.GetFloat("quady"), PlayerPrefs.GetFloat("quadz"), PlayerPrefs.GetFloat("quadw"));

        // We search the Text compoment of the two GameObject who will manage score and Highscore displaying.
        HscoreText = HighScoreObj.GetComponent<Text>();
        ScoreText = ScoreObj.GetComponent<Text>();

        // We then apply the Highscore (saved in the playerpref) and the current score (global value).
        HscoreText.text = "High score : " + PlayerPrefs.GetInt("HighScore");
        ScoreText.text = "Score : " + score;

       
    }

    Vector3 FixAcceleration(Vector3 acceleration)
    {
        // This function will calibrate our vector acceleration with the quaternion we created in the start function.
        Vector3 fixedAcceleration = calibration * acceleration;
        return (fixedAcceleration);
    }


    public void gameover()
    {
       
        GameLost = true; Time.timeScale = 0.0f; GameOverMenu.SetActive(true); pause.SetActive(false);
        if (score > PlayerPrefs.GetInt("HighScore")) // if your score is higher than the Highscore, replace it.
            PlayerPrefs.SetInt("HighScore", score);
    }
    

        void Update()
    {
        Vector3 dir; // Where will the camera will point.
        Vector3 fixeddir; // Camera direction calibrated.
        Vector3 position; // Position of the player.
        if (Paused == true) { pause.GetComponentInChildren<Text>().text = "Continue"; }else pause.GetComponentInChildren<Text>().text = "Pause";

        if (!alive && !GameLost) // By having tow boolean for this, we will make sure we will enter in this statement only once.
        {
            PlayerPrefs.SetFloat("savedspeed", speed);
            Time.timeScale = 0.0f; // Pause the game.
            if (score > PlayerPrefs.GetInt("HighScore")) // if your score is higher than the Highscore, replace it.
                PlayerPrefs.SetInt("HighScore", score);
            ads.ShowRewardAnswerScreen();


        }
       
            if (!Paused && !GameLost) // show must go on
        {
           // pause.SetActive(true);
            dir = new Vector3(0.0f, 0.0f, 0.0f); //First we initialise dir to a null value.
            position = transform.position; // then our position.

            speed += 0.25f * Time.deltaTime; // speed will increase by 0.25 each seconds.

            dir.x += Input.GetAxis("Horizontal");
            dir.y += Input.GetAxis("Vertical");
            //dir.x += Input.GetAxis("Mouse X");
            // dir.y += Input.GetAxis("Mouse Y");

            if (tocando == 1)
            {


                dir.x = Input.GetAxis("Mouse X") - 1;
                // dir.y = Input.GetAxis("Mouse Y")+1;

            }
            if (tocando == 2)
            {


                dir.x = Input.GetAxis("Mouse X") + 1;
                //dir.y = Input.GetAxis("Mouse Y") - 1;

            }

            if (tocando == 3)
            {



                dir.y = Input.GetAxis("Mouse Y") + 1;

            }

            if (tocando == 4)
            {



                dir.y = Input.GetAxis("Mouse Y") - 1;

            }

            if(tocando == 5)
            {


                dir.x += Input.GetAxis("Mouse X");
                dir.y += Input.GetAxis("Mouse Y");

            }

            if (PlayerPrefs.HasKey("giro"))
            {
                giroscopio = 1;

            }
            if (!PlayerPrefs.HasKey("giro"))
            {
                giroscopio = 0;

            }
            // we add Input value from horizontal and vertical axis.
            // Note that this wont work on mobile as we don't have controller like up and down key on mobile.

            if (giroscopio == 1) { dir=  Input.acceleration;  } // Take the accelerometer input into account.
            fixeddir = FixAcceleration(dir); // Fix the direction with our Fixacceleration function.

            fixeddir.x = Mathf.Clamp(fixeddir.x * 3.0f, -1.5f, 1.5f);
            fixeddir.y = Mathf.Clamp(fixeddir.y * 3.0f, -1.5f, 1.5f);
            // The multiply these value by 3 but clamp them so they wont be higher than 1 or lower than -1.

           

            zrotation = Mathf.Lerp(zrotation, -fixeddir.x / 10.0f, Time.deltaTime * 3f);
            xrotation = Mathf.Lerp(xrotation, -fixeddir.y / 10.0f, Time.deltaTime * 3f);
            // We then lerp these value with last frame value so it smooth it a bit.

            position.x += fixeddir.x * speed * Time.deltaTime; // move the player in the x axis depending on his input.
            position.z += speed * Time.deltaTime; // always make the player going forward.
            position.y += fixeddir.y * speed * 0.66f * Time.deltaTime; // move the player in the y axis depending on his input.
                                                                       // the player will be a little slower in the y axis than other axis.

            transform.position = position;
            transform.rotation = new Quaternion(xrotation, transform.rotation.y, zrotation, transform.rotation.w);
            //apply the rotation and the position.
           
          
            score = scoresave +  (int)(position.z); //score is equal to the z position of the player.
            ScoreText.text = "Score : " + score; // update the score.
            RenderSettings.ambientIntensity = 0.5f - ((float)score / 10000.0f); // Esthetics settings- The higher the score of
                                                                                // the player will be, the more "dark and contrasted" the game will look.
        }
    }

    void calib()
    {
        dir = new Vector3(0.0f, 0.0f, 0.0f);
    }

    void OnCollisionEnter(Collision collisionInfo)
    {
        // End the game if the player touch another object.
        alive = false;
        
    }


    // These 3 function are trigerred by the UI (that's why they are public).
    public void ContinueAdsShowsucess()
    {
        PlayerPrefs.SetInt("savedScore", score);
     
        alive = true; GameLost = false;
        showbtn.SetActive(false);
        Time.timeScale = 1.0f;
        RetryButton();
    }
    public void PauseMenu()
    {
        // the pause menu can only be activated if the game is not lost.
        if (!GameLost)
        {
            // These two statement will activate the menu if he is desactived and vice versa.
            if (PauseMenuobj.activeSelf == true)
            {
                PauseMenuobj.SetActive(false);
                Paused = false;
                Time.timeScale = 1.0f;
            }
            else
            {
                PauseMenuobj.SetActive(true);
                Paused = true;
                Time.timeScale = 0.0f;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Coin")
        {
            scoresave += 50;
        }
        
    }

    public void RetryButton()
    {
        SceneManager.LoadScene("Flaw");
        
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void toque() { tocando = 1; }
    public void toque2() { tocando = 2; }
    public void toque3() { tocando = 3; }
    public void toque4() { tocando = 4; }
    public void toque5() { tocando = 5; }
    public void soltar() { tocando = 0; }

   
}
