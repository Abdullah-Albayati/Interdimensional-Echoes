using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class HidingController : MonoBehaviour
{
    [SerializeField] private Image fadeImage;


    public bool IsInsideLocker { get; private set; }

   [SerializeField] private GameObject currentLocker;

    [SerializeField] GameObject player;
    [SerializeField] Transform lockerSpawnPoint;
    [SerializeField] float hidingDistance;

    private FirstPersonController playerController;
    private Rigidbody playerRigidbody;


    private void Start()
    {
        playerController = GetComponent<FirstPersonController>();
        playerRigidbody = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, hidingDistance))
        {
            GameObject hitObject = hit.collider.gameObject;

            if (hitObject.CompareTag("HidingLocker"))
            {
                currentLocker = hitObject.transform.parent.parent.gameObject;
                if (Input.GetButtonDown(GameManager.instance.interactButton) && playerRigidbody.velocity.magnitude < 0.1f)
                {
                    StartCoroutine(EnterOrExitLocker());
                }
            }

        } 
        else if (IsInsideLocker && Input.GetButtonDown(GameManager.instance.interactButton))
        { 
            StartCoroutine(EnterOrExitLocker());

        }

    }

    private IEnumerator EnterOrExitLocker()
    {
        yield return StartCoroutine(FadeToBlack());
        Transform targetPoint;
        if (IsInsideLocker)
        {
            targetPoint = currentLocker.transform.Find("OutsidePoint");
            IsInsideLocker = false;
            playerController.cameraCanMove = true;
            playerController.playerCanMove = true;
        }
        else
        {
            targetPoint = currentLocker.transform.Find("InsidePoint");
            IsInsideLocker = true;
            playerController.playerCanMove = false;
            playerController.cameraCanMove= false;
        }
        transform.position = targetPoint.position;
        transform.rotation = targetPoint.rotation;
        Camera.main.transform.rotation = targetPoint.rotation;
        yield return StartCoroutine(FadeFromBlack());
    }


    private IEnumerator FadeToBlack()
    {
        float duration = 0.5f;
        float elapsed = 0;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(0,1,elapsed / duration);
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        fadeImage.color = new Color(0, 0, 0, 1);
    }

    private IEnumerator FadeFromBlack()
    {
        float duration = .5f;
        float elapsed = 0;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1,0,elapsed / duration);
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;

        }
        fadeImage.color = new Color(0, 0, 0, 0);
    }


}
