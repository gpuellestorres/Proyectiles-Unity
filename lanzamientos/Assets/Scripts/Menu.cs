using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class Menu : MonoBehaviour {

    public Text nivelActual;
    string escenaElegida="A1";

    public string mayorLetra = "C";
    public int mayorNumero = 10;

	// Use this for initialization
	void Start () {
        if (PlayerPrefs.GetString("mayorEscenaDisponible").Equals(""))
        {
            PlayerPrefs.SetString("mayorEscenaDisponible", "A1");
        }	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButton("Start")) cargarNivel();
	}

    public void cerrar()
    {
        Application.Quit();
    }

    public void avanzarEscena()
    {
        string letraActual = escenaElegida.Substring(0, 1);
        string num = escenaElegida.Substring(1, escenaElegida.Length - 1);
        print(num);
        int numeroActual = int.Parse(escenaElegida.Substring(1, escenaElegida.Length - 1));
        if (numeroActual >= mayorNumero)
        {
            if (!letraActual.Equals(mayorLetra))
            {
                letraActual = ((char)(char.Parse(letraActual) + 1)).ToString();
                escenaElegida = letraActual + "1";

            }
        }
        else
        {
            escenaElegida = letraActual + (numeroActual + 1);
        }
        if (!estaDisponible(escenaElegida))
        {
            retrocederEscena();
            return;
        }

        nivelActual.text = escenaElegida;

        foreach (imagenNivel IMG in FindObjectsOfType<imagenNivel>())
        {
            Destroy(IMG.gameObject);
        }

        Instantiate(Resources.Load("ImgNiveles/" + escenaElegida));
    }

    private bool estaDisponible(string escenaElegida)
    {
        string letraActual = escenaElegida.Substring(0, 1);
        string num = escenaElegida.Substring(1, escenaElegida.Length - 1);

        string mayorDisp = PlayerPrefs.GetString("mayorEscenaDisponible");

        string letraDisp = mayorDisp.Substring(0, 1);
        string numDisp = mayorDisp.Substring(1, mayorDisp.Length - 1);

        if (letraActual.ToCharArray()[0] > letraDisp.ToCharArray()[0])
        {
            return false;
        }
        else
        {
            if (letraActual == letraDisp)
            {
                if (int.Parse(num) > int.Parse(numDisp)) return false;
                return true;
            }
            else return true;
        }

    }

    public void retrocederEscena()
    {
        string letraActual = escenaElegida.Substring(0, 1);
        string num = escenaElegida.Substring(1, escenaElegida.Length - 1);
        print(num);
        int numeroActual = int.Parse(escenaElegida.Substring(1, escenaElegida.Length - 1));
        if (numeroActual == 1)
        {
            if (!letraActual.Equals("A"))
            {
                letraActual = ((char)(char.Parse(letraActual) - 1)).ToString();
                escenaElegida = letraActual + "10";

            }
        }
        else
        {
            escenaElegida = letraActual + (numeroActual - 1);
        }

        nivelActual.text = escenaElegida;

        foreach (imagenNivel IMG in FindObjectsOfType<imagenNivel>())
        {
            Destroy(IMG.gameObject);
        }

        Instantiate(Resources.Load("ImgNiveles/" + escenaElegida));
    }

    public void cargarNivel()
    {
        SceneManager.LoadScene(escenaElegida);
    }
}
