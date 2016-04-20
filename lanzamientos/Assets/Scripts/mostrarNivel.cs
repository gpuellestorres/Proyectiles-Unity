using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class mostrarNivel : MonoBehaviour {

    Text Texto;

	// Use this for initialization
	void Start () {

        Text[] textos = FindObjectsOfType<Text>();

        foreach (Text texto in textos)
        {
            if (texto.name == "Text")
            {
                Texto = texto;
            }
        }

        string escena= SceneManager.GetActiveScene().name;

        if (escena != "Menu")
        {
            Texto.text = escena;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

