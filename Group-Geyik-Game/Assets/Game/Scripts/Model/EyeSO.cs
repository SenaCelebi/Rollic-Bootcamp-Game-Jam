using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Model/Eye")]
public class EyeSO : ScriptableObject
{
    public Color color; // g�z rengi
    public string colorHex; // g�z rengi
    public string colorName; // "red", "green", "blue"

    public Sprite sprite;   // UI ekran� i�in 2d resmi

    public string GetRecipeText()
    {
        return $" <color=#{colorHex}>{colorName}</color> Eye";
    }
}
