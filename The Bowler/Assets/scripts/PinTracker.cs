using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinTracker : MonoBehaviour
{

    public List<pinPosition> initPinPos = new List<pinPosition>();
    public List<pinPosition> currentPinPos = new List<pinPosition>();


    private bool cancheck;
    private bool canKill;

    void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            initPinPos.Add(transform.GetChild(i).GetComponent<pinInfo>().getPinPos());
            currentPinPos.Add(transform.GetChild(i).GetComponent<pinInfo>().getPinPos());
        }

        cancheck = true;
        canKill = false;
    }

    // Update is called once per frame
    void Update()
    {
        checkPos();
        killCheck();
        
        if (canKill)
        {
            StartCoroutine(killPin());
        }
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
        }
    }

    void killCheck()
    {
        if (cancheck)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                for (int j = 0; j < currentPinPos.Count; j++)
                {
                    if (transform.GetChild(i).GetComponent<pinInfo>().getPinPos().pinNum == currentPinPos[i].pinNum &&
                        (currentPinPos[i].pinPos != initPinPos[i].pinPos || currentPinPos[i].pinRot != initPinPos[i].pinRot) &&
                        currentPinPos[i].isHit)
                    {
                        canKill = true;

                        break;
                    }
                }
            }

            StartCoroutine(checkWait());
        }
    }

    IEnumerator killPin()
    {
        yield return new WaitForSeconds(5);

        {
            for (int i = 0; i < transform.childCount; i++)
            {
                for (int j = 0; j < currentPinPos.Count; j++)
                {
                    if (transform.GetChild(i).GetComponent<pinInfo>().getPinPos().pinNum == currentPinPos[i].pinNum &&
                        (currentPinPos[i].pinPos != initPinPos[i].pinPos || currentPinPos[i].pinRot != initPinPos[i].pinRot) &&
                        currentPinPos[i].isHit)
                    {
                        Debug.Log("kill: " + transform.GetChild(i).name);
                        Destroy(transform.GetChild(i).gameObject);

                        break;
                    }
                }
            }
        }
    }
}
