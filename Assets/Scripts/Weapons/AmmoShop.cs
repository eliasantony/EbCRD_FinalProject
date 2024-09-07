using System;
using UnityEngine;

public class AmmoShop : MonoBehaviour
{
    public PlayerPoints playerPoints;

    public bool BuyAmmo(ProjectileGun gun)
    {
        return GameManager.instance.PurchaseAmmo(gun);
    }
}

public class WeaponShop : MonoBehaviour
{
    public PlayerPoints playerPoints;

    public bool BuyGun(ProjectileGun gun)
    {
        throw new NotImplementedException();
    }
}
