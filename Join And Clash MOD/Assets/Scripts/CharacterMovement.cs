using UnityEngine;

public class CharacterMovement : PlayerMovement
{
    [Header("IS MOVE")]
    public bool isMove,isDie;
    [Header("Renderer")]
    public Renderer characterRenderer;

    public bool isNextPlayer;
    protected override void Start()
    {
        isMove = isDie = isNextPlayer = false;
        //ClimaxIdleAnimation += Idle;
        base.Start();
    }
    protected override void Update()
    {
        if (isNextPlayer)
        {
            base.Update();
        }
        
    }
    protected override void Move()
    {
        if (isMove)
        {
            base.Move();
            
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
            animator.SetTrigger(Animator.StringToHash("ToIdle"));
            characterRenderer.material = PlayerManager.instance.playerMaterial;
            if (!PlayerManager.instance.npc.Contains(this.gameObject))
            {
                PlayerManager.instance.npc.Add(this.gameObject);
            }
        }
        if (other.CompareTag("EnemyTrigger"))
        {
            ClimaxIdleAnimation?.Invoke();
        }
    }
}
