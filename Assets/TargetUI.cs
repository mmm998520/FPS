using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Com.ABCDE.MyApp
{
    public class TargetUI : MonoBehaviour
    {
        Image image;
        // Start is called before the first frame update
        void Start()
        {
            image = GetComponent<Image>();
        }

        // Update is called once per frame
        void Update()
        {
            if (PlayerManager.ShootTimer<0.5f)
            {
                if (PlayerManager.ShootIt)
                {
                    image.color = Color.red;
                }
                else
                {
                    image.color = Color.gray;
                }
            }
            else
            {
                image.color = Color.white;
            }
        }
    }
}
