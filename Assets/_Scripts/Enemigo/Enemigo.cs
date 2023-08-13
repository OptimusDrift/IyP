using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigo : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Projectil"))
        {
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }
    }
}
