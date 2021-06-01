using UnityEngine;
using UnityEngine.Advertisements;

using System.Collections;

public class InterstitialAdsScript : MonoBehaviour
{

   
    public padcontrol PadControl;
  
    public string gameId = "3057099";
    public string myPlacementId = "rewardedVideo";
    public bool testMode = true;

    void Start()
    {
      

        Advertisement.Initialize(gameId, testMode);

    }

   public void ShowRewardAnswerScreen()
    {
        if (Advertisement.IsReady(myPlacementId))
        {
           
            PadControl.showbtn.SetActive(true);
        }
        else
        {

            PadControl.gameover();

        }

    }
    public void ShowRewardedAd()
    {
        if (Advertisement.IsReady(myPlacementId))
        {
            var options = new ShowOptions { resultCallback = HandleShowResult };
            Advertisement.Show(myPlacementId, options);
        }
       
    }

    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("The ad was successfully shown.");
                PadControl.ContinueAdsShowsucess();





                break;
            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");
                PadControl.ContinueAdsShowsucess();
                break;
            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");
                PadControl.gameover();
                break;
        }
    }
}

