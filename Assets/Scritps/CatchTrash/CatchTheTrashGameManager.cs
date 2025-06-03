using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class CatchTheTrashGameManager : MonoBehaviour
{
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private int LifeCounter = 0;
    [SerializeField] private float LevelIncreaseTime = 10f;
    [SerializeField] private int currentLevel = 0;
    private float levelIncreaseTimer = 0f;

    public int CurLevel { get => currentLevel; private set => currentLevel = value; }

    private void Update()
    {
        levelIncreaseTimer += Time.deltaTime;
        if (levelIncreaseTimer >= LevelIncreaseTime)
        {
            CurLevel++;
            levelIncreaseTimer = 0f;
        }
    }

    public void LoseLife()
    {
        LifeCounter--;
        healthBar.OnLoseHealth(LifeCounter);
        if (LifeCounter <= 0 )
        {
            SceneManager.LoadScene(0);
        }
    }
}
