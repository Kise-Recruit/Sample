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
    }

    public void BreakStart()
    {
        foreach (Rigidbody rb in rigidBodies)
        {
            rb.isKinematic = false;
            rb.useGravity = false;

            float rndX = Random.Range(-2.0f, 2.0f);
            float rndY = Random.Range(0.0f, 1.5f);
            float rndZ = Random.Range(0.0f, 1.0f);

            Vector3 vec = new Vector3(rndX, rndY, rndZ);
            vec *= 100.0f;
            rb.AddForce(vec);
        }
    }
}
