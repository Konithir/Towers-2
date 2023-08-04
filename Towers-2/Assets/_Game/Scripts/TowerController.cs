using System.Threading;
using System.Threading.Tasks;
using TMPro;
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

        [SerializeField]
        private TextMeshProUGUI _livesCounter;

        private BulletController _bullet;
        private int _currentLives;
        private CancellationTokenSource _cancellationTokenSourceRotation;
        private CancellationTokenSource _cancellationTokenSourceShooting;
        private CancellationTokenSource _cancellationTokenSourceRespawn;

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
            else
            {
                _towerManager.CheckForGameEnd();
            }
        }

        public async void StartRotating()
        {
            _cancellationTokenSourceRotation = new CancellationTokenSource();

            try
            {
                while (gameObject.activeInHierarchy)
                {
                    await Task.Delay(Random.Range(_MIN_ROTATE_DELAY, _MAX_ROTATE_DELAY), _cancellationTokenSourceRotation.Token);

                    transform.localEulerAngles = new Vector3(0, Random.Range(0, 360), 0);
                }
            }
            catch
            {

            }
            finally
            {
                _cancellationTokenSourceRotation.Dispose();
                _cancellationTokenSourceRotation = null;
            }
          
        }

        public async void StartShooting()
        {
            _cancellationTokenSourceShooting = new CancellationTokenSource();

            try
            {
                while (gameObject.activeInHierarchy)
                {
                    await Task.Delay(_SHOOTING_DELAY, _cancellationTokenSourceShooting.Token);
                    if(gameObject.activeInHierarchy)
                    {
                        Shoot();
                    }
                }
            }
            catch
            {

            }
            finally
            {
                _cancellationTokenSourceShooting.Dispose();
                _cancellationTokenSourceShooting = null;
            }
         
        }

        public void Shoot()
        {
            _bullet = _bulletManager.FindInactiveBullet();
            _bullet.ResetBullet();

            _bullet.transform.position = _bulletSpawnPoint.transform.position;
            _bullet.transform.localEulerAngles = transform.localEulerAngles;

            _bullet.gameObject.SetActive(true);
            _bullet.AddForceForward();
            _bullet.StartDisableCountdown();
        }

        public void UpdateLivesCounter()
        {
            _livesCounter.text = _currentLives.ToString();
        }

        public void ResetTower()
        {
            _currentLives = _maxLives;
        }

        public void CancelTasks()
        {
            if (_cancellationTokenSourceRotation != null)
            {
                _cancellationTokenSourceRotation.Cancel();
            }

            if (_cancellationTokenSourceShooting != null)
            {
                _cancellationTokenSourceShooting.Cancel();
            }

            if (_cancellationTokenSourceRespawn != null)
            {
                _cancellationTokenSourceRespawn.Cancel();
            }
        }

        private async void RespawnAfterDelay()
        {
            _cancellationTokenSourceRespawn = new CancellationTokenSource();

            try
            {
                await Task.Delay(_DEATH_DELAY, _cancellationTokenSourceRespawn.Token);
                _towerManager.SpawnTower(this, false);
            }
            catch
            {

            }
            finally
            {
                _cancellationTokenSourceRespawn.Dispose();
                _cancellationTokenSourceRespawn = null;
            }
        }
    }
}

