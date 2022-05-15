using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Looter : MonoBehaviour
{
    [SerializeField]
    private int m_nbChoice;
    [SerializeField]
    private SpellGenerator m_spellGenerator;
    [SerializeField]
    private GameObject m_btn;
    private GameObject m_canvas;
    [SerializeField]
    private bool m_choiceControlled;

    private List<GameObject> m_buttons = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        m_canvas = GameObject.FindGameObjectWithTag("Canvas");
        if (!m_choiceControlled)
            m_nbChoice = Random.Range(1, m_nbChoice);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void destroyAllBtn()
    {
        for(int i = 0; i < m_buttons.Count; i++)
        {
            Destroy(m_buttons[i]);
        }
        Destroy(this.gameObject);
    }

     public void generateChoice()
    {
        for(int i = 0; i < m_nbChoice; i++)
        {
            m_buttons.Add(Instantiate(m_btn));
            //m_buttons[i].transform.parent = m_canvas.transform;
            m_buttons[i].transform.SetParent(m_canvas.transform);
            m_buttons[i].GetComponent<RectTransform>().position = new Vector3(1500/m_nbChoice + i * 500, 540, 0);
            m_buttons[i].GetComponent<UI_btnSelect>().SetSpell(m_spellGenerator.GenerateSpell());
            m_buttons[i].GetComponent<UI_btnSelect>().m_isSelected += destroyAllBtn;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            generateChoice();
        }
    }
}
