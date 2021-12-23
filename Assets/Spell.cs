using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    public float speed = 2;
    public float power = 10;  

    public Player target;

    private void Update()
    {
        transform.position += (transform.forward * speed * Time.deltaTime);
    }
    
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            target.DealDamage(power);

            Destroy(gameObject);

            return;
        }
    }
    
    private void OnTriggerStay(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            target.DealDamage(power);
            
            Destroy(this);

            return;
        }
    }
}
