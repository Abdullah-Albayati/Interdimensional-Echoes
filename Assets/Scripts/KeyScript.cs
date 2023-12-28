using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour
{
    [SerializeField] private string keyName = "defaultKey";
    [SerializeField] private LayerMask doorLayer;

    PickableObject currentItem;

    private void Start()
    {
        currentItem = GetComponent<PickableObject>();
    }

    private void Update()
    {
        RaycastHit hit;

        if(Input.GetButtonDown(GameManager.instance.interactButton) && currentItem.isPickedUp && gameObject.activeInHierarchy)
        {
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 3, doorLayer))
        {
                DoorController[] targetDoor = hit.collider.GetComponent<DoorController>().childDoorControllers;

                foreach (DoorController door in targetDoor)
                {
                if (door.isLocked)
                {
                    if (door.keyName == keyName)
                    {
                        door.UnlockDoor();
                    }
                    else
                        door.StartCoroutine(door.WrongKey());
                }

                }
                
        }

        }
    }
}
