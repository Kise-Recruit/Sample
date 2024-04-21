using UnityEngine;

namespace Player
{
    public class MoveState : IPlayerState
    {
        private const float ROTATE_TIME = 0.2f;

        private PlayerCharacter main;
        public PlayerState State => PlayerState.Move;

        public MoveState(PlayerCharacter player)
        {
            main = player;
        }

        public void Init()
        {
            main.StartAnimation();
        }

        public void Update()
        {
            var inputValue = main.MoveInput.ReadValue<Vector2>();
    
            if (inputValue == Vector2.zero)
            {
                main.ChangeState(PlayerState.Idle);
            }

            // カメラの方向から、X-Z平面の単位ベクトルを取得
            Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
 
            // 方向キーの入力値とカメラの向きから、移動方向を決定
            Vector3 targetMoveVec = cameraForward * inputValue.y + Camera.main.transform.right * inputValue.x;

            main.transform.position += targetMoveVec.normalized * Time.deltaTime;
            main.transform.forward = Vector3.Slerp(main.transform.forward, targetMoveVec, ROTATE_TIME);
        }

        public void Exit() {}
    }
}