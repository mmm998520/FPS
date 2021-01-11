using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;

namespace Com.ABCDE.MyApp
{
    public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
    {
        [Tooltip("指標- GameObject Beams")]
        public Transform[] HitPoses;
        bool IsFiring;
        public float FiringTimer;
        [Tooltip("玩家的血量")]
        public float HP = 1f;
        [Tooltip("玩家角色的 instance")]
        public static GameObject LocalPlayerInstance;
        [Tooltip("指標- GameObject PlayerUI")]
        [SerializeField]
        public GameObject PlayerUIPrefab;
        //public GameObject PlayerUIMinePrefab;
        Animator animator;
        public Transform NamePos;

        public static float HPofMine;
        public static float ShootTimer;
        public static bool ShootIt;

        void Awake()
        {
            // 記錄玩家角色的 instance, 避免在重載場景時, 又再生成一次
            if (photonView.IsMine)
            {
                PlayerManager.LocalPlayerInstance = this.gameObject;
                animator = GetComponent<Animator>();
            }

            // 標註玩家角色的 instance 不會在重載場景時被砍殺掉
            DontDestroyOnLoad(this.gameObject);
        }

        void Start()
        {
            CameraWork _cameraWork = gameObject.GetComponent<CameraWork>();
            if (_cameraWork != null)
            {
                if (photonView.IsMine)
                {
                    HPofMine = 1;
                    ShootTimer = 10;
                    ShootIt = false;
                    PlayerUI.MyPlayerManager = transform;
                    _cameraWork.OnStartFollowing();
                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().enabled = false;
                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                }
                else
                {
                    if (GetComponent<CameraWorkSet>())
                    {
                        GetComponent<CameraWorkSet>().enabled = false;
                    }
                }
            }
            else
            {
                Debug.LogError("playerPrefab- CameraWork component 遺失", this);
            }
            if (photonView.IsMine)
            {

            }
            else
            {
                if (PlayerUIPrefab != null)
                {
                    GameObject _uiGo = Instantiate(PlayerUIPrefab);
                    _uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
                }
                else
                {
                    Debug.LogWarning("指標- GameObject PlayerUI 為空值", this);
                }
            }

#if UNITY_5_4_OR_NEWER
            UnityEngine.SceneManagement.SceneManager.sceneLoaded +=
                (scene, loadingMode) =>
                {
                    if (this != null)
                    {
                        this.CalledOnLevelWasLoaded(scene.buildIndex);
                    }
                };
#endif
        }

        void Update()
        {
            if (photonView.IsMine)
            {
                ShootTimer += Time.deltaTime;
                Fire();
                Move();
                HPofMine = HP;
                if (HP <= 0f)
                {
                    GameManager.Instance.LeaveRoom();
                }
            }
            /*
            if (beams != null && IsFiring != beams.activeSelf)
            {
                beams.SetActive(IsFiring);
            }
            */

            /*
            if ((FiringTimer += Time.deltaTime) >= 0.5f)
            {
                if (IsFiring)
                {
                    IsFiring = false;
                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    beams.transform.GetChild(0).localPosition = Vector3.zero;
                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                }
            }
            */
        }

        void Move()
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                GetComponent<Rigidbody>().velocity = transform.rotation * new Vector3(Input.GetAxis("Horizontal") * 6, GetComponent<Rigidbody>().velocity.y, Input.GetAxis("Vertical") * 6);
            }
            else
            {
                GetComponent<Rigidbody>().velocity = transform.rotation * new Vector3(Input.GetAxis("Horizontal") * 3, GetComponent<Rigidbody>().velocity.y, Input.GetAxis("Vertical") * 3);
            }
            if (!Input.GetKey(KeyCode.LeftAlt))
            {
                transform.Rotate(0, Input.GetAxis("Mouse X") * 10, 0);
                float mouseY = Input.GetAxis("Mouse Y") * 0.01f;
                if ((CameraWorkSet.originCenterY >= 1.9f && CameraWorkSet.originCenterY <= 2.2f) || (CameraWorkSet.originCenterY > 2.2f && mouseY < 0) || (CameraWorkSet.originCenterY < 1.9f && mouseY > 0))
                {
                    CameraWorkSet.originCenterY += mouseY;
                    CameraWorkSet.centerOffsetY += mouseY;
                }
            }
            animator.SetBool("Run", GetComponent<Rigidbody>().velocity.magnitude > 0.01f);
        }

        void Fire()
        {
            // 按下發射鈕
            if (Input.GetButtonDown("Fire1"))
            {
                Debug.LogError(animator.GetCurrentAnimatorStateInfo(0).IsName("assault_combat_shoot"));
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("assault_combat_shoot") && ShootTimer > 0.5f)
                {
                    ShootTimer = 0;
                    if(CameraWorkSet.centerOffsetY < CameraWorkSet.originCenterY + 0.1f)
                    {
                        CameraWorkSet.centerOffsetY += 0.015f;
                    }
                    RaycastHit hit;
                    animator.SetTrigger("Shoot");
                    if (Physics.Raycast(new Ray(Camera.main.transform.position, Camera.main.ScreenToWorldPoint(new Vector3(Screen.width/2,Screen.height/2, 200)) - Camera.main.transform.position), out hit))
                    {
                        Debug.LogError(hit.collider.name + " : A", hit.collider.gameObject);
                        PlayerManager playerManager = hit.collider.GetComponent<PlayerManager>();
                        if (playerManager && playerManager != this)
                        {
                            playerManager.hurt();
                            ShootIt = true;
                        }
                        else
                        {
                            ShootIt = false;
                        }
                    }
                    if (!IsFiring)
                    {
                        IsFiring = true;
                        FiringTimer = 0;
                    }
                }
            }
        }

        public void hurt()
        {
            photonView.RPC("RPCHurt", RpcTarget.All);
            Debug.LogError(gameObject.name + " : B", gameObject);
        }

        [PunRPC]
        public void RPCHurt()
        {
            GetComponent<PlayerManager>().HP -= 0.1f;
            Debug.LogError(gameObject.name + " : C", gameObject);
        }
        /*
        void OnTriggerEnter(Collider other)
        {
            if (!photonView.IsMine)
            {
                return;
            }
            if (!other.name.Contains("Beam"))
            {
                return;
            }
            /*
             * if(other.transform == beams.transform.GetChild(0))
            {
                return;
            }
            * /
            Health -= 0.1f;
        }
        */
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                // 為玩家本人的狀態, 將 IsFiring 的狀態更新給其他玩家
                stream.SendNext(IsFiring);
                //stream.SendNext(Health);
                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                stream.SendNext(FiringTimer);
                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            }
            else
            {
                // 非為玩家本人的狀態, 單純接收更新的資料
                this.IsFiring = (bool)stream.ReceiveNext();
                //this.Health = (float)stream.ReceiveNext();
                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                this.FiringTimer = (float)stream.ReceiveNext();
                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            }
        }

#if !UNITY_5_4_OR_NEWER
void OnLevelWasLoaded(int level)
{
    this.CalledOnLevelWasLoaded(level);
}
#endif

        void CalledOnLevelWasLoaded(int level)
        {
            if (!Physics.Raycast(transform.position, -Vector3.up, 5f))
            {
                transform.position = new Vector3(0f, 5f, 0f);
            }
            GameObject _uiGo = Instantiate(this.PlayerUIPrefab);
            _uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
        }
    }
}