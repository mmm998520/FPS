using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ins : MonoBehaviour
{
    public GameObject floor, sideWall;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = -35; i <= 35; i++)
        {
            for (int j = -35; j <= 35; j++)
            {
                if ((i + 100) % 2 == 1 && (j + 100) % 2 == 1)
                {
                    Instantiate(floor, new Vector3(i, 0.5f, j), Quaternion.identity, transform);
                }
                if (Mathf.Abs(i) == 35|| Mathf.Abs(j) == 35)
                {
                    Instantiate(sideWall, new Vector3(i, 1.5f, j), Quaternion.identity, transform);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
