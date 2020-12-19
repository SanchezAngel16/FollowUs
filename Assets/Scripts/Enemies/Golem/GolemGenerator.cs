using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemGenerator : MonoBehaviour
{
    private void Start()
    {
        int golemCount = Random.Range(1, 3);
        for (int i = 0; i < golemCount; i++)
        {
            GameObject newGolem = Instantiate(PrefabManager.Instance.golem, transform);
            /*object[] initData = new object[3];
            initData[0] = i;
            initData[1] = transform.position.x;
            initData[2] = transform.position.y;
            
            GameObject newGolem = PhotonNetwork.InstantiateRoomObject(PrefabManager.Instance.golem.name, transform.position, Quaternion.identity, 0, initData);
            */
            //newGolem.transform.SetParent(transform);

            Golem g = newGolem.transform.GetChild(1).GetComponent<Golem>();
            g.setGolemAttributes(i, Util.getCorners(transform.position));
        }
    }
}
