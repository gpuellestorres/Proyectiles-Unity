using UnityEngine;
using System.Collections;

public class agrandar : MonoBehaviour {

    public float velocidad = 0.1f;
    public float periodo = 0.1f;
    float t0;

    public float tamañoMinimo = 0.1f;

    public float tamañoMaximo = 2.6f;

    // Use this for initialization
    void Start () {
        t0 = Time.timeSinceLevelLoad;
	}
	
	// Update is called once per frame
	void Update () {

        float tiempoActual = Time.timeSinceLevelLoad;
        if (tiempoActual >= t0 + periodo)
        {
            transform.localScale = new Vector2(transform.localScale.x+(tiempoActual-t0)*velocidad, 
                transform.localScale.y + (tiempoActual - t0) * velocidad);

            t0 = tiempoActual;
        }

        if (transform.localScale.x >= tamañoMaximo || transform.localScale.x<=tamañoMinimo)
        {
            velocidad = -velocidad;
        }
	}
}