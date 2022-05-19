using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody PlayerRB;
    public Camera PlayerCamera;
    public float mouseSensitivity = 90f;
    public float playerSpeed = 10f;
    public float jumpForce = 10f;
    public GameObject PortalA;
    public GameObject PortalB;
    public GameObject Player;
    public GameObject SpawnPointA;
    public GameObject SpawnPointB;


    private Vector3 targetPosition;
    private float rotationOnX;
    private bool isJumping;
    private Vector3 prevoiusMousePosition;
    private Vector3 prevoiusPlayerPosition;
    private float z = 0;
    private bool isWalking = false;
    private Vector3 lastGroundCenterPosition;


    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }


    void Update()
    {
        if (Input.mousePosition != prevoiusMousePosition)
        {
            rotateCamera();
        }
        if (Input.GetMouseButtonDown(1))
        {
            setPortalA();
        }
        if (Input.GetMouseButtonDown(0))
        {
            setPortalB();
        }
        if (Input.GetKey(KeyCode.E))
        {
            openDoor();
        }
        if (Input.GetKey(KeyCode.W))
        {
            PlayerMoveForward();
            isWalking = true;
        }
        if (Input.GetKey(KeyCode.S))
        {
            PlayerMoveBackward();
            isWalking = true;

        }
        if (Input.GetKey(KeyCode.D))
        {
            PlayerMoveRight();
            isWalking = true;

        }
        if (Input.GetKey(KeyCode.A))
        {
            PlayerMoveLeft();
            isWalking = true;

        }
        if (Input.GetKey(KeyCode.Space) && !isJumping)
        {
            PlayerJump();
            isJumping = true;
        }
      
        if (PlayerRB.transform.position == prevoiusPlayerPosition)
        {
            isWalking = false;
        }
        prevoiusPlayerPosition = PlayerRB.transform.position;
        //ZMIANA
        ArenaSpawnHandler.PlayerPosition = transform.position;
    }

  

    private void rotateCamera()
    {
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * mouseSensitivity;
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * mouseSensitivity;

        rotationOnX -= mouseY;
        rotationOnX = Mathf.Clamp(rotationOnX, -90f, 90f);
        PlayerCamera.transform.localEulerAngles = new Vector3(rotationOnX, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);
    }

    private void PlayerMoveForward()
    {
        transform.position += transform.forward * Time.deltaTime * playerSpeed;
    }
    private void PlayerMoveBackward()
    {
        transform.position -= transform.forward * Time.deltaTime * playerSpeed;
    }
    private void PlayerMoveRight()
    {
        transform.Translate(Vector3.right * Time.deltaTime * playerSpeed);
    }
    private void PlayerMoveLeft()
    {
        transform.Translate(-Vector3.right * Time.deltaTime * playerSpeed);
    }
    private void PlayerJump()
    {
        PlayerRB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }


    private void setPortalA()
    {
        var Ray = PlayerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(Ray, out hit))
        {
            targetPosition = hit.point;
            if (hit.collider.gameObject.tag == "Ground")
            {
                Vector3 lookDirection = targetPosition - transform.position;
                var rotation = Quaternion.LookRotation(lookDirection).eulerAngles;
                PortalA.transform.rotation = Quaternion.Euler(0,rotation.y+90f,0);
                Vector3 positionNormalized = new Vector3(targetPosition.x, targetPosition.y + 2f, targetPosition.z);
                PortalA.transform.position = positionNormalized;
            }

        }
    }
    private void setPortalB()
    {
        var Ray = PlayerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(Ray, out hit))
        {
            targetPosition = hit.point;
            if (hit.collider.gameObject.tag == "Ground")
            {
                Vector3 lookDirection = targetPosition - transform.position;
                var rotation = Quaternion.LookRotation(lookDirection).eulerAngles;
                PortalB.transform.rotation = Quaternion.Euler(0, rotation.y + 90f, 0);
                Vector3 positionNormalized = new Vector3(targetPosition.x, targetPosition.y + 2f, targetPosition.z);
                PortalB.transform.position = positionNormalized;
            }


        }
    }

    private void openDoor()
    {
        var Ray = PlayerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(Ray, out hit))
        {
            targetPosition = hit.point;
            if (hit.collider.gameObject.tag == "ArenaSpawner")
            {
                Animator anim = hit.collider.gameObject.GetComponent<Animator>();
                anim.SetBool("isDoorClicked", true);

                ArenaSpawnHandler.SpawnNewArena = true;
            }


        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isJumping = false;
            // ZMIANA
            ArenaSpawnHandler.LastArenaCenter = collision.gameObject.transform.position;
            

        }
        if (collision.gameObject.tag == "PortalA")
        {
            transform.position = SpawnPointB.transform.position;
            transform.rotation = SpawnPointB.transform.rotation;

        }
        if (collision.gameObject.tag == "PortalB")
        {
            transform.position = SpawnPointA.transform.position;
            transform.rotation = SpawnPointA.transform.rotation;

        }
        if (collision.gameObject.tag == "ArenaSpawner")
        {
            ArenaSpawnHandler.SpawnNewArena = true;

        }


    }

}
