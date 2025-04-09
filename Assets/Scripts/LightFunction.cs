using UnityEngine;
using UnityEngine.Timeline;

public class LightFunction : MonoBehaviour
{
    [SerializeField] private MovementController Controller;
    [SerializeField] private ControllableCharacter Character;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Shadow"))
            {
          
            Debug.Log("Shadow entered light zone: disabling movement");
           

                Controller.setBlockLight(true);
            Character.SetBlockedInLight(true);


            
        }
       
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Shadow"))
     
        {
            Debug.Log("Shadow exited light zone");


            Controller.setBlockLight(false);
            Character.SetBlockedInLight(false);
        }
    }
}
