﻿using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace Com.ABCDE.MyApp
{
    [RequireComponent(typeof(InputField))]
    public class PlayerNameInputField : MonoBehaviour
    {
        const string playerNamePrefKey = "PlayerName";

        void Start()
        {
            string defaultName = string.Empty;
            InputField _inputField =
                this.GetComponent<InputField>();
            if (_inputField != null)
            {
                if (PlayerPrefs.HasKey(playerNamePrefKey))
                {
                    defaultName = PlayerPrefs.
                        GetString(playerNamePrefKey);
                    _inputField.text = defaultName;
                }
            }
            // 設定遊戲玩家的名稱
            PhotonNetwork.NickName = defaultName;
        }

        public void SetPlayerName(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                Debug.LogError("Player Name is null or empty");
                return;
            }
            // 設定遊戲玩家的名稱
            PhotonNetwork.NickName = value;
            PlayerPrefs.SetString(playerNamePrefKey, value);
        }
    }
}