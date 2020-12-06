using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerController : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField]
    internal PlayerInput playerInput = null;
    [SerializeField]
    internal PlayerMovement playerMovement = null;
    [SerializeField]
    internal PlayerCollider playerCollider = null;
    [SerializeField]
    internal PlayerShooting playerShooting = null;

    [SerializeField]
    public Animator animator = null;
    [SerializeField]
    internal Rigidbody2D rb = null;

    [SerializeField]
    internal GameObject gun = null;

    public Vector2Int currentLocation;



    public int lifePoints;
    private int maxLifePoints = 100;

    [SerializeField]
    private Slider slider = null;
    

    internal bool living = true;
    //public bool targeting;

    [SerializeField]
    internal Button restart;

    [SerializeField]
    internal SpriteRenderer spriteRenderer = null;
    [SerializeField]
    internal SpriteRenderer gunSpriteRenderer = null;
    [SerializeField]
    internal Image ammoImage = null;

    private void Awake()
    {
        setPlayerComponents();
        initPlayer();
    }


    private void initPlayer()
    {
        living = true;
        lifePoints = maxLifePoints;
        slider.maxValue = maxLifePoints;
        slider.value = slider.maxValue;

        gunSpriteRenderer = gun.GetComponent<SpriteRenderer>();
    }

    private void setPlayerComponents()
    {
        slider = PlayerComponents.Instance.slider;
        restart = PlayerComponents.Instance.restartButton;
        ammoImage = PlayerComponents.Instance.ammoImage;
    }

    public void updateLifePoints(int points)
    {
        this.lifePoints += points;
        if (lifePoints >= maxLifePoints) lifePoints = maxLifePoints;
        else if(lifePoints <= 0)
        {
            //Kill player
            lifePoints = 0;
            living = false;
            gun.gameObject.SetActive(false);
            animator.SetBool("death", true);

            restart.gameObject.SetActive(true);
        }
        updateHealthUI();
    }

    private void updateHealthUI()
    {
        slider.value = this.lifePoints;
    }

    #region Pun callbacks

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(spriteRenderer.flipX);
        }else if (stream.IsReading)
        {
            spriteRenderer.flipX = (bool)stream.ReceiveNext();
        }
    }

    #endregion
}
