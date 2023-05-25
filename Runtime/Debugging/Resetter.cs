using System.Collections;
// using Sirenix.OdinInspector;
using UnityEngine;

namespace SensenToolkit.Debugging
{
    public class Resetter : MonoBehaviour
    {
        private Rigidbody _body;
        private Rigidbody2D _body2d;
        private Vector3 _bodyPosition;
        private Quaternion _bodyRotation;
        private Vector2 _body2dPosition;
        private float _body2dRotation;
        private Vector3 _transformPosition;
        private Quaternion _transformRotation;

#if UNITY_EDITOR
        private void Awake()
        {
            _body = GetComponent<Rigidbody>();
            _body2d = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            SaveState();
            StartCoroutine(ResetPhysicsLoop());
        }

        private IEnumerator ResetPhysicsLoop()
        {
            while (true)
            {
                yield return new WaitForSeconds(1f);
                ResetPhysics();
                ResetRotation();
            }
        }

        // [Button]
        public void ResetObj()
        {
            ResetPhysics();
            LoadState();
        }

        // [Button]
        private void ResetAllObjs()
        {
            foreach (var resetter in GameObject.FindObjectsOfType<Resetter>())
            {
                resetter.ResetObj();
            }
        }

        // [Button]
        private void ResetPhysics()
        {
            if (_body != null)
            {
                _body.velocity = Vector3.zero;
                _body.angularVelocity = Vector3.zero;
            }
            if (_body2d != null)
            {
                _body2d.velocity = Vector2.zero;
                _body2d.angularVelocity = 0f;
            }
        }

        private void SaveState()
        {
            if (_body != null)
            {
                _bodyPosition = _body.position;
                _bodyRotation = _body.rotation;
            }
            if (_body2d != null)
            {
                _body2dPosition = _body2d.position;
                _body2dRotation = _body2d.rotation;
            }
                _transformPosition = transform.position;
                _transformRotation = transform.rotation;
        }

        private void LoadState()
        {
            if (_body != null)
            {
                _body.position = _bodyPosition;
                _body.rotation = _bodyRotation;
            }
            if (_body2d != null)
            {
                _body2d.position = _body2dPosition;
                _body2d.rotation = _body2dRotation;
            }
                transform.SetPositionAndRotation(_transformPosition, _transformRotation);
        }

        // [Button]
        private void ResetRotation()
        {
            if (_body != null)
            {
                _body.rotation = _bodyRotation;
            }
            if (_body2d != null)
            {
                _body2d.rotation = _body2dRotation;
            }
                transform.rotation = _transformRotation;
        }
#endif
    }
}
