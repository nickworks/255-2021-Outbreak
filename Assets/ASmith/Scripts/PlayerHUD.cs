using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    public Image ammo1;
    public Image ammo2;
    public Image ammo3;
    public Image ammo4;
    public Image ammo5;
    public Image ammo6;
    public Image ammo7;
    public Image ammo8;
    public Image ammo9;
    public Image ammo10;
    float roundsInClip;

    void Start()
    {
        roundsInClip = 10;
    }

    void Update()
    {
        RenderAmmoCount();
    }

    private void RenderAmmoCount()
    {
        //if (PlayerWeapon.roundsInClip == 9)
        //{
        //    ammo10.enabled = false;
        //}
    }
}
