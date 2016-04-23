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

    public sonidoEntreEscenas sonidoNiveles;

    public Transform estrella;

	// Use this for initialization
	void Start () {
        Time.timeScale = 1;
        iniciarSonido();

        foreach (AudioSource Audio in FindObjectsOfType<AudioSource>())
        {
            if (Audio.name.Equals("MusicaMenu"))
            {
                Audio.Play();
            }
            else if (Audio.GetComponent<sonidoEntreEscenas>()!=null)
            {
                Audio.Stop();
            }
        }


        if (PlayerPrefs.GetString("mayorEscenaDisponible").Equals(""))
        {
            PlayerPrefs.SetString("mayorEscenaDisponible", "A1");
        }
        else if (!PlayerPrefs.GetString("ultimaEscenaJugada").Equals(""))
        {
            escenaElegida = PlayerPrefs.GetString("ultimaEscenaJugada");

            mostrarEscenaElegida();
        }
        else
        {
            escenaElegida = PlayerPrefs.GetString("mayorEscenaDisponible");

            mostrarEscenaElegida();
        }	
	}

    private void mostrarEscenaElegida()
    {
        nivelActual.text = escenaElegida;

        foreach (imagenNivel IMG in FindObjectsOfType<imagenNivel>())
        {
            Destroy(IMG.gameObject);
        }

        Instantiate(Resources.Load("ImgNiveles/" + escenaElegida));

        if (PlayerPrefs.GetString("estrella" + escenaElegida).Equals("true"))
        {
            estrella.position = new Vector3(3.7f, 3.1f, -4);
        }
        else
        {
            estrella.position = new Vector2(-20, -20);
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

        mostrarEscenaElegida();
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

        mostrarEscenaElegida();
    }

    public void cargarNivel()
    {
        
        if (FindObjectOfType<sonidoEntreEscenas>() == null)
        {
            Instantiate(sonidoNiveles);
        }
        FindObjectOfType<sonidoEntreEscenas>().GetComponent<AudioSource>().Play();

        SceneManager.LoadScene(escenaElegida);
    }

    public void sonido()
    {
        if (PlayerPrefs.GetString("sound").Equals("") || PlayerPrefs.GetString("sound").Equals("on"))
        {
            //FindObjectOfType<AudioListener>().enabled = false;
            AudioListener.pause = true;
            PlayerPrefs.SetString("sound", "off");

            foreach (Image IMG in FindObjectsOfType<Image>())
            {
                if (IMG.name.Equals("Sonido"))
                {
                    IMG.color = new Color(1f, 79f / 255f, 0, 192f / 255f);

                    IMG.transform.GetChild(0).GetComponent<Text>().text = "SOUND OFF";
                }
            }
        }
        else
        {
            //FindObjectOfType<AudioListener>().enabled = true;
            AudioListener.pause = false;
            PlayerPrefs.SetString("sound", "on");

            foreach (Image IMG in FindObjectsOfType<Image>())
            {
                if (IMG.name.Equals("Sonido"))
                {
                    IMG.color = new Color(93f / 255f, 1, 62f / 255f, 192f / 255f);

                    IMG.transform.GetChild(0).GetComponent<Text>().text = "SOUND ON";
                }
            }
        }
    }


    public void iniciarSonido()
    {
        if (PlayerPrefs.GetString("sound").Equals("") || PlayerPrefs.GetString("sound").Equals("on"))
        {
            //FindObjectOfType<AudioListener>().enabled = true;
            AudioListener.pause = false;
            PlayerPrefs.SetString("sound", "on");

            foreach (Image IMG in FindObjectsOfType<Image>())
            {
                if (IMG.name.Equals("Sonido"))
                {
                    IMG.color = new Color(93f / 255f, 1, 62f / 255f, 192f / 255f);

                    IMG.transform.GetChild(0).GetComponent<Text>().text = "SOUND ON";
                }
            }
        }
        else
        {
            //FindObjectOfType<AudioListener>().enabled = false;
            AudioListener.pause = true;
            PlayerPrefs.SetString("sound", "off");

            foreach (Image IMG in FindObjectsOfType<Image>())
            {
                if (IMG.name.Equals("Sonido"))
                {
                    IMG.color = new Color(1f, 79f / 255f, 0, 192f / 255f);

                    IMG.transform.GetChild(0).GetComponent<Text>().text = "SOUND OFF";
                }
            }
        }
    }
}
