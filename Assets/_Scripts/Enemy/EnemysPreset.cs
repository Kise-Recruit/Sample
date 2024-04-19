using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class EnemysPreset : MonoBehaviour
    {
        public void SetUpEnemys(Transform playerTransform, Transform enemyParent)
        {
            foreach(EnemyBase enemy in GetComponentsInChildren<EnemyBase>())
            {
                enemy.Init(playerTransform);
                enemy.transform.SetParent(enemyParent);
            }
        }
    }
}