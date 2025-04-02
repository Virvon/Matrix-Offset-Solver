using UnityEngine;

namespace Sources
{
    public class OffsetCube : MonoBehaviour
    {
        private Vector3 _modelPosition;
        private Vector3 _spacePosition;

        private void OnDrawGizmos()
        {
            if (_modelPosition == Vector3.zero || _spacePosition == Vector3.zero)
                return;
            
            Gizmos.DrawLine(transform.position, _modelPosition);
            Gizmos.DrawLine(transform.position, _spacePosition);
        }

        public void Initialize(Vector3 modelPosition, Vector3 spacePosition)
        {
            _modelPosition = modelPosition;
            _spacePosition = spacePosition;
        }
    }
}