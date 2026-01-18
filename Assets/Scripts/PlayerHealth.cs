using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Healthdefaultthing")]
    [SerializeField] private int HealthMax=100;
    [SerializeField] private Slider HealthSlider;

    [SerializeField] private SpriteRenderer Player;

    private int CurrentHealth;
    public int CurrentHealthA=>CurrentHealth;
    public int HealthMaxA=>HealthMax;


    void Start()
    {
        CurrentHealth=HealthMax;
        UpdateHealthUI();

    }

    void UpdateHealthUI()
    {
        if(HealthSlider!=null)
        {
            HealthSlider.maxValue=HealthMax;
            HealthSlider.value=CurrentHealth;
        }
    }

    void Die()
    {
        Debug.Log("DIED");
        //temp until i put the game over screen :p
    }

    public void TakeDamage(int damage)
    {
        if (CurrentHealth<=0)
        {
            return;
        }

        CurrentHealth-=damage;
        CurrentHealth=Mathf.Max(0,CurrentHealth);

        UpdateHealthUI();

        if(CurrentHealth<=0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        CurrentHealth=Mathf.Min(CurrentHealth+amount,HealthMax);
        UpdateHealthUI();

    }
}
