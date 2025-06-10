using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CountryEventListener : MonoBehaviour
{
    public Image avatarImage;
    public Image countryImage;
    public GameObject[] avatarSprite;
   

    public void OnEnable()
    {
        avatarImage.sprite = PlayerPrefsSaver.currentAvatarSprite;
    }

    public void Next()
    {
        CanvasScriptSplash.instance.LoadScene(2);
        if (TssAdsManager._Instance)
        {
            TssAdsManager._Instance.ShowInterstitial("CountryEventListener");
            TssAdsManager._Instance.HideRecBanner();
        }

        PlayerPrefsManager.Shown = true;
    }

    public void OnAvatarImageClicked(Sprite i, GameObject avatarPanelSelected)
    {
        foreach (var avatar in avatarSprite)
        {
            avatar.SetActive(false);
        }
        avatarPanelSelected.SetActive(true);
        countryImage.sprite = i;
        PlayerPrefsSaver.currentFlagSprite = i;
        
    }
}

public static class PlayerPrefsManager
{
    public static Sex GenderSelection
    {
        get => (Sex)PlayerPrefs.GetInt("GenderSelection", 0);
        set => PlayerPrefs.SetInt("GenderSelection", (int)value);
    }

    public static bool Shown
    {
        get => PlayerPrefs.GetInt("Shown", 0) == 0;
        set => PlayerPrefs.SetInt("Shown", value ? 1 : 0);
    }
}