using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneLight : MonoBehaviour
{
    public Light MyLight;
    private GUIBattery guiBattery;

    public float DefaultIntensity = 1f;
    public float YellowIntensity = 0.9f;
    public float RedIntensity = 0.8f;

    Coroutine blinkCoroutine;

    void Start()
    {
        //guiBattery = GUIManager.Inst.Get<GUIPlaying>().GuiBattery;
        //guiBattery.OnChangedValue.AddListener(OnChangedBattery);
        //guiBattery.OnChangedStatus.AddListener(OnChangeBatteryStatus);

        MyLight.enabled = false;
    }

    private void OnChangeBatteryStatus(GUIBattery.BatteryStatus status)
    {
        if (status == GUIBattery.BatteryStatus.GREEN)
        {
            StopBlinkLight();
            MyLight.intensity = DefaultIntensity;
        }
        else if (status == GUIBattery.BatteryStatus.YELLOW)
        {
            StopBlinkLight();
            MyLight.intensity = DefaultIntensity * YellowIntensity;
        }
        else if (status == GUIBattery.BatteryStatus.RED)
        {
            if (blinkCoroutine == null)
                blinkCoroutine = StartCoroutine(BlinkLight(0.1f, 2f, RedIntensity));
        }
        else
        {
            StopBlinkLight();
            MyLight.intensity = 0;
        }
    }

    void StopBlinkLight()
    {
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
            blinkCoroutine = null;
        }
    }

    IEnumerator BlinkLight(float minSec, float maxSec, float intensity)
    {
        float offSecond = 0.1f;
        while (true)
        {
            MyLight.intensity = Random.Range(0, intensity);
            yield return new WaitForSeconds(offSecond);
            MyLight.intensity = intensity;
            yield return new WaitForSeconds(Random.Range(minSec, maxSec));

        }
    }

    //private void OnChangedBattery(float value)
    //{
    //    MyLight.intensity = DefaultIntensity * value;
    //}

}
