using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Enemy
{
    public class AttackState : IEnemyState
    {
        private EnemyBase main;
        public AttackState(EnemyBase enemy) => main = enemy;

        public EnemyState State => EnemyState.Attack;
        public void Init() 
        {
            main.StartAnimation();
        }

        public void Update() {}
        public void Exit() {}
    }
}