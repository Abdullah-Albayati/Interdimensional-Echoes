using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class HidingController : MonoBehaviour
{
    [SerializeField] private Image fadeImage;


    public bool IsInsideLocker { get; private set; }

   [SerializeField] private LockerScript currentLocker;
   [SerializeField] float speedReductionFactor;
    [SerializeField] GameObject player,playerHand;
    [SerializeField] float hidingDistance;

    private FirstPersonController playerController;
    private Rigidbody playerRigidbody;
    [SerializeField] private AudioClip lockerSound;

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
                currentLocker = hitObject.GetComponent<LockerScript>();
                if (Input.GetButtonDown(GameManager.instance.interactButton))
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
        playerRigidbody.velocity = Vector3.zero;
        GameManager.instance.isInputDisabled = true;
        yield return StartCoroutine(FadeToBlack());
        player.GetComponent<AudioSource>().PlayOneShot(lockerSound);
        Transform targetPoint;
        if (IsInsideLocker)
        {
            targetPoint = currentLocker.outsidePoint;
            IsInsideLocker = false;
            playerController.cameraCanMove = true;
            playerController.playerCanMove = true;
            playerController.enableCrouch = true;
            currentLocker.isOccupied = false;
            UIManager.Instance.TogglePlayerUI(true);
            playerHand.SetActive(true);
        }
        else
        {
            var randPos = Random.Range(0,currentLocker.insidePoints.Length);
            targetPoint = currentLocker.insidePoints[randPos];
            IsInsideLocker = true;
            playerController.playerCanMove = false;
            playerController.cameraCanMove= false;
            playerController.enableCrouch = false;
            currentLocker.isOccupied = true;
            UIManager.Instance.TogglePlayerUI(false);
            playerHand.SetActive(false);
            if (playerController.isCrouched)
            {
                playerController.Crouch();
            } 
        }
        transform.position = targetPoint.position;
        transform.rotation = targetPoint.rotation;
        Camera.main.transform.rotation = targetPoint.rotation;
        GameManager.instance.isInputDisabled = false;
        yield return StartCoroutine(FadeFromBlack());
        
    }


    private IEnumerator FadeToBlack()
    {
        float duration = 0.5f;
        float elapsed = 0;
        playerController.playerCanMove = false;
        playerController.cameraCanMove = false;
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
