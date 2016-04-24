using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class CameraFollower : MonoBehaviour {
    
    public List<GameObject> idleCameras;
    public bool idleCamera = true;
    GameObject current;

    public float cameraSpeed = 10;
    public float rotationSpeed = 0.2f;
    public float camRoutine = 5.0f;
    public float idleCamSwitchTime = 15.0f;

	// Use this for initialization
	void Start () {
        StartCoroutine("CameraSwitcher");
	}
	
	// Update is called once per frame
	void Update () {
        SetView();
    }

    IEnumerator CameraSwitcher()
    {
        // run this cooroutine
        while (true)
        {
            // While there is nothing interesting to see
            while (idleCamera)
            {
                idleCameras = GameObject.FindGameObjectsWithTag("Camera-spot").ToList();
                PickNewIdleCamera();

                yield return new WaitForSeconds(idleCamSwitchTime);
            }

            // Wait for a new camera behaviour
            yield return new WaitForSeconds(camRoutine);
        }
    }

    void SetView()
    {
        Vector3 toTarget = (current.transform.position - transform.position).normalized;
        Vector3 target = current.transform.position - toTarget;
        transform.position = Vector3.MoveTowards(transform.position, target, cameraSpeed * Time.deltaTime);
        float angle = Quaternion.Angle(transform.rotation, current.transform.rotation);
        if (angle > 0.1f) {
            transform.rotation = Quaternion.Slerp(transform.rotation, current.transform.rotation, Time.deltaTime * rotationSpeed);
        }
    }

    void PickNewIdleCamera()
    {
        current = idleCameras[Random.Range(0, idleCameras.Count)];
        transform.position = current.transform.position;
    }
}
