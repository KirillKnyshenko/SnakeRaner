using UnityEngine;
using UnityEngine.Advertisements;

public class AdsInitializer : MonoBehaviour
{
    void Awake()
    {
        Advertisement.Initialize("4410523", false);
    }

    public static void ShownInterstitialAds()
    {
        if (Advertisement.IsReady())
        {
            Advertisement.Show("Interstitial_Android");
        }
    }
}