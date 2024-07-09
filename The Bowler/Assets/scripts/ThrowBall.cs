using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowBall : MonoBehaviour
{
    private Rigidbody ballRB;


    private List<Vector3> mousepositions = new List<Vector3>();
    private Vector3 midPoint;
    private Vector3 midDistance;
    private Vector3 ballForce;
    private Vector3 ballTorque;
    private float mousedistance;
    private float spin;
    private const int MOUSE_POSITION_INTERVAL = 1;
    private const int SPEED_MODIFIER = 4;
    private const float SPIN_MODIFIER = -2.66f;
    private const float OIL_ACCELERATION = 1.25f;
    private const int OIL_ACCELERATION_INTERVAL = 50;
    private int mouse_current_interval;
    private int oil_current_interval;
    private float initial_time;
    private float end_time;
    private float total_time;


    private bool gotinittime;
    private bool ballthrown;
    private bool hashit;


    private LineRenderer trace;
    // Start is called before the first frame update
    void Awake()
    {
        ballRB = GetComponent<Rigidbody>();


        mouse_current_interval = 0;
        oil_current_interval = 0;
        initial_time = 0;
        end_time = 0;

        gotinittime = false;
        ballthrown = false;
        hashit = false;

        trace = GameObject.FindWithTag("TraceThrow").GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!ballthrown && Input.GetMouseButton(0))
        {
            if (!gotinittime)
            {
                initial_time = Time.time;

                gotinittime = true;
            }


            if (mouse_current_interval > 0)
            {
                if (mouse_current_interval >= MOUSE_POSITION_INTERVAL)
                {
                    mouse_current_interval = 0;
                }
                else
                {
                    mouse_current_interval++;
                }
            }
            else
            {
                mousepositions.Add(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1.0f)));

                mouse_current_interval++;
            }


            if (mousepositions.Count > 0)
            {
                //trace throw path


                trace.positionCount = mousepositions.Count;
                for (int i = 0; i < mousepositions.Count; i++)
                {
                    Vector3 pos = new Vector3(mousepositions[i].x, mousepositions[i].y, mousepositions[i].z);

                    trace.SetPosition(i, pos);
                    trace.startWidth = 0.005f;
                    trace.endWidth = 0.005f;
                }
            }
        }


        if (!Input.GetMouseButton(0))
        {
            if (mousepositions.Count >= 2 && !ballthrown)
            {
                ballthrown = true;
                

                end_time = Time.time;
                total_time = end_time - initial_time;


                mousedistance = Vector3.Distance(mousepositions[0], mousepositions[mousepositions.Count - 1]);

                ballForce.x = (mousepositions[mousepositions.Count - 1].y - mousepositions[0].y) * SPEED_MODIFIER;
                ballForce.y = mousepositions[mousepositions.Count - 1].x - mousepositions[0].x;
                ballForce.z = (mousepositions[mousepositions.Count - 1].z - mousepositions[0].z) * SPEED_MODIFIER;

                ballRB.velocity = ballForce / total_time;

                ballRB.AddForce(ballForce, ForceMode.Impulse);


                //add torque on z axis to spin it


                midPoint = mousepositions[mousepositions.Count / 2];
                midDistance = (mousepositions[mousepositions.Count - 1] + mousepositions[0]) / 2;
                mousedistance = Vector3.Distance(mousepositions[0], mousepositions[mousepositions.Count - 1]);
                spin = (SPIN_MODIFIER * (midPoint.z - midDistance.z)) / mousedistance;

                ballTorque = new Vector3(spin, 0f, 0f);

                ballRB.AddTorque(ballTorque, ForceMode.VelocityChange);
            }



            if (!hashit && ballthrown)
            {
                if (oil_current_interval >= OIL_ACCELERATION_INTERVAL)
                {
                    ballRB.velocity *= OIL_ACCELERATION;

                    ballRB.AddTorque(ballTorque, ForceMode.VelocityChange);


                    oil_current_interval = 0;

                    Debug.Log("Ball Speed: " + ballRB.velocity);
                }
                else
                {
                    oil_current_interval++;
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 7)
        {
            hashit = true;
        }
    }
}