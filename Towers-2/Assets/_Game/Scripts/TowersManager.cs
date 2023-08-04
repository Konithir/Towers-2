using UnityEngine;

namespace Konithir.Tower2
{
    public class TowersManager : MonoBehaviour
    {
        [SerializeField]
        private TowerController[] _towerControllers;

        [SerializeField]
        private float _spawnRadius;

        public void SpawnTowers(int amountToSpawn)
        {
            for(int i = 0; i < amountToSpawn; i++)
            {
                SpawnTower(_towerControllers[i],true);
            }
        }

        public void SpawnTower(TowerController towerController, bool resetTower)
        {
            if(resetTower)
            {
                towerController.ResetTower();
            }

            towerController.transform.position = new Vector3(Random.Range(-_spawnRadius, _spawnRadius), 0, Random.Range(-_spawnRadius, _spawnRadius));
            towerController.gameObject.SetActive(true);
            towerController.StartRotating();
            towerController.StartShooting();
        }
    }
}
