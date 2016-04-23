using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class objetivo : MonoBehaviour {

    public bool destruido = false;
    public float tiempoParaSiguienteNivel = 3;
    bool cargandoEscena = false;

    private static AsyncOperation operation;

    AudioSource sonidoExito;

    // Use this for initialization
    void Start () {	
        
	}
	
	// Update is called once per frame
	void Update () {

        if (sonidoExito == null)
        {
            foreach (AudioSource Sonido in FindObjectsOfType<AudioSource>())
            {
                print(Sonido.name);
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

            cargandoEscena = true;
            cargarSiguienteNivel();
            destruido = false;
        }
	}

    private void cargarSiguienteNivel()
    {
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
