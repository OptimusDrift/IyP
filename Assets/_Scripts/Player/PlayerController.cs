using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private float velocidad = 5f;
    [SerializeField] public float fuerzaSalto = 16f;

    [SerializeField] public Transform chequeadorSuelo;
    [SerializeField] public float radioChequeador = 0.25f;
    [SerializeField] public LayerMask capaSuelo;

    private bool estaSaltando = false;
    private bool estaTocandoTierra = false;
    private float tiempoSalto = 0f;
    private float maxTiempoSalto = 1f;
    private float gravedadInicial;

    private float entradaHorizontal;
    private Rigidbody2D rb2D;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        gravedadInicial = rb2D.gravityScale;
    }

    private void Update()
    {
        estaTocandoTierra = TocandoTierra();
        entradaHorizontal = Input.GetAxis("Horizontal");

        AplicandoGravedad();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Saltar();
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
        //ToDo: Aqui animacion de caminar
    }
    private void Rotar()
    {
        if (entradaHorizontal != 0.0f)
        {
            float direccionX = Mathf.Sign(entradaHorizontal);
            transform.localScale = new Vector2(direccionX, transform.localScale.y);
        }
    }

    private void AplicandoGravedad()
    {
        //Para aplicar la gravedad setteamos al rg2D, una gravedad inicial de 3f para simular mejor la caiga "ligera" de Mario. Luego si se suelta la tecla antes de tiempo cae aun más rapído por el vj de Mario.

        if (!estaSaltando) return;

        if (Input.GetKey(KeyCode.Space))
        {
            tiempoSalto += Time.deltaTime;
        }
        if (Input.GetKeyUp(KeyCode.Space) && (tiempoSalto < maxTiempoSalto))
        {
            //Aaumentamos la escala de la gravedad para que el personaje caiga más rapido al suelo.
            rb2D.gravityScale = gravedadInicial * 3f;
        }

        if (rb2D.velocity.y < 0)
        {
            rb2D.gravityScale = gravedadInicial;
            if (estaTocandoTierra)
            {
                estaSaltando = false;
                tiempoSalto = 0f;
                //ToDo: Desactivar aqui animacion de salto.
            }
        }
    }
    private void Saltar()
    {
        if (estaSaltando) return;

        estaSaltando = true;
        rb2D.velocity = new Vector2(rb2D.velocity.x, 0f);
        rb2D.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
        //ToDo: Aqui animacion de salto y sonido.
    }
    private bool TocandoTierra()
    {
        return Physics2D.OverlapCircle(chequeadorSuelo.position, radioChequeador, capaSuelo);
    }
}