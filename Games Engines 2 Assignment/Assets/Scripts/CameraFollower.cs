using UnityEngine;
using UnityEngine.UI;
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

    public bool sceneFade = true;
    public bool fadingEnabled = false;
    public float fadeSpeed = 1.5f;
    bool fading = false;

    bool fadingTextEnabled = false;
    public new GUITexture guiTexture;
    public new Text guiText;

    public float displayTime = 5;
    public List<string> displayText;
    Queue<string> textQueue;
    
    void StartScene() {
        if (sceneFade) {
            FadeToCLear();
        }
    }
	void Start () {
        StartCoroutine("CameraSwitcher");
        StartCoroutine("TextViewer");
        textQueue = new Queue<string>();
        if (displayText.Count > 0) {
            foreach (string text in displayText) {
                textQueue.Enqueue(text);
            }
        }
	}

	// Late Update is called at end of each frame
	void LateUpdate () {
        if (fadingEnabled && fading) {
            FadeToBlack();
        }
        else {
            FadeToCLear();
        }
        if (fadingTextEnabled) {
            FadeInText();
        }
        else {
            FadeOutText();
        }
        SetView();
    }

    void FadeInText() {
        guiText.color = Color.Lerp(guiText.color, Color.white, fadeSpeed * Time.deltaTime);
    }
    void FadeOutText() {
        guiText.color = Color.Lerp(guiText.color, Color.clear, fadeSpeed * Time.deltaTime);
    }
    void FadeToBlack() {
        guiTexture.color = Color.Lerp(guiTexture.color, Color.black, fadeSpeed * Time.deltaTime);
    }
    void FadeToCLear() {
        guiTexture.color = Color.Lerp(guiTexture.color, Color.clear, fadeSpeed * Time.deltaTime);
    }

    IEnumerator TextViewer() {
        while (true) {
            yield return new WaitForSeconds(2);
            if (textQueue.Count > 0) {
                guiText.text = textQueue.Dequeue();
                fadingTextEnabled = true;
            }
            yield return new WaitForSeconds(displayTime);
            fadingTextEnabled = false;
        }
    }

    IEnumerator CameraSwitcher()
    {
        // run this cooroutine
        while (true)
        {
            // While there is nothing interesting to see
            while (idleCamera)
            {
                fading = false;
                idleCameras = GameObject.FindGameObjectsWithTag("Camera-spot").ToList();
                PickNewIdleCamera();

                yield return new WaitForSeconds(idleCamSwitchTime);
                fading = true;
                yield return new WaitForSeconds(1);
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

    public void DisplayText(string text) {
        textQueue.Enqueue(text);
    }
}
