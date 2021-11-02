using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Animator")]
    public Animator animator;
    [Header("Capsule collider")]
    public CapsuleCollider capsuleCollider;
    public float capsuleColliderHeight;
    [Header("Move Speed")]
    [SerializeField] private float moveSpeed;
    [Header("Clamp Value on X Axis")]
    [SerializeField] private float xMinimum,xMaximum;
    [Header("Move speed on X Axis")]
    [SerializeField] private float xMoveSpeed;

    private RaycastHit hit;

    protected virtual void Start()
    {
        InputManager.instance.OnMouseHold += Move;
        InputManager.instance.OnMouseUp += StopMove;
        InputManager.instance.OnMouseDown += StartRunAnimation;
        InputManager.instance.OnMouseDrag += PlayerSideMoves;

        
        capsuleCollider = GetComponent<CapsuleCollider>();

    }
    protected virtual void Update()
    {
        CreateEnemyList();
        
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
    protected void Die(bool isNpc)
    {
        animator.SetTrigger(Animator.StringToHash("Die"));
        capsuleCollider.height = capsuleColliderHeight;
        ChooseNextPlayer();

        InputManager.instance.OnMouseHold -= Move;
        InputManager.instance.OnMouseUp -= StopMove;
        InputManager.instance.OnMouseDown -= StartRunAnimation;
        InputManager.instance.OnMouseDrag -= PlayerSideMoves;

        if (isNpc)
        {
            Destroy(gameObject, 1f);
        }
        else
        {
            Invoke("DisablePlayer", 1f);
        }
       
    }

    protected virtual void PlayerSideMoves(float x)
    {
        transform.Translate(Vector3.right * x* xMoveSpeed * Time.deltaTime, Space.World);
        var position = transform.position;
        position.x = Mathf.Clamp(position.x, xMinimum, xMaximum);
        transform.position = position;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Obstacle") || collision.collider.CompareTag("Enemy"))
        {
            Die(false);
        }
    }
    private void OnDisable()
    {
        Die(false);
    }
    protected void CreateEnemyList()
    {
        if (Physics.SphereCast(transform.position, 15, transform.forward, out hit, 10, PlayerManager.instance.layerMask))
        {

            if (EnemyManager.instance.enemyList.Contains(hit.transform.gameObject))
            {
                return;
            }
            else
            {
                EnemyManager.instance.enemyList.Add(hit.transform.gameObject);
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + transform.forward * 5, 20);
    }
    protected void ChooseNextPlayer()
    {
        if(PlayerManager.instance.npc.Count > 0)
        {
            PlayerManager.instance.npc[0].GetComponent<CharacterMovement>().isNextPlayer = true;
            CameraManager.instance.target = PlayerManager.instance.npc[0].transform;
        }
    }
    private void DisablePlayer()
    {
        gameObject.SetActive(false);
    }
}
