using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ToggleEvent : UnityEvent<bool> { }

public class Detect2DOr3D : MonoBehaviour {
    [SerializeField] ToggleEvent onToggleScript;
    
    public GameObject m_caveMesh;
    public GameObject m_mainCamera;
    public GameObject m_fpsCamera;
    //public GameObject m_mapGenerator;

    // Use this for initialization
    void Start () {
        DetectOrientation();
	}
	
	void DetectOrientation () {
        if (m_caveMesh.transform.rotation.eulerAngles.x == 270) {
            m_fpsCamera.SetActive(false);
            m_mainCamera.SetActive(true);
            //m_mapGenerator.GetComponent<>
        } else {
            m_fpsCamera.SetActive(true);
            m_mainCamera.SetActive(false);
        }
    }
}
