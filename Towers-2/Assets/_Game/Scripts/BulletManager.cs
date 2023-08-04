using UnityEngine;

namespace Konithir.Tower2
{
    public class BulletManager : MonoBehaviour
    {
        [SerializeField]
        private BulletController[] _bulletsPool;

        public BulletController FindInactiveBullet()
        {
            for(int i = 0; i < _bulletsPool.Length; i++)
            {
                if (!_bulletsPool[i].gameObject.activeInHierarchy)
                {
                    return _bulletsPool[i];
                }
            }

            return null;
        }

        public void DisableAllBullets()
        {
            for (int i = 0; i < _bulletsPool.Length; i++)
            {
                _bulletsPool[i].ResetBullet();
                _bulletsPool[i].gameObject.SetActive(false);
            }
        }
    }
}
