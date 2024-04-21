using UnityEngine;
using System.Collections;

public class BreakWindow : MonoBehaviour
{
    [SerializeField] Transform explosionPoint;
    [SerializeField] private float explodeForce = 200f;
    [SerializeField] private float explodeRange = 10f;
    private Rigidbody[] rigidBodies;

    void Start()
    {
        rigidBodies = GetComponentsInChildren<Rigidbody>();
        Debug.Log(rigidBodies.Length);
    }

    public void BreakStart()
    {
        foreach (Rigidbody rb in rigidBodies)
        {
            rb.isKinematic = false;
            rb.useGravity = true;

            Vector3 vec = rb.transform.position - explosionPoint.position;
            vec.Normalize();
            rb.AddForce(vec);

            Debug.Log(vec);

            // rb.AddExplosionForce(explodeForce, explosionPoint.position, explodeRange, 0, ForceMode.Impulse);
        }
    }
}
