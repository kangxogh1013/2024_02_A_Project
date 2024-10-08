using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VeicleManager : MonoBehaviour
{
    public Vehicle[] vehicles;   //탈것 객체 배열 선언 한다.

    public Car car;              //자동차 선언
    public Bicycle bicycle;      //자전거 선언

    float Timer;                 //간단한 시간 float ㅂ변수 선언
    

    // Update is called once per frame
    void Update()
    {
        car.Move();            //이동 함수 호출
        bicycle.Move();

        Timer -= Time.deltaTime; //시간을 줄인다.

        if(Timer < 0)            //1초마다 호출 되게한다.
        {
            car.Horn();           //경적 함수 호출
            bicycle.Horn();
            Timer = 1;
        }
    }
}
