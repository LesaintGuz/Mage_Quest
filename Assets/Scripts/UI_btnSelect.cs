using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_btnSelect : MonoBehaviour
{
    private spells m_spell;
    private PlayerBehavior m_player;
    private Transform m_spellSpawner;
    [SerializeField]
    private Text[] m_caracsTxt;
    [SerializeField]
    private Image[] m_caracsImg;

    public delegate void BtnSelectEvent();
    public event BtnSelectEvent m_isSelected;

    /// <summary>
    /// Called on button clicked
    /// add the spell linked to the button to the player list
    /// emit a signal to destroy all the other button linked to th chest
    /// </summary>
    public void SelectSpell()
    {
        m_player.AddSpell(m_spell);
        m_isSelected();
    }

    /// <summary>
    /// display spells information on the button
    /// link the spell to a spawner from the player according to it's type
    /// </summary>
    /// <param name="aSpell">spell linked to the button</param>
    public void SetSpell(spells aSpell)
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        m_player = playerObj.GetComponent<PlayerBehavior>();
        m_spell = aSpell;
        if (m_spell.GetSpellType() == "left")
        {
            m_spellSpawner = playerObj.transform.GetChild(0).GetChild(3).GetComponent<Transform>();
        }
        else if (m_spell.GetSpellType() == "right")
        {
            m_spellSpawner = playerObj.transform.GetChild(0).GetChild(2).GetComponent<Transform>();
        }
        else if (m_spell.GetSpellType() == "back")
        {
            m_spellSpawner = playerObj.transform.GetChild(0).GetChild(1).GetComponent<Transform>();
        }
        else
        {
            m_spellSpawner = playerObj.transform.GetChild(0).GetChild(0).GetComponent<Transform>();
        }
        m_spell.SetSpawnerTransform(m_spellSpawner);


        GetComponent<Image>().color = GetButtonColorAccordingToRarirty();
        m_caracsTxt[0].text = "Dmg : " + m_spell.GetDamage().ToString();
        m_caracsTxt[1].text = "Spd : " + m_spell.GetSpeed().ToString();
        m_caracsTxt[2].text = "Scale : " + m_spell.GetScale().ToString();
        m_caracsTxt[3].text = "Pi : " + m_spell.GetPierce().ToString();
        m_caracsTxt[4].text = "Col : " + m_spell.GetCoolDown().ToString();
        m_caracsTxt[5].text = "NbPr : " + m_spell.GetNbProjectile().ToString();
        m_caracsTxt[6].text = m_spell.GetSpellType();
        m_caracsTxt[7].text = m_spell.GetName();
        m_caracsImg[0].sprite = m_spell.GetShapeImg();
        m_caracsImg[0].color = m_spell.GetSpellColor();
        m_caracsImg[1].sprite = Resources.Load<Sprite>(m_spell.GetElementIconName());

        /*GameObject obj = Resources.Load<GameObject>(m_spell.getElement().ToString());
        caracsImg[1].sprite = obj.GetComponent<SpriteRenderer>().sprite;*/
    }

    /// <summary>
    /// Get a button color according  to the spell rarity
    /// buttons can be brown (common), blue (rare), purple (legendary)
    /// </summary>
    /// <returns>Color, spell color</returns>
    private Color GetButtonColorAccordingToRarirty()
    {
        Color buttonColor = new Color(0, 0, 0);
        switch (m_spell.GetSpellRarity())
        {
            case 0:
                buttonColor = new Color(0.56f, 0.47f, 0.3f);
                break;
            case 1:
                buttonColor = new Color(0.14f, 0.81f, 0.89f);
                break;
            case 2:
                buttonColor = new Color(0.84f, 0.14f, 0.89f);
                break;
        }
        return buttonColor;
    }
}
