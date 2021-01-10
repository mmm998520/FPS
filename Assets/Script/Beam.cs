using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.ABCDE.MyApp
{
    public class Beam : MonoBehaviour
    {
        void Start()
        {

        }

        void Update()
        {
            transform.Translate(Vector3.forward * Time.deltaTime * 20);
        }
    }
}
