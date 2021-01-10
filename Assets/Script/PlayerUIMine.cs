using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Com.ABCDE.MyApp
{
    public class PlayerUIMine : MonoBehaviour
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

        void Awake()
        {
            this.transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);
        }

        void Update()
        {
            // 當有不明原因, Photon 沒有將 Player 相關的 instance 清乾淨時
            if (target == null)
            {
                Debug.LogError("I did");
                Destroy(this.gameObject);
                return;
            }
            targetTransform = target.transform;
            if (playerHealthSlider != null)
            {
                playerHealthSlider.value = target.Health;
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
            print(target);
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