using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndShoot : MonoBehaviour
{

    #region Variables
    
    private Rigidbody rb;
    private Vector3 startPos;
    private Vector3 realasePos;

    [Header("Camera Properties")]
    [Space(15)]
    
    [SerializeField]
    private Transform cameraTransform;

    [SerializeField]
    private Camera ZoomCamera;

    [SerializeField]
    private float scrollSpeed = 10f;

    [SerializeField]
    private Vector3 cameraOffset;

    [Header("Free Look Camera Properties")]
    [Space(15)]

    [SerializeField]
    private float mouseSensitivity = 3f;

    [SerializeField]
    private Vector3 distanceFromTarget;

    [SerializeField]
    private float smoothTime = 10;

    [Header("Line Direction Properties")]
    [Space(15)] 

    [SerializeField]
    private LineRenderer directionLine;
    [SerializeField]
    private Vector3 lineOffset;
    
    [SerializeField]
    private float lineMultiplier = 2;
    [SerializeField]
    private float lineAngle;
    
    [SerializeField]
    private float maxLineMagnitude = 100;

    [Header("Force Impulse Properties")]
    [Space(15)]

    [SerializeField]
    private float forceMultiplier = 2;

    [SerializeField]
    private float maxForceMagnitude = 100;

    [Header("Particle System")]
    [Space(15)]

    [SerializeField]
    private ParticleSystem dustParticles;

    [SerializeField]
    private ParticleSystem respawnParticles;

    #endregion

    #region Private Variables

    private Vector3 vel = Vector3.zero;
    private float rotationX;
    private float rotationY;
    private Vector3 currentRotation;
    private Vector3 cameraRotation;
    private Vector3 cameraTempRot = Vector3.zero;
    private Vector3 cameraOffset2;
    private Vector3 cameraOffset1;
    private bool isShoot;
    private bool isShooting;
    private bool isGrounded;
    private bool firstThrow = true;

    #endregion

    void Awake() {

        rb = GetComponent<Rigidbody>();
        ZoomCamera = Camera.main;
        respawnParticles.Stop();
        cameraRotation = cameraTransform.localEulerAngles;
        cameraOffset1 = cameraOffset;
        cameraOffset2 = cameraOffset * 2;
    }

    void Update() {

        if(Input.GetKeyDown(KeyCode.R))
        {
            Reset();
        }

        if(Input.GetMouseButton(1))
        {
            FreeLook();
        }
        else
        {
            MoveCamera();

            if(isShooting)
            {   
                //Cogemos un vector temporal para marcar la dirección del tiro
                Vector3 tempo = startPos - Input.mousePosition;
                
                //Clampleamos el valor max
                if(tempo.magnitude >= maxLineMagnitude)
                {
                    tempo = tempo.normalized * maxLineMagnitude;
                }

                cameraOffset = cameraOffset2;

                //Aplicamos una ligera rotación sobre el eje y
                tempo = Quaternion.AngleAxis(lineAngle, Vector3.up) * tempo;

                //Colocamos las posiciones de la linea en sus respectivos puntos
                directionLine.gameObject.SetActive(true);
                directionLine.SetPosition(0, transform.position + lineOffset);
                directionLine.SetPosition(1, transform.position + tempo * lineMultiplier);
            }
            else
            {
                directionLine.gameObject.SetActive(false);
            }
        }

        Zoom();
    }

    void OnMouseDown() {
        //Cuando clicamos pillamos la refe de donde empiza y decimos que está cargando el disparo
        startPos = Input.mousePosition;
        isShooting = true;
        respawnParticles.Stop();
    }   

    void OnMouseUp()
    {
        //Cuando soltamos el ratón metemos el impulso y quitamos la linea de dirección
        realasePos = Input.mousePosition;
        isShooting = false;
        cameraOffset = cameraOffset1;
        Shoot(startPos-realasePos);
        dustParticles.Play();

        if(firstThrow)
        {
            firstThrow = false;
        }
        else if(!firstThrow)
        {
            //Se le acabaron los intentos
            if (DiceManager.instance.UpdateTrys())
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                transform.position = DiceManager.instance.ActualCheckPoint().position;
                rb.AddForce(0, -20, 0);
                respawnParticles.Emit(15);
            }

        }
        
    }

    private void Shoot(Vector3 force)
    {

        if(isShoot)
        {
            return;
        }
        else
        {
            //Clampeamos el valor máximo del vector 
            if(force.magnitude >= maxForceMagnitude)
            {
                force = force.normalized * maxForceMagnitude;
            }

            //Añadimos el impulso al cubo
            rb.AddForce(new Vector3(force.x, force.y, force.y) * forceMultiplier, ForceMode.Impulse);
            //Añadimos también una pequeña rotación al cubo cada vez que lo lanzamos para darle algo de random
            rb.AddTorque(Random.Range(0,200), Random.Range(0,200),Random.Range(0,200));

            isShoot = false;
        }

    }

    private void Zoom()
    {
        if(ZoomCamera.orthographic)
        {
            ZoomCamera.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
        }
        else
        {
            ZoomCamera.fieldOfView -= Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;

            if(ZoomCamera.fieldOfView <= 25)
            {
                ZoomCamera.fieldOfView = 25;
            }
            else if(ZoomCamera.fieldOfView >= 75)
            {
                ZoomCamera.fieldOfView = 75;
            }
        }
    }

    private void MoveCamera()
    {
        //Movemos la cámara hacia la posición del cubo
        cameraTransform.position = Vector3.SmoothDamp(cameraTransform.position, transform.position + cameraOffset, ref vel, smoothTime);

        cameraTransform.localEulerAngles = cameraRotation;
    }

    private void FreeLook()
    {

        distanceFromTarget = cameraTransform.position - transform.position;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        rotationX -= mouseY;
        rotationY += mouseX;

        rotationX = Mathf.Clamp(rotationX, -60,60);

        Vector3 nextRotation = new Vector3(rotationX, rotationY);

        currentRotation = Vector3.SmoothDamp(currentRotation, nextRotation,ref vel, smoothTime);

        cameraTransform.localEulerAngles = currentRotation;

        cameraTransform.position = transform.position - cameraTransform.forward * distanceFromTarget.magnitude;
    }

    private void Reset()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = DiceManager.instance.ActualCheckPoint().position + new Vector3(0,10,0);
        rb.AddForce(0, -20, 0);
        respawnParticles.Emit(15);
        firstThrow = true;
    }
}
