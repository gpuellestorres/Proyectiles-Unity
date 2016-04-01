using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class pelota : MonoBehaviour {

    public float fuerza = 0;
    public float multiplicadorFuerza = 3;
    public float gravedad = 2;

    public float segundosChoque = 2;
    float t0Choque = 0;

    float movimientoX;
    float movimientoY;

    public float tiempoRefresco = 0.01f;
    float t0;

    bool movimiento = false;
    bool inicioMovimiento = false;

    bool choque = false;

    public flecha Flecha;

    public Transform[] objetosBencina;

    Vector3 posicionInicial;
    Vector3 posicionInicialFlecha;

    // Use this for initialization
    void Start() {

        posicionInicial = transform.position;
        posicionInicialFlecha = Flecha.transform.position;

        Flecha = Instantiate(Flecha);

        int i = 0;
        List<Transform> objsBenc = new List<Transform>();

        foreach (Transform barra in objetosBencina)
        {
            objsBenc.Add(Instantiate(barra.gameObject).transform);
        }

        objetosBencina = objsBenc.ToArray();

        reiniciar();
	}

    void reiniciar()
    {
        fuerza = 0;
        movimiento = false;
        inicioMovimiento = false;

        foreach (Transform Objeto in objetosBencina)
        {
            Objeto.position = new Vector2(-20, -4);
        }

        transform.position = posicionInicial;

        Flecha.transform.position = posicionInicialFlecha;
        Flecha.controlarPosicion = true;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0) && !movimiento)
        {
            inicioMovimiento = true;
            t0 = Time.timeSinceLevelLoad;
        }

        float tiempoActual = Time.timeSinceLevelLoad;

        if (inicioMovimiento)
        {
            if (tiempoActual >= t0 + tiempoRefresco)
            {
                if (Input.GetMouseButton(0))
                {
                    fuerza += 0.3f;

                    int indiceObjetos = (int)fuerza;
                    if (indiceObjetos >= objetosBencina.Length)
                    {
                        fuerza = 15;
                        return;
                    }

                    objetosBencina[indiceObjetos].position = new Vector2(-8, -4 + 0.3f * indiceObjetos);
                }
                else
                {
                    movimiento = true;
                    inicioMovimiento = false;
                    movimientoX = Flecha.x / Flecha.distanciaRespectoAlCentro * fuerza * multiplicadorFuerza;
                    movimientoY = Flecha.y / Flecha.distanciaRespectoAlCentro * fuerza * multiplicadorFuerza;
                }

                t0 = Time.timeSinceLevelLoad;
            }
        }

        if (movimiento)
        {
            Flecha.controlarPosicion = false;
            Flecha.transform.position = new Vector2(-20, -20);

            if (tiempoActual >= t0 + tiempoRefresco)
            {
                float diferenciaTiempo = tiempoActual - t0;

                transform.position = new Vector2(transform.position.x + movimientoX * diferenciaTiempo,
                    transform.position.y + movimientoY * diferenciaTiempo);
                t0 = Time.timeSinceLevelLoad;
                movimientoY -= gravedad * diferenciaTiempo;
            }

            if (transform.position.y < -6 || transform.position.y > 6 || transform.position.x > 10) reiniciar();

            if (choque)
            {
                if (Time.timeSinceLevelLoad > t0Choque + segundosChoque)
                {
                    choque = false;
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (choque) return;

        choque = true;
        t0Choque = Time.timeSinceLevelLoad;

        print("Colisión con objeto");
        if (other.GetComponent<objetivo>()!=null)
        {
            other.GetComponent<objetivo>().destruir();
        }
        else if (other.GetComponent<obstaculo>() != null)
        {
            Vector3 rotacion = other.GetComponent<obstaculo>().transform.eulerAngles;
            modificarTrayectoria(rotacion, other.tag);
        }
    }

    private void modificarTrayectoria(Vector3 rotacion, string tag)
    {
        print(tag);

        float normal = rotacion.z - 180;

        if (tag.Equals("derecho"))
        {
            normal += 180;
        }
        else if (tag.Equals("arriba"))
        {
            normal -= 90;
        }
        else if (tag.Equals("abajo"))
        {
            normal += 90;
        }

        float fuerzaActual = movimientoX * movimientoX + movimientoY * movimientoY;
        fuerzaActual = Mathf.Sqrt(fuerzaActual);

        movimientoX = Mathf.Cos(Mathf.Deg2Rad * normal) * fuerzaActual * multiplicadorFuerza + movimientoX;
        movimientoY = Mathf.Sin(Mathf.Deg2Rad * normal) * fuerzaActual * multiplicadorFuerza + movimientoY;
    }
}
