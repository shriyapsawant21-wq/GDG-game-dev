using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Config")]
    [SerializeField] private int HealthMax = 100;
    [SerializeField] private Slider HealthSlider;

    private int CurrentHealth;

    void Start()
    {
        CurrentHealth = HealthMax;
        UpdateHealthUI();
    }

    public void UpdateHealthUI()
    {
        if (HealthSlider != null)
        {
            HealthSlider.maxValue = HealthMax;
            HealthSlider.value = CurrentHealth;
        }
    }

    public void TakeDamage(int damage)
    {
        if (CurrentHealth <= 0) return;

        CurrentHealth -= damage;
        CurrentHealth = Mathf.Max(0, CurrentHealth);
        UpdateHealthUI();

        if (CurrentHealth <= 0) Die();
    }

    void Die()
    {
        Debug.Log("DIED");
        if (GameManager.Instance != null)
            GameManager.Instance.GameOver();
    }
}