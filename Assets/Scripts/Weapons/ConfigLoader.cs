using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class ConfigLoader : MonoBehaviour
{
    public GunConfig gunConfig;

    public IEnumerator LoadGunConfig(string weaponName)
    {
        string path = Path.Combine(Application.streamingAssetsPath, weaponName + ".json");

        #if UNITY_WEBGL
            // WebGL-specific file loading via HTTP
            path = Path.Combine(Application.streamingAssetsPath, weaponName + ".json");
        #endif

        UnityWebRequest request = UnityWebRequest.Get(path);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            gunConfig = JsonUtility.FromJson<GunConfig>(request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Error loading config file: " + request.error);
        }
    }

}