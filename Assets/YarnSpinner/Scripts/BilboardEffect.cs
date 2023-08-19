using UnityEngine;

public class BilboardEffect : MonoBehaviour
{
    [SerializeField] private bool isOnlyRotatingYAxis = true;
    // Update is called once per frame
    void LateUpdate()
    {
        //ensuring dialogue bubble is always facing camera
        if (isOnlyRotatingYAxis)
        {
            transform.rotation = Quaternion.Euler(0f, Camera.main.transform.rotation.eulerAngles.y, 0f);
        }
        else 
        {
            transform.rotation = Camera.main.transform.rotation; //incase camera gimbal rotated on X axis (pitched up & down) 
        }
        
    }

    
}
