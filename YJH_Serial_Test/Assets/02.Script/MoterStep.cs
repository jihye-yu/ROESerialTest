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


    //��ſ����� ���� ����
    public SerialConnect main_serial;

    // ���� ��ư ������ ���� ����Ʈȭ
    public List<Button> btn_dir = new List<Button>();

    private void Awake()
    {
        if (main_serial == null) { main_serial = GetComponent<SerialConnect>(); }
    }
    void Start()
    {
        //�׽�Ʈ�� ���� ��ư�� �Է� ���
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
            //�����̽� ������ �����Լ� �ߵ�
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
        //�������� �Լ�
        bit_2Moter_Step(0, MODE.stop, CHANNEL.moter1, DIR.CW, CHANNEL.moter2, DIR.CW);
    }
    private void OnClick_Up()
    {
        //���� �Լ� 1����  
        bit_2Moter_Step(0, MODE.infinity, CHANNEL.moter1, DIR.CCW, CHANNEL.moter2, DIR.CW);
    }
    private void OnClick_Down()
    {
        // ���� �Լ�
        bit_2Moter_Step(0, MODE.infinity, CHANNEL.moter1, DIR.CW, CHANNEL.moter2, DIR.CCW);
    }
    private void OnClick_Left()
    {
        //�� �� ȸ�� �Լ�
        bit_2Moter_Step(0, MODE.infinity, CHANNEL.moter1, DIR.CW, CHANNEL.moter2, DIR.CW);
    }
    private void OnClick_Right()
    {
        //���� �� ȸ�� �Լ�
        bit_2Moter_Step(0, MODE.infinity, CHANNEL.moter1, DIR.CCW, CHANNEL.moter2, DIR.CCW);
    }

    /// <summary>
    /// ����1�� ������ �Լ�. 
    /// </summary>
    /// <param name="step"></ ������ �Ÿ� param>
    /// <param name="channel"></ moter1 = 00, moter2 = 01 param>
    /// <param name="dir"></ CW = 0, CCW = 1 param>
    /// <param name="mode"></ stop(�������� ���) = 0 , infinity(step ���� ������ �����̰� ���� 0�̸� ����ȸ��) = 1  param>
    private void bit_1Moter_Step(int step, CHANNEL channel, DIR dir, MODE mode)//speed�� �׻� 0
    {
        //<<���� data ��Ģ>>
        //data1 // step(LSB)
        //-----------
        //      //bit(0:1) : step(MSB)
        //      //bit(2:3) : < channel >
        //data2 //bit(4) : < dir >
        //      //bit(5) : < speed >
        //      //bit(6) : < mode >

        //����(moter1)�� CCW ����, CW ����
        //������ (moter2)�� CW ���� ,  CCW ����

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
    /// ���� 2�� ������ �Լ� �����Լ�.
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



    //���̿� ����� �������� �Է��ϴ� �Լ�, 
    public void Wirte_RoE_Moter(byte a , byte b)
    {
        byte[] moter = { 0xF0, 0x00, 0x05, 0x02, a, b, 0xF7 };

        main_serial.sp_main.Write(moter, 0, moter.Length);
    }

    public void Wirte_RoE_Moter(byte a, byte b,byte c, byte d) // ���� 2�� �϶�
    {
        
        byte[] moter = { 0xF0, 0x00, 0x05, 0x04, a, b, c, d, 0xF7 };

        main_serial.sp_main.Write(moter, 0, moter.Length);
    }
}
