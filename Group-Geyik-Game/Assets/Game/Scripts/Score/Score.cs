using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    public static Score Instance { get; private set; }
    
    public static int stars = 3;
    public static int totalCoin = 0;

    private void Awake()
    {
        Instance = this;
    } 

  
    /*
     * Nerede player yanl�� se�im yaparsa star say�s�n� azalt.
     * Score.stars--
     */

}
