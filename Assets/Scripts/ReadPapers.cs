using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ReadPapers : MonoBehaviour
{
  [SerializeField] private float maxDistance;
    [SerializeField] private TextMeshProUGUI readPaperTxt;

    private bool isCursorConfined = true;
 
    private void Update()
    {

       /* if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isCursorConfined)
            {
                UnlockCursor();
            }
            else
            {
                ConfineCursor();
            }
        }

        if (Input.GetMouseButtonDown(0) && !isCursorConfined)
        {
            ConfineCursor();
        }*/

        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            GameObject hitObject = hit.collider.gameObject;
            if (hitObject.CompareTag("Paper"))
            {
                PaperScript paperScript = hitObject.GetComponent<PaperScript>();
                Debug.Log(hitObject.name);

                    readPaperTxt.gameObject.SetActive(true);
                    readPaperTxt.text = "Press T to read note";
                if (Input.GetButtonDown(GameManager.instance.readNotesButton))
                {
                    readPaperTxt.text = string.Empty;
                    readPaperTxt.gameObject.SetActive(false);
                    if (paperScript.panelContainer.transform.GetChild(0).gameObject.activeSelf)
                    {
                        UIManager.Instance.OpenPaperMenu();
                        paperScript.LeavePaper();
                    }
                    else
                    {

                    UIManager.Instance.OpenPaperMenu();
                        paperScript.ReadPaper();
                    }
                }
                
                
            }
        }
        else
        {
            return;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * maxDistance);
    }

    private void ConfineCursor()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        isCursorConfined = true;
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isCursorConfined = false;
    }
}
