using System.Collections;
using MoreMountains.NiceVibrations;
using UnityEngine;

public static class Extentions
{
    private const float _delayIntervalBetweenWinHaptics = 0.15f;
    public static void PlayWinVibration(int countSteps, MonoBehaviour monoBehaviour)
    {
        monoBehaviour.StartCoroutine(PlayWinVibration(countSteps));
    }

    public static void StopAllVibraions()
    {
        MMVibrationManager.StopAllHaptics();
    }

    private static IEnumerator PlayWinVibration(int countSteps)
    {
        WaitForSeconds delay = new WaitForSeconds(_delayIntervalBetweenWinHaptics);

        for (int i = 0; i < countSteps; i++)
        {
            MMVibrationManager.Haptic(HapticTypes.MediumImpact);
            yield return delay;
        }
    }
}
