using UnityEngine;
using UnityEngine.InputSystem;

public class HubMove : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onMove(InputAction.CallbackContext context)
    {
        gameObject.transform.position += new Vector3(context.ReadValue<Vector2>().x, 0, 0);
    }
}
