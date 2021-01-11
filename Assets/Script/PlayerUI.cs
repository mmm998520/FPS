using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Com.ABCDE.MyApp
{
    public class PlayerUI : MonoBehaviour
    {
        [Tooltip("玩家名稱")]
        [SerializeField]
        private Text playerNameText;

        [Tooltip("玩家血量")]
        [SerializeField]
        private Slider playerHealthSlider;

        private PlayerManager target;

        [Tooltip("名字字串在角色頭頂的距離")]
        [SerializeField]
        private Vector3 screenOffset = new Vector3(0f, 300f, 0f);

        float characterControllerHeight = 0f;
        Transform targetTransform;
        Vector3 targetPosition;
        bool hitPlayer = false;

        public static Transform MyPlayerManager;
        void Awake()
        {
            this.transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);
        }

        void Update()
        {
            // 當有不明原因, Photon 沒有將 Player 相關的 instance 清乾淨時
            if (target == null || target.photonView.IsMine)
            {
                Destroy(this.gameObject);
                return;
            }
            targetTransform = target.transform;
            hitPlayer = false;
            if(Vector3.Angle(MyPlayerManager.forward,targetTransform.position- MyPlayerManager.position)<45)
            {
                for(int i = 0; i < 12; i++)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(new Ray(Camera.main.transform.position, target.HitPoses[i].position - Camera.main.transform.position), out hit))
                    {
                        if (hit.collider.transform == targetTransform)
                        {
                            hitPlayer = true;
                            break;
                        }
                    }
                }
            }
            if (hitPlayer)
            {
                Debug.LogError("hit");
            }
            else
            {
                Debug.LogError("nothit");
            }
            /*
            for (int i = 0; i < Screen.width; i += Screen.width / 100)
            {
                for (int j = 0; j < Screen.height; j += Screen.height / 100)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(new Ray(Camera.main.transform.position, Camera.main.ScreenToWorldPoint(new Vector3(i,j,50))), out hit))
                    {
                        if(hit.collider.transform == targetTransform)
                        {
                            hitPlayer = true;
                        }
                    }
                }
                if (hitPlayer)
                {
                    break;
                }
            }
            */

            if (playerHealthSlider != null)
            {
                playerHealthSlider.value = target.HP;
            }
        }

        void LateUpdate()
        {
            if (hitPlayer)
            {
                if (targetTransform != null)
                {
                    targetPosition = targetTransform.GetComponent<PlayerManager>().NamePos.position;
                    this.transform.position = Camera.main.WorldToScreenPoint(targetPosition);
                }
            }
            else
            {
                this.transform.position = Vector3.right * 999999;
            }
        }

        public void SetTarget(PlayerManager _target)
        {
            if (_target == null)
            {
                Debug.LogError("傳入的 PlayerManager instance 為空值", this);
                return;
            }

            target = _target;
            if (playerNameText != null)
            {
                playerNameText.text = target.photonView.Owner.NickName;
            }

            CharacterController characterController = _target.GetComponent<CharacterController>();
            if (characterController != null)
            {
                characterControllerHeight = characterController.height;
            }
        }
    }
}