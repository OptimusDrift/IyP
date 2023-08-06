using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movimiento : MonoBehaviour
{
    [SerializeField] public float velocidadHorizontal = 1f;

    private float temporizador = 0f;
    private Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        rb.velocity = new Vector2(velocidadHorizontal, rb.velocity.y);
    }

    private void FixedUpdate()
    {
        //Es lo mismo que decir: (rb.velocity.x > -0.1f && rb.velocity.x < 0.1f)
        if (Math.Abs(rb.velocity.x) < 0.1f) // Comprobamos si la velocidad en x es un valor cercano a 0...
        {
            if (temporizador > 0.3f) //Utilizamos un temporizador para que no cambie su dirección de manera instantanea al colisionar. Si no lo hacemos, cuando el objeto caiga desde arriba, cambiara su dirección de manera no esperada. 
            {
                velocidadHorizontal *= -1;
            }
            temporizador += Time.deltaTime;
        }
        else
        {
            temporizador = 0;
        }

        rb.velocity = new Vector2(velocidadHorizontal, rb.velocity.y);
    }
}
