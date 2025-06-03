using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image[] healths;
    [SerializeField] private Sprite fullHealth;
    [SerializeField] private Sprite emptHealth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (var health in healths)
        {
            health.sprite = fullHealth;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnLoseHealth(int currentHealth)
    {
        if (healths.Length < currentHealth) return;
;
        healths[currentHealth].sprite =  emptHealth;
    }
}
