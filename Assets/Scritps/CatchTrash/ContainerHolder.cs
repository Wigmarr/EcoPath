using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ContainerHolder : MonoBehaviour
{

    [SerializeField] private CatchTrashCan trashCanPlastic;
    [SerializeField] private CatchTrashCan trashCanPaper;
    [SerializeField] private CatchTrashCan trashCanFood;
    private CatchTrashCan currentTrashCan;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentTrashCan = trashCanPlastic;
        ChangeTrashCan(TrashType.Plastic);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void SetPlastic()
    {
        ChangeTrashCan(TrashType.Plastic);
    }

    public void SetFood()
    {
        ChangeTrashCan(TrashType.Food);
    }

    public void SetPaper()
    {
        ChangeTrashCan(TrashType.Paper);
    }

    void ChangeTrashCan(TrashType type)
    {
        currentTrashCan.gameObject.SetActive(false);
        switch (type)
        {
            case TrashType.Paper:
                currentTrashCan = trashCanPaper;
                break;
            case TrashType.Food:
                currentTrashCan = trashCanFood;
                break;
            case TrashType.Plastic:
                currentTrashCan = trashCanPlastic;
                break;
        }
        currentTrashCan.gameObject.SetActive(true);

    } 
}
