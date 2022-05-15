using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ElementListing;

public abstract class StatusEffect : MonoBehaviour
{
    protected Status m_status { get; set; }
    protected Elements m_element { get; set; }
    protected bool m_isActive;
    protected bool m_ApplyEffect;
    protected int m_stack;
    private const float m_CoolDown = 10 ; // sec
    protected int m_damage { get; }
    private timerCZ m_clockCoolDown;

    public delegate void StatusEffectEvent();
    public event StatusEffectEvent isFinallyTheEndEffect;

    public StatusEffect(int stack ,int damage)
    {
        this.m_stack = stack;
        this.m_damage = damage;
        this.m_isActive = false;
        m_clockCoolDown = new timerCZ(m_CoolDown);
    }


    public void AddStack()
    {
        this.m_stack++;
        if (m_clockCoolDown.IsStart() == true)
        {
            ResetTimer();
        }
        m_clockCoolDown.StartTimer();
    }

    public void AddStack(int nbStackToAdd)
    {
        this.m_stack += nbStackToAdd;
        if (m_clockCoolDown.IsStart() == true)
        {
            ResetTimer();
        }
        m_clockCoolDown.StartTimer();
    }

    public void DelStack()
    {
        this.m_stack--;
        if (m_stack <=  0)
        {
            m_clockCoolDown.StopTimer();
            isFinallyTheEndEffect();
        }
    }

    public void DelStack(int nbStackToDel)
    {
        this.m_stack -= nbStackToDel;
        if (m_stack <= 0)
        {
            m_clockCoolDown.StopTimer();
            isFinallyTheEndEffect();
        }
    }

    private void ResetTimer()
    {
        m_clockCoolDown.ResetTimer();
    }
    
    protected void CoolDownManager()
    {
        if (m_clockCoolDown.End())
        {
            DelStack();
            if(m_stack > 0)
            {
                m_clockCoolDown.StartTimer();
            }
        }
    }

    public virtual void StartEffect()
    {
        m_isActive = true;
        AddStack();
    }

    public virtual void StopEffect()
    {
        m_isActive = true;
        this.m_stack = 0;
        isFinallyTheEndEffect -= StopEffect;
    }

    public int GetDamage()
    {
        return this.m_damage;
    }

    public Status GetStatus()
    {
        return this.m_status;
    }

    public Elements GetElement()
    {
        return this.m_element;
    }

    public bool GetIsActive()
    {
        return this.m_isActive;
    }

    public void StartEffect(int nbStack)
    {
        m_isActive = true;
        AddStack(nbStack);
    }
    public abstract void Effect();

    void Update()
    {
        if (m_clockCoolDown.IsStart())
        {
            CoolDownManager();
        }
    }
}

// poison mega dégat au bout des stack 
// Freezze ralentissement + gelre au bou de x stack 
// irradier au bout d'un certaint stack meur ou petite chance de transformé

public class Burning : StatusEffect
{
    timerCZ clockicloka; // Timer for deal damage every 1 sec
    public Burning (int stack, int damage) : base(stack,damage)
    {
        this.m_status = Status.burning;
        this.m_element = Elements.Fire;
        clockicloka = new timerCZ(1);
    }

    public override void StartEffect()
    {
        base.StartEffect();
        clockicloka.m_isFinallyTheEnd += this.Effect;
    }
    
    public override void StopEffect()
    {
        base.StopEffect();
        clockicloka.m_isFinallyTheEnd -= this.Effect;
    }

    public override void Effect()
    {
        gameObject.GetComponent<Personnage>().TakeDamage(this.m_damage,this.m_element);
        clockicloka.StartTimer(); // restarTimer
    }

}

public class Freeze : StatusEffect
{
    private const int limiteStackToActivateEffect = 5;
    private const float perCent_SpeedMalus = 40;
    public Freeze (int stack, int damage) : base(stack,damage)
    {
        this.m_status = Status.freeze;
        this.m_element = Elements.Ice;
    }

    public override void Effect()
    {
        if (this.m_stack >= limiteStackToActivateEffect) // freeze
        {
            float speedPerso = gameObject.GetComponent<Personnage>().GetVit();
            gameObject.GetComponent<Personnage>().SetVitMalus(-speedPerso);

            this.DelStack(limiteStackToActivateEffect); // del stack 
        }
        else // slowing
        {
            Personnage perso = gameObject.GetComponent<Personnage>();
            perso.SetVitMalus((perCent_SpeedMalus * perso.GetVit()) / 100);
        }
    }

    public override void StopEffect()
    {
        base.StopEffect();
        gameObject.GetComponent<Personnage>().SetVitMalus(0);
    }
}

public class Poisonning : StatusEffect{

    private const int limiteStackToActivateEffect = 5;
    private const float perCent_LifeLost = 20;
    private timerCZ clockicloka; // Timer for deal damage every 2 sec
    public Poisonning(int stack,int damage): base(stack, damage)
    {
        this.m_status = Status.poisonned;
        this.m_element = Elements.Biohazard;
        this.clockicloka = new timerCZ(2);
    }

    public override void StartEffect()
    {
        base.StartEffect();
        clockicloka.m_isFinallyTheEnd += this.Effect;
    }

    public override void StopEffect()
    {
        base.StopEffect();
        clockicloka.m_isFinallyTheEnd -= this.Effect;
    }

    public override void Effect()
    {
        if (this.m_stack >= limiteStackToActivateEffect)
        {
            Personnage perso = gameObject.GetComponent<Personnage>();
            perso.TakeDamage((int)(perCent_LifeLost * perso.GetLife()) / 100,this.m_element);
        }
        gameObject.GetComponent<Personnage>().TakeDamage(this.m_damage, this.m_element);
        clockicloka.StartTimer();
    }
}

public class Irradiate : StatusEffect
{
    public Irradiate(int stack,int damage) : base(stack, damage)
    {

    }

    public override void Effect()
    {

    }
}