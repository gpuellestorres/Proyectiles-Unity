using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class objetivo : MonoBehaviour {

    public bool destruido = false;
    public float tiempoParaSiguienteNivel = 3;
    bool cargandoEscena = false;

    private static AsyncOperation operation;

    public Button Reintentar;
    public Button Siguiente;
    Vector3 posicionSiguiente, posicionReintentar;
    public Transform fondoNegro;

    AudioSource sonidoExito;

    // Use this for initialization
    void Start () {
        quitarElementosMenu();
    }

    private void quitarElementosMenu()
    {
        foreach (Button boton in FindObjectsOfType<Button>())
        {
            if (boton.name.Equals("NEXT"))
            {
                Reintentar = boton;
                posicionReintentar = new Vector3(Reintentar.transform.position.x, Reintentar.transform.position.y, -9.5f);

                Reintentar.transform.position = new Vector2(-10000, -10000);
            }
            else if (boton.name.Equals("RETRY"))
            {
                Siguiente = boton;
                posicionSiguiente = new Vector3(Siguiente.transform.position.x, Siguiente.transform.position.y, -9.5f);

                Siguiente.transform.position = new Vector2(-10000, -10000);
            }
        }
    }

    public void cargarEsteNivel()
    {
        Reintentar.onClick.RemoveAllListeners();

        operation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        operation.allowSceneActivation = true;        
    }

    // Update is called once per frame
    void Update () {

        if (Reintentar == null)
        {
            quitarElementosMenu();
        }

        if (sonidoExito == null)
        {
            foreach (AudioSource Sonido in FindObjectsOfType<AudioSource>())
            {
                if (Sonido.name.Equals("sonidoObjetivo(Clone)"))
                    sonidoExito = Sonido;
            }
            
        }

        if (destruido && !cargandoEscena)
        {
            transform.position = new Vector2(-20, -20);
            Time.timeScale = 0;

            if (sonidoExito.isPlaying && !AudioListener.pause)
            {
                return;
            }

            Reintentar.transform.position = posicionReintentar;
            Siguiente.transform.position = posicionSiguiente;

            Instantiate(fondoNegro);

            cargandoEscena = true;
            destruido = false;
        }
	}

    public void cargarSiguienteNivel()
    {
        Siguiente.onClick.RemoveAllListeners();

        //Se guarda si se encontró la estrella
        if(FindObjectOfType<estrella>().encontrada())
        PlayerPrefs.SetString("estrella" + SceneManager.GetActiveScene().name, "true");

        //Se carga el siguiente nivel

        int fasesJugadas = PlayerPrefs.GetInt("contadorFases");

        fasesJugadas++;
        PlayerPrefs.SetInt("contadorFases", fasesJugadas);

        string nivelActual = SceneManager.GetActiveScene().name;

        char[] caracteres = nivelActual.ToCharArray();

        char letra = caracteres[0];
        string numero = "";

        for (int i = 1; i < caracteres.Length; i++)
        {
            numero += caracteres[i];
        }

        int num = int.Parse(numero);
        num++;

        if (Application.CanStreamedLevelBeLoaded(letra + "" + num))
        {
            guardarMayorDisponible(letra + "" + num);
            operation = SceneManager.LoadSceneAsync(letra + "" + num);
        }
        else if (Application.CanStreamedLevelBeLoaded((++letra) + "1"))
        {
            guardarMayorDisponible((letra) + "1");
            operation = SceneManager.LoadSceneAsync((letra) + "1");
        }
        else
        {
            operation = SceneManager.LoadSceneAsync("Menu");
        }

        operation.allowSceneActivation = true;
    }

    private void guardarMayorDisponible(string nivel)
    {
        PlayerPrefs.SetString("ultimaEscenaJugada", nivel);
        if (esMayorAMayorDisponible(nivel))
            PlayerPrefs.SetString("mayorEscenaDisponible", nivel);
    }

    private bool esMayorAMayorDisponible(string escenaCargar)
    {
        if (PlayerPrefs.GetString("mayorEscenaDisponible") == "") return true;

        string letraActual = escenaCargar.Substring(0, 1);
        string num = escenaCargar.Substring(1, escenaCargar.Length - 1);

        string mayorDisp = PlayerPrefs.GetString("mayorEscenaDisponible");

        string letraDisp = mayorDisp.Substring(0, 1);
        string numDisp = mayorDisp.Substring(1, mayorDisp.Length - 1);

        if (letraActual.ToCharArray()[0] > letraDisp.ToCharArray()[0])
        {
            return true;
        }
        else
        {
            if (letraActual == letraDisp)
            {
                if (int.Parse(num) > int.Parse(numDisp)) return true;
                return false;
            }
            else return false;
        }

    }
}
