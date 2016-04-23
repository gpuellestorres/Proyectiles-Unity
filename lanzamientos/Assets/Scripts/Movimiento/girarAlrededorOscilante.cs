﻿using UnityEngine;
using System.Collections;

public class girarAlrededorOscilante : MonoBehaviour {

    public float velocidadGiro = 1;
    public Vector2 centro = new Vector2(0, 0);
    public float gradosInicio = 0;
    public float gradosFin = 90;

    public bool girarDerecha = true;
    public float grados = 0;

    float t0 = 0;
    float distanciaAlCentro;

    Vector2 posicionInicial;

    public bool activo = true;

    bool iniciado = false;
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

        print(distanciaAlCentro);
        iniciado = true;
    }

    internal void reiniciar()
    {
        if (iniciado)
        {
            activo = true;
            transform.position = posicionInicial;
            print(posicionInicial);
            grados = Mathf.Rad2Deg * Mathf.Atan2(posicionInicial.y - centro.y, posicionInicial.x - centro.x);
        }

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

        grados = grados % 360;

        transform.position =
            new Vector2(centro.x + Mathf.Cos(grados * Mathf.Deg2Rad) * distanciaAlCentro,
            centro.y + Mathf.Sin(grados * Mathf.Deg2Rad) * distanciaAlCentro);

        if ((grados<=gradosInicio && girarDerecha) || (grados>=gradosFin && !girarDerecha))
        {
            girarDerecha = !girarDerecha;
        }

            t0 = tiempoActual;
    }
}