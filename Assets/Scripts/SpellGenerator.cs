using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ElementListing;

public class SpellGenerator : MonoBehaviour
{
    [SerializeField]
    private int[] m_powerValues; // 200, 350, 700
    [SerializeField]
    private float[] m_powerRate; //500, 200, 50
    private float m_powerRateSum = 0;

    //spells caracteristics are bridle by their rarity
    [SerializeField]
    private int m_maxProjectileSpeed;//4
    [SerializeField]
    private int m_maxDamage;//150
    [SerializeField]
    private int m_maxBulletPierce;//7
    [SerializeField]
    private int m_maxScale;//5
    [SerializeField]
    private int m_maxCooldown;//13
    [SerializeField]
    private int m_maxProjectile;//7

    // Start is called before the first frame update
    void Start()
    {
        //Set up the amount of all the rate of the specific weapon
        for (int i = 0; i < m_powerValues.Length; i++)
            m_powerRateSum += m_powerRate[i];
    }

    /// <summary>
    /// Generate a random spell
    /// </summary>
    /// <returns>Spell generated</returns>
    public spells GenerateSpell()
    {
        spells aSpell = new spells();

        //set the spell rarity
        int spellRarity = GetProbabilityAccordingToRate();
        aSpell.SetSpellRarity(spellRarity);

        //set the spell power using it's rarity
        float spellPowerF = m_powerValues[spellRarity] * (1 + Random.Range(-0.15f, 0.15f));
        int spellPower = (int)spellPowerF;

        //give a random element to the spell
        Elements element = (Elements)Random.Range(0, 11);
        aSpell.SetElement(element);

        //set the spell type (aoe and turn around doesn't use bullet pierce so they are naturally more powerfull than other spell types that's why they cost spell power 
        List<string> type = new List<string> { "front", "back", "left", "right", "autoAim", "aoe", "turnAround", "boomerang" };
        int randomTypeID = Random.Range(0, type.Count);
        aSpell.SetSpellType(type[randomTypeID]);
        if (randomTypeID == 5 || randomTypeID == 6)
            spellPower -= 100;

        //set the icon use for the spell (it's just a visual variation)
        List<string> shape = new List<string> { "Spear", "Sword", "Saber", "Shield", "Puddle", "Hammer" };
        int randomShapeID = Random.Range(0, shape.Count);
        aSpell.SetSpellShape(shape[randomShapeID]);

        //while the spell power isn't consume it generate a lsit of 6 ids that corresponds to a caracteristic of the spell
        while (spellPower > 10)
        {
            List<int> spellCara = new List<int> { 0, 1, 2, 3, 4, 5 };
            //if the spell power is > 10 and there is still some unset caracteristic
            //the program set add some value to a spell's random caracteristic using an id from 0 to 6
            //and then retrieve the value of the cracteristic generated from spell power
            //it also delete the id of the caracteristic from the list in order to be sure to add points to the other one before increasing again this caracteristic
            while (spellCara.Count != 0 && spellPower > 10)
            {
                int randomCaraID = Random.Range(0, spellCara.Count - 1);
                int randomCara = spellCara[randomCaraID];
                GenerateCara(randomCara, aSpell, ref spellPower, spellRarity);
                spellCara.RemoveAt(randomCaraID);
            }
        }

        //set the name of the spell
        aSpell.SetName(GenerateName(spellRarity, element, shape[randomShapeID]));
        /*float power = Random.Range(0, 100);
        power /= 100.0f;*/
        return aSpell;
    }

