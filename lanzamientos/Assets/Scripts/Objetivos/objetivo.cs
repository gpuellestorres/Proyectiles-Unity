using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class objetivo : MonoBehaviour {

    public bool destruido = false;
    public float tiempoParaSiguienteNivel = 3;
    bool cargandoEscena = false;

    private static AsyncOperation operation;

    // Use this for initialization
    void Start () {	
	}
	
	// Update is called once per frame
	void Update () {
        if (destruido && !cargandoEscena)
        {
            cargandoEscena = true;
            transform.position = new Vector2(-20, -20);
            cargarSiguienteNivel();
            destruido = false;
        }
	}

    private void cargarSiguienteNivel()
    {
        int fasesJugadas = PlayerPrefs.GetInt("contadorFases");
        if (fasesJugadas == 0)
        {
            fasesJugadas = 1;
            PlayerPrefs.SetInt("contadorFases", fasesJugadas);
        }
        else
        {
            fasesJugadas++;
            PlayerPrefs.SetInt("contadorFases", fasesJugadas);
        }


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
            operation = SceneManager.LoadSceneAsync(letra + "" + num);
        }
        else if (Application.CanStreamedLevelBeLoaded((++letra) + "1"))
        {
            operation = SceneManager.LoadSceneAsync((letra) + "1");
        }
        else
        {
            operation = SceneManager.LoadSceneAsync("Menu");
        }

        operation.allowSceneActivation = true;
    }
}
