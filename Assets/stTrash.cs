using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.InputSystem;

public class stTrash : MonoBehaviour
{
    private bool isDraging = false;
    private bool isOnTrashCan = false;
    private bool isOnConveir = false;
    private bool isInAir = true;
    
    private bool isLanding = false;
    private bool isRising = false;
    
    private Camera mainCamera;
    private float CameraZDistance;

    private Dictionary<TrashType, Collision2D> canCollisions;
    Vector2 movement = Vector2.zero;

    [SerializeField] private Rigidbody2D rb = null;
    [SerializeField] float fallSpeed = 1.0f;
    [SerializeField] float conveirSpeed = 2.0f;


    private void Start()
    {
        mainCamera = Camera.main;
        CameraZDistance =
            mainCamera.WorldToScreenPoint(transform.position).z; //z axis of the game object for screen view
    }
    private void FixedUpdate()
    {
        if (isOnConveir)
        {
            movement.x -= Time.fixedDeltaTime * conveirSpeed;
        } else
        {
            movement.x = 0;
        } 

        if (isInAir)
        {
            movement.y -= Time.fixedDeltaTime*fallSpeed;
        } else
        {
            movement.y = 0;
        }
        if (isOnTrashCan)
        {
            
        }
        

        Move();
    }

    private void Move()
    {
        Vector2 clampedMovement = new Vector2((int)movement.x, (int)movement.y);
        if (clampedMovement.magnitude >= 1.0f)
        {
            movement -= clampedMovement;
            if (clampedMovement != Vector2.zero)
            {
                rb.MovePosition(new Vector2(transform.position.x, transform.position.y) + clampedMovement);
            }
        }
    }

    private void OnMouseDown()
    {
        isDraging = true;
    }

    private void OnMouseDrag()
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        mousePos.z = Camera.main.nearClipPlane;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        //Vector2 worldPosVec2 = new Vector2(worldPos.x, worldPos.y);


        Vector3 moveTo = worldPos - transform.position;

        rb.MovePosition(new Vector2(worldPos.x,worldPos.y));
    }
    private void OnMouseUp()
    {
        isDraging = false;
        HandleRelease();
    }

    private void HandleRelease()
    {
        if (!isOnTrashCan && !isOnConveir)
        {
            isInAir = true;
        } else if (isOnTrashCan)
        {
            isRising = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("TrashCan"))
        {
            var trashType = collision.gameObject.GetComponent<CatchTrashCan>().TrashType;
            isOnTrashCan = true;
            isInAir = false;
            canCollisions.Add(trashType, collision);
        }
        else if (collision.collider.CompareTag("Conveir"))
        {
            isOnConveir = true;
            isInAir = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        
        if (collision.collider.CompareTag("TrashCan"))
        {
            var trashType = collision.gameObject.GetComponent<CatchTrashCan>().TrashType;
            canCollisions.Remove(trashType);
            isOnTrashCan = false;
        }
        else if (collision.collider.CompareTag("Conveir"))
        {
            isOnConveir = false;
        }

    }

    private void OnMouseEnter()
    {
        Debug.Log("MouseEntered");
    }
}
