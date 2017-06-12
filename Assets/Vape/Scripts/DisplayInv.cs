using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayInv : MonoBehaviour
{
    public float vievCoef = 8; // отношение размеров (горизонтали к вертикали) поля зрения проэкционной камеры, которая смотрит на инвентарь
    void Update()
    {
        float selfWidth = Screen.width;
        float selfHeight = selfWidth / vievCoef;
        gameObject.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, selfWidth);
        gameObject.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, selfHeight);
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            GameObject invCamera = inventory.Instance.transform.Find("invCamera").gameObject;
            float camSize = invCamera.GetComponent<Camera>().orthographicSize;
            float multiple = camSize / selfHeight * 2; // вся эта математика действует только пока инвентарь прижат к верхнему краю экрана (первая половина скрипта)
            float relX = (mousePos.x - selfWidth / 2) * multiple;
            float relY = (mousePos.y - Screen.height + selfHeight / 2) * multiple;
            Vector3 relPos = new Vector3(relX, relY, 0); // координаты клика относительно камеры (в пространстве инвентаря)
            Vector3 absPos = invCamera.transform.position + relPos; // координаты начала рейкаста
            Ray ray = new Ray(absPos, Vector3.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 20))
            {
                GameObject hitObj = hit.collider.gameObject;
                int index = Mathf.RoundToInt((hitObj.transform.position.x - inventory.Instance.transform.position.x) / inventory.Instance.invDistance);
                //находим номер объекта в списке инвентаря по его положению
                inventory.Instance.PopItem(index); //выкидываем объект из инвентаря
            }
        }
    }
}