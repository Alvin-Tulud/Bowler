using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinTracker : MonoBehaviour
{

    private List<pinPosition> initPinPos = new List<pinPosition>();
    private List<pinPosition> currentPinPos = new List<pinPosition>();


    private bool cancheck;

    void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            initPinPos.Add(transform.GetChild(i).GetComponent<pinInfo>().getPinPos());
            currentPinPos.Add(transform.GetChild(i).GetComponent<pinInfo>().getPinPos());
        }
    }

    // Update is called once per frame
    void Update()
    {
        checkPos();
    }

    IEnumerator checkWait()
    {
        cancheck = false;

        yield return new WaitForSeconds(5);

        cancheck = true;
    }

    void checkPos()
    {
        if (cancheck)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                for (int j = 0; j < currentPinPos.Count; j++)
                {
                    if (transform.GetChild(i).GetComponent<pinInfo>().getPinPos().pinNum == currentPinPos[i].pinNum)
                    {
                        currentPinPos[i] = transform.GetChild(i).GetComponent<pinInfo>().getPinPos();
                    }
                }
            }

            StartCoroutine(checkWait());
        }
    }
}
