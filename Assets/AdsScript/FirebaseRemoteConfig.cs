﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Analytics;
using Firebase.Extensions;
using System;
using System.Threading.Tasks;
using Firebase.Crashlytics;
using GameAnalyticsSDK;

public class FirebaseRemoteConfig : MonoBehaviour
{
    DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;
    public static bool firebaseInitialized = false;

    public void InternetAvailable()
    {
        if (Application.isMobilePlatform)
            FirebaseApp.LogLevel = LogLevel.Error;
        GameAnalytics.Initialize();
        OnFireBase();
    }

    void OnFireBase()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.LogError(
                    "Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }

    void InitializeFirebase()
    {
//        Debug.Log("Enabling data collection.");
        FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
   //     Debug.Log("Set user properties.");
        FirebaseAnalytics.SetUserProperty(
            FirebaseAnalytics.UserPropertySignUpMethod,
            "Google");
        // Set the user ID.
        // FirebaseAnalytics.SetUserId("uber_user_510");
        // Set default session duration values.
        // FirebaseAnalytics.SetSessionTimeoutDuration(new TimeSpan(0, 30, 0));
        firebaseInitialized = true;
        FirebaseApp app = FirebaseApp.DefaultInstance;
        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelStart);
        Dictionary<string, object> defaults = new()
        {
//================Remote Config===================
        };

        Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(defaults)
            .ContinueWithOnMainThread(task =>
            {
                Crashlytics.ReportUncaughtExceptionsAsFatal = true;
                FetchDataAsync();
            });
        CheckInternet();
    }

    public Task FetchDataAsync()
    {
//        Debug.Log("Fetching data...");
        Task fetchTask =
            Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.FetchAsync(
                TimeSpan.Zero);
        return fetchTask.ContinueWithOnMainThread(FetchComplete);
    }

    public TssAdsManager adsManager;

    void FetchComplete(Task fetchTask)
    {
        if (fetchTask.IsCanceled)
        {
           // Debug.Log("Fetch canceled.");
        }
        else if (fetchTask.IsFaulted)
        {
           // Debug.Log("Fetch encountered an error.");
        }
        else if (fetchTask.IsCompleted)
        {
//            Debug.Log("Fetch completed successfully!");
        }

        var info = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.Info;
        switch (info.LastFetchStatus)
        {
            case Firebase.RemoteConfig.LastFetchStatus.Success:
                Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.ActivateAsync()
                    .ContinueWithOnMainThread(task =>
                    {
//                        Debug.Log(String.Format("Remote data loaded and ready (last fetch time {0}).",
                    //        info.FetchTime));
                        GetRemoteData();
                        //print(GlobalConstant.TSS_Admob_Banner_MID);
                    });
                break;
            case Firebase.RemoteConfig.LastFetchStatus.Failure:
                switch (info.LastFetchFailureReason)
                {
                    case Firebase.RemoteConfig.FetchFailureReason.Error:
                        Debug.Log("Fetch failed for unknown reason");
                        break;
                    case Firebase.RemoteConfig.FetchFailureReason.Throttled:
                        Debug.Log("Fetch throttled until " + info.ThrottledEndTime);
                        break;
                }

                //adsManager.Init();
                break;
            case Firebase.RemoteConfig.LastFetchStatus.Pending:
                Debug.Log("Latest Fetch call still pending.");
                break;
        }
    }

