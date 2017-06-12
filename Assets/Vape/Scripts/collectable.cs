using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class collectable : MonoBehaviour
{
    public int maxNumber = 1;
    public float pickDistance = 1F;
    public bool inInventory;
    // Use this for initialization
    void Start()
    {
        inInventory = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!inInventory)
        {
            if (Vector3.Distance(transform.position, PlayerPosManager.Instance.playerPos) < pickDistance)
            {
                if (maxNumber == 1)
                {
                    inventory.Instance.invList.Add(new inventoryCell(gameObject, false));
                }
                else
                {
                    bool added = false;
                    foreach (inventoryCell cell in inventory.Instance.invList)
                    {
                        if (cell.type.tag == tag)
                        {
                            if (cell.number < maxNumber)
                            {
                                cell.number++;
                                cell.UpdateText();
                                added = true;
                                Destroy(gameObject);
                                break;
                            }
                        }
                    }
                    if (!added)
                    {
                        inventory.Instance.invList.Add(new inventoryCell(gameObject, true));
                    }
                }
                inInventory = true;
            }
        }
    }
}