using System.Collections;
using UnityEngine;
public class Player : MonoBehaviour
{
    [Header("Animator")]
    public Animator animator;
    [Header("Capsule collider")]
    public CapsuleCollider capsuleCollider;
    public float capsuleColliderHeight;

    [Header("Castle Enemy")]
    public CastleEnemy castleEnemy;
    [Header("Boss Enemy")]
    public BossEnemy bossEnemy;
    [Header("Enemy Transform")]
    public Transform targetTransform;

    [Header("Move Speed")]
    [SerializeField] private float moveSpeed;
    [Header("Clamp Value on X Axis")]
    [SerializeField] private float xMinimum,xMaximum;
    [Header("Move speed on X Axis")]
    [SerializeField] private float xMoveSpeed;
    [Header("Move speed on NPC")]
    [SerializeField] private float xMoveSpeedNPC;

    [Header("Health")]
    public float health;   
    [Header("Attack Power")]
    public float attackPower;

    private RaycastHit hit;
    public bool isTargetAvailable;
    public bool isPlayerDead;

    protected void Start()
    {
        InputManager.instance.OnMouseHold += Move;
        InputManager.instance.OnMouseUp += Idle;
        InputManager.instance.OnMouseDown += StartRunAnimation;
        InputManager.instance.OnMouseDrag += PlayerSideMoves;
        
        PlayerManager.instance.OnClimaxIdleAnimation += Idle;
        PlayerManager.instance.OnClimaxWinAnimation += Win;

        capsuleCollider = GetComponent<CapsuleCollider>();
        isPlayerDead = false;
    }
  
