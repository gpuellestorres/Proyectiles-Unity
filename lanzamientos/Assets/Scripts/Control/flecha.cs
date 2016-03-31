using UnityEngine;
using System.Collections;

public class flecha : MonoBehaviour {

    public Vector2 centro = new Vector2(-9, -4);
    public float distanciaRespectoAlCentro = 1.5f;

    public float x, y;

    public bool controlarPosicion = true;

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        Vector2 diferenciaCentroMouse = new Vector2(Input.mousePosition.x - centro.x, Input.mousePosition.y - centro.y);

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

        if(controlarPosicion)
        transform.position = new Vector2(centro.x + x, centro.y + y);

        float angulo = Mathf.Rad2Deg * Mathf.Acos(x / distanciaRespectoAlCentro);

        transform.rotation = Quaternion.Euler(0, 0, 270 + angulo);
	}
}
