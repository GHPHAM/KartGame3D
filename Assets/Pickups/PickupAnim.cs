/***************************************************************
*file: PickupAnim.cs
*author: Ryan Davies
*class: CS 4700 – Game Development
*assignment: final program
*date last modified: 4/24/26
*
*purpose: This script manages basic animation for pickups. It should be 
*attached to the same GameObject as the PickupItem script.
*
****************************************************************/
using UnityEngine;

public class PickupAnim : MonoBehaviour
{
    [Header("Spin")]
    [SerializeField] private float spinSpeed = 45f;

    [Header("Bob")]
    [SerializeField] private float bobHeight = 0.5f;
    [SerializeField] private float bobSpeed = 1f;

    private Vector3 initialPosition;

    private void Start()
    {
        initialPosition = transform.position;
    }

    private void Update()
    {
        transform.Rotate(0f, 0f, spinSpeed * Time.deltaTime);

        float newY = initialPosition.y + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
