using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Cinemachine;
using Cysharp.Threading.Tasks;

namespace Player
{
    public class UltimateState : IPlayerState
    {
        private PlayerCharacter main;
        private Action onStartAttack;
        private Action onEndAttack;
        private Action cleateUltimateTexture;
        private CancellationTokenSource cts;

        private Dictionary<CinemachineVirtualCamera, float> ultimateCameraDictionary = new Dictionary<CinemachineVirtualCamera, float>();

        public PlayerState State => PlayerState.Ultimate;

        public UltimateState(PlayerCharacter player, Action onStartAttack, Action onEndAttack, Action cleateUltimateTexture)
        {
            main = player;
            this.onStartAttack = onStartAttack;
            this.onEndAttack = onEndAttack;
            this.cleateUltimateTexture = cleateUltimateTexture;

            // カメラのブレンドリストから、カメラ＋遷移の時間の Dictionary を作成
            var camerablends = main.BrainCamera.m_CustomBlends.m_CustomBlends;
            foreach(var customBlend in main.BrainCamera.m_CustomBlends.m_CustomBlends)
            {
                var targetVirtualCam = main.UltimateVirtualCameras.Find(cam => cam.gameObject.name == customBlend.m_From);
                ultimateCameraDictionary.Add(targetVirtualCam, customBlend.m_Blend.m_Time);
            }
        }

        public void Init() 
        {
            main.StartAnimation();

            // ウルト用のカメラにする
            main.DefaultCamera.enabled = false;

            cts?.Cancel();
            cts = new CancellationTokenSource();
            PlayUltimateCameraAsync(cts.Token).Forget();
        }

        public void Update() {}
        public void Exit() 
        {
            onEndAttack();
            cts?.Cancel();
            cts = null;
        }

        private async UniTask PlayUltimateCameraAsync(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            int camIndex = 0;

            foreach(var item in ultimateCameraDictionary)
            {
                camIndex++;

                if (item.Key.gameObject.name == "ChangeCamera_2")
                {
                    main.SetAnimSpeedScale(1.0f);
                }
                else if (item.Key.gameObject.name == "ChangeCamera_3")
                {
                    main.SetAnimSpeedScale(0.2f);
                }
                else if (item.Key.gameObject.name == "ChangeCamera_4")
                {
                    main.SetAnimSpeedScale(1.0f);
                    cleateUltimateTexture();
                }
                else if (item.Key.gameObject.name == "ChangeCamera_5")
                {
                    main.UltimateBreakWindow.SetActive(true);
                    main.BreakeWindow.BreakStart();

                    main.UltimateImpactEffect.gameObject.SetActive(true);
                    main.UltimateImpactEffect.Play();

                    onStartAttack();
                }
                else
                {
                    main.SetAnimSpeedScale(1.0f);
                }

                item.Key.enabled = false;
                await UniTask.Delay(TimeSpan.FromMilliseconds(item.Value * 1000.0f), false, PlayerLoopTiming.Update, token);

                if (token.IsCancellationRequested)
                {
                    break;
                }
            }

            onEndAttack();

            while(main.GetAnimationPlayTime < 0.95f)
            {
                await UniTask.Yield(token);
                if (token.IsCancellationRequested)
                {
                    break;
                }
            }


            // 通常のカメラに戻す
            main.UltimateBreakWindow.SetActive(false);
            main.DefaultCamera.enabled = true;

            // 必殺用の全カメラを戻す
            foreach(var cam in ultimateCameraDictionary.Keys)
            {
                cam.enabled = true;
            }

            main.SetAnimSpeedScale(1.0f);

            main.ChangeState(PlayerState.Idle);
        }
    }
}