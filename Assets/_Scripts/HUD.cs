using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [Header("Configuración de vidas: ")]
    public TextMeshProUGUI textoVidas;
    public Image imagenVidas;
    public Sprite spriteSinVida;

    [Header("Configuración de textos: ")]
    public TextMeshProUGUI textoMonedas;
    public TextMeshProUGUI textoGameOver;
    public static HUD Instancia;
    private void Awake()
    {
        if (Instancia == null)
        {
            Instancia = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    void Start()
    {
        
    }

    public void ActualizarVidas()
    {   
        if (GameManager.Instancia.vidas >= 0)
        {
            textoVidas.text = "x" + GameManager.Instancia.vidas.ToString();
            if(GameManager.Instancia.vidas == 0)
            {
                imagenVidas.sprite = spriteSinVida;
            }
        }
    }

    public void ActualizarMonedas()
    {
        textoMonedas.text = "x" + GameManager.Instancia.monedas.ToString();
    }

    public void GameOver()
    {
        textoGameOver.gameObject.SetActive(true);
    }
}
