using UnityEngine;

public class enter : MonoBehaviour
{
    public GameObject player;
    private MeshCollider meshCol;

    private void Start()
    {
        meshCol = GetComponent<MeshCollider>();
    }
    public void onTrigger(Collider other )
    {
        if(other.gameObject == player)
        {
            meshCol.enabled = true;
            Debug.Log("enter");
        }
       
    }



}
