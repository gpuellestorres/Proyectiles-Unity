using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class cerrar : MonoBehaviour {

    public float tiempoDobleCierre = 1;
    float t0=-1;
    public Text textoSalir;
 
	// Use this for initialization
	void Start () {

        Text[] textos = FindObjectsOfType<Text>();

        foreach (Text texto in textos)
        {
            if (texto.name == "textoSalir")
            {
                textoSalir = texto;
                quitarMensaje();
            }
        }
	
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (t0 == -1)
            {
                t0 = Time.timeSinceLevelLoad;

                mostrarMensaje();
            }
            else if (Time.timeSinceLevelLoad < t0 + tiempoDobleCierre)
            {
                //Application.Quit();
                SceneManager.LoadScene("Menu");
            }
            else {
                t0 = -1;
                quitarMensaje();
            }
        }
        else
        {
            if (Time.timeSinceLevelLoad > t0 + tiempoDobleCierre)
            {
                t0 = -1;
                quitarMensaje();
            }
        }

	}

    private void quitarMensaje()
    {
        textoSalir.text = "";
    }

    private void mostrarMensaje()
    {
        textoSalir.text = "PRESS AGAIN TO EXIT";
    }
}
