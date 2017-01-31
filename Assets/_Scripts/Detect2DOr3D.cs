using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ToggleEvent : UnityEvent<bool> { }

public class Detect2DOr3D : MonoBehaviour {
    public GameObject m_caveMesh;
    public GameObject m_mainCamera;
    public GameObject m_fpsCamera;

    //public CustomSimplePlayer script3D;
    //public Custom2DSimplePlayer script2D;
    //public GameObject m_mapGenerator;

    public GameObject m_playerController;

    // Use this for initialization
    void Start () {
        DetectOrientation();
    }

    void DetectOrientation () {
        if (m_caveMesh.transform.rotation.eulerAngles.x == 270 || m_caveMesh.transform.rotation.eulerAngles.x == -90) {
            m_fpsCamera.SetActive(false);
            m_mainCamera.SetActive(true);

            m_playerController.AddComponent<CapsuleCollider2D>();
            m_playerController.AddComponent<Rigidbody2D>();
            m_playerController.GetComponent<Rigidbody2D>().gravityScale = 0;  // This prevents the player character from sliding down, since the map is tilted 270 degrees or -90 degrees.
            m_playerController.AddComponent<Custom2DSimplePlayer>();
        } else if (m_caveMesh.transform.rotation.eulerAngles.x == 0) {
            m_fpsCamera.SetActive(true);
            m_mainCamera.SetActive(false);

            m_playerController.AddComponent<CapsuleCollider>();
            m_playerController.AddComponent<Rigidbody>();
            m_playerController.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            m_playerController.AddComponent<CustomSimplePlayer>();
        }
    }
}
