using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class GameObjManager : MonoBehaviour
{
    [Header("Simulation Settings")]
    [SerializeField]
    private float m_TopBound = 400.0f;
    [SerializeField]
    private float m_BottomBound = -400.0f;
    [SerializeField]
    private float m_LeftBound = -500f;
    [SerializeField]
    private float m_RightBound = 500f;
    [SerializeField]
    private float m_WorldResetBottom;
    [SerializeField]
    private float m_WorldResetTop;
    [SerializeField] 
    private Transform m_RootObject;

    [Header("Whale Settings")]
    [SerializeField]
    private GameObject m_Prefab;
    [SerializeField]
    private float m_Speed = 3f;
    private List<GameObject> m_GOs = new List<GameObject>();

    private void Update()
    {
        Debug.Log("Update");
        #if UNITY_EDITOR
        if (Input.GetKeyDown("space"))
           SpawnGOs();
        #endif

        #if UNITY_IOS || UNITY_ANDROID
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
           SpawnGOs();
        #endif
        Move();
    }


    public void SpawnGOs(int count = 1)
    {
        m_WorldResetTop = m_RootObject.position.z + m_TopBound;
        m_WorldResetBottom = m_RootObject.position.z + m_BottomBound;

        for(int i =0; i < count; i++)
        {
            AddGO(m_Prefab);
        }
    }

    void AddGO(GameObject prefab)
    {
        float xVal = UnityEngine.Random.Range(m_LeftBound, m_RightBound) + m_RootObject.position.x;
        float yVal = UnityEngine.Random.Range(-50.0f, 50.0f) + m_RootObject.position.y;
        float zVal = (m_RootObject.position.z) + UnityEngine.Random.Range(m_BottomBound, m_TopBound);
        Vector3 pos = new Vector3(xVal, yVal, zVal);
        Quaternion rotation = new Quaternion(m_RootObject.rotation.x, m_RootObject.rotation.y, m_RootObject.rotation.z, m_RootObject.rotation.w);
        GameObject go = Object.Instantiate(prefab, pos, rotation, m_RootObject);
        m_GOs.Add(go);
    }


    private void Move()
    {
        Quaternion originalRot = m_RootObject.transform.rotation;    
        m_RootObject.transform.rotation = originalRot * Quaternion.AngleAxis(Time.deltaTime * m_Speed * 0.5f, Vector3.forward);
        foreach(var go in m_GOs)
        {
            go.transform.position -= (Vector3)(Time.deltaTime * m_Speed * math.forward(go.transform.rotation));
        }
    }
}






