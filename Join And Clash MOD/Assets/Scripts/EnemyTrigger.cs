using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrigger : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        GameManager.instance.currentGameState = GameManager.GameState.Climax;
        PlayerManager.instance.currentPlayerStates = PlayerStates.ClimaxIdle;
        PlayerManager.instance.SwitchPlayerState();
        EnemyManager.instance.StartSpawningEnemies();
        Destroy(this.gameObject, 0.5f);
    }
}
