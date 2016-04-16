using UnityEngine;
using System.Collections;

public class agrandar : MonoBehaviour {

    public float velocidad = 0.1f;
    public float periodo = 0.1f;
    float t0;

    public float tamañoReinicioX = 0.1f;
    public float tamañoReinicioY = 0.1f;

    public float tamañoReiniciarX = 2.6f;
    public float tamañoReiniciarY = 2.6f;

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

        if (transform.localScale.x >= tamañoReiniciarX
            || transform.localScale.y>=tamañoReiniciarY)
        {
            transform.localScale = new Vector2(tamañoReinicioX, tamañoReinicioY);
        }
	}
}