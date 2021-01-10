using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Com.ABCDE.MyApp
{
    public class PlayerAnimatorManager : MonoBehaviourPun
    {
        private Animator animator;
        [SerializeField]
        private float directionDampTime = 0.25f;

        void Start()
        {
            animator = GetComponent<Animator>();
            if (!animator)
            {
                Debug.LogError(
                    "PlayerAnimatorManager- Animator component 遺失", this);
            }
        }

        void Update()
        {
            if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
            {
                return;
            }
            if (!animator)
            {
                return;
            }
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            // 只有在跑動時, 才可以跳躍.
            if (stateInfo.IsName("Base Layer.Run"))
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    animator.SetTrigger("Jump");
                }
            }
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            if (v < 0)
            {
                v = 0;
            }
            animator.SetFloat("Speed", h * h + v * v);
            animator.SetFloat("Direction", h, directionDampTime, Time.deltaTime);
        }
    }
}