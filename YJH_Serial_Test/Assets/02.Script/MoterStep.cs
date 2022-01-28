using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoterStep : MonoBehaviour
{
    //--- for the moter----
    enum CHANNEL
    {
        moter1 = 0, // left
        moter2 = 1  // right
    }
    enum DIR
    {
        CW = 0,
        CCW = 1
    }
    //enum SPEED
    //{
    //    slow = 0,
    //    fast =1
    //}
    enum MODE
    {
        stop = 0,
        infinity = 1
    }
    //----------------------


    //통신연결을 위한 주축
    public SerialConnect main_serial;

    // 빠른 버튼 관리를 위해 리스트화
    public List<Button> btn_dir = new List<Button>();

    private void Awake()
    {
        if (main_serial == null) { main_serial = GetComponent<SerialConnect>(); }
    }
    void Start()
    {
        //테스트를 위한 버튼식 입력 방식
        btn_dir[0].onClick.AddListener(OnClick_Stop);
        btn_dir[1].onClick.AddListener(OnClick_Up);
        btn_dir[2].onClick.AddListener(OnClick_Down);
        btn_dir[3].onClick.AddListener(OnClick_Left);
        btn_dir[4].onClick.AddListener(OnClick_Right);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //스페이스 누르면 정지함수 발동
            OnClick_Stop();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            OnClick_Up();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            OnClick_Down();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            OnClick_Left();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            OnClick_Right();
        }

        //test
        if (Input.GetKeyDown(KeyCode.Q))
        {
            byte[] test_ = { 240, 0, 5, 4, 100, 16, 100, 4, 247 };
            main_serial.sp_main.Write(test_, 0, test_.Length);
        }

    }

    private void OnClick_Stop()
    {
        //모터중지 함수
        bit_2Moter_Step(0, MODE.stop, CHANNEL.moter1, DIR.CW, CHANNEL.moter2, DIR.CW);
    }
    private void OnClick_Up()
    {
        //전진 함수 1모터  
        bit_2Moter_Step(0, MODE.infinity, CHANNEL.moter1, DIR.CCW, CHANNEL.moter2, DIR.CW);
    }
    private void OnClick_Down()
    {
        // 후진 함수
        bit_2Moter_Step(0, MODE.infinity, CHANNEL.moter1, DIR.CW, CHANNEL.moter2, DIR.CCW);
    }
    private void OnClick_Left()
    {
        //왼 좌 회전 함수
        bit_2Moter_Step(0, MODE.infinity, CHANNEL.moter1, DIR.CW, CHANNEL.moter2, DIR.CW);
    }
    private void OnClick_Right()
    {
        //오른 우 회전 함수
        bit_2Moter_Step(0, MODE.infinity, CHANNEL.moter1, DIR.CCW, CHANNEL.moter2, DIR.CCW);
    }

    /// <summary>
    /// 모터1개 돌리는 함수. 
    /// </summary>
    /// <param name="step"></ 움직일 거리 param>
    /// <param name="channel"></ moter1 = 00, moter2 = 01 param>
    /// <param name="dir"></ CW = 0, CCW = 1 param>
    /// <param name="mode"></ stop(정지에만 사용) = 0 , infinity(step 값이 있으면 움직이고 종료 0이면 무한회전) = 1  param>
    private void bit_1Moter_Step(int step, CHANNEL channel, DIR dir, MODE mode)//speed는 항상 0
    {
        //<<로이 data 규칙>>
        //data1 // step(LSB)
        //-----------
        //      //bit(0:1) : step(MSB)
        //      //bit(2:3) : < channel >
        //data2 //bit(4) : < dir >
        //      //bit(5) : < speed >
        //      //bit(6) : < mode >

        //왼쪽(moter1)은 CCW 전진, CW 후진
        //오른쪽 (moter2)은 CW 전진 ,  CCW 후진

        int data1_step = step % 128;
        int data2_step = step / 128;
        int c = (int)channel * 4;
        int d = (int)dir * 16;
        int speed = 0;
        int m = (int)mode * 64;

        byte data1 = (byte)data1_step;
        byte data2 = (byte)(data2_step + c+ d + speed + m);

        print("data1 = " +data1.ToString());
        print("data2 = " + data2.ToString());

        Wirte_RoE_Moter(data1,data2);

    }
    /// <summary>
    /// 모터 2개 돌리는 함수 전달함수.
    /// </summary>
    private void bit_2Moter_Step(int step, MODE mode, CHANNEL c1 , DIR d1, CHANNEL c2, DIR d2)
    {
        //bit_1Moter_Step(step, c1, d1, mode);
        //bit_1Moter_Step(step, c2, d2, mode);

        int step1 = step % 128;
        int step2 = step / 128;
        
        int cha1 = (int)c1 * 4;
        int cha2 = (int)c2 * 4;

        int dir1 = (int)d1 * 16;
        int dir2 = (int)d2 * 16;

        int speed = 0;
        int m = (int)mode * 64;

        byte data1 = (byte)step1;
        byte data2 = (byte)(step2 + cha1 + dir1 + speed + m);
        byte data3 = (byte)step1;
        byte data4 = (byte)(step2 + cha2 + dir2 + speed + m);


        print("data1 = " + data1.ToString());
        print("data2 = " + data2.ToString());
        print("data3 = " + data3.ToString());
        print("data4 = " + data4.ToString());

        Wirte_RoE_Moter(data1, data2, data3, data4);
    }



    //로이에 명령을 내리도록 입력하는 함수, 
    public void Wirte_RoE_Moter(byte a , byte b)
    {
        byte[] moter = { 0xF0, 0x00, 0x05, 0x02, a, b, 0xF7 };

        main_serial.sp_main.Write(moter, 0, moter.Length);
    }

    public void Wirte_RoE_Moter(byte a, byte b,byte c, byte d) // 모터 2개 일때
    {
        
        byte[] moter = { 0xF0, 0x00, 0x05, 0x04, a, b, c, d, 0xF7 };

        main_serial.sp_main.Write(moter, 0, moter.Length);
    }
}
