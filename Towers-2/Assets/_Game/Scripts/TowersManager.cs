using UnityEngine;
using UnityEngine.Events;

namespace Konithir.Tower2
{
    public class TowersManager : MonoBehaviour
    {
        [SerializeField]
        private TowerController[] _towerControllers;

        [SerializeField]
        private float _spawnRadius;

        public UnityEvent OnGameEnd;

        private int _tempActiveTowers;

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
            towerController.UpdateLivesCounter();
            towerController.StartRotating();
            towerController.StartShooting();
        }

        public void DisableAllTowers()
        {
            for (int i = 0; i < _towerControllers.Length; i++)
            {
                _towerControllers[i].CancelTasks();
                _towerControllers[i].gameObject.SetActive(false);
            }
        }

        public void CheckForGameEnd()
        {
            _tempActiveTowers = 0;

            for(int i = 0; i < _towerControllers.Length; i++)
            {
                if (_towerControllers[i].gameObject.activeInHierarchy)
                {
                    _tempActiveTowers++;
                }

                if(_tempActiveTowers > 1)
                {
                    return;
                }
            }

            OnGameEnd?.Invoke();
        }
    }
}
