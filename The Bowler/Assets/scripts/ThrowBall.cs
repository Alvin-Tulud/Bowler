using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowBall : MonoBehaviour
{
    private Rigidbody ballRB;


    public List<Vector3> mousepositions = new List<Vector3>();
    public Vector3 midpoint;
    public Vector3 mousedistance;
    public float spin;
    private const int POSITION_INTERVAL = 2;
    private int current_interval;
    private float initial_time;
    private float end_time;
    private float total_time;


    private bool gotinittime;
    private bool ballthrown;
    // Start is called before the first frame update
    void Start()
    {
        ballRB = GetComponent<Rigidbody>();


        current_interval = 0;
        initial_time = 0;
        end_time = 0;


        ballthrown = false;
        
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


            if (current_interval > 0)
            {
                if (current_interval >= POSITION_INTERVAL)
                {
                    current_interval = 0;
                }
                else
                {
                    current_interval++;
                }
            }
            else
            {
                mousepositions.Add(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1.0f)));
                //Debug.Log("Camera pos: " + Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1.0f)));
                //Debug.Log("regular pos: " + Input.mousePosition);

                current_interval++;
            }

        }


        if (!Input.GetMouseButton(0))
        {
            if (mousepositions.Count >= 3 && !ballthrown)
            {
                ballthrown = true;


                end_time = Time.time;
                total_time = end_time - initial_time;


                mousedistance.x =  mousepositions[mousepositions.Count - 1].x - mousepositions[0].x;
                mousedistance.z = mousepositions[mousepositions.Count - 1].y - mousepositions[0].y;

                Vector3 ballForce = mousedistance * 2;
                ballRB.AddForce(ballForce, ForceMode.Impulse);

                //add torque on z axis to spin it

                midpoint = mousepositions[mousepositions.Count / 2];
                spin = midpoint.x / Vector3.Distance(mousepositions[0], mousepositions[mousepositions.Count - 1]);

                Vector3 ballTorque = new Vector3(spin, 0f, 0f);

                ballRB.AddRelativeTorque(ballTorque, ForceMode.Impulse);

                Debug.DrawRay(transform.position, ballTorque * 1000, Color.cyan, 1000f);
                Debug.DrawRay(transform.position, mousedistance * 1000, Color.magenta, 1000f);
            }
        }
    }
}
