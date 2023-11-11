using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraManager : MonoBehaviour, IWorldSwitcher
{
    private Camera camera;

    private void Awake() {
        camera = GetComponent<Camera>();
    }

    private void OnEnable() {
        WorldSwitcher.Instance.RegisterSwitcher(this);
    }

    private void OnDisable() {
        WorldSwitcher.Instance.UnregisterSwitcher(this);
    }
    
    public void OnSwitchWorld(bool firstWorldActive)
    {
        camera.transform.eulerAngles = new Vector3(0, 0, firstWorldActive ? 0 : 180);
    }
}
