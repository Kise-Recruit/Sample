using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class MoveState : IEnemyState
    {
        private EnemyBase main;
        public MoveState(EnemyBase enemy) => main = enemy;

        public EnemyState State => EnemyState.Move;
        public void Init() 
        {
            main.StartAnimation();
        }

        public void Update() 
        {
            // 向く
            var direction = main.PlayerPosition - main.transform.position;
            direction.y = 0;
 
            var lookRotation = Quaternion.LookRotation(direction, Vector3.up);
            main.transform.rotation = Quaternion.Lerp(main.transform.rotation, lookRotation, 0.1f);

            // 移動
            main.transform.position += main.transform.forward * Time.deltaTime;
        }

        public void Exit() {}
    }
}