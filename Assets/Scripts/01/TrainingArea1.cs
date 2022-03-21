using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrainingArea1 : MonoBehaviour
{
    //public Button btn;
    public Transform potionTrans;
    public GameObject endurancePotionPrefab;
    private GameObject endurancePotionGo;

    public PlayerAgent1 agent;

    void Start()
    {
        //this.btn.onClick.AddListener(() => {
        //    var rand = Random.value * 8 -4;    //[0.0 ~ 1.0] * 8 -> [0.0 ~ 8.0] -4  --> [-4.0 ~ 4.0]
        //    var tPos = new Vector3(rand, 0.5f, rand);
        //    this.potionTrans.localPosition = tPos;
        //    var dis = Vector3.Distance(tPos, this.agent.transform.localPosition);
        //    Debug.Log(dis);
        //});    
    }

    public void ResetArea()
    {
        var rand = Random.value * 8 -4;    //[0.0 ~ 1.0] * 8 -> [0.0 ~ 8.0] -4  --> [-4.0 ~ 4.0]
        var tPos = new Vector3(rand, 0.5f, rand);
        this.potionTrans.localPosition = tPos;
        this.agent.transform.localPosition = new Vector3(0, 0.6f, 0);

        //포이즌 포션 못먹고 에피소드 종료 되었을경우 제거 
        if (this.endurancePotionGo != null)
        {
            Destroy(this.endurancePotionGo);
        }

        if (Random.value * 100 <= 50)
        {
            var randXZ = Random.value * 8 - 4;
            var tpos = new Vector3(randXZ, 0.5f, randXZ);
            this.endurancePotionGo = Instantiate(this.endurancePotionPrefab, this.transform);
            this.endurancePotionGo.transform.localPosition = tpos;
        }
    }
}
