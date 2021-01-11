using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun.Demo.PunBasics;

namespace Com.ABCDE.MyApp
{
    public class CameraWorkSet : MonoBehaviour
    {
        public static float centerOffsetY = 2.09f, originCenterY = 2.09f;
        CameraWork cameraWork;

        void Start()
        {
            cameraWork = GetComponent<CameraWork>();
        }

        void Update()
        {
            centerOffsetY = Mathf.Lerp(centerOffsetY, originCenterY, Time.deltaTime * 10);
            cameraWork.centerOffset = new Vector3(0, centerOffsetY, 0);
        }
    }
}