using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrigger : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("A");
        GameManager.instance.currentGameState = GameManager.GameState.Climax;
        Debug.Log("b");
        PlayerManager.instance.currentPlayerStates = PlayerStates.ClimaxIdle;
        Debug.Log("c");
        PlayerManager.instance.SwitchPlayerState();
        Debug.Log("d");
        EnemyManager.instance.StartSpawningEnemies();
        Debug.Log("e");
        gameObject.SetActive(false);
        //Destroy(this.gameObject, 1f);
    }
}
