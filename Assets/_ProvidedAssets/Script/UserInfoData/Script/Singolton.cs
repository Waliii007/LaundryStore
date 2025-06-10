using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singolton : MonoBehaviour
{
    public static Singolton Instance;
    public GenderSelectionPanel avatarPanelEventListener;
    public CountryEventListener countryEventListener;
    public UserInfoCanvasManager canvasManager; 
    private void OnEnable()
    {
        Instance = this;
    }
}
