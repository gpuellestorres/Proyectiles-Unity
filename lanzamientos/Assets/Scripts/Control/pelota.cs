using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Advertisements;
using System;
using UnityEngine.SceneManagement;

public class pelota : MonoBehaviour {

    public float escalaTiempo = 1;

    public Transform ultimoColisionador;

    public float fuerza = 0;
    public float multiplicadorFuerza = 3;
    public float multiplicadorFuerzaRebote = 1.5f;
    public float fuerzaBomba = 3.5f;
    public float gravedad = 2;
    public float minimoFuerzaRebote = 1;

    public float segundosChoque = 2;
    float t0Choque = 0;

    public float movimientoX;
    public float movimientoY;

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

        Time.timeScale = escalaTiempo;

        int fasesJugadas = PlayerPrefs.GetInt("contadorFases");
        
        if (fasesJugadas!=0 && fasesJugadas % 3 == 0)
        {
            Advertisement.Initialize("29239", true);

            if (Advertisement.IsReady())
            {
                Advertisement.Show();
            }
        }

        posicionInicial = transform.position;
        posicionInicialFlecha = Flecha.transform.position;

        Flecha = Instantiate(Flecha);
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

        //Flecha.transform.position = posicionInicialFlecha;
        Flecha.controlarPosicion = true;


        foreach (bomba Bomba in FindObjectsOfType<bomba>())
        {
            Bomba.reiniciar();
        }

        foreach (horizontal Horizontal in FindObjectsOfType<horizontal>())
        {
            Horizontal.reiniciar();
        }
    }

    // Update is called once per frame
    void Update()
    {
        bool presionandoFire = false;

        if (
            (Input.touchCount > 0 &&
            //(Input.GetTouch(0).position.x > Screen.width / 2 && Input.GetTouch(0).position.y < Screen.height / 2))
            (Input.GetTouch(0).position.x > Screen.width / 2)
            ||
            (Input.touchCount > 1 &&
            //(Input.GetTouch(1).position.x > Screen.width / 2 && Input.GetTouch(1).position.y < Screen.height / 2))
            (Input.GetTouch(1).position.x > Screen.width / 2))))
        {
            presionandoFire = true;
        }

        if (((Application.platform!=RuntimePlatform.Android && Input.GetMouseButtonDown(0)) 
            || Input.GetButtonDown("ps4_X") 
            || presionandoFire)
            && !inicioMovimiento && !movimiento)
        {
            inicioMovimiento = true;
            t0 = Time.timeSinceLevelLoad;
            //Fire.finEjecucion();
        }

        float tiempoActual = Time.timeSinceLevelLoad;

        if (inicioMovimiento)
        {
            if (tiempoActual >= t0 + tiempoRefresco)
            {
                if (
                    (Input.GetMouseButton(0) && Application.platform!=RuntimePlatform.Android)
                    || Input.GetButton("ps4_X") 
                    || presionandoFire)
                {
                    fuerza += 13.5f * (tiempoActual - t0);

                    int indiceObjetos = (int)(fuerza*4);

                    if (indiceObjetos >= objetosBencina.Length)
                    {
                        fuerza = 9;
                        return;
                    }

                    objetosBencina[indiceObjetos].position = new Vector3(-7.6f, -4 + 0.14f * indiceObjetos, -1);
                    if(indiceObjetos>0)objetosBencina[indiceObjetos-1].position = new Vector3(-7.6f, -4 + 0.14f * (indiceObjetos-1), -1);
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

            if (transform.position.y < -6 || transform.position.y > 6 || transform.position.x > 10 || transform.position.x < -10)
                reiniciar();

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
        trabajarColision(other, "enter");
    }

    void OnTriggerStay2D(Collider2D other)
    {
        trabajarColision(other, "stay");
    }

    void OnTriggerExit2D(Collider2D other)
    {
        trabajarColision(other, "exit");
    }

    private void trabajarColision(Collider2D other, string tipo)
    {
        if (tipo.Equals("exit"))
        {
            if (ultimoColisionador == other.transform)
            {
                ultimoColisionador = null;
            }
        }

        t0Choque = Time.timeSinceLevelLoad;

        //print("Colisión con objeto");
        if (other.GetComponent<objetivo>() != null && tipo!="enter" && !other.GetComponent<objetivo>().destruido)
        {
            other.GetComponent<objetivo>().destruido = true;
        }
        else if (other.GetComponent<obstaculo>() != null && !choque && ultimoColisionador != other.transform && tipo.Equals("enter"))
        {
            ultimoColisionador = other.transform;
            choque = true;
            Vector3 rotacion = other.GetComponent<obstaculo>().transform.eulerAngles;
            modificarTrayectoria(rotacion, other.tag);
        }
        else if (other.GetComponent<bomba>() != null && other.GetComponent<bomba>().borrar!=true && !choque)
        {
            choque = true;
            Vector2 diferenciaPosiciones =
                new Vector2(transform.position.x - other.transform.position.x,
                    transform.position.y - other.transform.position.y);
            modificarTrayectoriaBomba(diferenciaPosiciones, other.transform.localScale.x);
            other.GetComponent<bomba>().borrar = true;

            if (other.GetComponent<girarAlrededor>() != null)
            {
                other.GetComponent<girarAlrededor>().activo = false;
            }
        }
        else if (other.GetComponent<portalA>() != null && !choque && tipo.Equals("enter"))
        {
            //Vector2 diferencia = new Vector2(other.transform.position.x - transform.position.x, other.transform.position.y - transform.position.y);
            Vector2 diferencia = new Vector2(0, 0);
            Transform portalB = other.GetComponent<portalA>().portalB.transform;
            transform.position = new Vector2(portalB.position.x + diferencia.x, portalB.position.y + diferencia.y);
        }
    }

    private void modificarTrayectoriaBomba(Vector2 diferenciaPosiciones, float escala)
    {

        float hipotenusa = Mathf.Pow(diferenciaPosiciones.x, 2) + Mathf.Pow(diferenciaPosiciones.y, 2);
        hipotenusa = Mathf.Sqrt(hipotenusa);

        float razonFuerzas = fuerzaBomba / hipotenusa;

        float fuerzaBombaX = diferenciaPosiciones.x * razonFuerzas * escala / 1.2f;
        float fuerzaBombaY = diferenciaPosiciones.y * razonFuerzas * hipotenusa * escala / 1.2f;

        movimientoX = fuerzaBombaX;
        movimientoY = fuerzaBombaY;
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
        else if (tag.Equals("abajo izquierda"))
        {
            normal += 45;
        }
        else if (tag.Equals("abajo derecha"))
        {
            normal += 135;
        }
        else if (tag.Equals("arriba izquierda"))
        {
            normal -= 45;
        }
        else if (tag.Equals("arriba derecha"))
        {
            normal -= 135;
        }

        float fuerzaActual = movimientoX * movimientoX + movimientoY * movimientoY;
        fuerzaActual = Mathf.Sqrt(fuerzaActual);

        float fuerzaRebote = fuerzaActual * multiplicadorFuerzaRebote;

        //print(fuerzaRebote);

        if (fuerzaRebote < minimoFuerzaRebote)
        {
            fuerzaRebote = minimoFuerzaRebote;
        }

        movimientoX = Mathf.Cos(Mathf.Deg2Rad * normal) * fuerzaRebote + movimientoX;
        movimientoY = Mathf.Sin(Mathf.Deg2Rad * normal) * fuerzaRebote + movimientoY;
    }
}
