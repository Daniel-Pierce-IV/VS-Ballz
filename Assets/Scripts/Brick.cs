using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Brick : MonoBehaviour, IPoolable<Brick>
{
    [SerializeField] private Text hitpointText;

    private int hitpoints;
    private GameObjectPool<Brick> pool;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        TakeDamage();
    }

    private void TakeDamage()
    {
        hitpoints--;
        RefreshHitpointText();
        if (hitpoints <= 0) DeactivateSelf();
    }

    public void RefreshHitpointText()
    {
        hitpointText.text = hitpoints.ToString();
    }

    public void SetHitpoints(int value)
    {
        hitpoints = value;
        RefreshHitpointText();
    }

    public void SetPool(GameObjectPool<Brick> pool)
    {
        if(this.pool == null) this.pool = pool;
    }

    private void DeactivateSelf()
    {
        hitpoints = 0;
        pool.ReturnObject(this);
        this.gameObject.SetActive(false);
    }
}