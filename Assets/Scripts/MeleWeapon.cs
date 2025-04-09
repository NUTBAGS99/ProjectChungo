using UnityEngine;

public abstract class MeleeWeapon : MonoBehaviour
{
    [Header("Weapon Settings")]
    public int damage = 25;
    public float attackCooldown = 0.5f;
    public float attackRange = 1f;
    public LayerMask hitLayers;

    protected float lastAttackTime = 0f;

    // Called from the inheriting class when input is triggered
    public void TryAttack()
    {
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            PerformAttack();
            lastAttackTime = Time.time;
        }
    }

    // Core attack behavior, to be overridden
    protected abstract void PerformAttack();

    // Optional: visualize attack range
    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + transform.forward * 0.5f, attackRange);
    }
}
