using System.Threading.Tasks;
using UnityEngine;

namespace Konithir.Tower2
{
    public class TowerController : MonoBehaviour, IShootable, IDamageable
    {
        [SerializeField]
        private TowersManager _towerManager;

        [SerializeField]
        private BulletManager _bulletManager;

        [SerializeField]
        private int _maxLives = 3;

        [SerializeField]
        private GameObject _bulletSpawnPoint;

        private BulletController _bullet;
        private int _currentLives;

        private const int _MIN_ROTATE_DELAY = 10;
        private const int _MAX_ROTATE_DELAY = 1000;
        private const int _DEATH_DELAY = 2000;
        private const int _SHOOTING_DELAY = 1000;

        public void GetDamage()
        {
            gameObject.SetActive(false);
            _currentLives--;

            if(_currentLives > 0)
            {
                RespawnAfterDelay();
            }
        }

        public async void StartRotating()
        {
            while(gameObject.activeInHierarchy)
            {
                await Task.Delay(Random.Range(_MIN_ROTATE_DELAY, _MAX_ROTATE_DELAY));

                transform.localEulerAngles = new Vector3(0, Random.Range(0, 360), 0);
            }
        }

        public async void StartShooting()
        {
            while (gameObject.activeInHierarchy)
            {
                await Task.Delay(_SHOOTING_DELAY);
                Shoot();
            }
        }

        public void Shoot()
        {
            _bullet = _bulletManager.FindInactiveBullet();
            _bullet.ResetBullet();

            _bullet.transform.position = _bulletSpawnPoint.transform.position;
            _bullet.transform.localEulerAngles = _bulletSpawnPoint.transform.forward;

            _bullet.gameObject.SetActive(true);
            _bullet.AddForceForward();
        }

        public void ResetTower()
        {
            _currentLives = _maxLives;
        }

        private async void RespawnAfterDelay()
        {
            await Task.Delay(_DEATH_DELAY);
            _towerManager.SpawnTower(this, false);
        }
    }

}

