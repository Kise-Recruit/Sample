using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Enemy
{
    public class IdleState : IEnemyState
    {
        private EnemyBase main;
        public IdleState(EnemyBase enemy) => main = enemy;

        public EnemyState State => EnemyState.Idle;
        public void Init() 
        {
            main.StartAnimation();
        }

        public void Update() {}
        public void Exit() {}
    }
}