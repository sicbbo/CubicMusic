using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectInfo
{
    public GameObject goPrefab;
    public int count;
    public Transform poolParent;
}

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;

    [SerializeField] private ObjectInfo[] objectInfo = null;

    public Queue<GameObject> noteQueue = new Queue<GameObject>();

    private void Start()
    {
        instance = this;
        noteQueue = InsertQueue(objectInfo[0]);
    }

    private Queue<GameObject> InsertQueue(ObjectInfo objectInfo)
    {
        Queue<GameObject> queue = new Queue<GameObject>();
        for (int i = 0; i < objectInfo.count; i++)
        {
            GameObject clone = Instantiate(objectInfo.goPrefab, transform.position, Quaternion.identity);
            clone.SetActive(false);
            if (objectInfo.poolParent != null)
                clone.transform.SetParent(objectInfo.poolParent);
            else
                clone.transform.SetParent(this.transform);

            queue.Enqueue(clone);
        }

        return queue;
    }
}