using UnityEngine;

public class AttackHitBox : MonoBehaviour
{
    enum ColliderType
    {
        Box,
        Capsule
    }
    [SerializeField] ColliderType colliderType;

    private BoxCollider boxCollider;
    private CapsuleCollider capsuleCollider;
    private int attackPow;
    public int AttackPow => attackPow;

    public void Init(int attackPow)
    {
        this.attackPow = attackPow;

        if (colliderType == ColliderType.Box)
        {
            boxCollider = GetComponent<BoxCollider>();
        }

        if (colliderType == ColliderType.Capsule)
        {
            capsuleCollider = GetComponent<CapsuleCollider>();
        }
    }

    public void OnAttackStart()
    {
        if (colliderType == ColliderType.Box)
        {
            boxCollider.enabled = true;
        }

        if (colliderType == ColliderType.Capsule)
        {
            capsuleCollider.enabled = true;
        }
    }

    public void OnAttackEnd()
    {
        if (colliderType == ColliderType.Box)
        {
            boxCollider.enabled = false;
        }

        if (colliderType == ColliderType.Capsule)
        {
            capsuleCollider.enabled = false;
        }
    }
}
