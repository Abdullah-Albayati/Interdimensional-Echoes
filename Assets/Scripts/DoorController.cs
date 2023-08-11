using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorController : MonoBehaviour
{
    private bool isOpen = false;
    [Header(header: "Door Settings")]
    [SerializeField] private bool canBeClosed = true;
    [SerializeField] private bool startOpen = false;
    [SerializeField] private bool startLocked;
    [SerializeField] public bool isLocked = false;
    public string keyName;

    private float interactDistance = 3;

    [Header("Colors")]
    [SerializeField] private Color openColor;
    [SerializeField] private Color closeColor;
    [SerializeField] private Color wrongKeyColor;

    [Header("Resources")]
    [SerializeField] private LayerMask doorLayer;
    private Animator doorAnim;
    private Renderer doorRenderer;
    [SerializeField] private Color emissionColor;

    [Header("Audio")]
    [SerializeField] private AudioClip openSound;
    [SerializeField] private AudioClip closeSound;
    [SerializeField] private AudioClip unlockSound;
    [SerializeField] private AudioClip wrongKeySound;
    private AudioSource audioSource;

    [Header("Door Lock UI")]
    public GameObject lockMessageUI;
    public Text lockMessageText;

    [SerializeField] private bool autoClose = false;
    [SerializeField] private float autoCloseDelay = 5f;

    [HideInInspector] public DoorController[] childDoorControllers;

    public bool IsOpen
    {
        get { return isOpen; }
        set
        {
            isOpen = value;
            ChangeHighlightColor(isOpen);
        }
    }

    private Color defaultColor;
    private bool isMouseOver = false;

    private void Awake()
    {
        doorAnim = GetComponentInParent<Animator>();
        doorRenderer = GetComponent<Renderer>();
        if (doorRenderer == null)
            doorRenderer = GetComponentInChildren<Renderer>();

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        emissionColor = closeColor * 5;
        defaultColor = doorRenderer.material.GetColor("_EmissionColor");

        childDoorControllers = transform.parent.GetComponentsInChildren<DoorController>();
    }

    private void Start()
    {
        foreach (var controller in childDoorControllers)
        {
            canBeClosed = controller.canBeClosed;
            startOpen = controller.startOpen;
            startLocked = controller.startLocked;
            keyName = controller.keyName;
            openColor = controller.openColor;
            closeColor = controller.closeColor;
            wrongKeyColor = controller.wrongKeyColor;
            autoClose = controller.autoClose;
            autoCloseDelay = controller.autoCloseDelay;

            closeSound = controller.closeSound;
            openSound = controller.openSound;
            unlockSound = controller.unlockSound;
            wrongKeySound = controller.wrongKeySound;

            if (startOpen)
            {
                IsOpen = startOpen;
                controller.OpenDoor();
            }

            if (startLocked)
            {
                isLocked = startLocked;
            }
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown(GameManager.instance.interactButton))
        {
            InteractWithDoor();
        }
    }

    private void InteractWithDoor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, interactDistance, doorLayer) && hit.transform == transform)
        {
            if (doorAnim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
            {
                if (IsOpen)
                {
                    if (!canBeClosed)
                        return;
                    CloseDoor();
                }
                else
                {
                    if (isLocked)
                        return;
                    OpenDoor();
                }
            }
        }
    }

    public void OpenDoor()
    {
        foreach (var controller in childDoorControllers)
        {
            controller.IsOpen = true;
        }

        doorAnim.SetBool("isOpen", true);
        audioSource.PlayOneShot(openSound);

        if (autoClose)
            StartCoroutine(AutoCloseDoor());
    }

    public void CloseDoor()
    {
        foreach (var controller in childDoorControllers)
        {
            controller.IsOpen = false;
        }

        doorAnim.SetBool("isOpen", false);
        audioSource.PlayOneShot(closeSound);
    }

    private void ChangeHighlightColor(bool isOpen)
    {
        doorRenderer.material.SetColor("_EmissionColor", isOpen ? openColor * 5 : emissionColor);
    }

    public void UnlockDoor()
    {
        foreach (var controller in childDoorControllers)
        {
            if (controller.isLocked)
                controller.isLocked = false;
        }

        InteractWithDoor();
        audioSource.PlayOneShot(unlockSound);
    }

    public IEnumerator WrongKey()
    {
        doorRenderer.material.SetColor("_EmissionColor", wrongKeyColor * 5);
        audioSource.PlayOneShot(wrongKeySound);
        yield return new WaitForSeconds(3);
        if (isOpen)
        {
            doorRenderer.material.SetColor("_EmissionColor", openColor * 5);
        }
        else
        {
            doorRenderer.material.SetColor("_EmissionColor", emissionColor);
        }
    }

    private IEnumerator AutoCloseDoor()
    {
        
        yield return new WaitForSeconds(autoCloseDelay);
        if(IsOpen)
        CloseDoor();
    }

    private void OnMouseEnter()
    {
        isMouseOver = true;
        if (!isOpen && !isLocked)
            doorRenderer.material.SetColor("_EmissionColor", openColor * 5);
    }

    private void OnMouseExit()
    {
        isMouseOver = false;
        if (!isOpen && !isLocked)
            doorRenderer.material.SetColor("_EmissionColor", defaultColor);
    }
}
