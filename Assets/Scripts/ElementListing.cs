using System.Collections;
using System.Collections.Generic;

namespace ElementListing
{
    //0 = normal, 1 = fire, 2 = water, 3 = ice, 4 = dirt, 5 = lightning, 6 = radiation, 7 = void, 8 = biohazard, 9 = nature, 10 = light, 11 = darkness
    public enum Elements 
    {
        Normal,
        Fire,
        Water,
        Ice,
        Dirt,
        Lightning,
        Radiation,
        Void,
        Biohazard,
        Nature,
        Light,
        Darkness,
    }

    public enum Status
    {
        burning,
        freeze,
        irradiate, // chance d'evoluer en ennemie élite 
        poisonned,
    }

    public enum Direction
    {
        X,
        Y
    }

    public enum Emplacement
    {
        Haut,
        Bas,
        Gauche,
        Droite
    }
}