using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    [Header("Component")]
    [SerializeField] private SortingGroup sorting;

    [Header("Value")]
    public float speed = 5f;
    public bool CanMove;
    public int loseTime = 0;

    [Header("Col")]
    public LayerMask colMask;

    [Header("Skin")]
    [SerializeField] private GameObject skin_0;
    [SerializeField] private GameObject skin_1;
    [SerializeField] private GameObject skin_2;
    [SerializeField] private GameObject skin_3;

    private Rigidbody2D rb;
    // Giá trị điều khiển (-1 trái, 0 đứng, 1 phải)
    private float moveDirection;
    private bool canChoice;

    private UIGameScreen gameScreen;
    private ObstcleChoice obstcleChoice;

    private GameObject currentSkin;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Init()
    {
        gameScreen = UIManager.Instance.GetActiveScreen<UIGameScreen>();
        CanMove = true;

        ChangeSkin(loseTime);

        // Input
        gameScreen.leftBtn.OnButtonDown = MoveLeft;
        gameScreen.leftBtn.OnButtonUp = StopMove;

        gameScreen.rightBtn.OnButtonDown = MoveRight;
        gameScreen.rightBtn.OnButtonUp = StopMove;

        gameScreen.choiceBtn.OnButtonDown = ChoiceAction;
    }

    //private void Update()
    //{
    //    if (GamePlay.TargetPlatform == PlatformEnum.Mobile) return;

    //    if (Input.GetKey(KeyCode.LeftArrow))
    //    {
    //        MoveLeft();
    //    }
    //    else if (Input.GetKey(KeyCode.RightArrow))
    //    {
    //        MoveRight();
    //    }
    //    else
    //    {
    //        StopMove();
    //    }
    //}

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveDirection * speed, rb.velocity.y);
    }

    // Được gọi từ Button
    public void MoveLeft()
    {
        if (!CanMove) return;

        moveDirection = -1;
        Flip(-1);

        animator.SetBool("IsMove", true);
    }

    public void MoveRight()
    {
        if (!CanMove) return;

        moveDirection = 1;
        Flip(1);

        animator.SetBool("IsMove", true);
    }

    public void StopMove()
    {
        moveDirection = 0;

        animator.SetBool("IsMove", false);
    }

    void Flip(int direction)
    {
        transform.localScale = new Vector3(direction, 1, 1);
    }

    public void PushLayer()
    {
        sorting.sortingLayerID = SortingLayer.NameToID("Extra");
    }

    public void ResetLayer()
    {
        sorting.sortingLayerID = SortingLayer.NameToID("Default");
    }

    public void ChoiceAction()
    {
        if (!canChoice) return;
        if (!CanMove) return;

        obstcleChoice.ShowChoice();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(Utils.CheckLayerMaskCollier2D(colMask, other))
        {
            canChoice = true;

            obstcleChoice = other.GetComponent<ObstcleChoice>();
            obstcleChoice.ShowChoice();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (Utils.CheckLayerMaskCollier2D(colMask, other))
        {
            canChoice = false;

            obstcleChoice = null;
        }
    }

    [Button]
    public void ChangeSkin(int id)
    {
        if(currentSkin != null)
        {
            currentSkin.SetActive(false);
        }

        switch (id)
        {
            case 0:
                currentSkin = skin_0;
                break;
            case 1:
                currentSkin = skin_1;
                break;
            case 2:
                currentSkin = skin_2;
                break;
            case 3:
                currentSkin = skin_3;
                break;
        }

        currentSkin.SetActive(true);
        animator = currentSkin.GetComponent<Animator>();
    }

    public void LostCoin()
    {
        loseTime++;
        ChangeSkin(loseTime);

        if (loseTime >= 3)
        {
            GamePlay.Instance.LoseGame();
        }
    }
}
