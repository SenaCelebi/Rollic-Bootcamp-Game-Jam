using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Model/Dress")]
public class DressSO : ScriptableObject
{
    [SerializeField] private GameObject dressModel; // giysi tipi
    [SerializeField] private Sprite image2D; // ? ui ekran� i�in 2d resmi
    [SerializeField] private Color color;
}
