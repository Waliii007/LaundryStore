using System;
using LaundaryMan;
using UnityEngine;

public class UserInfoCanvasManager : MonoBehaviour
{
    public UserInfoStates currentState;
    public UserInfoStates previousState;
    public GameObject[] allCanvasStates;

    private void OnEnable()
    {
        CurrentStateChanger(UserInfoStates.AvatarSelection);
    }

    public void CurrentStateChanger(UserInfoStates newState)
    {
        previousState = currentState;
        currentState = newState;

        allCanvasStates[(int)previousState].SetActive(false);
        allCanvasStates[(int)currentState].SetActive(true);
        switch (newState)
        {
            case UserInfoStates.PlayerProfile:
                TssAdsManager._Instance?.ShowBanner("PlayerProfile");
                TssAdsManager._Instance?.admobInstance.TopShowBanner();
                TssAdsManager._Instance?.ShowInterstitial("PlayerProfile");
                break;
            case UserInfoStates.AvatarSelection:
                TssAdsManager._Instance?.ShowBanner("PlayerProfile");
                TssAdsManager._Instance?.admobInstance.TopShowBanner();
                break;
            case UserInfoStates.CountrySelection:
                TssAdsManager._Instance?.ShowBanner("PlayerProfile");
                TssAdsManager._Instance?.admobInstance.TopShowBanner();
                TssAdsManager._Instance?.ShowInterstitial("PlayerProfile");
                break;
            case UserInfoStates.SceneLoading:

                break;
        }
    }
}

public enum UserInfoStates
{
    PlayerProfile,
    AvatarSelection,
    CountrySelection,
    SceneLoading
}