    public void GetRemoteData()
    {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.WindowsEditor)
        {
            if (GlobalConstant.isLogger) print("Android platform is supported.");
            GlobalConstant.ISMAXON =
                Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue("ISMAXON")
                    .BooleanValue;
            if (GlobalConstant.isLogger) print("19");

            GlobalConstant.AdsON =
                Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue("isAdsOn")
                    .BooleanValue;
            if (GlobalConstant.isLogger) print("18");

            GlobalConstant.TSS_Admob_Banner_MID =
                Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance
                    .GetValue("BannerMid").StringValue;
            if (GlobalConstant.isLogger) print("17");

            GlobalConstant.TSS_Admob_Inter_IdMid =
                Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance
                    .GetValue("InterMid").StringValue;
            if (GlobalConstant.isLogger) print("16");
            GlobalConstant.TSS_Admob_Rewarded_Id_Mid =
                Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance
                    .GetValue("RewardedMId").StringValue;
            if (GlobalConstant.isLogger) print("15");
            GlobalConstant.TSS_Admob_AppOpen_Id_Mid =
                Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance
                    .GetValue("AppOpenMid").StringValue;
            if (GlobalConstant.isLogger) print("14");
            GlobalConstant.TSS_Admob_Banner_HIGH =
                Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance
                    .GetValue("BannerHigh").StringValue;
            if (GlobalConstant.isLogger) print("13");
            GlobalConstant.TSS_Admob_Inter_IdHigh =
                Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance
                    .GetValue("InterHigh").StringValue;
            if (GlobalConstant.isLogger) print("12");
            GlobalConstant.TSS_Admob_Rewarded_Id_High =
                Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance
                    .GetValue("RewardedHigh").StringValue;
            if (GlobalConstant.isLogger) print("11");
            GlobalConstant.TSS_Admob_AppOpen_IdHigh =
                Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance
                    .GetValue("AppOpenHigh").StringValue;

            if (GlobalConstant.isLogger) print("10");
            GlobalConstant.TSS_Admob_Banner_Simple =
                Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance
                    .GetValue("BannerSimple").StringValue;
            if (GlobalConstant.isLogger) print("9");
            GlobalConstant.TSS_Admob_Inter_IdLow =
                Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance
                    .GetValue("InterSimple").StringValue;
            if (GlobalConstant.isLogger) print("8");
            GlobalConstant.TSS_Admob_Rewarded_Id_Simple =
                Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance
                    .GetValue("RewardedSimple").StringValue;
            if (GlobalConstant.isLogger) print("7");
            GlobalConstant.TSS_Admob_AppOpen_Id_Low =
                Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance
                    .GetValue("AppOpenLow").StringValue;
            if (GlobalConstant.isLogger) print("6");

            GlobalConstant.InterstitialAdUnitId =
                Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance
                    .GetValue("InterstitialMax").StringValue;
            if (GlobalConstant.isLogger) print("5");
            GlobalConstant.RewardedAdUnitId =
                Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance
                    .GetValue("RewardedAdMax").StringValue;
            if (GlobalConstant.isLogger) print("4");
            GlobalConstant.RateUsLink =
                Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance
                    .GetValue("RateUsLink").StringValue;
            if (GlobalConstant.isLogger) print("3");
            GlobalConstant.PrivacyPoliciesLInk =
                Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance
                    .GetValue("PrivacyPoliciesLInk").StringValue;

            if (GlobalConstant.isLogger) print("2");
            GlobalConstant.adPriority = (TssAdsManager.AdPriority)(int)Firebase.RemoteConfig.FirebaseRemoteConfig
                .DefaultInstance
                .GetValue("adPriority").DoubleValue;
            

            if (GlobalConstant.isLogger) print("1");
            if (GlobalConstant.isLogger)
            {
                print(GlobalConstant.TSS_Admob_Inter_IdHigh);
                print(GlobalConstant.TSS_Admob_Inter_IdMid);
                print(GlobalConstant.TSS_Admob_Inter_IdLow);

                print(GlobalConstant.TSS_Admob_AppOpen_Id_Low);
                print(GlobalConstant.TSS_Admob_AppOpen_Id_Mid);
                print(GlobalConstant.TSS_Admob_AppOpen_IdHigh);

                print(GlobalConstant.TSS_Admob_Banner_Simple);
                print(GlobalConstant.TSS_Admob_Banner_MID);
                print(GlobalConstant.TSS_Admob_Banner_HIGH);

                print(GlobalConstant.TSS_Admob_Rewarded_Id_High);
                print(GlobalConstant.TSS_Admob_Rewarded_Id_Mid);
                print(GlobalConstant.TSS_Admob_Rewarded_Id_Simple);
            }

            adsManager.Init();
        }

        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            GlobalConstant.ISMAXON =
                (bool)Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue("ISMAXONIOS")
                    .BooleanValue;
            if (GlobalConstant.isLogger)
                print("19");

            GlobalConstant.AdsON =
                (bool)Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue("isAdsOnIOS")
                    .BooleanValue;
            if (GlobalConstant.isLogger)
                print("18");

            GlobalConstant.TSS_Admob_Banner_MID =
                Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance
                    .GetValue("BannerMidIOS").StringValue;
            if (GlobalConstant.isLogger)
                print("17");

