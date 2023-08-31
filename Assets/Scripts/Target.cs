using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour, IDamageable {

    public float health;

   public void TakeDamage(float damage)
    {
        Debug.Log("Taking damage: " + damage);
        health -= damage;
        if (health <= 0) {
            Debug.Log("Object destroyed!");
            Destroy(gameObject);
        } else {
            Debug.Log("Object health: " + health);
        }
    }

}
