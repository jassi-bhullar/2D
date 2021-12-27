using UnityEngine;

public class PhoneScreenPivot : MonoBehaviour
{
    // This script is attached to an empty pivot
    // gameobject which is placed at top-right
    // side of the playable Game Area you want to any
    // device screen to cover completely. For example,
    // You may want the player to see a certain object
    // near a screen corner irrespective on the screen resolution.
    // In doing so, some extra game area on device may
    // appear ethier on left and right, OR top and bottom
    // of the screen based on the Screen Resolution

    void Start()
    {
        // Instead of resolution, Screen.safeArea
        // may be a better choice

        Resolution res = Screen.currentResolution;
        float res_ratio = (float)res.width / (float)res.height;

        float pivot_ratio = transform.position.x / transform.position.y;

        if(res_ratio > pivot_ratio)
            Camera.main.orthographicSize = transform.position.y;
        else
            Camera.main.orthographicSize = transform.position.x / res_ratio;
    }
}
