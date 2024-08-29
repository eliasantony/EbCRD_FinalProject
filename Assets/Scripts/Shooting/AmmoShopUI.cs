using System;
using UnityEngine;
using UnityEngine.UI;

public class AmmoShopUI : MonoBehaviour
{
    public AmmoShop ammoShop;
    public ProjectileGun gun;
    public Text pointsText;
    public PlayerPoints playerPoints;

    private void Start()
    {
        
    }

    private void Update()
    {
        pointsText.text = "Points: " + playerPoints.points;
    }


}
