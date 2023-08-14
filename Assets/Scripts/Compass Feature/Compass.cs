using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass : MonoBehaviour
{
    //Holds the reference to the objects the user wants to navigate to
    public Transform target;
    public RectTransform compassNeedle;

    private void Update()
    {
        if(target!=null)
        {
            Vector3 targetDirection = target.position - compassNeedle.position;
            float angle = Mathf.Atan2(targetDirection.y, targetDirection.x)*Mathf.Rad2Deg;
            compassNeedle.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

}
