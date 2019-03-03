using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class PlayerController : MonoBehaviour
{
    public delegate void PlaceBlock(Vector3 position);
    public static event PlaceBlock OnPlaceBlock;

    public delegate void DestroyBlock();
    public static event DestroyBlock OnDestroyBlock;

    //Movement Variables
    [Header("Player Movement")]
    public float playerMoveSpeed = 5;
    public float playerRotationSpeed = 90;
    public Vector2 cameraYRotationLimits;
    private Rigidbody theRB;
    public float jumpForce = 50;

    //Block Creation Variables
    [Header("Block Creation")]
    public PhysicMaterial wallPhysicsMat;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        theRB = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //Player Movement Input
        if ((Input.GetAxis("Vertical") != 0) || (Input.GetAxis("Horizontal") != 0))
        {
            MovePlayer();
        }

        if ((Input.GetAxis("Mouse X") != 0) || (Input.GetAxis("Mouse Y") != 0))
        {
            RotatePlayer();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayerJump();
        }

        //Player Block Control Inputs
        if (Input.GetMouseButtonDown(0))
        {
            CreateBlock();
        }

        if (Input.GetMouseButtonDown(1))
        {
            RemoveBlock();
        }

        //Respawning Player if they fall out of bounds
        if (this.transform.position.y <= -10)
        {
            Respawn();
        }
    }


    /*
    ==================================================
    Player Movement
    ==================================================
    */
    private void MovePlayer()
    {
        Vector3 moveDirection = Vector3.zero;

        if (Input.GetAxis("Vertical") != 0)
        {
            moveDirection += transform.forward * Input.GetAxis("Vertical");
        }
        if (Input.GetAxis("Horizontal") != 0)
        {
            moveDirection += transform.right * Input.GetAxis("Horizontal");
        }

        moveDirection.Normalize();

        Vector3 newPosition = this.transform.position + (moveDirection * playerMoveSpeed * Time.deltaTime);
        theRB.MovePosition(newPosition);
    }

    private void RotatePlayer()
    {
        if (Input.GetAxis("Mouse X") != 0)
        {
            float inputX = Input.GetAxis("Mouse X");
            inputX = Mathf.Clamp(inputX, -1, 1);

            //Affects The Parent Player GameObject
            Vector3 newPlayerRotation = Vector3.zero;
            newPlayerRotation.y += inputX * playerRotationSpeed * Time.deltaTime;

            this.transform.Rotate(newPlayerRotation);
        }

        if (Input.GetAxis("Mouse Y") != 0)
        {
            float inputY = Input.GetAxis("Mouse Y");
            inputY = Mathf.Clamp(inputY, -1, 1);

            //Affects The Parent Player GameObject
            Vector3 newPlayerRotation = Vector3.zero;
            newPlayerRotation.x -= inputY * playerRotationSpeed * Time.deltaTime;

            Camera.main.transform.Rotate(newPlayerRotation);

            //Limiting Rotation
            Vector3 setRotation = Camera.main.transform.localEulerAngles;
            if (setRotation.x > 180)
            {
                setRotation.x -= 360;
            }

            if (setRotation.x > cameraYRotationLimits.y)
            {
                setRotation.x = cameraYRotationLimits.y;
            }
            else if (setRotation.x < cameraYRotationLimits.x)
            {
                setRotation.x = cameraYRotationLimits.x;
            }
            Camera.main.transform.localEulerAngles = setRotation;
        }
    }

    private void PlayerJump()
    {
        RaycastHit hit;
        Ray ray = new Ray
        {
            direction = Vector3.down,
            origin = this.transform.position
        };

        if (Physics.Raycast(ray, out hit))
        {
            float distanceToGround = Vector3.Distance(this.transform.position, hit.point);
            if (distanceToGround <= 1)
            {
                theRB.AddForce(Vector3.up * jumpForce);
            }
        }
    }

    private void Respawn()
    {
        Debug.Log("Respawning Player");
        this.transform.position = new Vector3(0, 8, 0);
    }


    /*
    ==================================================
    Player Block Handling
    ==================================================
    */
    private void CreateBlock()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        if (Physics.Raycast(ray, out hit))
        {
            GameObject primitive = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Vector3 primitivePosition = hit.point + (hit.normal * 0.5f);
            primitivePosition.x = (Mathf.Floor(primitivePosition.x) + 0.5f);
            primitivePosition.y = (Mathf.Floor(primitivePosition.y) + 0.5f);
            primitivePosition.z = (Mathf.Floor(primitivePosition.z) + 0.5f);

            primitive.transform.position = primitivePosition;
            primitive.tag = "Test";

            primitive.GetComponent<BoxCollider>().material = wallPhysicsMat;

            Debug.Log("Created Test Block");
        }
    }

    private void RemoveBlock()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        if (Physics.Raycast(ray, out hit))
        {
            GameObject primitive = hit.transform.gameObject;
            if (primitive.tag == "Test")
            {
                Destroy(primitive);
            }
        }
    }
}
