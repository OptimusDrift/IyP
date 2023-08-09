using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class BloqueDestructible : MonoBehaviour
{
    [SerializeField] public bool esDestructible = false;
    [SerializeField] public GameObject prefabRompible;
    [SerializeField] public GameObject prefabPowerUp;

    private bool moviendose = false;
    private bool estaVacio = false;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Cabeza"))
        {
            PlayerController playerController = collision.transform.parent.GetComponent<PlayerController>();  // Accedemos al componente/script creado por nosotros pero en el GameObject "PADRE", ya que, estamos haciendo un solapamiento con el "Trigger" del componente de la Cabeza.
            if (esDestructible && playerController.estado == 1) //Si el bloque es rompible y el personaje es grande...
            {
                Romper();
            }else if (!estaVacio && prefabPowerUp != null)
            {
                if (!moviendose) // Es el operador de negación "!". Es lo mismo que poner (moviendose == false)
                {
                    MostrarPowerUp();
                    StartCoroutine(Animacion());
                    estaVacio = true;
                }
            }
            else if(!moviendose)
            {
                StartCoroutine(Animacion());
            }
        }
    }
    IEnumerator Animacion()
    {
        moviendose = true;
        float tiempo = 0f;
        float duracion = 0.08f;

        Vector3 posicionInicial = transform.position;
        Vector3 posicionDestino = transform.position + Vector3.up * 0.5f;

        while (tiempo < duracion) // Mientras el tiempo sea menor a la duración...
        {
            // Movemos suavemente la posición del objeto desde posicionInicial hasta posicionDestino a medida que el tiempo avanza durante la duración de la animación.
            // Aplicamos interpolación lineal entre puntos en el espacio con tiempo proporcional a la duración total de la animación.
            transform.position = Vector2.Lerp(posicionInicial, posicionDestino, tiempo / duracion);
            tiempo += Time.deltaTime;

            yield return null; // Sale del bucle, pausa la ejecución y continúa en el siguiente frame.
        }
        transform.position = posicionDestino;
        tiempo = 0f;

        while (tiempo < duracion)
        {
            // Aplicamos un bucle similar al anterior, pero en sentido inverso.
            transform.position = Vector2.Lerp(posicionDestino, posicionInicial, tiempo / duracion);
            tiempo += Time.deltaTime;
            yield return null; 
        }
        transform.position = posicionInicial;
        moviendose = false;
    }
    private void Romper()
    {
        GameObject objeto;
        // Creamos objetos rompibles y les asignamos velocidades en diferentes direcciones.

        //Moviéndose hacia arriba a la derecha.
        objeto = Instantiate(prefabRompible, transform.position, Quaternion.identity);
        objeto.GetComponent<Rigidbody2D>().velocity = new Vector2(3f, 8f); 
        Destroy(objeto, 4f);

        //Moviéndose hacia arriba a la izquierda.
        objeto = Instantiate(prefabRompible, transform.position, Quaternion.identity);
        objeto.GetComponent<Rigidbody2D>().velocity = new Vector2(-3f, 8f); //Se mueve hacia arriba a la izquierda
        Destroy(objeto, 4f);

        //Moviéndose hacia abajo a la derecha.
        objeto = Instantiate(prefabRompible, transform.position, Quaternion.identity);
        objeto.GetComponent<Rigidbody2D>().velocity = new Vector2(3f, -10f);
        Destroy(objeto, 4f);

        //Moviéndose hacia abajo a la izquierda.
        objeto = Instantiate(prefabRompible, transform.position, Quaternion.identity);
        objeto.GetComponent<Rigidbody2D>().velocity = new Vector2(-3f, -10f); //Se mueve hacia abajo a la izquierda
        Destroy(objeto, 4f);

        // Destruimos el objeto actual después de un corto tiempo
        Destroy(this.gameObject, 0.1f);
    }
    
    private void MostrarPowerUp()
    {
        GameObject powerUp = Instantiate(prefabPowerUp, transform.position + Vector3.up * 1f, Quaternion.identity);
    }
}
