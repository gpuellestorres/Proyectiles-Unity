using UnityEngine;
using System.Collections;
using System;

public class girarAlrededor : MonoBehaviour
{

    public float velocidadGiro = 1;
    public Vector2 centro = new Vector2(0, 0);

    public bool girarDerecha = true;
    float grados = 0;

    float t0 = 0;
    float distanciaAlCentro;

    Vector2 posicionInicial;

    public bool activo=true;
    // Use this for initialization
    void Start()
    {
        t0 = Time.timeSinceLevelLoad;
        posicionInicial = transform.position;
        grados = Mathf.Rad2Deg * Mathf.Atan2(transform.position.y - centro.y, transform.position.x - centro.x);
        distanciaAlCentro =
            Mathf.Sqrt(
                Mathf.Pow(transform.position.x - centro.x, 2) +
                Mathf.Pow(transform.position.y - centro.y, 2));
    }

    internal void reiniciar()
    {
        activo = true;
        transform.position = posicionInicial;
        grados = Mathf.Rad2Deg * Mathf.Atan2(posicionInicial.y - centro.y, posicionInicial.x - centro.x);
        print(grados);
    }

    // Update is called once per frame
    void Update()
    {
        if (!activo)
        {
            t0 = Time.timeSinceLevelLoad;
            return;         
        }

        float tiempoActual = Time.timeSinceLevelLoad;

        if (girarDerecha)
        {
            grados -= velocidadGiro * (tiempoActual - t0);
        }
        else
        {
            grados += velocidadGiro * (tiempoActual - t0);
        }
        transform.position =
            new Vector2(centro.x + Mathf.Cos(grados * Mathf.Deg2Rad) * distanciaAlCentro,
            Mathf.Sin(centro.y + grados * Mathf.Deg2Rad) * distanciaAlCentro);

        t0 = tiempoActual;
    }
}
