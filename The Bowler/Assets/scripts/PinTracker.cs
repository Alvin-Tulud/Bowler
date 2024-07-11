using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PinTracker : MonoBehaviour
{

    private List<pinPosition> initPinPos = new List<pinPosition>();
    private List<pinPosition> currentPinPos = new List<pinPosition>();


    private bool canCheck;
    private bool canKill;
    private bool gotInfo;
    private bool canReset;

    public GameObject pinObj;

    void Awake()
    {
        canCheck = true;
        canKill = false;
        gotInfo = false;
        canReset = false;
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
        canCheck = false;

        yield return new WaitForSeconds(5);

        canCheck = true;
    }

    void checkPos()
    {
        if (canCheck)
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
        if (canCheck)
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
        if (!canReset)
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

            canReset = true;
        }
    }

    public void resetPins()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy (transform.GetChild(i).gameObject);
        }


        for (int i = 0; i < initPinPos.Count; i++)
        {
            GameObject pinSpawn;

            pinSpawn = Instantiate(pinObj, initPinPos[i].pinPos, initPinPos[i].pinRot, transform);

            pinSpawn.name = "Pin_" + (i + 1).ToString("D2");
        }
    }

    public bool getCanReset() {  return canReset; }

    public void setCanReset(bool set) { canReset = set; }
}
