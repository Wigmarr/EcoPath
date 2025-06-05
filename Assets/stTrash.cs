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
    [SerializeField] float tossTime = 1.0f;
    float tossSpeedX = 0f;
    float tossSpeedY = 0f;
    [SerializeField] SpriteRenderer spriteRenderer = null;
    [SerializeField] TrashType type = TrashType.Unsorted;
    private void Start()
    {

        mainCamera = Camera.main;
        CameraZDistance =
            mainCamera.WorldToScreenPoint(transform.position).z; //z axis of the game object for screen view
    }
    private void FixedUpdate()
    {
        if (canCollisions.Count == 0)
        {
            isOnTrashCan = false;
            isRising = false;
            isLanding = false;
            tossSpeedX = 0;
            tossSpeedY = 0;
            closesetTrashCan = null;
        }

        if (isDraging)
        {
            OnMouseDrag();
            return;
        }


        if (isLanding || isRising)
        {
            JumpToTrashCan();
            Move();
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
        
        Move();

    }

    private void JumpToTrashCan()
    {

        var targetPos = closesetTrashCan.transform.position;
        if (!isLanding && !isRising)
        {
            isRising = true;
            tossSpeedX = (targetPos - transform.position).x / tossTime;
            tossSpeedY = ((targetPos - transform.position).y +2.5f*2 )/ tossTime;
        }
        
        if (isRising)
        {
            if (transform.position.y > 2.5 + closesetTrashCan.transform.position.y)
            {
                isRising = false;
                isLanding = true;
                spriteRenderer.sortingLayerID = SortingLayer.NameToID("BackGround");
                
            } else
            {
                movement.y = Time.fixedDeltaTime * tossSpeedY;
            }
        }
        
        if (isLanding )
        {
            if (Mathf.Abs((transform.position - closesetTrashCan.transform.position).magnitude) <= 1)
            {
                
                Destroy(gameObject);
                return;
            }
            movement.y -= Time.fixedDeltaTime*tossSpeedY;
        }
        movement.x = Time.fixedDeltaTime * tossSpeedX;
    }

    private void Move()
    {
        Vector2 clampedMovement = new Vector2(movement.x, movement.y);
        //if (clampedMovement.magnitude >= 0.0625)
        //{
            movement -= clampedMovement;
            if (clampedMovement != Vector2.zero)
            {
                rb.MovePosition(new Vector2(transform.position.x, transform.position.y) + clampedMovement);
            }
        //}
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
        if (!isOnTrashCan && !isOnConveir && !isRising && !isLanding)
        {

            isInAir = true;
        } else if (isOnTrashCan)
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
            isRising = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("TrashCan"))
        {
        Debug.Log("EnterTrash");
            Debug.Log("isLanding: " + isLanding);
            Debug.Log("isRising: " + isRising);
            Debug.Log("isInAir: " + isInAir);
            var trashType = collision.gameObject.GetComponent<SortTrashCan>().TrashType;
            isOnTrashCan = true;
            //isInAir = false;
            canCollisions.Add(trashType, collision);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("TrashCan"))
        {
            Debug.Log("ExitTrash");

            var trashType = other.gameObject.GetComponent<SortTrashCan>().TrashType;
            canCollisions.Remove(trashType);
            Debug.Log("isLanding: " + isLanding);
            Debug.Log("isRising: " + isRising);
            Debug.Log("isInAir: " + isInAir);
            if (canCollisions.Count == 0)
            {
                //isOnTrashCan = false;
                //if (!isDraging && !isOnConveir )
                //{
                //    isInAir = true;
                //}
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
       if (collision.collider.CompareTag("Conveir"))
        {
            isOnConveir = true;
            isInAir = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log("ExitCollision");

       
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
