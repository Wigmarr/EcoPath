using Unity.VisualScripting;
using UnityEngine;

public class CatchTrashCan : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private TrashType trashType;
    [SerializeField] private CatchTheTrashGameManager gameManager;

    public TrashType TrashType { get => trashType; private set => trashType = value; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        FlipContainer();
    }

    private void FlipContainer()
    {
        if (transform.parent != null)
        {

            if (transform.parent.rotation.y < 0 && spriteRenderer.flipX == false)
            {
                spriteRenderer.flipX = true;
            }
            else if (transform.parent.rotation.y >= 0 && spriteRenderer.flipX == true)
            {
                spriteRenderer.flipX = false;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Trash"))
        {

            Destroy(collision.gameObject);
            if (TrashType != collision.GetComponent<Trash>().Type) {
                gameManager.LoseLife();
            }
        }
    }
}
