using UnityEngine;

public class cameraChanger : MonoBehaviour
{

   
    public Transform cameraTransform;

   
    public float originalY = 6.82f;
    public float originalZ = -7.57f;
    public Vector3 originalRotation = new Vector3(35.35f, 334f, 0.4567f);

  
    public float birdseyeY = 10f;
    public float birdseyeZ = 11.6F;
    public Vector3 birdseyeRotation = new Vector3(97f, 0f, 0.4567f);

    
    public float sideY = 6.82f;
    public float sideZ = -7.57f;
    public Vector3 sideRotation = new Vector3(35.35f, 0f, 0.4567f);


    private enum ViewMode { Original, Birdseye, Side }
    private ViewMode currentView;

    void Start()
    {
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        SwitchToOriginalView();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SwitchToOriginalView();
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            SwitchToBirdseyeView();
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            SwitchToSideView();
    }

    private void LateUpdate()
    {
        UpdateCameraTransform();
    }
    private void UpdateCameraTransform()
    {
        Vector3 pos = cameraTransform.position;

        switch (currentView)
        {
            case ViewMode.Original:
                pos.y = originalY;
                pos.z = originalZ;
                cameraTransform.rotation = Quaternion.Euler(originalRotation);
                break;

            case ViewMode.Birdseye:
                pos.y = birdseyeY;
                pos.z = birdseyeZ;
                cameraTransform.rotation = Quaternion.Euler(birdseyeRotation);
                break;

            case ViewMode.Side:
                pos.y = sideY;
                pos.z = sideZ;
                cameraTransform.rotation = Quaternion.Euler(sideRotation);
                break;
        }

        cameraTransform.position = pos;
    }

    private void SwitchToOriginalView() => currentView = ViewMode.Original;
    private void SwitchToBirdseyeView() => currentView = ViewMode.Birdseye;
    private void SwitchToSideView() => currentView = ViewMode.Side;
}

