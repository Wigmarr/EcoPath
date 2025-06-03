using UnityEditor.SearchService;
using UnityEngine;

public class TrashDispenser : MonoBehaviour
{
    [SerializeField] private GameObject[] trashObjects;
    [SerializeField] private float spawnRate = 1f;
    [SerializeField] private Transform[] points;
    [SerializeField] private CatchTheTrashGameManager gameManager;
    private float timer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = spawnRate;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = spawnRate;
            int trashIndx = Random.Range(0, trashObjects.Length);
            float posX = Random.Range(transform.position.x + points[0].localPosition.x, transform.position.x + points[1].localPosition.x);
            Vector3 pos = new Vector3(posX, transform.position.y, transform.position.z);
            GameObject obj = Instantiate(trashObjects[trashIndx], pos, transform.rotation);
            obj.transform.Rotate(0, 0, Random.Range(-180, 180));
            obj.GetComponent<Rigidbody2D>().gravityScale = obj.GetComponent<Rigidbody2D>().gravityScale + gameManager.CurLevel*0.1f;
        }
    }


    private void FixedUpdate()
    {
           
    }
}
