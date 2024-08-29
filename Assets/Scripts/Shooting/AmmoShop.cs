using UnityEngine;

public class AmmoShop : MonoBehaviour
{
    public PlayerPoints playerPoints;

    public bool BuyAmmo(ProjectileGun gun)
    {
        if (gun != null && playerPoints.SpendPoints(gun.ammoCost))
        {
            gun.AddAmmo(gun.magazineSize); // Add one magazine worth of ammo
            UIManager.instance.UpdateAmmoDisplay(gun.bulletsLeft + " / " + gun.totalAmmo);
            return true;
        }
        else
        {
            UIManager.instance.UpdateReloadDisplay("Not enough points or no weapon equipped!");
            return false;
        }
    }
}