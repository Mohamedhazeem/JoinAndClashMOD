using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Animator")]
    public Animator animator;

    [Header("Move Speed")]
    [SerializeField] private float moveSpeed;
    [Header("Clamp Value on X Axis")]
    [SerializeField] private float xMinimum,xMaximum;
    [Header("Move speed on X Axis")]
    [SerializeField] private float xMoveSpeed;
 
    protected void Start()
    {
        InputManager.instance.OnMouseHold += Move;
        InputManager.instance.OnMouseUp += StopMove;
        InputManager.instance.OnMouseDown += StartRunAnimation;
        InputManager.instance.OnMouseDrag += PlayerSideMoves;

    }

    protected void StartRunAnimation()
    {
        if (PlayerManager.instance.currentPlayerStates == PlayerStates.Running && GameManager.instance.currentGameState == GameManager.GameState.GamePlay)
        {
            animator.SetTrigger(Animator.StringToHash("Run"));
        }
    }
    protected virtual void Move()
    {        
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime, Space.World);
    }
    protected void StopMove()
    {
        if (PlayerManager.instance.currentPlayerStates == PlayerStates.Idle && GameManager.instance.currentGameState == GameManager.GameState.GamePlay)
        {
            animator.SetTrigger(Animator.StringToHash("Idle"));
        }
    }
    protected virtual void PlayerSideMoves(float x)
    {
        transform.Translate(Vector3.right * x* xMoveSpeed * Time.deltaTime, Space.World);
        var position = transform.position;
        position.x = Mathf.Clamp(position.x, xMinimum, xMaximum);
        transform.position = position;
    }
 
}