    /// <summary>
    /// Generate a name to the spell according to various parameters
    /// Spells name are made of 3 words (1 for each parameters)
    /// </summary>
    /// <param name="spellRarity">rarity of the spell</param>
    /// <param name="element">spell elements</param>
    /// <param name="shape">shape of the spell, "Spear", "Sword", "Saber", "Shield", "Puddle", "Hammer"</param>
    /// <returns>spell name</returns>
    private string GenerateName(int spellRarity, Elements element, string shape)
    {
        string spellName;
        string[] w_rarity = new string[] { "" };
        string[] w_element = new string[] {"" };
        string[] w_shape = new string[] { "" };

        //rarity word (1st word of the name)
        //create a list of all the word that can be used according  to spell's rarity
        switch (spellRarity)
        {
            case 0:
                w_rarity = new string[]{ "Dusty", "Old", "Broken", "Common", "Ordinary" };
                break;
            case 1:
                w_rarity = new string[] { "Noble", "Limited", "Special", "Mysterious", "Uncomon" };
                break;
            case 2:
                w_rarity = new string[] { "Godlike", "Extraordinary", "Heroic", "Outstanding", "Unic" };
                break;
        }

        //element word (2nd word of the name)
        //create a list of all the word that can be used according  to spell's element
        switch (element)
        {
            case Elements.Biohazard:
                w_element = new string[]{ "Hazardous", "Mutating", "Biologic"};
                break;
            case Elements.Darkness:
                w_element = new string[] { "Dark", "Black", "Demonic" };
                break;
            case Elements.Dirt:
                w_element = new string[] { "Rocky", "Stone", "Rugged" };
                break;
            case Elements.Fire:
                w_element = new string[] { "Burning", "Fire", "Infernal" };
                break;
            case Elements.Ice:
                w_element = new string[] { "Cold", "Freezing", "Ice" };
                break;
            case Elements.Light:
                w_element = new string[] { "Sacred", "Holy", "Shiny" };
                break;
            case Elements.Lightning:
                w_element = new string[] { "Electric", "Lightning", "Bzz Bzz" };
                break;
            case Elements.Nature:
                w_element = new string[] { "Green", "Wood", "Eco" };
                break;
            case Elements.Normal:
                w_element = new string[] { "White", "Normal", "Blank" };
                break;
            case Elements.Radiation:
                w_element = new string[] { "Radiated", "Radioactive", "Nuclear" };
                break;
            case Elements.Void:
                w_element = new string[] { "Void", "Abyssal", "Glutonous" };
                break;
            case Elements.Water:
                w_element = new string[] { "Wet", "Blobby", "Blue" };
                break;
        }

        //shape word (3rd word of the name)
        //create a list of all the word that can be used according  to spell's shape
        switch (shape)
        {
            case "Spear":
                w_shape = new string[] { "Spear", "Javelin", "Pike", "Shaft"};
                break;
            case "Sword":
                w_shape = new string[] { "Sword", "Claymore", "Blade", "Steel" };
                break;
            case "Saber":
                w_shape = new string[] { "Rapier", "Saber", "Scimitar", "Cutlass" };
                break;
            case "Shield":
                w_shape = new string[] { "Shield", "Aegis", "Ward", "Guard" };
                break;
            case "Puddle":
                w_shape = new string[] { "Puddle", "Mere", "Area", "Circle" };
                break;
            case "Hammer":
                w_shape = new string[] { "Mass", "Hammer", "Mallet", "Breaker" };
                break;
        }
        //get a random word from the 3 lists and add a space between them
        spellName = w_rarity[Random.Range(0, w_rarity.Length)] + " " + w_element[Random.Range(0, w_element.Length)] + " " + w_shape[Random.Range(0, w_shape.Length)];
        return spellName;
    }

