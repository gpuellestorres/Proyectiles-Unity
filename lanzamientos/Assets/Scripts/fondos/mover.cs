using UnityEngine;
using System.Collections;

public class mover : MonoBehaviour {

    public float velocidad = 0.1f;
    public float periodo = 0.1f;
    float t0;

    public float posicionReinicioX = -4.08f;
    public float posicionReinicioY = -4.08f;

    public float posicionReiniciarX = 3.72f;
    public float posicionReiniciarY = 3.72f;

    // Use this for initialization
    void Start () {
        t0 = Time.timeSinceLevelLoad;
    }
	
	// Update is called once per frame
	void Update () {
        float tiempoActual = Time.timeSinceLevelLoad;
        if (tiempoActual >= t0 + periodo)
        {
            transform.localPosition = new Vector2(transform.localPosition.x + (tiempoActual - t0) * velocidad,
                transform.localPosition.y + (tiempoActual - t0) * velocidad);

            t0 = tiempoActual;
        }

        if (transform.localPosition.x >= posicionReiniciarX
            || transform.localPosition.y >= posicionReiniciarY)
        {
            transform.localPosition = new Vector2(posicionReinicioX, posicionReinicioY);
        }
    }
}
