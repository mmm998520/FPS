using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

namespace Com.ABCDE.MyApp
{
    public class PlayerUIMine : MonoBehaviour
    {
        public Transform HPBar;
        void Start()
        {

        }

        void Update()
        {
            HPBar.localScale = new Vector3(PlayerManager.HPofMine, 1, 1);
        }
    }
}