    protected virtual void Update()
    {
        CreateEnemyList();
        Nearest();

        if (GameManager.instance.currentGameState == GameManager.GameState.Climax && (PlayerManager.instance.currentPlayerStates == PlayerStates.Running ||PlayerManager.instance.currentPlayerStates == PlayerStates.Attack))
        {
            if (isTargetAvailable)
            {
                Chase(targetTransform);
            }           
        }
    }
    public void Chase(Transform target)
    {
        if (target != null)
        {
            if (Vector3.Distance(transform.position, target.position) > 4)
            {
                var pos = target.position;
                pos.y = 0.25f;
                target.position = pos;
                transform.LookAt(target);
                transform.Translate(transform.forward * moveSpeed * Time.deltaTime, Space.World);
                StartRunAnimation();
            }
            else
            {
                AttackAnimation();
            }
        }
        else
        {
            var newTarget = EnemyManager.instance.EnemyTransform();

            if (newTarget != null)
            {
                targetTransform = newTarget;
                castleEnemy = targetTransform.GetComponent<CastleEnemy>();
            }
            else
            {
                PlayerManager.instance.currentPlayerStates = PlayerStates.Win;
                GameManager.instance.currentGameState = GameManager.GameState.Win;
                PlayerManager.instance.SwitchPlayerStates();
                GameManager.instance.SwitchGameStates();
                isTargetAvailable = false;
            }
        }
    }
    protected virtual void StartRunAnimation()
    {
        if (PlayerManager.instance.currentPlayerStates == PlayerStates.Running && GameManager.instance.currentGameState == GameManager.GameState.GamePlay)
        {
            animator.SetBool(Animator.StringToHash("Idle"), false);
            animator.SetBool(Animator.StringToHash("Run"), true);
        }
        else if(PlayerManager.instance.currentPlayerStates == PlayerStates.Attack && GameManager.instance.currentGameState == GameManager.GameState.Climax)
        {
            animator.SetBool(Animator.StringToHash("Idle"), false);
            animator.SetBool(Animator.StringToHash("Run"), true);
        }
    }
    protected virtual void Move()
    {        
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime, Space.World);
    }
    protected void Idle()
    {
        if (PlayerManager.instance.currentPlayerStates == PlayerStates.Idle && GameManager.instance.currentGameState == GameManager.GameState.GamePlay)
        {
            animator.SetBool(Animator.StringToHash("Run"), false);
            animator.SetBool(Animator.StringToHash("Idle"), true);
        }
        else if(PlayerManager.instance.currentPlayerStates == PlayerStates.ClimaxIdle && GameManager.instance.currentGameState == GameManager.GameState.Climax)
        {
            animator.SetBool(Animator.StringToHash("Run"), false);
            animator.SetBool(Animator.StringToHash("Idle"), true);
        }
    }
    protected void Win()
    {
        if(PlayerManager.instance.currentPlayerStates == PlayerStates.Win)
        {
            animator.SetBool(Animator.StringToHash("Attack"), false);
            animator.SetBool(Animator.StringToHash("Win"), true);            
        }
    }
    public virtual void CheckHealth()
    {
        if (health <= 0)
        {
            Die(false);
        }
    }
    protected void AttackAnimation()
    {
        animator.SetBool(Animator.StringToHash("Idle"), false);
        animator.SetBool(Animator.StringToHash("Attack"), true);
    }
    protected void Die(bool isNpc)
    {
        animator.SetTrigger(Animator.StringToHash("Die"));
        capsuleCollider.height = capsuleColliderHeight;
       
        InputManager.instance.OnMouseHold -= Move;
        InputManager.instance.OnMouseUp -= Idle;
        InputManager.instance.OnMouseDown -= StartRunAnimation;
        InputManager.instance.OnMouseDrag -= PlayerSideMoves;
        
        PlayerManager.instance.OnClimaxIdleAnimation -= Idle;
        PlayerManager.instance.OnClimaxWinAnimation -= Win;

        if (PlayerManager.instance.npc.Contains(gameObject))
        {
            PlayerManager.instance.npc.Remove(gameObject);
        }

        if (isNpc && isPlayerDead)
        {
            Destroy(gameObject, 1f);
        }
        else
        {           
            Invoke("DisablePlayer",0f);
            ChooseNextPlayer();
            isPlayerDead = true;
        }

        if (isPlayerDead && PlayerManager.instance.npc.Count <= 0) 
        {
            EnemyManager.instance.currentEnemyStates = EnemyStates.Win;
            GameManager.instance.currentGameState = GameManager.GameState.Lose;
            EnemyManager.instance.SwitchEnemyStates();
            GameManager.instance.SwitchGameStates();
        }
    }
    
    public virtual void Attack()
    {
        if (targetTransform != null)
        {
            if(castleEnemy != null)
            {
                castleEnemy.health -= attackPower;
                if (castleEnemy.health <= 0)
                {
                    castleEnemy.CheckHealth();
                    targetTransform = null;
                    castleEnemy = null;
                }
            }
            else
            {
                bossEnemy.CurrentHealth -= attackPower;
                bossEnemy.HealthBarAnimation();
                if (bossEnemy.CurrentHealth <= 0)
                {
                    bossEnemy.CheckHealth();
                    targetTransform = null;
                    bossEnemy = null;
                }
            }
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
        if ((collision.collider.CompareTag("Obstacle") || collision.collider.CompareTag("Enemy")) && GameManager.instance.currentGameState != GameManager.GameState.Climax)
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
    protected void Nearest()
    {
        float distance = 0;
        if (EnemyManager.instance.enemyList.Count > 0 && PlayerManager.instance.npc.Count > 0)
        {
            for (int i = 0; i < EnemyManager.instance.enemyList.Count; i++)
            {
                distance = Vector3.Distance(transform.position, EnemyManager.instance.enemyList[i].transform.position);
                if (distance < PlayerManager.instance.enemyrange)
                {
                    var enemy = EnemyManager.instance.enemyList[i].GetComponent<Enemy>();
                    enemy.isMove = true;
                    EnemyManager.instance.currentEnemyStates = EnemyStates.Chase;
                    if (PlayerManager.instance.npc.Count > 0 && GameManager.instance.currentGameState != GameManager.GameState.Climax)
                    {
                        var npc = PlayerManager.instance.npc[0].GetComponent<NPCPlayer>();
                        npc.moveSpeed = 8;
                        npc.RemoveFromInputManager();
                        var pos = npc.transform.position;
                        var x = Mathf.Lerp(pos.x, enemy.transform.position.x, Time.deltaTime * xMoveSpeedNPC);
                        pos.x = x;
                        npc.transform.position = pos;
                    }
                }
            }
        }
        else if (EnemyManager.instance.enemyList.Count > 0 && PlayerManager.instance.npc.Count == 0)
        {
            for (int i = 0; i < EnemyManager.instance.enemyList.Count; i++)
            {
                distance = Vector3.Distance(transform.position, EnemyManager.instance.enemyList[i].transform.position);
                if (distance < PlayerManager.instance.enemyrange)
                {
                    var enemy = EnemyManager.instance.enemyList[i].GetComponent<Enemy>();

                    if (!enemy.HasPlayerPassed())
                    {

                        EnemyManager.instance.currentEnemyStates = EnemyStates.Chase;
                        enemy.Chase(transform);

                    }
                    else if (enemy.HasPlayerPassed())
                    {
                        EnemyManager.instance.currentEnemyStates = EnemyStates.Idle;
                        enemy.Idle(transform);

                    }
                }
            }
        }               
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + transform.forward * 5, 20);
    }
    protected void ChooseNextPlayer()
    {
        if(PlayerManager.instance.npc.Count > 0 && !PlayerManager.instance.currentPlayer.activeInHierarchy)
        {
            Debug.LogError("Error");
            PlayerManager.instance.npc[0].GetComponent<NPCPlayer>().isNextPlayer = true;
            CameraManager.instance.target = PlayerManager.instance.npc[0].transform;
        }
    }
    private void DisablePlayer()
    {
        gameObject.SetActive(false);
    }
}
