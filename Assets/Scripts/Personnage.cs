using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ElementListing;
using Pathfinding;

public class Personnage : MonoBehaviour
{
    [SerializeField]
    private int m_lifeMax ;
    [SerializeField]
    private int m_life { get; set; }
    private float m_speed { get; set; }
    private float m_speedMalus { get; set; }
    private int m_damageBonus { get; set; }
    private int m_elementResistBonus { get; set; }
    private Dictionary<Elements, int>  m_elementResistance;
    private Dictionary<Status,StatusEffect> m_status; // possède Status,nbStack,duree,degats
    private Elements m_element;
    private int m_level;
    [SerializeField]
    private GameObject m_potion;

    public void SetChara(float speed = 1,int resists = 0, int life = 500, Elements element = Elements.Void, int level = 1)
    {
        this.m_life = life;
        this.m_lifeMax = life;
        m_level = level;
        m_element = element;
        if(gameObject.tag != "Player")
        {
            GetComponent<AIPath>().maxSpeed = speed;
            GetComponentInChildren<SpriteRenderer>().color = GetElementColor(element);
        }
        
        m_elementResistance = new Dictionary<Elements, int>
        {
            { Elements.Biohazard,resists },
            { Elements.Darkness,resists },
            { Elements.Dirt,resists },
            { Elements.Fire,resists },
            { Elements.Ice,resists },
            { Elements.Light,resists },
            { Elements.Lightning,resists },
            { Elements.Nature,resists },
            { Elements.Normal,resists },
            { Elements.Radiation,resists },
            { Elements.Void,resists},
            { Elements.Water,resists }
        };

        m_status = new Dictionary<Status, StatusEffect>
        {
            {Status.burning, new Burning(0,0)},
            {Status.freeze, new Freeze(0,0)},
            {Status.irradiate, new Irradiate(0,0)},
            {Status.poisonned, new Poisonning(0,0)}
        };
    }

    public Elements GetElement()
    {
        return m_element;
    }
    public void TakeDamage(int damage, Elements type) {
        int damageTaken = damage - m_elementResistance[type];
        if (damageTaken < 0)
        {
            damageTaken = 0;
        }

        this.m_life -= damageTaken;
        //life -= damage;
        IsDead();
    }

    public int GetLifeMax()
    {
        return m_lifeMax;
    }

    public void ActiveStatusEffect(Status statusToActivate)
    {
        switch (statusToActivate)
        {
            case Status.burning:
                m_status[Status.burning].StartEffect();
                m_status[Status.burning].isFinallyTheEndEffect += m_status[Status.burning].StopEffect;
                break;
            case Status.freeze:
                m_status[Status.freeze].StartEffect();
                m_status[Status.freeze].isFinallyTheEndEffect += m_status[Status.freeze].StopEffect;
                break;
            case Status.irradiate:
                m_status[Status.irradiate].StartEffect();
                m_status[Status.irradiate].isFinallyTheEndEffect += m_status[Status.irradiate].StopEffect;
                break;
            case Status.poisonned:
                m_status[Status.poisonned].StartEffect();
                m_status[Status.poisonned].isFinallyTheEndEffect += m_status[Status.poisonned].StopEffect;
                break;
        }
    }

    public void SetVitMalus(float speedToSet)
    {
        if (speedToSet <= 0)// Negative value only
        {
            this.m_speedMalus -= speedToSet;
        }
    }

    public int GetLife()
    {
        return this.m_life;
    }

    public float GetVit()
    {
        return this.m_speed;
    }

    public void SetElementResistBonus(int bonusToAdd)
    {
        this.m_elementResistBonus += bonusToAdd;
    }

    public void SetDamageBonus(int bonusToAdd)
    {
        this.m_damageBonus += bonusToAdd;
    }

    public void AddLife(int lifeToAdd)
    {
        this.m_life += lifeToAdd;
        if (this.m_life > m_lifeMax)
        {
            this.m_life = m_lifeMax;
        }
    }

    private void IsDead()
    {
        if (m_life <= 0)
        {
            if(transform.gameObject.tag == "Enemy")
            {
                int luck = Random.Range(0, 100);
                if (luck < 10 + m_level)
                    Instantiate(m_potion).GetComponent<Potion>().SetPower(Random.Range(25,100 + 10 * m_level));
            }

            // bodycount ++ ;  // to gameMaster
            Destroy(this.gameObject);
        }

    }

    /// Status damage burning,freeze,irradiatepoisonned
    
    void Update()
    {
        // Retie un stack au bout de du timer 
        // Si touché a nouveau compte réinitialise le compte au début puis add un stack 
    }

    //todo fin a way to avoid duplication
    private Color GetElementColor(Elements anElement)
    {
        Color aColor = new Color(0, 0, 0);
        switch (anElement)
        {
            case Elements.Biohazard:
                aColor = new Color(1, 0.27f, 0f);
                break;
            case Elements.Darkness:
                aColor = new Color(0.29f, 0f, 0.51f);
                break;
            case Elements.Dirt:
                aColor = new Color(0.55f, 0.27f, 0.07f);
                break;
            case Elements.Fire:
                aColor = new Color(0.55f, 0, 0f);
                break;
            case Elements.Ice:
                aColor = new Color(0.68f, 0.85f, 0.90f);
                break;
            case Elements.Light:
                aColor = new Color(1f, 0.87f, 0.68f);
                break;
            case Elements.Lightning:
                aColor = new Color(0f, 0.75f, 1f);
                break;
            case Elements.Nature:
                aColor = new Color(0, 0.5f, 0);
                break;
            case Elements.Normal:
                aColor = new Color(1f, 1f, 1f);
                break;
            case Elements.Radiation:
                aColor = new Color(1f, 0.84f, 0f);
                break;
            case Elements.Void:
                aColor = new Color(0.5f, 0f, 0.83f);
                break;
            case Elements.Water:
                aColor = new Color(0.12f, 0.56f, 1f);
                break;
        }
        return aColor;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Trap")
            Destroy(this.gameObject);
    }
}
