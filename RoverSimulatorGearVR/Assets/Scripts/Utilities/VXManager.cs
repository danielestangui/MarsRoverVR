using System;
using System.Collections;
using UnityEngine;
using UnityEngine.XR;

public class VXManager : MonoBehaviour {


    public void enableVR()
    {
        foreach (string str in XRSettings.supportedDevices) Debug.Log(str);

        StartCoroutine(LoadDevice("Oculus", true));
    }

    IEnumerator LoadDevice(string newDevice, bool enable)
    {
        XRSettings.LoadDeviceByName(newDevice);
        //VRSettings.LoadDeviceByName("OpenVR");
       yield return null;
        XRSettings.enabled = enable;
    }

    public void disableVR()
    {
        StartCoroutine(LoadDevice("", false));
    }
}
