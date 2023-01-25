using UnityEngine;

namespace SpyroClone.AI
{
    [RequireComponent(typeof(BoxCollider))]
    public class PatrolArea : MonoBehaviour
    {
        BoxCollider areaCollider;

        float areaWidth;
        float areaDepth;
        Vector3 generatedPoint;

        private void Awake()
        {
            areaCollider = GetComponent<BoxCollider>();
        }

        private void Start()
        {
            areaWidth = areaCollider.bounds.size.x;
            areaDepth = areaCollider.bounds.size.z;
            generatedPoint = transform.position;
        }

        public Vector3 GetGeneratedPoint()
        {
            return generatedPoint;
        }

        public void GenerateRandomPoint()
        {
            float x = Random.Range(-areaWidth / 2, areaWidth / 2);
            float z = Random.Range(-areaDepth / 2, areaDepth / 2);
            Vector3 pointInsideVolume = new Vector3(x, 0, z);
            generatedPoint = pointInsideVolume + transform.position;
        }
    }
}