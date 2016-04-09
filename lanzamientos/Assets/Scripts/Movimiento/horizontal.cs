using UnityEngine;
using System.Collections;

public class horizontal : MonoBehaviour {

    public float velocidad = 3;
    public float tiempoRefresco = 0.01f;
    float t0 = 0;

    public float limiteIzquierdo = 4.5f;
    public float limiteDerecho = -4.5f;

    public int direccion = 1;

    // Use this for initialization
    void Start () {
        t0 = Time.timeSinceLevelLoad;
    }
	
	// Update is called once per frame
	void Update () {
        float tiempoActual = Time.timeSinceLevelLoad;
        if (tiempoActual >= tiempoRefresco + t0)
        {
            float tiempoTranscurrido = tiempoActual - t0;

            float posicionX = transform.position.x + velocidad * tiempoTranscurrido * direccion;

            if (direccion == 1)
            {
                if (posicionX >= limiteDerecho)
                {
                    posicionX = limiteDerecho;
                    direccion = -1;
                }
            }
            else
            {
                if (posicionX <= limiteIzquierdo)
                {
                    posicionX = limiteIzquierdo;
                    direccion = 1;
                }
            }

            transform.position = new Vector2(posicionX, transform.position.y);

            t0 = tiempoActual;
        }
    }
}
