using UnityEngine;
using UnityEngine.Timeline;

public class LightFunction : MonoBehaviour
{
    [SerializeField] private MovementController Controller;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Shadow"))
            {
          
            Debug.Log("Shadow entered light zone: disabling movement");
           

                Controller.setBlockLight(true);


            
        }
       
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Shadow"))
     
        {



           Controller.setBlockLight(false);
        }
    }
}
