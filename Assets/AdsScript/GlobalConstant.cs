using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalConstant
{
    public static bool isLogger;
    public static bool AdsON = true;
    public static bool ShowAppOpen = false;
    public static bool ISMAXON = false;
    public static bool UseAdBidding;
    public static string TSS_Admob_Inter_IdHigh =
        "ca-app-pub-3940256099942544/1033173712";

    public static string MoreGamesLink;
    public static float adTimer;
       

    public static string TSS_Admob_Inter_IdMid =
        "ca-app-pub-3940256099942544/1033173712";

    public static string TSS_Admob_Inter_IdLow =
        "ca-app-pub-3940256099942544/1033173712";

    public static string TSS_Admob_Rewarded_Id_Mid =
        "ca-app-pub-3940256099942544/5224354917";

    public static string TSS_Admob_Rewarded_Id_High =
        "ca-app-pub-3940256099942544/5224354917";

    public static string TSS_Admob_Rewarded_Id_Simple =
        "ca-app-pub-3940256099942544/5224354917";


    public static string TSS_Admob_AppOpen_Id_Low = "ca-app-pub-3940256099942544/9257395921";
    public static string TSS_Admob_AppOpen_Id_Mid = "ca-app-pub-3940256099942544/9257395921";
    public static string TSS_Admob_AppOpen_IdHigh = "ca-app-pub-3940256099942544/9257395921";


    public static string TSS_Admob_Banner_Simple =
        "ca-app-pub-3940256099942544/9214589741";

    public static string TSS_Admob_Banner_MID = "ca-app-pub-3940256099942544/9214589741";

    public static string TSS_Admob_Banner_HIGH =
        "ca-app-pub-3940256099942544/9214589741";


    private static bool _isBannerShowing, _isBannerReady;
    private static bool _isFlooringBannerReady, _isFlooringBannerShowing;
    private static bool isMRecShowing;
    public static TssAdsManager.AdPriority adPriority = 0;


    public static string MaxSdkKey = "";

    public static string InterstitialAdUnitId = "0bf5dd259a7babe3";
    public static string RewardedAdUnitId = "5d75002bbc4126b9";

    public static string RateUsLink
    {
        get => PlayerPrefs.GetString("RateUsLink");
        set => PlayerPrefs.SetString("RateUsLink", value);
    }

    public static string PrivacyPoliciesLInk
    {
        get => PlayerPrefs.GetString("PrivacyPoliciesLInk","https://sites.google.com/view/adrenaline-rush-pp/home");
        set => PlayerPrefs.SetString("PrivacyPoliciesLInk", value);
    }


    public static void RewardedAdsWatched(Action ac)
    {
        ac.Invoke();
    }
}