            GlobalConstant.TSS_Admob_Inter_IdMid =
                Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance
                    .GetValue("InterMidIOS").StringValue;
            if (GlobalConstant.isLogger)
                print("16");
            GlobalConstant.TSS_Admob_Rewarded_Id_Mid =
                Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance
                    .GetValue("RewardedMIdIOS").StringValue;
            if (GlobalConstant.isLogger)
                print("15");
            GlobalConstant.TSS_Admob_AppOpen_Id_Mid =
                Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance
                    .GetValue("AppOpenMidIOS").StringValue;
            if (GlobalConstant.isLogger)
                print("14");
            GlobalConstant.TSS_Admob_Banner_HIGH =
                Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance
                    .GetValue("BannerHighIOS").StringValue;
            if (GlobalConstant.isLogger)
                print("13");
            GlobalConstant.TSS_Admob_Inter_IdHigh =
                Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance
                    .GetValue("InterHighIOS").StringValue;
            if (GlobalConstant.isLogger)
                print("12");
            GlobalConstant.TSS_Admob_Rewarded_Id_High =
                Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance
                    .GetValue("RewardedHighIOS").StringValue;
            if (GlobalConstant.isLogger)
                print("11");
            GlobalConstant.TSS_Admob_AppOpen_IdHigh =
                Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance
                    .GetValue("AppOpenHighIOS").StringValue;

            if (GlobalConstant.isLogger)
                print("10");
            GlobalConstant.TSS_Admob_Banner_Simple =
                Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance
                    .GetValue("BannerSimpleIOS").StringValue;
            if (GlobalConstant.isLogger)
                print("9");
            GlobalConstant.TSS_Admob_Inter_IdLow =
                Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance
                    .GetValue("InterSimpleIOS").StringValue;
            if (GlobalConstant.isLogger)
                print("8");
            GlobalConstant.TSS_Admob_Rewarded_Id_Simple =
                Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance
                    .GetValue("RewardedSimpleIOS").StringValue;
            if (GlobalConstant.isLogger)
                print("7");
            GlobalConstant.TSS_Admob_AppOpen_Id_Low =
                Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance
                    .GetValue("AppOpenLowIOS").StringValue;
            if (GlobalConstant.isLogger)
                print("6");

            GlobalConstant.InterstitialAdUnitId =
                Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance
                    .GetValue("InterstitialMaxIOS").StringValue;
            if (GlobalConstant.isLogger)
                print("5");
            GlobalConstant.RewardedAdUnitId =
                Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance
                    .GetValue("RewardedAdMaxIOS").StringValue;
            if (GlobalConstant.isLogger)
                print("4");
            GlobalConstant.RateUsLink =
                Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance
                    .GetValue("RateUsLinkIOS").StringValue;
            if (GlobalConstant.isLogger)
                print("3");
            GlobalConstant.PrivacyPoliciesLInk =
                Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance
                    .GetValue("PrivacyPoliciesLInkIOS").StringValue;
            if (GlobalConstant.isLogger)
                print("2");
            GlobalConstant.adPriority = (TssAdsManager.AdPriority)(int)Firebase.RemoteConfig.FirebaseRemoteConfig
                .DefaultInstance
                .GetValue("adPriorityIOS").DoubleValue;
             
                print("GlobalConstant.adPriority");

            adsManager.Init();
            if (GlobalConstant.isLogger)
            {
                print(GlobalConstant.TSS_Admob_Inter_IdHigh);
                print(GlobalConstant.TSS_Admob_Inter_IdMid);
                print(GlobalConstant.TSS_Admob_Inter_IdLow);

                print(GlobalConstant.TSS_Admob_AppOpen_Id_Low);
                print(GlobalConstant.TSS_Admob_AppOpen_Id_Mid);
                print(GlobalConstant.TSS_Admob_AppOpen_IdHigh);

                print(GlobalConstant.TSS_Admob_Banner_Simple);
                print(GlobalConstant.TSS_Admob_Banner_MID);
                print(GlobalConstant.TSS_Admob_Banner_HIGH);

                print(GlobalConstant.TSS_Admob_Rewarded_Id_High);
                print(GlobalConstant.TSS_Admob_Rewarded_Id_Mid);
                print(GlobalConstant.TSS_Admob_Rewarded_Id_Simple);
            }

            
        }
    }

    void CheckInternet()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            TSS_AnalyticalManager.instance.CustomOtherEvent("Internet_Available");
        }
        else
        {
            TSS_AnalyticalManager.instance.CustomOtherEvent("Internet_NotAvailable");
        }
    }
}