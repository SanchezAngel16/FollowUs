using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    private LightningGenerator[] lightnings = new LightningGenerator[4];
    private Vector2[] lightningPoints;

    private float waitTime;
    private float waitLightningTime;
    private float startWaitTime;
    private float startWaitLightningTime;
    private void Start()
    {
        startWaitTime = 7f;
        startWaitLightningTime = 5f;
        waitTime = startWaitTime;
        waitLightningTime = startWaitLightningTime;
        setLightningPoints();
        generateLightnings();
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
        if(waitTime <= 2 && waitTime > 0)
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
        int randNextPos = -1;
        for (int i = 0; i < lightnings.Length; i++)
        {
            do
            {
                randNextPos = Random.Range(0, lightnings.Length);
            } while (randNextPos == i || lightnings[randNextPos].nextIndex == i);
            lightnings[i].setNext(lightnings[randNextPos].firePoint.transform);
            lightnings[i].nextIndex = randNextPos;
        }
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
        for (int i = 0; i < 4; i++)
        {
            do
            {
                newRandPos = Random.Range(0, lightningPoints.Length);
            } while (randomPos.Contains(newRandPos));
            randomPos.Add(newRandPos);
        }
        randomPos.Sort();

        for (int i = 0; i < 4; i++)
        {
            GameObject lightningGenerator = Instantiate(EnemyPrefabManager.Instance.lightningGenerator, transform);
            lightningGenerator.transform.position = lightningPoints[randomPos[i]];
            lightnings[i] = lightningGenerator.GetComponent<LightningGenerator>();
        }
        setShootingDirections();
    }
}
