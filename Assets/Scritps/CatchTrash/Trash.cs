using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines.Interpolators;
using UnityEngine.UIElements;

public enum TrashType
{
    Plastic,
    Paper,
    Food,
    Metal,
    Unsorted,
    Glass
}

public class Trash : MonoBehaviour
{
    [SerializeField] private TrashType type = TrashType.Plastic;

    internal TrashType Type { get => type; private set => type = value; }
    [SerializeField] private float lerpTimer = 1f;
    private float currentTime = 0;
    private int currentDirection = 1;
    private float lerpCoeff = 10;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        currentTime += Time.deltaTime * currentDirection*lerpCoeff;
        transform.localScale = Vector3.Lerp(new Vector3(0.97f, 0.97f, 1), new Vector3(1.03f, 1.03f, 1), currentTime / lerpTimer);
        if (currentTime > lerpTimer || currentTime < 0)
        {
            currentDirection *= -1;
        }
    }


}