    /// <summary>
    /// Generate a spell caracteristic using an id, the spellPower and the spell rarity
    /// </summary>
    /// <param name="caraID">id of the specific caracteristic, 0 = speed, 1 = spell damage, 2 = bullet pierce, 3 = scale, 4 = cooldown, 5 = amount of bullet spawned</param>
    /// <param name="aSpell"></param>
    /// <param name="spellPower"></param>
    /// <param name="spellRarity"></param>
    private void GenerateCara(int caraID, spells aSpell, ref int spellPower, int spellRarity)
    {
        int maxValue = 0;
        int minValue = 0;
        int intensity = 0;

        int caraValue = 0;
        if(caraID == 0)
        {
            minValue = 1;
            maxValue = m_maxProjectileSpeed;
            intensity = 10;

            caraValue = GetRandomValue(ref minValue, ref maxValue, spellPower, intensity);
            //if cara to add is > to the max that the spell can handle, it's bridle
            if (aSpell.GetSpeed() + caraValue > m_maxProjectileSpeed * (1 + spellRarity))
                caraValue = m_maxProjectileSpeed - aSpell.GetSpeed();

            aSpell.AddProjectileSpeed(caraValue);
        }else if(caraID == 1)
        {
            minValue = 40;
            maxValue = m_maxDamage;
            intensity = 1;
            caraValue = GetRandomValue(ref minValue, ref maxValue, spellPower, intensity);
            if (aSpell.GetDamage() + caraValue > m_maxDamage * (1 + spellRarity))
                caraValue = m_maxDamage - aSpell.GetDamage();

            aSpell.AddDamage(caraValue);
        }else if(caraID == 2)
        {
            minValue = 1;
            maxValue = m_maxBulletPierce;
            intensity = 15;
            caraValue = GetRandomValue(ref minValue, ref maxValue, spellPower, intensity);
            if (aSpell.GetPierce() + caraValue > m_maxBulletPierce * (1 + spellRarity))
                caraValue = m_maxBulletPierce - aSpell.GetPierce();
            aSpell.AddPierce(caraValue);
        }else if(caraID == 3)
        {
            minValue = 1;
            maxValue = m_maxScale;
            intensity = 5;
            caraValue = GetRandomValue(ref minValue, ref maxValue, spellPower, intensity);
            if (aSpell.GetScale() + caraValue > m_maxScale * (1 + spellRarity))
                caraValue = m_maxScale - aSpell.GetScale();
            aSpell.AddScale(caraValue);
        }else if (caraID == 4)
        {
            maxValue = m_maxCooldown;
            minValue = 1;
            intensity = 10;
            caraValue = GetRandomValue(ref minValue, ref maxValue, spellPower, intensity);
            if (aSpell.GetCoolDown() - caraValue < 0.5f)
                caraValue = (int) (aSpell.GetCoolDown() - 0.5f);
            aSpell.RetrieveCooldown(caraValue);
        }
        else if (caraID == 5)
        {
            minValue = 1;
            maxValue = m_maxProjectile;
            intensity = 15;
            caraValue = GetRandomValue(ref minValue, ref maxValue, spellPower, 25);
            if (aSpell.GetNbProjectile() + caraValue > m_maxProjectile * (1 + spellRarity))
                caraValue = m_maxProjectile - caraValue;
            aSpell.AddProjectile(caraValue);
        }
        spellPower -= caraValue * intensity;
    }

    /// <summary>
    /// Return a random number between a min and a max value
    /// This number must also be minor than spellPower/intensity of the caracteristic
    /// </summary>
    /// <param name="minValue"></param>
    /// <param name="maxValue"></param>
    /// <param name="spellPower">amount of points a spell can use</param>
    /// <param name="intensity">intensity of the caracteristic to generate (amount of point to retrieve from spell power, to each point to add to the caracteristic)</param>
    /// <returns></returns>
    private int GetRandomValue(ref int minValue, ref int maxValue, int spellPower, int intensity)
    {
        if (maxValue * intensity > spellPower)
            maxValue = spellPower / intensity;
        //if a problem occur with min and max, set them corectly
        if(maxValue < minValue)
        {
            int interm = minValue;
            minValue = maxValue;
            maxValue = interm;
        }
        return Random.Range(minValue, maxValue);
    }

    /// <summary>
    /// Generate a spell's rarity
    /// </summary>
    /// <returns>spell rarity</returns>
    private int GetProbabilityAccordingToRate()
    {
        float rdn = Random.Range(0, m_powerRateSum);
        Debug.Log(rdn);
        int choice = 0;
        while (rdn - m_powerRate[choice] > 0)
        {
            rdn -= m_powerRate[choice]; 
            choice++;
        }
        return choice;
    }
}
