using UnityEngine;
using System.Collections;

public class bomba : MonoBehaviour {

    public bool borrar = false;
    public float tiempoDesaparecer = 0.5f;
    float t0 = 0;

    bool posicionInicioEncontrada = false;

    Vector2 posicionInicio;

    // Use this for initialization
    void Start () {
        if (!posicionInicioEncontrada)
        {
            posicionInicio = transform.position;
            posicionInicioEncontrada = true;
        }
        reiniciar();
	}

    public void reiniciar()
    {
        if(posicionInicioEncontrada)transform.position = posicionInicio;
        borrar = false;
        t0 = 0;
    }
	
	// Update is called once per frame
	void Update () {

        if (borrar)
        {
            if (t0 == 0)
            {
                t0 = Time.timeSinceLevelLoad;
            }
            else
            {
                if (Time.timeSinceLevelLoad >= t0 + tiempoDesaparecer)
                {
                    desaparecer();
                }
            }
        }
	}

    void desaparecer()
    {
        transform.position = new Vector2(-30, -30);
    }
}
