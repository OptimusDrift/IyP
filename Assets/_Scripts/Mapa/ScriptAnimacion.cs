using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScriptAnimacion : MonoBehaviour
{
    public Sprite[] sprites;
    public float frameTime = 0.1f;
    private int animacionFrame;
    public bool stop = false;
    SpriteRenderer spriteRenderer;
    Image image;
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        image = GetComponent<Image>();
    }

    private void Start()
    {
        StartCoroutine(Animacion());
    }

    IEnumerator Animacion()
    {
        while (!stop)
        {
            if(spriteRenderer != null)
            {
                spriteRenderer.sprite = sprites[animacionFrame];
            }else if(image != null)
            {
                image.sprite = sprites[animacionFrame];
            }
            
            animacionFrame++;
            if (animacionFrame == sprites.Length)
            {
                animacionFrame = 0;
            }
            yield return new WaitForSeconds(frameTime);
        }
    }
}
