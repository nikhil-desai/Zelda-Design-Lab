using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class ItemManager : MonoBehaviour
{
    [Header("UI Reference")]
    public TextMeshProUGUI itemText;

    [Header("References")]
    public GameObject bombPrefab;
    public Transform holdPoint;
    private CharacterController _characterController;

    [Header("Settings")]
    public int bombCount = 5;
    public float throwForwardForce = 8f;
    public float throwUpwardForce = 3f;

    private GameObject _heldBomb;

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        UpdateUI();
    }

    void Update()
    {
        if (Keyboard.current.tKey.wasPressedThisFrame)
        {
            if (_heldBomb == null && bombCount > 0)
            {
                PickUpBomb();
            }
            else if (_heldBomb != null)
            {
                ThrowBomb();
            }
        }
    }

    void PickUpBomb()
    {
        bombCount--;
        UpdateUI();

        _heldBomb = Instantiate(bombPrefab, holdPoint.position, holdPoint.rotation);
        _heldBomb.transform.SetParent(holdPoint);

        Rigidbody rb = _heldBomb.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true; 

        Collider col = _heldBomb.GetComponent<Collider>();
        if (col != null) col.enabled = false;
    }

    public void ThrowBomb() // This was missing or private!
    {
        if (_heldBomb == null) return;

        _heldBomb.transform.SetParent(null);
        Rigidbody rb = _heldBomb.GetComponent<Rigidbody>();
        Collider col = _heldBomb.GetComponent<Collider>();

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            if (col != null) col.enabled = true;

            // Apply Momentum
            Vector3 playerVelocity = _characterController.velocity;
            Vector3 throwVector = (transform.forward * throwForwardForce) + (transform.up * throwUpwardForce);
            rb.AddForce(throwVector + playerVelocity, ForceMode.Impulse);
        }

        _heldBomb = null;
    }

    // Must be PUBLIC so BombLogic can see it!
    public void NotifyBombExploded()
    {
        _heldBomb = null;
    }

    void UpdateUI()
    {
        if (itemText != null)
        {
            itemText.text = $"Bombs: {bombCount}";
            itemText.color = (bombCount <= 0) ? Color.red : Color.white;
        }
    }
}