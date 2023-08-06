using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoProjectilFlor : MonoBehaviour
{
    public float fuerzaReboteY  = 3f;
    public float velocidadHorizontal = 2f; // Velocidad máxima en el eje X
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(velocidadHorizontal, rb.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verificar si el objeto colisiona con el suelo con la capa "Suelo" 
        Debug.Log("Colisionó con " + (collision.gameObject.layer == LayerMask.NameToLayer("Suelo")));
        if (collision.gameObject.layer == LayerMask.NameToLayer("Suelo"))
        {
            rb.AddForce(Vector2.up * fuerzaReboteY, ForceMode2D.Impulse);
        }
    }
}
