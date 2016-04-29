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
    public float fuerzaGravedad = 0.5f;
    public float gravedad = 2;
    public float minimoFuerzaRebote = 1;
    public float velocidadOrbita = 45;
    public float fuerzaOrbita = 20;

    public float segundosChoque = 2;
    float t0Choque = 0;

    public float movimientoX;
    public float movimientoY;

    public float tiempoRefresco = 0.01f;
    float t0;

    public bool movimiento = false;
    bool inicioMovimiento = false;
    bool enOrbita = false;

    Transform objetoOrbita;
    float gradosOrbita = 0;
    public bool girarDerecha = true;
    Vector2 centroOrbita;
    float distanciaAlCentroOrbita;

    bool choque = false;

    public flecha Flecha;

    public Transform[] objetosBencina;

    Vector3 posicionInicial;
    Vector3 posicionInicialFlecha;

    public AudioSource sonidoRebote;
    public AudioSource sonidoBomba;
    public AudioSource sonidoPortal;
    public AudioSource sonidoObjetivo;
    public AudioSource sonidoReiniciar;

    public Transform botonDisparar;
    SpriteRenderer spriteBotonBomba;
    Color original, alternativo;
    
    Camera camara;

    Gravedad[] Gravedad;


    public float limiteInferior = -6;
    public float limiteSuperior = 7;
    public float limiteIzquierdo = -10;
    public float limiteDerecho = 10;

    // Use this for initialization
    void Start() {

        Time.timeScale = escalaTiempo;

        camara = FindObjectOfType<Camera>();

        int fasesJugadas = PlayerPrefs.GetInt("contadorFases");
        
        if (fasesJugadas!=0 && fasesJugadas % 4 == 0)
        {
            if (Advertisement.IsReady())
            {
                Advertisement.Show();
            }
            PlayerPrefs.SetInt("contadorFases", 0);
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

        sonidoBomba = Instantiate(sonidoBomba);
        sonidoRebote = Instantiate(sonidoRebote);
        sonidoPortal = Instantiate(sonidoPortal);
        sonidoObjetivo = Instantiate(sonidoObjetivo);
        sonidoReiniciar = Instantiate(sonidoReiniciar);

        if (Application.platform == RuntimePlatform.Android)
        {
            botonDisparar = Instantiate(botonDisparar);
            botonDisparar.name = "Fire2(Clone)";

            spriteBotonBomba = botonDisparar.GetComponent<SpriteRenderer>();

            original = spriteBotonBomba.color;
            alternativo = new Color(original.a, original.g, original.b, 190f / 255);
        }

        Gravedad = FindObjectsOfType<Gravedad>();

        reiniciar();        
	}

    void reiniciar()
    {
        fuerza = 0;
        movimiento = false;
        inicioMovimiento = false;
        enOrbita = false;
        objetoOrbita = null;

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

        FindObjectOfType<estrella>().volverPosicionInicial();
    }

    // Update is called once per frame
    void Update()
    {
        bool presionandoFire = false;

        if (Input.touchCount > 0)
        {
            foreach (RaycastHit2D hit in Physics2D.RaycastAll(camara.ScreenToWorldPoint(Input.touches[0].position), Vector3.forward))
            {
                print(hit.collider.name);
                if (hit.collider.name.Equals("Fire2(Clone)"))
                {
                    spriteBotonBomba.color = alternativo;
                    presionandoFire = true;
                }
            }
        }
        else if (RuntimePlatform.Android==Application.platform)
        {
            spriteBotonBomba.color = original;
        }
        if (Input.touchCount > 1)
        {
            foreach (RaycastHit2D hit in Physics2D.RaycastAll(camara.ScreenToWorldPoint(Input.touches[1].position), -Vector2.up))
                if (hit.collider.name.Equals("Fire2(Clone)"))
                {
                    spriteBotonBomba.color = alternativo;
                    presionandoFire = true;
                }
        }

        /*if (
            (Input.touchCount > 0 &&
            //(Input.GetTouch(0).position.x > Screen.width / 2 && Input.GetTouch(0).position.y < Screen.height / 2))
            (Input.GetTouch(0).position.x > Screen.width / 2)
            ||
            (Input.touchCount > 1 &&
            //(Input.GetTouch(1).position.x > Screen.width / 2 && Input.GetTouch(1).position.y < Screen.height / 2))
            (Input.GetTouch(1).position.x > Screen.width / 2))))
        {
            presionandoFire = true;
        }//*/

        if (((Application.platform!=RuntimePlatform.Android && Input.GetMouseButtonDown(0)) 
            || Input.GetButtonDown("ps4_X") 
            || presionandoFire)
            && !inicioMovimiento && !movimiento && !enOrbita)
        {
            inicioMovimiento = true;
            t0 = Time.timeSinceLevelLoad;
            //Fire.finEjecucion();
        }

        float tiempoActual = Time.timeSinceLevelLoad;

        if (inicioMovimiento && !enOrbita)
        {
            if (tiempoActual >= t0 + tiempoRefresco)
            {
                float tiempoTranscurrido = tiempoActual - t0;

                if (tiempoTranscurrido >= 0.05f)
                {
                    tiempoTranscurrido = 0.05f;
                }

                if (
                    (Input.GetMouseButton(0) && Application.platform!=RuntimePlatform.Android)
                    || Input.GetButton("ps4_X") 
                    || presionandoFire)
                {
                    fuerza += 13.5f * (tiempoTranscurrido);

                    int indiceObjetos = (int)(fuerza*4);

                    if (indiceObjetos >= objetosBencina.Length)
                    {
                        fuerza = 9;
                        return;
                    }

                    for (int i = indiceObjetos; i > 0 && i > indiceObjetos - 3; i--)
                    {
                        objetosBencina[i].position = new Vector3(-7.6f, -4 + 0.14f * i, -1);
                    }
                    
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

        if (enOrbita)
        {
            if (((Application.platform != RuntimePlatform.Android && Input.GetMouseButtonDown(0))
            || Input.GetButtonDown("ps4_X")
            || presionandoFire))
            {
                enOrbita = false;
                movimiento = true;

                int enteroGradosOrbita = ((int)(gradosOrbita / 10)) * 10;

                if (gradosOrbita % 10 > 5) gradosOrbita = 10;
                else gradosOrbita = 0;

                gradosOrbita += enteroGradosOrbita;

                float xGiroOrbita = 0;
                float yGiroOrbita = 0;

                if (objetoOrbita.GetComponent<girarAlrededor>() != null)
                {
                    girarAlrededor datosGiro = objetoOrbita.GetComponent<girarAlrededor>();
                    if (datosGiro.girarDerecha)
                    {
                        xGiroOrbita = Mathf.Cos((datosGiro.grados - 90) * Mathf.Deg2Rad) * datosGiro.distanciaAlCentro;
                        yGiroOrbita = Mathf.Sin((datosGiro.grados - 90) * Mathf.Deg2Rad) * datosGiro.distanciaAlCentro;
                    }
                    else
                    {
                        xGiroOrbita = Mathf.Cos((datosGiro.grados + 90) * Mathf.Deg2Rad) * datosGiro.distanciaAlCentro;
                        yGiroOrbita = Mathf.Sin((datosGiro.grados + 90) * Mathf.Deg2Rad) * datosGiro.distanciaAlCentro;
                    }
                }

                if (girarDerecha)
                {
                    movimientoX = Mathf.Cos((gradosOrbita - 90) * Mathf.Deg2Rad) * fuerzaOrbita + xGiroOrbita;
                    movimientoY = Mathf.Sin((gradosOrbita - 90) * Mathf.Deg2Rad) * fuerzaOrbita + yGiroOrbita;
                }
                else
                {
                    movimientoX = Mathf.Cos((gradosOrbita + 90) * Mathf.Deg2Rad) * fuerzaOrbita + xGiroOrbita;
                    movimientoY = Mathf.Sin((gradosOrbita + 90) * Mathf.Deg2Rad) * fuerzaOrbita + yGiroOrbita;
                }
            }
            else {

                centroOrbita = objetoOrbita.position;

                float diferenciaTiempo = tiempoActual - t0;
                if (girarDerecha)
                {
                    gradosOrbita -= velocidadOrbita * (diferenciaTiempo);
                }
                else
                {
                    gradosOrbita += velocidadOrbita * (diferenciaTiempo);
                }
                transform.position =
                    new Vector2(centroOrbita.x + Mathf.Cos(gradosOrbita * Mathf.Deg2Rad) * distanciaAlCentroOrbita,
                    centroOrbita.y + Mathf.Sin(gradosOrbita * Mathf.Deg2Rad) * distanciaAlCentroOrbita);

                t0 = tiempoActual;
            }
        }

        if (movimiento && !enOrbita)
        {
            float influenciaGravedadX = 0,
                influenciaGravedadY=0;

            foreach (Gravedad gravedad in Gravedad)
            {
                float distancia = Mathf.Pow(gravedad.transform.position.x - transform.position.x, 2) + Mathf.Pow(gravedad.transform.position.y - transform.position.y, 2);
                distancia = Mathf.Sqrt(distancia);
                float razonX = (gravedad.transform.position.x - transform.position.x) / (distancia * distancia);
                float razonY = (gravedad.transform.position.y - transform.position.y) / (distancia * distancia);

                influenciaGravedadX += fuerzaGravedad * gravedad.transform.localScale.x * razonX / distancia;
                influenciaGravedadY += fuerzaGravedad * gravedad.transform.localScale.y * razonY / distancia;
            }

            //print(influenciaGravedadX + " - " + influenciaGravedadY);

            Flecha.controlarPosicion = false;
            Flecha.transform.position = new Vector2(-20, -20);

            if (tiempoActual >= t0 + tiempoRefresco)
            {
                float diferenciaTiempo = tiempoActual - t0;

                transform.position = new Vector2(transform.position.x + movimientoX * diferenciaTiempo + influenciaGravedadX,
                    transform.position.y + movimientoY * diferenciaTiempo + influenciaGravedadY);
                t0 = Time.timeSinceLevelLoad;
                movimientoY -= gravedad * diferenciaTiempo;
            }

            if (transform.position.y < limiteInferior || transform.position.y > limiteSuperior
                || transform.position.x > limiteDerecho || transform.position.x < limiteIzquierdo)
            {
                sonidoReiniciar.Play();
                reiniciar();
            }


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
        if (other.GetComponent<estrella>() != null && !choque && tipo.Equals("enter"))
        {
            //Vector2 diferencia = new Vector2(other.transform.position.x - transform.position.x, other.transform.position.y - transform.position.y);
            other.transform.position = new Vector2(-20, -20);
        }
        else if (other.GetComponent<objetivo>() != null && tipo != "enter" && !other.GetComponent<objetivo>().destruido)
        {
            sonidoObjetivo.Play();
            other.GetComponent<objetivo>().destruido = true;
        }
        else if (other.GetComponent<obstaculo>() != null && !choque && ultimoColisionador != other.transform && tipo.Equals("enter"))
        {
            ultimoColisionador = other.transform;
            choque = true;
            Vector3 rotacion = other.GetComponent<obstaculo>().transform.eulerAngles;
            modificarTrayectoria(rotacion, other.tag);
            sonidoRebote.Play();
        }
        else if (other.GetComponent<bomba>() != null && other.GetComponent<bomba>().borrar != true && !choque)
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

            sonidoBomba.Play();
        }
        else if (other.GetComponent<portalA>() != null && !choque && tipo.Equals("enter"))
        {
            //Vector2 diferencia = new Vector2(other.transform.position.x - transform.position.x, other.transform.position.y - transform.position.y);
            Vector2 diferencia = new Vector2(0, 0);
            Transform portalB = other.GetComponent<portalA>().portalB.transform;
            transform.position = new Vector2(portalB.position.x + diferencia.x, portalB.position.y + diferencia.y);
            sonidoPortal.Play();
        }
        else if (other.GetComponent<Gravedad>() != null)
        {
            sonidoReiniciar.Play();
            reiniciar();
        }
        else if (other.GetComponent<orbita>() != null && !enOrbita && tipo.Equals("enter"))
        {
            objetoOrbita = other.transform;
            distanciaAlCentroOrbita = objetoOrbita.transform.localScale.x * 4;
            centroOrbita = objetoOrbita.position;
            girarDerecha = objetoOrbita.GetComponent<orbita>().girarDerecha;

            gradosOrbita = other.GetComponent<orbita>().gradosInicio;            

            enOrbita = true;
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

        float cambioX = Mathf.Cos(Mathf.Deg2Rad * normal) * fuerzaRebote;
        float cambioY = Mathf.Sin(Mathf.Deg2Rad * normal) * fuerzaRebote;

        movimientoX = cambioX + movimientoX;
        movimientoY = cambioY + movimientoY;
    }
}
