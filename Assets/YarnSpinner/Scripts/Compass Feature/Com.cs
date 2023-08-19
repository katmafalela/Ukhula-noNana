using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Com : MonoBehaviour
{
    public Transform playerTransform;
    Vector3 direction;

    private void Update()
    {
        direction.z = playerTransform.eulerAngles.y;
        transform.localEulerAngles = direction;
    }
}
