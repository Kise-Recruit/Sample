using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public enum PlayerState
    {
        Idle,
        Move,
        Run,
        Avoid,
        Jump,
        Attack,
        Ultimate,
        ReceiveDamage,
        Die,
    }

    public interface IPlayerState
    {
        PlayerState State { get; }
        public void Init() {}
        public void Update() {}
        public void Exit() {}
    }
}