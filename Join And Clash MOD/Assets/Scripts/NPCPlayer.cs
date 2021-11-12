using UnityEngine;

public class NPCPlayer : Player
{
    [Header("IS MOVE")]
    public bool isMove,isDie,isTriggerByPlayer;
    [Header("Renderer")]
    public Renderer characterRenderer;

    public bool isNextPlayer;
    protected new void Start()
    {
        isMove = isDie = isNextPlayer = isTriggerByPlayer= false;
        InputManager.instance.OnMouseHold += Move;
        InputManager.instance.OnMouseUp += Idle;
        InputManager.instance.OnMouseDown += StartRunAnimation;
        InputManager.instance.OnMouseDrag += PlayerSideMoves;

        capsuleCollider = GetComponent<CapsuleCollider>();
        PlayerManager.instance.OnClimaxIdleAnimation += Idle;
        PlayerManager.instance.OnClimaxWinAnimation += Win;
    }
    protected override void Update()
    {//base.Update();
        if (isNextPlayer)
        {
            CreateEnemyList();
            Nearest();
        }
        if (GameManager.instance.currentGameState == GameManager.GameState.Climax && PlayerManager.instance.currentPlayerStates == PlayerStates.Attack)
        {
            if (isTargetAvailable)
            {
                Chase(targetTransform);
            }
        }

    }
    protected override void Move()
    {
        if (isMove)
        {
            base.Move();
            
        }        
    }
    protected override void StartRunAnimation()
    {
        if (isMove)
        {
            base.StartRunAnimation();
        }
        
    }
    public override void CheckHealth()
    {
        if (health <= 0)
        {
            Die(true);
        }
    }
    protected override void PlayerSideMoves(float x)
    {
        if (isMove)
        {
            base.PlayerSideMoves(x);
        }        
    }
    private void OnCollisionEnter(Collision collision)
    {

        if ((collision.collider.CompareTag("Obstacle") || collision.collider.CompareTag("Enemy")) && GameManager.instance.currentGameState != GameManager.GameState.Climax)
        {
            isMove = false;
            isDie = true;
            PlayerManager.instance.npc.Remove(this.gameObject);
            Die(true);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isDie)
        {
            isMove = true;
            transform.tag = other.transform.tag;
            if (!isTriggerByPlayer)
            {
                animator.SetBool(Animator.StringToHash("Run"), true);
                characterRenderer.material = PlayerManager.instance.playerMaterial;
                Debug.LogError("QWERTy");
            }
          
            if (!PlayerManager.instance.npc.Contains(this.gameObject))
            {
                PlayerManager.instance.npc.Add(this.gameObject);
            }

            isTriggerByPlayer = true;
        }

    }
}
