using System.IO;
using UnityEngine;

public class ConfigLoader : MonoBehaviour
{
    public GunConfig gunConfig;
    // public BulletConfig bulletConfig;

    public void LoadGunConfig(string weaponName)
    {
        string path = Path.Combine(Application.streamingAssetsPath, weaponName + ".json");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            gunConfig = JsonUtility.FromJson<GunConfig>(json);
        }
        else
        {
            Debug.LogError("Config file not found: " + path);
        }
    }

    /*
    public void LoadBulletConfig(string bulletName)
    {
        string path = Path.Combine(Application.streamingAssetsPath, bulletName + ".json");
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            bulletConfig = JsonUtility.FromJson<BulletConfig>(json);
        }
        else
        {
            Debug.LogError("Config file not found: " + path);
        }
    }
    **/
}