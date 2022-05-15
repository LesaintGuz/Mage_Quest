using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ElementListing;


public class spells : MonoBehaviour
{
    [SerializeField]
    private string m_name;
    [SerializeField]
    private float m_cooldown = 13.5f;
    [SerializeField]
    private int m_damage = 40;
    [SerializeField]
    private int m_bulletPierce = 1;
    [SerializeField]
    private string m_spellShape;
    [SerializeField]
    private int m_projectileSpeed = 2;
    [SerializeField]
    private int m_nbProjectile = 1;
    [SerializeField]
    private int m_scale = 1;
    [SerializeField]
    private string m_spellType;
    [SerializeField]
    private Elements m_anElement;

    private int m_spellRarity;

    [SerializeField]
    private Transform m_spawnerTransform;
    [SerializeField]
    private bool m_isEnvironmental = false;
    private timerCZ m_timer;


    /* public spells(string name, float cooldown, int damage, int bulletPierce, string spellType, string spellShape, int projectileSpeed, int scale, Elements enElement)
     {
         m_name = name;
         m_cooldown = cooldown;
         m_damage = damage;
         m_bulletPierce = bulletPierce;
         m_spellType = spellType;
         m_spellShape = spellShape;
         m_projectileSpeed = projectileSpeed;
         m_scale = scale;
         m_anElement = enElement;

         m_currentTime = m_cooldown;
     }*/

    /// <summary>
    /// If the spell come from an enemy or the environment
    /// it must be init in the start function
    /// </summary>
    public void Start()
    {
        if (m_isEnvironmental)
            StartTimer();
    }

    /// <summary>
    /// Function used to set first player's spell
    /// </summary>
    /// <param name="name">do nothing if name is different from "autoAtt" (first Player's spell)</param>
    public spells(string name = "defaut")
    {
        if (name == "autoAtt")
        {
            m_cooldown = 2.0f;
            m_anElement = Elements.Normal;
            m_spellType = "front";
            m_spellShape = "Spear";
            m_damage = 40;
        }
        /*m_timer = new timerCZ(m_cooldown);
        m_timer.isFinallyTheEnd += cast;
        m_timer.startTimer();*/

    }

    /// <summary>
    /// Set caracteristic of an environmental spell (trap that continuously spawn a bullet)
    /// it's a fire bullet destroyed when it hits 1 personnage
    /// </summary>
    /// <param name="cooldwon">amount of time between each bullet</param>
    /// <param name="damage">bullet's damage</param>
    /// <param name="projSpeed">bullet's move speed</param>
    /// <param name="scale">bullet's scale</param>
    public void SetEnvironmentalSpell(float cooldwon, int damage, int projSpeed, int scale)
    {
        m_isEnvironmental = true;
        m_name = "ArrowSpawner";
        m_cooldown = cooldwon;
        m_damage = damage;
        m_bulletPierce = 1;
        m_spellShape = "Arrow";
        m_projectileSpeed = projSpeed;
        m_scale = scale;
        m_spellType = "back";
        m_anElement = Elements.Fire;
    }

    /// <summary>
    /// Set up bullet timer and start it
    /// </summary>
    public void StartTimer()
    {
        m_timer = new timerCZ(m_cooldown);
        m_timer.m_isFinallyTheEnd += Cast;
        m_timer.StartTimer();
    }

