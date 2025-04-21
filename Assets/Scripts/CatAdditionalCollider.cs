using UnityEngine;

public class CatAdditionalCollider : MonoBehaviour
{
    private ControllableCharacter parent;

    void Start()
    {
        parent = GetComponentInParent<ControllableCharacter>();        
    }

    void OnCollisionEnter(Collision collision)
    {
        parent.PushBlock(collision.gameObject, collision.rigidbody);
    }

    void OnCollisionStay(Collision collision)
    {
        parent.PushBlock(collision.gameObject, collision.rigidbody);
    }
}