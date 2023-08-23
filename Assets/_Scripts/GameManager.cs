using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instancia;

    public int monedas;
    public int vidas;

    public bool esGameOver = false;

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
    private void Start()
    {
        monedas = 0;
        vidas = 3;
        esGameOver = false;

        HUD.Instancia.ActualizarVidas();
    }
    public void GameOver()
    {
        if (esGameOver)
        {
            Debug.Log("GameOver");
            HUD.Instancia.GameOver();
            esGameOver = false;
            StartCoroutine(ReiniciarEscena());
        }
    }
    public void PerderVida()
    {
        vidas--;
        HUD.Instancia.ActualizarVidas();
        
    }
    public void AñadirMoneda()
    {
        monedas++;
        HUD.Instancia.ActualizarMonedas();
    }

    IEnumerator ReiniciarEscena()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
