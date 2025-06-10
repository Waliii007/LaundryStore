using System;
using LaundaryMan;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuEventListener : MonoBehaviour
{
    public Button playbutton;
    public Image logo;

    private void OnEnable()
    {
        if (SoundManager.instance)
        {
            SoundManager.instance.Play(SoundName.BackGround);
        }

        if (TssAdsManager._Instance)
        {
            TssAdsManager._Instance.ShowBanner("MainMenu");
            TssAdsManager._Instance.admobInstance.TopShowBanner();
            TssAdsManager._Instance.HideRecBanner();
        }

        logo.sprite = PlayerPrefsManager.GenderSelection switch
        {
            Sex.Male => boySprite,
            Sex.Female => girlSprite,
            _ => logo.sprite
        };
    }

    public Sprite boySprite;
    public Sprite girlSprite;

    public void Settings()
    {
        CanvasScriptSplash.instance.ChangeCanvas(CanvasStats.Setting);
        if (SoundManager.instance)
        {
            SoundManager.instance.Play(SoundName.Click);
        }
    }
    public void UserInfo()
    {
        CanvasScriptSplash.instance.LoadScene(1);
        if (SoundManager.instance)
        {
            SoundManager.instance.Play(SoundName.Click);
        }
        TssAdsManager._Instance.ShowInterstitial("UserInfo");
    }

    public void Play()
    {
        playbutton.interactable = false;
        CanvasScriptSplash.instance.LoadScene(3);
        if (SoundManager.instance)
        {
            SoundManager.instance.Play(SoundName.Click);
        }
    }

    public void RateUs()
    {
        Application.OpenURL(GlobalConstant.RateUsLink);
        if (SoundManager.instance)
        {
            SoundManager.instance.Play(SoundName.Click);
        }
        //       TssAdsManager._Instance.ShowInterstitial("MainMenu");
    }

    public void PrivacyPolicy()
    {
        Application.OpenURL(GlobalConstant.PrivacyPoliciesLInk);
        if (SoundManager.instance)
        {
            SoundManager.instance.Play(SoundName.Click);
        }
        // TssAdsManager._Instance.ShowInterstitial("MainMenu");
    }

    public void MoreGame()
    {
        Application.OpenURL(GlobalConstant.MoreGamesLink);
        if (SoundManager.instance)
        {
            SoundManager.instance.Play(SoundName.Click);
        }
        //      TssAdsManager._Instance.ShowInterstitial("MainMenu");
    }
}