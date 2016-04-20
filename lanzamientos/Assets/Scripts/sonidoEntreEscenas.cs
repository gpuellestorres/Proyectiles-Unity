using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class sonidoEntreEscenas : MonoBehaviour {

    public AudioClip[] pistasSonido;

    private static sonidoEntreEscenas instance = null;

    AudioSource sonido;

    public static sonidoEntreEscenas Instance
    {
        get
        { return instance; }
    }
    void Awake()
    {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
            return;
        } else {
            sonido = GetComponent<AudioSource>();
            sonido.clip = obtenerMusicaAlAzar();
            sonido.Play();
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        if (sonido == null)
        {
            sonido = GetComponent<AudioSource>();
        }
        else if (!sonido.isPlaying && !SceneManager.GetActiveScene().name.Equals("Menu"))
        {
            sonido.clip = obtenerMusicaAlAzar();
            sonido.Play();
        }
    }

    private AudioClip obtenerMusicaAlAzar()
    {
        int pos = (int)(Random.value * pistasSonido.Length);
        if (pos == pistasSonido.Length) pos--;
        return pistasSonido[pos];
    }
}
