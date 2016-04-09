using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class puntero : MonoBehaviour {

    Camera camera;

    public float velocidadJoystick=5;
    float t0;
    
    aparicionStick stick;

    Vector2 posicionAnteriorAndroid;

    float moviendoXant = 0, moviendoYant = 0, Xandroid =0, YAndroid=0;

    float arriba, abajo, izquierda, derecha;

    public float deadZoneAndroid = 0.5f;

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
        else if (Input.touchCount > 0)
        {
            float movimientoX = 0;
            float movimientoY = 0;

            if (Input.touchCount > 0 &&
            (Input.GetTouch(0).position.x < Screen.width / 2 && Input.GetTouch(0).position.y < Screen.height / 2))
            {
                Touch touchActual = Input.GetTouch(0);
                if (touchActual.phase == TouchPhase.Moved)
                {
                    movimientoX = touchActual.deltaPosition.x;
                    movimientoY = touchActual.deltaPosition.y;
                }
            }
            else if (Input.touchCount > 1 &&
            (Input.GetTouch(1).position.x < Screen.width / 2 && Input.GetTouch(1).position.y < Screen.height / 2))
            {
                Touch touchActual = Input.GetTouch(1);
                if (touchActual.phase == TouchPhase.Moved)
                {
                    movimientoX = touchActual.deltaPosition.x;
                    movimientoY = touchActual.deltaPosition.y;
                }
            }
            else
            {
                moviendoXant = 0;
                moviendoYant = 0;
                Xandroid = 0;
                YAndroid = 0;
            }

            if (movimientoX > deadZoneAndroid || movimientoX < -deadZoneAndroid)
            {
                moviendoXant = movimientoX;
            }
                if(movimientoY > deadZoneAndroid || movimientoY < -deadZoneAndroid)
            {                
                moviendoYant = movimientoY;
            }
            Xandroid = moviendoXant / 2;
            YAndroid = moviendoYant / 2;

            if (Xandroid > 1) Xandroid = 1;
            if (YAndroid > 1) YAndroid = 1;
            if (Xandroid < -1) Xandroid = -1;
            if (YAndroid < -1) YAndroid = -1;

            transform.position = new Vector3(transform.position.x + Xandroid * velocidadJoystick * (t1 - t0),
                transform.position.y + YAndroid * velocidadJoystick * (t1 - t0), -1);

            //if (stick != null) stick.finEjecucion();
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
