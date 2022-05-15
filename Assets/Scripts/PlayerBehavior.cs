using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// inherit from Personnage class
/// </summary>
public class PlayerBehavior : Personnage
{
    [SerializeField]
    private List<spells> m_spells = new List<spells>(); 
    private Rigidbody2D m_rb;
    Vector2 m_lookDirection = new Vector2(1, 0);
    [SerializeField]
    private Transform m_spawner;
    [SerializeField]
    private int m_movespeed;

    private Animator m_anim;

    /// <summary>
    /// Init a player
    /// </summary>
    private void Start()
    {
        m_rb = gameObject.GetComponent<Rigidbody2D>();
        m_anim = GetComponent<Animator>();
        spells aSpell = new spells("autoAtt");
        aSpell.SetSpawnerTransform(transform.GetChild(0).GetComponent<Transform>());
        aSpell.StartTimer();
        m_spells = new List<spells> { aSpell };
        SetChara(m_movespeed, 0, GetLifeMax());
    }

    /// <summary>
    /// define player movement and rotation according to the keys pressed
    /// </summary>
    public void Movement()
    {
        // défini les input
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector2 move = new Vector2(horizontal, vertical);

        m_anim.SetFloat("Horizontal", horizontal);
        m_anim.SetFloat("Vertical", vertical);
        m_anim.SetFloat("Speed", move.magnitude);

        // récupère la direction du personnage 
        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            m_lookDirection.Set(move.x, move.y);
            m_lookDirection.Normalize();
        }

        // recup la position actuel 
        Vector2 characterPosition = m_rb.position;

        //changement de position lors de l'activation des input 
        if(vertical == 0)
        {
            characterPosition.x = characterPosition.x + m_movespeed * horizontal * Time.deltaTime;
        }
        else
        {
            characterPosition.x = characterPosition.x + m_movespeed * horizontal *0.75f * Time.deltaTime;
        }
        if(horizontal == 0)
        {
            characterPosition.y = characterPosition.y + m_movespeed * vertical * Time.deltaTime;
        }
        else
        {
            characterPosition.y = characterPosition.y + m_movespeed * vertical * 0.75f * Time.deltaTime;
        }
        
        //affecte au personnage sa nouvelle position 
        m_rb.MovePosition(characterPosition);
        
        //rotate spawner in order that the face spawner must be in front of the player
        if(move.x == 0)
        {
            if(move.y < 0)
            {
                m_spawner.rotation = Quaternion.Euler(new Vector3(0,0,0));
            }
            else if (move.y > 0)
            {
                m_spawner.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
            }
        }else
        {
            if (move.x > 0)
            {
                m_spawner.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
            }
            else if (move.x < 0)
            {
                m_spawner.rotation = Quaternion.Euler(new Vector3(0, 0, 270));
            }
        }
    }

    /// <summary>
    /// move the player every fixedUpdate
    /// </summary>
    private void FixedUpdate()
    {
        Movement();
    }

    /// <summary>
    /// Add a new spell to the player's list
    /// </summary>
    /// <param name="aSpell"> new spell wich will be triggered automatically</param>
    public void AddSpell(spells aSpell)
    {
        m_spells.Add(aSpell);
        aSpell.StartTimer();
    }

    /// <summary>
    /// Check against which object the player enter in collision
    /// if it's an enemy, player loose life
    /// </summary>
    /// <param name="collision">object that the tag must be check</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            TakeDamage(10, collision.gameObject.GetComponent<Personnage>().GetElement());
        }

        if (collision.gameObject.tag == "Trap")
            Destroy(this.gameObject);
    }
}
