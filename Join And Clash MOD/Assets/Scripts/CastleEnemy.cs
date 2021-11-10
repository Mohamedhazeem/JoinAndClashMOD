using UnityEngine;
public class CastleEnemy : Enemy
{
    public Transform targetTransform;
    public bool isTargetAvailable;
    protected override void Start()
    {
        base.Start();
        isTargetAvailable = true;
    }

    protected override void Update()
    {
        
        if (isTargetAvailable)
        {
            Chase(targetTransform);
        }

    }
    //IEnumerator SpawnEnemiesCouroutine(int enemyCount)
    //{
    //    while (enemySpawnCount < enemyCount)
    //    {
    //        enemySpawnCount++;
    //        yield return waitForSeconds;
    //        GameObject gameObject = Instantiate(enemyPrefab, enemiesSpawnPoint.transform.position, Quaternion.Euler(0, 180, 0));
    //        var castleEnemy = gameObject.GetComponent<CastleEnemy>();
    //        castleEnemy.targetTransform = PlayerManager.instance.PlayerAndNPCTransform();
    //        currentEnemyStates = EnemyStates.Chase;
    //    }
    //}
    public override void Chase(Transform target)
    {
        Debug.Log(target.gameObject.activeInHierarchy);
        if(target != null && target.gameObject.activeInHierarchy)
        {
            if(Vector3.Distance(transform.position,target.position) > 1.5)
            {
                base.Chase(target);
            }
            else
            {
                Attack();
            }
            
        }
        else
        {
            targetTransform = null;
            var newTarget = PlayerManager.instance.PlayerAndNPCTransform();        
            if (newTarget != null)
            {
                targetTransform = newTarget;
            }
            else
            {
                EnemyManager.instance.currentEnemyStates = EnemyStates.Idle;
                Idle(); 
                isTargetAvailable = false;
            }
        }
    }
    public void Attack()
    {
        animator.SetBool(Animator.StringToHash( "Attack"), true);
    }
}
