using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private float velocidad = 5f;
    [SerializeField] public float fuerzaSalto = 16f;
    [SerializeField] private float tiempoFlorActiva = 10f;

    [SerializeField] public Transform chequeadorSuelo;
    [SerializeField] public float radioChequeador = 0.25f;
    [SerializeField] public LayerMask capaSuelo;

    [SerializeField]
    private GameObject florProyectilPrefabricado;


    private bool estaSaltando = false;
    private bool estaTocandoTierra = false;
    private float tiempoSalto = 0f;
    private float maxTiempoSalto = 1f;
    private float gravedadInicial;

    private bool florActiva = false;

    public int puntaje = 0;
    public int estado = 0; // Si es pequeño es 0. Si es grande es 1

    private float entradaHorizontal;
    private Rigidbody2D rb2D;
    private Animator animaciones;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animaciones = GetComponent<Animator>();
    }
    private void Start()
    {
        gravedadInicial = rb2D.gravityScale;
    }

    private void Update()
    {
        estaTocandoTierra = TocandoTierra();
        animaciones.SetBool("EstaTocandoTierra", estaTocandoTierra);
        entradaHorizontal = Input.GetAxis("Horizontal");

        AplicandoGravedad();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Saltar();
        }
        if (florActiva)
        {
            DispararFlor();
        }
    }

    private void FixedUpdate()
    {
        Mover();
    }

    private void Mover()
    {
        rb2D.velocity = new Vector2(entradaHorizontal * velocidad, rb2D.velocity.y);
        Rotar();
    }
    private void Rotar()
    {
        if (entradaHorizontal != 0.0f)
        {
            float direccionX = Mathf.Sign(entradaHorizontal);
            transform.localScale = new Vector2(direccionX, transform.localScale.y);
        }
    }

    //Este método se encarga de simular la gravedad en el personaje y su caída "ligera" similar a la de Mario.
    private void AplicandoGravedad()
    {
        if (!estaSaltando) return; //Si no estamos en medio de un salto, no es necesario aplicar gravedad
        

        if (Input.GetKey(KeyCode.Space)) // Si se mantiene presionada la tecla de salto, acumulamos tiempo de salto
        {
            tiempoSalto += Time.deltaTime;
        }
        if (Input.GetKeyUp(KeyCode.Space) && (tiempoSalto < maxTiempoSalto)) //Si se suelta la tecla de salto antes de alcanzar el máximo tiempo de salto...
        {
            // Aumentamos temporalmente la gravedad para que el personaje caiga más rápido
            rb2D.gravityScale = gravedadInicial * 3f;
        }

        if (rb2D.velocity.y < 0) // Si el personaje está cayendo después de un salto...
        {
            rb2D.gravityScale = gravedadInicial; // Restauramos la gravedad

            if (estaTocandoTierra) // Si ha tocado el suelo. Reiniciamos todos sus valores por defecto
            {
                estaSaltando = false;
                tiempoSalto = 0f;
                animaciones.SetBool("Saltando", estaSaltando);
            }
        }
    }
    private void Saltar()
    {
        if (estaSaltando) return; //Si estamos en medio de un salto, no necesitamos hacer nada.

        estaSaltando = true;
        rb2D.velocity = new Vector2(rb2D.velocity.x, 0f);
        rb2D.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
        //ToDo: Aqui sonido.
        animaciones.SetBool("Saltando", estaSaltando);
    }
    private void SoloSaltar(int fuerza)
    {
        rb2D.velocity = new Vector2(rb2D.velocity.x, 0f);
        rb2D.AddForce(Vector2.up * fuerza, ForceMode2D.Impulse);
        //ToDo: Aqui sonido.
        animaciones.SetBool("Saltando", estaSaltando);
    }
    private bool TocandoTierra()
    {
        return Physics2D.OverlapCircle(chequeadorSuelo.position, radioChequeador, capaSuelo);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red; // Establece el color del círculo (puedes cambiarlo si lo deseas)
        Gizmos.DrawWireSphere(chequeadorSuelo.position, radioChequeador); // Dibuja el círculo de colisión
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Moneda")) //Busca cual es el tag del objeto con el que colisiona y si es igual a "Moneda" entonces destruye el objeto y suma 1 al puntaje.
        {
            Destroy(collision.gameObject); //Destruye el objeto con el que colisiona.
            puntaje++; //Suma 1 al puntaje.
            GameManager.Instancia.AñadirMoneda();
        }
        if (collision.gameObject.CompareTag("Flor"))
        {
            Destroy(collision.gameObject); //Destruye el objeto con el que colisiona.
            ActivarFlor();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Hongo"))
        {
            Destroy(collision.gameObject);
            ActivarHongo();
        }
        
        if (collision.gameObject.CompareTag("Enemigo")) //si colisiona con un enemigo
        {
            if (estado == 1) //si es grande, se vuelve chiquito
            {
               estado = 0;
               //ToDo: animación de chiquito
            }
            else  //si es chiquito, se termina el juego
            {
                GameManager.Instancia.PerderVida();
                SoloSaltar(10);
                Destroy(collision.gameObject);
                if(GameManager.Instancia.vidas <= 0)
                {
                    MuerteJugador();
                }
            }
        }
    }

    private void ActivarFlor()
    {
        florActiva = true;
        StartCoroutine(DesactivarFlor());
    }

    IEnumerator DesactivarFlor()
    {
        yield return new WaitForSeconds(tiempoFlorActiva);
        florActiva = false;
    }


    private void DispararFlor()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            GameObject florProyectil = Instantiate(florProyectilPrefabricado, transform.position, Quaternion.identity);
            MovimientoProjectilFlor proyectil = florProyectil.transform.GetChild(0).GetComponent<MovimientoProjectilFlor>();
            if(proyectil != null)
            {
                proyectil.direccion = transform.localScale.x;
            }
        }
    }
    public void ActivarHongo()
    {
        //ToDo: Animacion de grande;
        estado = 1;
        puntaje++;
        GameManager.Instancia.AñadirMoneda();
        Debug.Log("Activado Hongo");
    }

    void MuerteJugador()
    {
        SoloSaltar(16);
        gameObject.layer = LayerMask.NameToLayer("JugadorGameOver");
        if (GameManager.Instancia.vidas <= 0)
        {
            GameManager.Instancia.esGameOver = true;
            GameManager.Instancia.GameOver();
        }
        
        //ToDo: animacion de muerte
        //cartel de derrota?
    }

}
