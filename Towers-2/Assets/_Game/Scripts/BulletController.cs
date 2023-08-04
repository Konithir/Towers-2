using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Konithir.Tower2
{
    public class BulletController : MonoBehaviour
    {
        [SerializeField]
        private float _bulletSpeed;

        [SerializeField]
        private Rigidbody _rigidbody;

        private IDamageable _interface;
        private const int _DISABLE_DELAY = 2000;
        private Task _disableTask;
        private CancellationTokenSource _cancellationTokenSource;

        private void OnTriggerEnter(Collider other)
        {
            _interface = other.GetComponent<IDamageable>();

            if(_interface != null)
            {
                _interface.GetDamage();
                gameObject.SetActive(false);
            }
        }

        public void AddForceForward()
        {
            _rigidbody.AddForce(transform.forward * _bulletSpeed);
        }

        public void ResetBullet()
        {
            _interface = null;
            _rigidbody.angularVelocity = Vector3.zero;
            _rigidbody.angularDrag = 0;
            _rigidbody.velocity = Vector3.zero;

            if(_disableTask != null)
            {
                _cancellationTokenSource?.Cancel();
                _disableTask.Dispose();
                _disableTask = null;
            }
        }

        public void StartDisableCountdown()
        {
            _disableTask = DisableBullet();
        }

        private async Task DisableBullet()
        {
            _cancellationTokenSource = new CancellationTokenSource();

            await Task.Delay(_DISABLE_DELAY, _cancellationTokenSource.Token);
            gameObject.SetActive(false);
        }
    }
}
