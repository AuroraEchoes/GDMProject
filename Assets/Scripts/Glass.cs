using UnityEngine;

public class Glass : MonoBehaviour
{
    private Collider thisCollider;

    void Start()
    {
        thisCollider = GetComponent<Collider>();
        GameObject[] shadows = GameObject.FindGameObjectsWithTag("Shadow");
        foreach (GameObject shadow in shadows)
        {
            Collider shadowCollider = shadow.GetComponent<CharacterController>();
            Physics.IgnoreCollision(thisCollider, shadowCollider);
        }
    }
}
