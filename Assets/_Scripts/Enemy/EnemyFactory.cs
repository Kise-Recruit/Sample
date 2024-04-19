using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Enemy
{
    public class EnemyFactory : MonoBehaviour
    {
        [SerializeField] Transform spownPointTransform;
        [SerializeField] Transform enemysParentTransform;
        [SerializeField] List<EnemysPreset> enemyPresetList = new List<EnemysPreset>();

        private float generateTimer = 0.0f;
        private Transform playerTransform;
        private Transform[] spownPositionList;

        public void Init(Transform playerTransform)
        {
            this.playerTransform = playerTransform;
            spownPositionList = spownPointTransform.GetComponentsInChildren<Transform>();
        }

        void Update()
        {
            generateTimer -= Time.deltaTime;

            if (generateTimer <= 0.0f)
            {
                GenerateEnemyPreset();
            }
        }

        private void GenerateEnemyPreset()
        {
            // 敵集団のプリセットを生成
            int spownRndIndex = Random.Range(0, spownPositionList.Length);
            int enemypresetRndIndex = Random.Range(0, enemyPresetList.Count);
            EnemysPreset newPreset = Instantiate(enemyPresetList[enemypresetRndIndex], spownPositionList[spownRndIndex].position, Quaternion.identity);
            newPreset.SetUpEnemys(playerTransform, enemysParentTransform);
        }
    }
}