using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class mostrarNivel : MonoBehaviour {

    Text Texto;

	// Use this for initialization
	void Start () {
        Texto = FindObjectOfType<Text>();
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
