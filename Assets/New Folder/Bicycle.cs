using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//구체 클라스 : 자전거
public class Bicycle : Vehicle
{
    //MOVe 메서드 재정의
    public override void Move()
    {
        base.Move();   //기본이동
        //자전거 만의 추가 동작
        transform.Rotate(0, 10, 0);
    }

    public override void Horn()
    {
        Debug.Log("자전거 경적 : 딩딩");
    }
}
