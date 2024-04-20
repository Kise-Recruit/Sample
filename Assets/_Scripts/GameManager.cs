using System.Collections;
using System.Collections.Generic;
using Enemy;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] PlayerCharacter playerCharacter;
    [SerializeField] EnemyFactory enemyFactory;

    void Start()
    {
        enemyFactory.Init(playerCharacter.transform);
    }

    void Update()
    {
        
    }
}
