[System.Serializable]
public class GunConfig
{
    public float shootForce;
    public float upwardForce;
    public float timeBetweenShooting;
    public float spread;
    public float reloadTime;
    public float timeBetweenShots;
    public int magazineSize;
    public int bulletsPerTap;
    public bool allowButtonHold;
    public float recoilForce;
    public float aimHeight;
    public float aimSide;
}

[System.Serializable]
public class BulletConfig
{
    public float bounciness;
    public bool useGravity;
    public int explosionDamage;
    public float explosionRange;
    public float explosionForce;
    public int maxCollisions;
    public float maxLifetime;
    public bool explodeOnTouch;
}