using UnityEngine;

public class Sword : MeleeWeapon
{
    protected override void PerformAttack()
    {
        Vector3 origin = transform.position + transform.forward * 0.5f;
        Collider[] hits = Physics.OverlapSphere(origin, attackRange, hitLayers);

        foreach (var hit in hits)
        {
            EnemyController enemy = hit.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Debug.Log("Sword hit " + hit.name);
            }
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left-click
        {
            TryAttack();
        }
    }
}
