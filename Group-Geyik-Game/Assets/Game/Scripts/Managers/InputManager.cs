using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// oyuncudan al�nacak t�m girdiler
/// hocam�z bunun yerine hepsini PlayerController'�n i�inden yap�yordu
/// </summary>
public class InputManager : MonoBehaviour
{
    public float HorizontalInput { get; private set; }
    public float VerticalInput { get; private set; }

    private Vector2 inputDrag;
    private Vector2 previousMousePosition;

    private Vector3 prevPathPoint;

    public bool isDrag = false;
    public Transform handTransform;

    public static bool IsClickingDown { get; private set; }
    public static bool IsClickingLeftDown { get; private set; }
    public static bool IsClickingRightDown { get; private set; }
    public static bool IsClickingLeftUp { get; private set; }
    public static bool IsClickingRightUp { get; private set; }
    public static bool IsClickingLeft { get; private set; }
    public static bool IsClickingRight { get; private set; }
    public static bool IsClickDownAnything { get; private set; }
    public static bool IsClicking { get; private set; }

    private void ReceiveClickInputs()
    {
        IsClickingRight = Input.GetMouseButton(1);
        IsClickingRightUp = Input.GetMouseButtonUp(1);
        IsClickingRightDown = Input.GetMouseButtonDown(1);

        IsClickingLeft = Input.GetMouseButton(0);
        IsClickingLeftUp = Input.GetMouseButtonUp(0);
        IsClickingLeftDown = Input.GetMouseButtonDown(0);

        IsClickDownAnything = Input.anyKeyDown;
        IsClickingDown = IsClickingLeftDown || IsClickingRightDown;

        IsClicking = IsClickingLeft || IsClickingRight;
    }

    private void ReceiveAxisInputs()
    {
        HorizontalInput = Input.GetAxisRaw(StringData.HORIZONTAL);
        VerticalInput = Input.GetAxisRaw(StringData.VERTICAL);
    }

    void Update()
    {
        //ReceiveAxisInputs();
        //ReceiveClickInputs();

        //MerveninKodu();
        HocaninKodu();

    }
    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.y -= 2.3f;
        mouseWorldPos.z = -3f;
        return mouseWorldPos;
    }
    private void HocaninKodu()
    {
        handTransform.position = Input.mousePosition;
        handTransform.gameObject.SetActive(false);

        if ((Input.GetMouseButton(0) || Input.GetMouseButton(1)) && EventSystem.current.IsPointerOverGameObject())
        {
            isDrag = true;
            if (handTransform.GetComponent<Image>().sprite == null)
            {
                //handTransform.GetComponent<Image>().sprite = ItemSelectUI.Instance.
            handTransform.gameObject.SetActive(true);
            }

        }
        if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
        {
            isDrag = false;
            handTransform.GetComponent<Image>().sprite = null;
        }
        if (isDrag)
        {
            handTransform.gameObject.SetActive(true);
        }


    }
    //private void MerveninKodu()
    //{
    //    if (Input.touchCount > 0 && dragingObj == null) //ekranda bir dokunma alg�lanm�� ama s�r�klenen obje bilgisini almam��sak �al�� demek.
    //    {
    //        Debug.Log("dokand�");
    //        for (int i = 0; i <= Input.touchCount; i++)
    //        {
    //            Touch touch = Input.GetTouch(i);
    //            if (touch.phase == TouchPhase.Began)
    //            {

    //                RaycastHit2D hit = Physics2D.Raycast(UtilsClass.GetScreenToWorldPosition(), Vector2.zero);
    //                if (hit.collider != null && hit.collider.tag == "dragingObject")//s�r�k�lenebilir objelere bu tag� vermen gerekiyor. 
    //                {
    //                    isDrag = true;
    //                    dragingObj = hit.collider.gameObject; // burada dokundu�u objenin bilgisini al�yor
    //                    dragingObjPos = dragingObj.transform.position;// burada da o objenin transform bilgisini al�yor.
    //                }
    //            }
    //            if (touch.phase == TouchPhase.Moved)
    //            {
    //                if (isDrag)
    //                {
    //                    dragingObjPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position); // buras� objenin yerini d�nya kordinat�na g�re ta��man� sa�l�yor.
    //                    dragingObjPos.z = 0;// z'yi s�f�rlamazsan sa�ma sapan yerlere gidip ekrandan ��kabiliyor.
    //                }
    //            }
    //            if (touch.phase == TouchPhase.Ended)
    //            {
    //                isDrag = false;
    //                dragingObj = null;
    //            }

    //        }

    //    }
    //}

    //private void OnMouseDrag()//bu sadece unityde �al��mas� i�in... Yukar�daki kod unity i�inde �al��m�yor ��nk�
    //{
    //    dragingObjPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //    dragingObjPos.z = 0;

    //}

}
