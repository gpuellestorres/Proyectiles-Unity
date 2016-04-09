using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class puntero : MonoBehaviour {

    Camera camera;

    public float velocidadJoystick=5;
    float t0;
    
    aparicionStick stick;

    Vector2 posicionAnteriorAndroid;

    bool moviendoIzquierdaAndroid = false;
    bool moviendoDerechaAndroid = false;
    bool moviendoArribaAndroid = false;
    bool moviendoAbajoAndroid = false;

    float arriba, abajo, izquierda, derecha;

    // Use this for initialization
    void Start() {
        camera = FindObjectOfType<Camera>();
        t0 = Time.timeSinceLevelLoad;

        stick = FindObjectOfType<aparicionStick>();

        arriba = camera.ScreenToWorldPoint(new Vector2(0, Screen.height)).y;
        abajo = camera.ScreenToWorldPoint(new Vector2(0, 0)).y;
        izquierda = camera.ScreenToWorldPoint(new Vector2(0, 0)).x;
        derecha = camera.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x;
    }

    // Update is called once per frame
    void Update()
    {
        Text texto = FindObjectOfType<Text>();
        if (Input.touchCount > 0) texto.text = Input.touches[0].position.x + " - " + Input.touches[0].position.y;
        else texto.text = Input.mousePosition.x + " - " + Input.mousePosition.y;

        float t1 = Time.timeSinceLevelLoad;

        if ((Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse X") != 0) && Application.platform != RuntimePlatform.Android)
        {
            Vector2 mousePosition = Input.mousePosition;
            Vector3 position = camera.ScreenToWorldPoint(mousePosition);

            if (Application.platform != RuntimePlatform.Android) transform.position = new Vector3(position.x, position.y, -1);
        }
        else if ((Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) && Application.platform != RuntimePlatform.Android)
        {
            transform.position = new Vector3(transform.position.x + Input.GetAxis("Horizontal") * velocidadJoystick * (t1 - t0),
                transform.position.y + Input.GetAxis("Vertical") * velocidadJoystick * (t1 - t0), -1);
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.touchCount > 0)
            {
                int i = 0;
                /*moviendoDerechaAndroid = false;
                moviendoIzquierdaAndroid = false;
                moviendoArribaAndroid = false;
                moviendoAbajoAndroid = false;//*/

                for (i = Input.touchCount-1; i > 0; i--)
                {
                    Touch touchActual = Input.GetTouch(i);
                    if (touchActual.position.x < Screen.width / 2 && touchActual.position.y < Screen.height / 2)
                    {
                        Vector2 deltaPosicion = touchActual.deltaPosition;

                        if (deltaPosicion.x>5)
                        {
                            moviendoDerechaAndroid = true;
                            moviendoIzquierdaAndroid = false;
                        }
                        else if (deltaPosicion.x<5)
                        {
                            moviendoDerechaAndroid = false;
                            moviendoIzquierdaAndroid = true;
                        }

                        if (deltaPosicion.y>5)
                        {
                            moviendoArribaAndroid = true;
                            moviendoAbajoAndroid = false;
                        }
                        else if (deltaPosicion.y<5)
                        {
                            moviendoArribaAndroid = false;
                            moviendoAbajoAndroid = true;
                        }
                        break;
                    }
                }
                float movimientoX = 0;
                float movimientoY = 0;

                if (moviendoIzquierdaAndroid) movimientoX = -1;
                else if (moviendoDerechaAndroid) movimientoX = 1;

                if (moviendoArribaAndroid) movimientoY = 1;
                else if (moviendoAbajoAndroid) movimientoY = -1;

                transform.position = new Vector3(transform.position.x + movimientoX * velocidadJoystick * (t1 - t0),
                    transform.position.y + movimientoY * velocidadJoystick * (t1 - t0), -1);

                //if (stick != null) stick.finEjecucion();
            }
        }

        if (transform.position.x < izquierda + 0.2f || transform.position.x > derecha - 0.2f ||
            transform.position.y < abajo + 0.2f || transform.position.y > arriba - 0.2f)
        {
            float y = transform.position.y;
            if (y < abajo + 0.2f) y = abajo + 0.2f;
            else if (y > arriba - 0.2f) y = arriba - 0.2f;

            float x = transform.position.x;
            if (x < izquierda + 0.2f) x = izquierda + 0.2f;
            else if (x > derecha - 0.2f) x = derecha - 0.2f;

            transform.position = new Vector3(x, y, -1);
        }

        t0 = t1;
    }
    
}
