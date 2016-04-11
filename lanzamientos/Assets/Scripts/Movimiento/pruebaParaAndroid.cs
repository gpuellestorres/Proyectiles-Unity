using UnityEngine;
using System.Collections;

public class pruebaParaAndroid : MonoBehaviour {

    public Vector2 centro;

    Camera camera;

    bool empezar = false;

    // Use this for initialization
    void Start () {
        camera = FindObjectOfType<Camera>();
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(0) && !empezar)
        {
            centro = camera.ScreenToWorldPoint(Input.mousePosition);
            empezar = true;
        }
        else if (Input.GetMouseButton(0) && empezar)
        {
            Vector2 nuevaPosicion = camera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 movimiento = new Vector2(nuevaPosicion.x - centro.x, nuevaPosicion.y - centro.y);

            float x = movimiento.x;
            float y = movimiento.y;

            /*if (Mathf.Abs(x) > Mathf.Abs(y))
            {
                y = y / x;
                if (x > 0) x = 1;
                else x = -1;
            }
            else if (Mathf.Abs(x) < Mathf.Abs(y))
            {
                x = x / y;
                if (y > 0) y = 1;
                else y = -1;
            }
            else
            {
                x = 0; y = 0;
            }//*/

            transform.position = new Vector2(transform.position.x + x / 10, transform.position.y + y / 10);
        }
        else
        {
            empezar = false;
        }

	}
}