using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarteData : IComparable<CarteData>
{
    public int valeur;
    public Enseigne enseigne;
    public Color color;

    public CarteData(int valeur, Enseigne enseigne)
    {
        this.valeur = valeur;
        this.enseigne = enseigne;
    }

    public int CompareTo(CarteData other)
    {
        return valeur.CompareTo(other.valeur);
    }
}