    /// <summary>
    /// Function called at the end of a timer
    /// Cast the spell
    /// restart the timer when spell is cast
    /// </summary>
    public void Cast()
    {
        //bullet gameObject is load from spell shape, so they musn't be moved and their name musn't be changed without modifying spell generator class
        GameObject spellShape;
        if (m_spellType != "aoe")
        {
            spellShape = Resources.Load<GameObject>(m_spellShape);
        }
        else
        {
            spellShape = Resources.Load<GameObject>("puddle");
        }

        Vector3 rotation = m_spawnerTransform.rotation.eulerAngles;
        rotation.z -= 40.0f;
        GameObject bullet;
        Transform spawner = m_spawnerTransform;
        for (int i = 0; i < m_nbProjectile; i++)
        {
            //if there is more than 1 bullet spawned by the spell, bullets are spawned from differant angle than the transformed one
            if (m_nbProjectile > 1)
            {
                rotation.z += 80.0f / m_nbProjectile;
            }
            else
            {
                rotation.z += 40.0f;
            }

            if (m_spellType == "autoAim" || m_spellType == "turnAround")
            {
                //spawn bullet all around the player, not near from a specific direction
                float angle = i * 2 * Mathf.PI / m_nbProjectile;
                spawner.position = FindObjectOfType<PlayerBehavior>().transform.position + new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0) * 1.0f;
            }

            bullet = Instantiate(spellShape, spawner.position, Quaternion.Euler(rotation));
            bullet.GetComponent<Bullet>().SetBullet(m_spellType, m_projectileSpeed, m_damage, m_bulletPierce, m_anElement, m_isEnvironmental);
            bullet.GetComponent<SpriteRenderer>().color = GetSpellColor();
        }
        m_timer.StartTimer();
    }

    /// <summary>
    /// Get a spell color according to it's element
    /// </summary>
    /// <returns></returns>
    public Color GetSpellColor()
    {
        Color aColor = new Color(0, 0, 0);
        switch (m_anElement)
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

    /// <summary>
    /// get the Icon associated to the spell's element
    /// </summary>
    /// <returns></returns>
    public string GetElementIconName()
    {
        string iconName = "UI/ElementIcon/";
        switch (m_anElement)
        {
            case Elements.Biohazard:
                iconName += "BioHazard";
                break;
            case Elements.Darkness:
                iconName += "DarkSasuke";
                break;
            case Elements.Dirt:
                iconName += "Dirt";
                break;
            case Elements.Fire:
                iconName += "Feuje";
                break;
            case Elements.Ice:
                iconName += "Freeze";
                break;
            case Elements.Light:
                iconName += "Light";
                break;
            case Elements.Lightning:
                iconName += "Lighting";
                break;
            case Elements.Nature:
                iconName += "Nature";
                break;
            case Elements.Normal:
                iconName += "Poison";
                break;
            case Elements.Radiation:
                iconName += "Radiation";
                break;
            case Elements.Void:
                iconName += "Void";
                break;
            case Elements.Water:
                iconName += "Water";
                break;
        }
        //iconName += ".png";
        return iconName;
    }

///////////////////////////////////////////////Get set functions\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
public void SetName(string name)
    {
        m_name = name;
    }

    public string GetName()
    {
        return m_name;
    }

    public void SetSpawnerTransform(Transform spawnerTransform)
    {
        m_spawnerTransform = spawnerTransform;
    }

    public void RetrieveCooldown(float cooldown)
    {
        m_cooldown -= cooldown;
        if (m_cooldown < 0.5f)
            m_cooldown = 0.5f;
    }

    public void AddDamage(int damage)
    {
        m_damage += damage;
    }

    public void AddPierce(int pierce)
    {
        m_bulletPierce += pierce;
    }

    public void AddProjectileSpeed(int projectileSpeed)
    {
        m_projectileSpeed += projectileSpeed;
    }

    public void AddScale(int scale)
    {
        m_scale += scale;
    }

    public void AddProjectile(int nbProjectile)
    {
        m_nbProjectile += nbProjectile;
    }

    public void SetSpellRarity(int spellRarity)
    {
        m_spellRarity = spellRarity;
    }

    public void SetSpellType(string spellType)
    {
        m_spellType = spellType;
    }

    public void SetSpellShape(string spellShape)
    {
        m_spellShape = spellShape;
    }

    public void SetElement(Elements element)
    {
        m_anElement = element;
    }

    public int GetDamage()
    {
        return m_damage;
    }

    public int GetPierce()
    {
        return m_bulletPierce;
    }

    public int GetScale()
    {
        return m_scale;
    }

    public int GetSpeed()
    {
        return m_projectileSpeed;
    }

    public int GetNbProjectile()
    {
        return m_nbProjectile;
    }

    public float GetCoolDown()
    {
        return m_cooldown;
    }

    public Elements GetElement()
    {
        return m_anElement;
    }

    public string GetSpellType()
    {
        return m_spellType;
    }

    public int GetSpellRarity()
    {
        return m_spellRarity;
    }

    public Sprite GetShapeImg()
    {
        GameObject obj;
        if (m_spellType != "aoe")
        {
            obj = Resources.Load<GameObject>(m_spellShape);
        }
        else
        {
            obj = Resources.Load<GameObject>("puddle");
        }

        return obj.GetComponent<SpriteRenderer>().sprite;
    }
}