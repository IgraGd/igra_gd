using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class inventory : MonoBehaviour
{
    public static inventory Instance { get; private set; }
    public List<inventoryCell> invList = new List<inventoryCell>();
    public float invDistance = 1.5F;
    public GameObject InventoryFrame;
    public float popDistance = 5;

    public void Awake()
    {
        Instance = this;
    }

    void DeleteCell(int index)
    {
        invList[index].delete();
        invList.RemoveAt(index);
        for (int i = index; i < invList.Count; i++)
        {
            invList[i].UpdatePos(i);
        }
    }
    public void PopItem(int index)
    {
        GameObject poppedObject = Instantiate(invList[index].type, Vector3.zero, Quaternion.identity);
        poppedObject.GetComponent<Rigidbody>().isKinematic = false;
        poppedObject.GetComponent<collectable>().inInventory = false;
        Destroy(poppedObject.GetComponent<RotationInv>());
        poppedObject.layer = 0;
        if (invList[index].number > 1)
        {
            invList[index].number--;
            invList[index].UpdateText();
        }
        else
        {
            DeleteCell(index);
        }
        poppedObject.transform.position = PlayerPosManager.Instance.transform.position + PlayerPosManager.Instance.transform.rotation * Vector3.forward * popDistance + new Vector3(0, 1, 0);
        // выбрасываем объект на popDistance метров вперед
    }
}

public class inventoryCell
{
    public GameObject type; // предмет
    public int number; // кол-во предметов
    public GameObject frame; // текстовое поле для количества, сюда можно еще какую-нибудь рамку прицепить
    public Vector3 delta = new Vector3(0.25F, -0.3F, -5); // смещение текста относительно кубика

    public inventoryCell(GameObject itemObject, bool isStockable)
    {
        type = itemObject;
        number = 1;
        type.GetComponent<Rigidbody>().isKinematic = true;
        // Да, инвентарь где-то на уровне, он должен быть достаточно далеко
        // Кстати, можете перетаскивать инвентарь куда угодно, но не поворачивайте, иначе сломаете мне рейкаст (выбор предмета)
        type.AddComponent<RotationInv>();
        type.layer = 8; // видимость камеры инвентаря
        frame = Object.Instantiate(inventory.Instance.InventoryFrame, Vector3.zero, Quaternion.identity);
        UpdatePos(inventory.Instance.invList.Count);
        if (isStockable)
        {
            frame.GetComponent<Text>().text = "1";
        }
    }
    public void UpdateText()
    {
        frame.GetComponent<Text>().text = number.ToString();
    }
    public void UpdatePos(int index)
    {
        float place = index * inventory.Instance.invDistance;
        type.transform.position = inventory.Instance.transform.position + new Vector3(place, 0, 0);
        frame.transform.position = type.transform.position + delta;
    }
    public void delete()
    {
        Object.Destroy(type);
        Object.Destroy(frame);
    }
}