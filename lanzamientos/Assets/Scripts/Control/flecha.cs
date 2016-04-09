using UnityEngine;
using System.Collections;

public class flecha : MonoBehaviour {

    Vector2 centro;
    public float distanciaRespectoAlCentro = 1.5f;

    public float x, y;

    public bool controlarPosicion = true;

    puntero puntero;

    // Use this for initialization
    void Start () {
        pelota Pelota = FindObjectOfType<pelota>();
        puntero = FindObjectOfType<puntero>();
        centro = new Vector2(Pelota.transform.position.x, Pelota.transform.position.y);
    }
	
	// Update is called once per frame
	void Update () {
        Vector2 diferenciaCentroMouse = new Vector2(puntero.transform.position.x - centro.x, puntero.transform.position.y - centro.y);

        float razonXY = 0;
        if (diferenciaCentroMouse.y != 0)
        {
            razonXY = diferenciaCentroMouse.x / diferenciaCentroMouse.y;
        }

        float hipotenusa2 = distanciaRespectoAlCentro * distanciaRespectoAlCentro;

        x = 1 + razonXY * razonXY;
        x = hipotenusa2 / x;
        x = Mathf.Sqrt(x);

        y = x;
        x = x * razonXY;

        

        float angulo = Mathf.Rad2Deg * Mathf.Acos(x / distanciaRespectoAlCentro);
        if (puntero.transform.position.y >= centro.y)
        {
            transform.rotation = Quaternion.Euler(0, 0, 270 + angulo);
            if (controlarPosicion)
                transform.position = new Vector2(centro.x + x, centro.y + y);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 90 + angulo);
            if (controlarPosicion)
                transform.position = new Vector2(centro.x - x, centro.y - y);
        }
	}
}
