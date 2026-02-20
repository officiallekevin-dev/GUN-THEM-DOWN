using UnityEngine;
using UnityEngine.InputSystem;

public class Mouse_To_World : MonoBehaviour
{
    // Offset in degrees to adjust the sprite's default rotation (e.g. if sprite faces up, set to -90)
    public float rotationOffset = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    //Later we can add some code where if the z rotation is higher or lower than 90 then we do x.

    // Update is called once per frame
    void Update()
    {
        // 1. Get the mouse position from the Input system (Screen Space: x, y)
        Vector3 mouseScreenPosition = Mouse.current.position.ReadValue();

        // 2. Set the Z distance to match the camera's distance from the object's plane
        // Ideally, this should be dynamically calculated if the camera moves in Z, 
        // but for 2D standard setup, taking the difference works well.
        mouseScreenPosition.z = -Camera.main.transform.position.z;

        // 3. Convert Screen Space point to World Space point
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);

        // 4. Calculate the direction vector from the object to the mouse
        Vector3 direction = mouseWorldPosition - transform.position;

        // 5. Calculate the angle in degrees
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 6. Apply rotation to the object (Z-axis rotation for 2D)
        // Adding rotationOffset allows for correction if the art isn't drawn facing Right (0 deg)
        transform.rotation = Quaternion.Euler(0, 0, angle + rotationOffset);
    }
}
