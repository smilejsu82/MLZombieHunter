using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class PlayerAgent1 : Agent
{
    public float speed = 2f;
    private Rigidbody rBody;
    public TrainingArea1 area;
    public Animator anim;

    public override void Initialize()
    {
        this.rBody = this.GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        this.rBody.velocity = Vector3.zero;
        this.rBody.angularVelocity = Vector3.zero;
        this.area.ResetArea();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(this.transform.localPosition);    //3
        sensor.AddObservation(area.potionTrans.localPosition);  //3
        sensor.AddObservation(Vector3.Distance(area.potionTrans.localPosition, this.transform.localPosition));  //1

        //facing potion to agent
        var c = this.transform.localPosition - area.potionTrans.localPosition;
        var nor = c.normalized;
        sensor.AddObservation(nor);//3

    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        var h = actions.DiscreteActions[0]; //0, 1, 2가 나오게 Heuristic에서 전달
        //h를 벡터로
        h -= 1; // 0, 1, 2 --> -1, 0, 1

        var v = actions.DiscreteActions[1];
        v -= 1;

        var dir = new Vector3(h, 0, v);

        var spd = (actions.DiscreteActions[2] + 1) * 0.5f; //1,2,3,4,5 * 0.5 --> max 2.5


        //Debug.Log(dir);

        if (dir != Vector3.zero)
        {
            //var movement = dir * this.speed * Time.fixedDeltaTime;
            float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
            this.transform.eulerAngles = Vector3.up * angle;

            var movement = this.transform.forward * spd * Time.fixedDeltaTime;
            this.rBody.MovePosition(this.transform.position + movement);

            this.anim.Play("RunFWD_AR");

        }
        else
        {
            this.anim.Play("IdleBattle01_AR");
        }

        if (this.transform.localPosition.y < -1f)
        {
            EndEpisode();
        }

        this.AddReward(1f / this.MaxStep);

    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var h = Input.GetAxisRaw("Horizontal"); //-1, 0, 1 ---> 0, 1, 2
        var v = Input.GetAxisRaw("Vertical");   //-1, 0, 1

        var discreteActionOut = actionsOut.DiscreteActions;
        discreteActionOut[0] = (int)h + 1;  //0: 왼쪽, 1: 안움직임, 2: 오른쪽
        discreteActionOut[1] = (int)v + 1;  //0: 위, 1: 안움직임, 2: 아래 

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag.Equals("HealthPotion"))
        {
            //Debug.Log("get potion");
            AddReward(1.0f);
            EndEpisode();
        }
        else if (collision.transform.tag.Equals("EndurancePotion"))
        {
            //Debug.Log("test endurance potion!");
            AddReward(-1.0f);
            Destroy(collision.gameObject);
            EndEpisode();
        }
    }
}
