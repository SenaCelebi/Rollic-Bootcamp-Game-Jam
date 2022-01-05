using UnityEngine;
using NaughtyAttributes;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private Transform heartOn;
    [SerializeField] private Transform heartOff;

    [SerializeField] private float lerpSpeed = 2f; //ye�il bar dolma hareket h�z�

    private Transform barTransform;
    private float currentProgress = 0f; //yap�lan g�rev
    private float maxProgress = 3f; //toplam g�rev


    private void Awake()
    {
        barTransform = transform.Find(StringData.BAR);
        heartOn.gameObject.SetActive(false);
        heartOff.gameObject.SetActive(true);
    }

    private void Start()
    {
        barTransform.localScale = new Vector3(0f, 1f, 1f);
    }
    private void Update()
    {
        CheckGreenBar();
    }

    private void CheckGreenBar()
    {
        //ye�il bar�n g�rev tamamland�k�a yava��a artmas�
        barTransform.localScale = new Vector3(Mathf.Lerp(barTransform.localScale.x, UpdateProgressAmountNormalized(), lerpSpeed * Time.deltaTime), 1f, 1f);
        if (currentProgress >= maxProgress)
        {
            heartOn.gameObject.SetActive(true);
            heartOff.gameObject.SetActive(false);
        }
    }

    [Button]
    private void OneTaskDone()
    {
        currentProgress += 1;
        UpdateProgressAmountNormalized();
    }
    private float UpdateProgressAmountNormalized()
    {
        return (currentProgress / maxProgress);
    }
}
