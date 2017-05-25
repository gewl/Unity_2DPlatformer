using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour {

    private int currentHealth;
    public int CurrentHealth { get { return currentHealth; } }

    private GameObject heartPrefab;
    protected GameObject[] uiHearts;

    private PlayerController playerController;

    private const string HEART_PREFAB = "Prefabs/UIElements/UIHeart";

	void Start () {

        currentHealth = 3;
        uiHearts = new GameObject[currentHealth];

        heartPrefab = Resources.Load<GameObject>(HEART_PREFAB);

        playerController = GameObject.Find("Player").GetComponent<PlayerController>();

        for (int i = 0; i < currentHealth; i++)
        {
            GameObject uiHeart = Instantiate(heartPrefab, this.transform, false);
            uiHeart.name = "Heart_" + i;
            uiHeart.transform.localPosition = new Vector2(15f + (i * 35f), -15f);

            uiHearts[i] = uiHeart;
        }
    }

    public int DamagePlayer()
    {
        GameObject lastHeart = uiHearts[currentHealth - 1];
        Destroy(lastHeart);
        currentHealth -= 1;
        return currentHealth;
    }

    public void RefreshHealth()
    {
        currentHealth = 3;
        for (int i = 0; i < currentHealth; i++)
        {
            GameObject uiHeart = Instantiate(heartPrefab, this.transform, false);
            uiHeart.name = "Heart_" + i;
            uiHeart.transform.localPosition = new Vector2(15f + (i * 35f), -15f);

            uiHearts[i] = uiHeart;
        }

    }

}
