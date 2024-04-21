using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class ReceiveDamageState : IEnemyState
    {
        public ReceiveDamageState(EnemyBase enemy) => main = enemy;
        public EnemyState State => EnemyState.ReceiveDamage;

        private EnemyBase main;
        private Vector3 knockBackVeck;
        private float knockBackTime = 0.0f;

        public void Init() 
        {
            main.StartAnimation();

            knockBackVeck = main.transform.position - main.PlayerPosition;
            knockBackVeck.y = 0;
            knockBackVeck.Normalize();
            knockBackTime = 0.0f;
        }

        public void Update()
        {
            main.transform.position += knockBackVeck * Time.deltaTime;
            knockBackTime += Time.deltaTime;

            if (knockBackTime >= 0.6f)
            {
                main.ChangeState(EnemyState.Move);
            }
        }

        public void Exit() {}
    }
}