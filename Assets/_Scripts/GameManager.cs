using System.Collections;
using System.Collections.Generic;
using Enemy;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Player.PlayerCharacter playerCharacter;
    [SerializeField] EnemyFactory enemyFactory;

    void Start()
    {
        playerCharacter.Init();
        enemyFactory.Init(playerCharacter.transform);
    }

    void Update()
    {
        
    }
}
