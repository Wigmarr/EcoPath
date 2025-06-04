using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.U2D;
using UnityEngine.UIElements;

public class stTrash : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool isDraging = false;
    private bool isOnTrashCan = false;
    private bool isOnConveir = false;
    private bool isInAir = true;
    
    private bool isLanding = false;
    private bool isRising = false;
    

    private Camera mainCamera;
    private float CameraZDistance;

    private Dictionary<TrashType, Collider2D> canCollisions = new Dictionary<TrashType, Collider2D>();
    Vector2 movement = Vector2.zero;
    private Collider2D closesetTrashCan;
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
        if (canCollisions.Count< 0)
        {
            isOnTrashCan = false;
        }
        if (isDraging)
        {
            OnMouseDrag();
            return;
        }

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
            if (!isLanding && !isRising)
            {
                float minDistance = 10000;

                foreach (var collider in canCollisions)
                {

                    float currDistance = (collider.Value.gameObject.transform.position - transform.position).magnitude;
                    if (currDistance < minDistance)
                    {
                        closesetTrashCan = collider.Value;
                    }
                }
            }
            JumpToTrashCan();
            
        }
        Move();

    }

    private void JumpToTrashCan()
    {
        if (!isLanding && !isRising)
        {
            isRising = true;
        }
        
        if (isRising)
        {
            if (transform.position.y > 2.5 + closesetTrashCan.transform.position.y)
            {
                isRising = false;
                isLanding = true;
            }
        }
        if (isLanding && (transform.position - closesetTrashCan.transform.position).magnitude <= 0.001)
        {
            Destroy(gameObject);
        }




    }

    private void Move()
    {
        Vector2 clampedMovement = new Vector2(movement.x, movement.y);
        if (clampedMovement.magnitude >= 0.0625)
        {
            movement -= clampedMovement;
            if (clampedMovement != Vector2.zero)
            {
                rb.MovePosition(new Vector2(transform.position.x, transform.position.y) + clampedMovement);
            }
        }
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("TrashCan"))
        {
            var trashType = collision.gameObject.GetComponent<CatchTrashCan>().TrashType;
            isOnTrashCan = true;
            isInAir = false;
            canCollisions.Add(trashType, collision);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("TrashCan"))
        {
            var trashType = other.gameObject.GetComponent<CatchTrashCan>().TrashType;
            canCollisions.Remove(trashType);
            isOnTrashCan = false;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("EnteredCollision");
        
       if (collision.collider.CompareTag("Conveir"))
        {
            isOnConveir = true;
            isInAir = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log("EnteredCollision");

       
        if (collision.collider.CompareTag("Conveir"))
        {
            isOnConveir = false;
            isInAir = true;
        }

    }

    private void OnMouseEnter()
    {
        Debug.Log("MouseEntered");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDraging = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDraging = false;
        HandleRelease();
    }
}
