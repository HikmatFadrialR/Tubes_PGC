using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int baseHealth, Damage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnCollisionEnter(Collision col) {
        if (col.gameObject.tag == "Peluru") {
            baseHealth = baseHealth - Damage;
            if (baseHealth <= 0 ) Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // void OnMouseDown() {
    //     baseHealth = baseHealth - Damage;
    //     print(baseHealth);
    //     if (baseHealth <= 0) Destroy(gameObject);
    // }
}
