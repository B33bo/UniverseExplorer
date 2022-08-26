using UnityEngine;
using System.Collections.Generic;

namespace Universe
{
    public class PhysicsObject : MonoBehaviour
    {
        private static List<PhysicsObject> others;

        Vector3 velocity;
        Vector3 force;

        [SerializeField]
        private float Mass;

        [SerializeField]
        private float Radius;

        private void Awake()
        {
            if (others is null)
                others = new List<PhysicsObject>();
            others.Add(this);
        }

        private void Update()
        {
            foreach (var physicsObj in others)
            {
                AttractTo(physicsObj);
            }
        }

        private void AttractTo(PhysicsObject other)
        {
            Vector3 direction = transform.position - other.transform.position;
            float distance = direction.magnitude;

            if (distance > other.Radius)
            {
                float forceMagnitude = (other.Mass * Mass) / (distance * distance);
                force = direction.normalized * forceMagnitude;
                force = -force;
            }


            //force += Mass * Vector3.down;
            velocity += force / Mass * Time.deltaTime;

            transform.position += velocity * Time.deltaTime;
        }

        private void OnDestroy()
        {
            others.Remove(this);
        }
    }
}
