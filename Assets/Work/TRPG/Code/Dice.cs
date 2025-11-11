using Core;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private List<Transform> faces;
    [SerializeField] private float rollPower = 5f;
    [SerializeField] private float torquePower = 10f;
    public ReactiveProperty<int> Result = new();

    private bool _isRolling = false;

    private void FixedUpdate()
    {
        if (_isRolling == false) return;

        if (rb.IsSleeping())
        {
            _isRolling = false;
            int topFaceValue = CheckTopFace();
            Result.Value = topFaceValue;
        }
    }

    [ContextMenu("Roll Dice")]
    public void Roll()
    {
        _isRolling = true;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.AddForce(Vector3.up * rollPower, ForceMode.Impulse);
        rb.AddTorque(new Vector3(
            Random.Range(-torquePower, torquePower),
            Random.Range(-torquePower, torquePower),
            Random.Range(-torquePower, torquePower)
        ), ForceMode.Impulse);
    }

    public int CheckTopFace()
    {
        float maxHighY = -Mathf.Infinity;
        int topFaceIndex = -1;

        for (int i = 0; i < faces.Count; i++)
        {
            if (faces[i].position.y > maxHighY)
            {
                maxHighY = faces[i].position.y;
                topFaceIndex = i;
            }
        }

        Debug.Log($"Top face index: {topFaceIndex}, Value: {topFaceIndex + 1}");
        return topFaceIndex + 1;
    }
}
