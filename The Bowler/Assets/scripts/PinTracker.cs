using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinTracker : MonoBehaviour
{

    private List<pinPosition> initPinPos = new List<pinPosition>();
    private List<pinPosition> currentPinPos = new List<pinPosition>();


    private bool cancheck;
    private bool canKill;
    private bool gotInfo;

    public GameObject pinObj;

    void Awake()
    {
        cancheck = true;
        canKill = false;
        gotInfo = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gotInfo)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                initPinPos.Add(transform.GetChild(i).GetComponent<pinInfo>().getPinPos());
                currentPinPos.Add(transform.GetChild(i).GetComponent<pinInfo>().getPinPos());
            }

            gotInfo = true;
        }

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

            //Debug.Log("check");
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
            //Debug.Log("cankill");

            StartCoroutine(checkWait());
        }
    }

    IEnumerator killPin()
    {
        yield return new WaitForSeconds(2);

        {
            for (int i = 0; i < transform.childCount; i++)
            {
                for (int j = 0; j < currentPinPos.Count; j++)
                {
                    //Debug.Log(transform.GetChild(i).name + ":" + currentPinPos[i].pinNum + ": " + (transform.GetChild(i).GetComponent<pinInfo>().getPinPos().pinNum == currentPinPos[i].pinNum && (currentPinPos[i].pinPos != initPinPos[i].pinPos || currentPinPos[i].pinRot != initPinPos[i].pinRot) && currentPinPos[i].isHit));

                    if (transform.GetChild(i).GetComponent<pinInfo>().getPinPos().pinNum == currentPinPos[i].pinNum &&
                        (currentPinPos[i].pinPos != initPinPos[i].pinPos || currentPinPos[i].pinRot != initPinPos[i].pinRot) &&
                        currentPinPos[i].isHit)
                    {
                        //Debug.Log("kill: " + transform.GetChild(i).name);
                        Destroy(transform.GetChild(i).gameObject);

                        break;
                    }
                }
            }
        }
    }
}
