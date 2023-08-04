using UnityEngine;

namespace Konithir.Tower2
{
    public class RotateToCamera : MonoBehaviour
    {
        [SerializeField]
        private Camera _camera;

        private void LateUpdate()
        {
            transform.rotation = _camera.transform.rotation;
        }
    }
} 
