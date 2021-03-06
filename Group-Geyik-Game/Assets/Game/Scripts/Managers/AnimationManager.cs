using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class AnimationManager : MonoBehaviour
{
    public static AnimationManager Instance { get; private set; }

    [SerializeField] private Animator loadingGameUI;
    [SerializeField] private Animator introGameUI;
    [SerializeField] private Animator inGameUI;
    [SerializeField] private Animator failGameUI;
    [SerializeField] private Animator winGameUI;
    [SerializeField] private Animator wheelUI;
    [SerializeField] private Animator modelFemale;
    [SerializeField] private Animator modelMale;

    [SerializeField] private Transform introUI;


    private void Awake()
    {
        Instance = this;

        introUI.gameObject.SetActive(false);
    }

    [Button]
    public void ActivateLoadingGameUI()
    {
        loadingGameUI.SetBool(StringData.ISACTIVE, true);
        StartCoroutine("PlayIntroUI");
    }
    [Button]
    public void DeactivateLoadingGameUI()
    {
        loadingGameUI.SetBool(StringData.ISACTIVE, false);
    }
    private IEnumerator PlayIntroUI()
    {
        yield return new WaitForSeconds(5f);
        introUI.gameObject.SetActive(true);

    }

    [Button]
    public void ActivateIntroGameUI()
    {
        introGameUI.SetBool(StringData.ISACTIVE, true);
    }
    [Button]
    public void DeactivateIntroGameUI()
    {
        introGameUI.SetBool(StringData.ISACTIVE, false);
    }


    [Button]
    public void ActivateInGameUI()
    {
        inGameUI.SetBool(StringData.ISACTIVE, true);
    }
    [Button]
    public void DeactivateInGameUI()
    {
        inGameUI.SetBool(StringData.ISACTIVE, false);
    }
    [Button]
    public void ActivateDanceFemale()
    {
        modelFemale.SetBool(StringData.ISDANCING, true);
    }
    [Button]
    public void DeactivateDanceFemale()
    {
        modelFemale.SetBool(StringData.ISDANCING, false);
    }
    public void ActivateDanceMale()
    {
        modelMale.SetBool(StringData.ISDANCING, true);
    }
    [Button]
    public void DeactivateDanceMale()
    {
        modelMale.SetBool(StringData.ISDANCING, false);
    }
    [Button]
    public void ActivateFailGameUI()
    {
        failGameUI.SetBool(StringData.ISACTIVE, true);
    }
    [Button]
    public void DeactivateFailGameUI()
    {
        failGameUI.SetBool(StringData.ISACTIVE, false);
    }

    [Button]
    public void ActivateWinGameUI()
    {
        winGameUI.SetBool(StringData.ISACTIVE, true);
    }
    [Button]
    public void DeactivateWinGameUI()
    {
        winGameUI.SetBool(StringData.ISACTIVE, false);
    }
    [Button]
    public void ActivateWheelUI()
    {
        wheelUI.SetBool(StringData.ISACTIVE, true);
    }
    [Button]
    public void DeactivateWheelUI()
    {
        wheelUI.SetBool(StringData.ISACTIVE, false);
    }

}
