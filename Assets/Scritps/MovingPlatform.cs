using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform[] points;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float moveTime = 1f;
    private float timeLeft;
    private Transform currentDest;
    private int nextIndex = 0;
    private float threshold = 0.5f;
    

    // Start is called before the first frame update
    void Start()
    {
        setNextDest();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        timeLeft -= Time.deltaTime;
        float dist = Vector2.Distance(currentDest.position, rb.position);
        float speed = dist / timeLeft;
        if (dist > threshold)
        {
            //rb.AddForce(new Vector2(currentDest.position.x - rb.position.x, currentDest.position.y - rb.position.y).normalized * speed),);
            rb.linearVelocity = new Vector2(currentDest.position.x - rb.position.x, currentDest.position.y - rb.position.y).normalized * speed;
        } else
        {
            setNextDest();
        }

        //rb.AddForce(new Vector2(currentDest.position.x - rb.position.x, currentDest.position.y - rb.position.y).normalized * speed);
    }

    void setNextDest()
    {
        currentDest = points[nextIndex];
        nextIndex = (nextIndex + 1) % points.Length;
        timeLeft = moveTime;

    }
}
