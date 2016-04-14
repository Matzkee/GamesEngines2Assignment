using UnityEngine;
using System.Collections;

public class CameraFollower : MonoBehaviour {
    
    public GameObject[] idleCameras;
    public bool idleCamera = true;
    GameObject current;

    public float rotationSpeed = 0.2f;
    public float camRoutine = 5.0f;
    public float idleCamSwitchTime = 15.0f;

	// Use this for initialization
	void Start () {
        idleCameras = GameObject.FindGameObjectsWithTag("Camera-spot");

        StartCoroutine("CameraSwitcher");
	}
	
	// Update is called once per frame
	void Update () {
        TravelToObject();
        RotateToObject();
    }

    IEnumerator CameraSwitcher()
    {
        // run this cooroutine
        while (true)
        {
            // While there is nothing interesting to see
            while (idleCamera)
            {
                PickNewIdleCamera();

                yield return new WaitForSeconds(idleCamSwitchTime);
            }

            // Wait for a new camera behaviour
            yield return new WaitForSeconds(camRoutine);
        }
    }

    void TravelToObject()
    {
        transform.position = current.transform.position;
    }

    void RotateToObject()
    {
        float angle = Quaternion.Angle(transform.rotation, current.transform.rotation);
        if (angle > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, current.transform.rotation, Time.deltaTime * rotationSpeed);
        }
    }

    void PickNewIdleCamera()
    {
        current = idleCameras[Random.Range(0, idleCameras.Length)];
    }
}
