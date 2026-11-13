using System.Collections;
using System.Data;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public static PlayerStatus Instance { get; set; }
    public float currentHealth;
    public float maxHealth;
    public float currentCalories;
    public float maxCalories;
    float distantceTravelled = 0;
    Vector3 lastPosition;
    public GameObject playerBody;
    public float currentHydrationPercent;
    public float maxHydrationPercent;
    public bool isHydrationAvtive;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    private void Start()
    {
        currentHealth = maxHealth;
        currentCalories = maxCalories;
        currentHydrationPercent = maxHydrationPercent;
        StartCoroutine(decreaseHydration());
    }
   IEnumerator decreaseHydration()
    {
        while (true)
        {
            yield return new WaitForSeconds(10);
            currentHydrationPercent -= 1;
        }
    }
    void Update()
    {

        distantceTravelled += Vector3.Distance(playerBody.transform.position, lastPosition);
        lastPosition = playerBody.transform.position;
        if (distantceTravelled >= 5)
        {
            currentCalories -= 1;
            distantceTravelled = 0;
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            currentHealth -= 10f;
        }
    }
    public void setHealth(float value)
    {
        currentHealth = Mathf.Clamp(value, 0f, maxHealth);
    }

    public void setCalories(float value)
    {
        currentCalories = Mathf.Clamp(value, 0f, maxCalories);
    }

    public void setHydration(float value)
    {
        currentHydrationPercent = Mathf.Clamp(
            value, 0f, maxHydrationPercent
        );
    }
}
