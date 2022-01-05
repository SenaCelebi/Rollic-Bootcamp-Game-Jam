using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

public class ItemSelectUI : MonoBehaviour
{
    [SerializeField] private List<Transform> itemList; // UI'daki 3 slot

    private HairTypeListSO hairList;
    private EyeListSO eyeList;

    [ShowNonSerializedField] private const int maxItemCount = 3;

    private void Awake()
    {
        //datalar�m�z
        hairList = Resources.Load<HairTypeListSO>(typeof(HairTypeListSO).Name);
        eyeList = Resources.Load<EyeListSO>(typeof(EyeListSO).Name);

        //GetHairs();
    }

    [Button]
    private void GetHairs()
    {
        List<int> tempList = new List<int>();
        foreach (Transform transform in itemList)
        {
            Image image = transform.Find(StringData.IMAGE).GetComponent<Image>();

            bool isAllItemSlotsFilled = maxItemCount == tempList.Count;

            // Farkl� sa�lar�n farkl� s�rada olacak �ekilde UI'a dizilmesi. 
            //3'ten fazla sa� varsa sadece 3 tanesinin al�nmas�. ��ld�rd�m yazarken
            while (!isAllItemSlotsFilled)
            {
                int randomIndex = UnityEngine.Random.Range(0, hairList.list.Count);

                if (!tempList.Contains(randomIndex))
                {
                    image.sprite = hairList.list[randomIndex].sprite;
                    image.color = UtilsClass.GetColorFromString(hairList.list[randomIndex].colorHex);
                    tempList.Add(randomIndex);
                    isAllItemSlotsFilled = true;
                }
            }
        }
        tempList.Clear();
    }
    
    [Button]
    private void GetEyes()
    {
        List<int> tempList = new List<int>();
        foreach (Transform transform in itemList)
        {
            Image image = transform.Find(StringData.IMAGE).GetComponent<Image>();

            bool isAllItemSlotsFilled = maxItemCount == tempList.Count;

            while (!isAllItemSlotsFilled)
            {
                int randomIndex = UnityEngine.Random.Range(0, eyeList.list.Count);

                if (!tempList.Contains(randomIndex))
                {
                    image.sprite = eyeList.list[randomIndex].sprite;
                    image.color = UtilsClass.GetColorFromString(eyeList.list[randomIndex].colorHex);
                    tempList.Add(randomIndex);
                    isAllItemSlotsFilled = true;
                }
            }
        }
        tempList.Clear();
    }
}
