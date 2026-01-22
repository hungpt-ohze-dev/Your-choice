using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    [Header("Component")]
    [SerializeField] private Animator animator;
    [SerializeField] private SortingGroup sorting;

    [Header("Value")]
    public float speed = 5f;

    [Header("Col")]
    public LayerMask colMask;


    private Rigidbody2D rb;
    // Giá trị điều khiển (-1 trái, 0 đứng, 1 phải)
    private float moveDirection;
    private bool canChoice;

    private UIGameScreen gameScreen;

    private ObstcleChoice obstcleChoice;

    public bool CanMove;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Init()
    {
        gameScreen = UIManager.Instance.GetActiveScreen<UIGameScreen>();
        CanMove = true;

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
}
