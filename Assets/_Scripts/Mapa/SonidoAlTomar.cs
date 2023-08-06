using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonidoAlTomar : MonoBehaviour
{
    public AudioClip clip; // Asigna el audio que deseas reproducir desde el ControladorAudio

    // Método para reproducir el sonido desde el ControladorAudio
    private void ReproducirSonido()
    {
        if (clip == null)
            return;

        // Buscar el objeto "ControladorAudio" en la escena
        GameObject controladorAudio = GameObject.Find("ControladorAudio");

        // Obtener o agregar un componente AudioSource al objeto "ControladorAudio"
        AudioSource audioSource = controladorAudio.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = controladorAudio.AddComponent<AudioSource>();
        }

        // Reproducir el audio desde el ControladorAudio
        audioSource.PlayOneShot(clip);
    }

    // Método que se ejecuta justo antes de que el objeto sea destruido
    private void OnDestroy()
    {
        // Llamar al método para reproducir el sonido antes de que se destruya el objeto
        ReproducirSonido();
    }
}
