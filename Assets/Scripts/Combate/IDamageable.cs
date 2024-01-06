using UnityEngine;

public interface IDamageable
{
    void TakeDamageAndKnockback(Vector3 attackDirection, GameObject attacker);
}
