using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    private void Awake()
    {
    }
    void Start()
    {
        // “еперь, провер€ем существование экземпл€ра
        if (instance == null)
        { // Ёкземпл€р менеджера был найден
            instance = this; // «адаем ссылку на экземпл€р объекта
        }
        else if (instance == this)
        { // Ёкземпл€р объекта уже существует на сцене
            Destroy(gameObject); // ”дал€ем объект
        }
        GameObject[] objs = GameObject.FindGameObjectsWithTag("GameManager");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void onScene1()
    {
        SceneManager.LoadScene(0);
    }
    public void onScene2()
    {
        SceneManager.LoadScene(1);
    }
    public void onScene3()
    {
        SceneManager.LoadScene(2);
    }

    public void onScene4()
    {
        SceneManager.LoadScene(3);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
