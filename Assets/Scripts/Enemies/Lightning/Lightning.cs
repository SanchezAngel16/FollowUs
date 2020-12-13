using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviourPunCallbacks
{
    private const int lightningGeneratorCount = 4;
    private LightningGenerator[] lightnings = new LightningGenerator[lightningGeneratorCount];
    private Vector2[] lightningPoints;

    private float waitTime;
    private float waitLightningTime;
    [SerializeField]
    private float startWaitTime = 7f;
    [SerializeField]
    private float startWaitLightningTime = 5f;

    private void Awake()
    {
        setLightningPoints();
    }
    private void Start()
    {
        waitTime = startWaitTime;
        waitLightningTime = startWaitLightningTime;
        if (PhotonNetwork.IsMasterClient)
        {
            generateLightnings();
        }
    }

    private void setLightningPoints()
    {
        lightningPoints = new Vector2[9];
        float xPos = -3.5f;
        float increment = 3.5f;
        int cont = 0;
        Vector2 currentPos = transform.position;
        for(int i = 0; i < 9; i++)
        {
            if(i >= 3 && i < 6) lightningPoints[i] = new Vector2(currentPos.x + xPos, currentPos.y + 3);
            else if(i >= 6) lightningPoints[i] = new Vector2(currentPos.x + xPos, currentPos.y + 0);
            else lightningPoints[i] = new Vector2(currentPos.x + xPos, currentPos.y + -3.5f);

            cont++;
            xPos += increment;

            if (cont == 3)
            {
                cont = 0;
                xPos = -3.5f;
            }
        }
    }

    private void Update()
    {
        if (waitTime <= 2 && waitTime > 0)
        {
            playAnimations(true);
        }

        if (waitTime < 0)
        {
            if (waitLightningTime >= 0)
            {
                playAnimations(false);
                turnLightnings(true);
                waitLightningTime -= Time.deltaTime;
            }
            else
            {
                turnLightnings(false);
                setShootingDirections();
                waitLightningTime = startWaitLightningTime;
                waitTime = startWaitTime;
            }
        }
        else
        {
            waitTime -= Time.deltaTime;
        }
    }

    private void setShootingDirections()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        object[] indexes = new object[lightningGeneratorCount];
        object[] randomPositions = new object[lightningGeneratorCount];
        int randNextPos = -1;
        for (int i = 0; i < lightnings.Length; i++)
        {
            do
            {
                randNextPos = Random.Range(0, lightnings.Length);
            } while (randNextPos == i /*|| lightnings[randNextPos].nextIndex == i*/);
            indexes[i] = i;
            randomPositions[i] = randNextPos;
        }
        photonView.RPC("setShootDirections", RpcTarget.All, indexes, randomPositions);
    }

    private void playAnimations(bool play)
    {
        for (int i = 0; i < lightnings.Length; i++) lightnings[i].playAnimations(play);
    }


    private void turnLightnings(bool turn)
    {
        for (int i = 0; i < lightnings.Length; i++)
        {
            lightnings[i].shoot(turn);
        }
    }

    private void generateLightnings()
    {
        List<int> randomPos = new List<int>();
        int newRandPos = 0;
        for (int i = 0; i < lightningGeneratorCount; i++)
        {
            do
            {
                newRandPos = Random.Range(0, lightningPoints.Length);
            } while (randomPos.Contains(newRandPos));
            randomPos.Add(newRandPos);
        }
        randomPos.Sort();

        object[] positions = new object[lightningGeneratorCount];
        
        for (int i = 0; i < lightningGeneratorCount; i++)
        {
            positions[i] = randomPos[i];
        }

        photonView.RPC("instantiateLightnings", RpcTarget.All, positions);
        setShootingDirections();
    }

    #region RPC Calls

    [PunRPC]
    public void instantiateLightnings(object[] positions)
    {
        for (int i = 0; i < 4; i++)
        {
            GameObject lightningGenerator = Instantiate(PrefabManager.Instance.lightningGenerator, transform);
            lightningGenerator.transform.position = lightningPoints[(int)positions[i]];
            lightnings[i] = lightningGenerator.GetComponent<LightningGenerator>();
        }
    }

    [PunRPC]
    public void setShootDirections(object[] indexes, object[] nextPositions)
    {
        for(int i = 0; i < lightnings.Length; i++)
        {
            lightnings[(int)indexes[i]].setNext(lightnings[(int)nextPositions[i]].firePoint.transform);
            lightnings[(int)indexes[i]].nextIndex = (int)nextPositions[i];
        }
    }

    #endregion

}
