using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarRatingController : MonoBehaviour
{
    public Animator anim => GetComponent<Animator>();
    public static int stars = 0;
    private void Update()
    { 
        Aasdasdasd();


    }


    private void Aasdasdasd()
    {
        switch (stars)
        {
            case 3:
                anim.SetBool("star3Bool", true);
                break;
            case 2:
                anim.SetBool("star2Bool", true);
                break;
            case 1:
                anim.SetBool("star1Bool", true);
                break;
            default:
                break;
        }
    }
}
