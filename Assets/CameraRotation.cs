using UnityEngine;

public class StrictZAxisCamera : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float panningSpeed = 5f;
    [SerializeField] private float rotationSpeed = 100f;


    private float initialXRot;
    private float initialZRot;

    void Start()
    {
        initialXRot = transform.eulerAngles.x;
        initialZRot = transform.eulerAngles.z;
    }


    void Update()
    {


        if (Input.GetKey(KeyCode.UpArrow))
        {
            float currentYRot = transform.eulerAngles.y;
            transform.rotation = Quaternion.Euler(
                initialXRot,
                currentYRot + rotationSpeed * Time.deltaTime,
                initialZRot
            );
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            float currentYRot = transform.eulerAngles.y;
            transform.rotation = Quaternion.Euler(
                initialXRot,
                currentYRot - rotationSpeed * Time.deltaTime,
                initialZRot
            );
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Vector3 newPos = transform.position;
            newPos.z += panningSpeed * Time.deltaTime;
            transform.position = newPos;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            Vector3 newPos = transform.position;
            newPos.z -= panningSpeed * Time.deltaTime;
            transform.position = newPos;
        }
    }
}