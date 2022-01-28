using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO.Ports;

public class InputColors : MonoBehaviour
{
    //��ſ����� ���� ����
    public SerialConnect main_serial;
    
    // ���� ��ư ������ ���� ����Ʈȭ
    public List<Button> btn_Colors = new List<Button>();

    public List<Button> btn_piano = new List<Button>();

    public Button SoundPlay;

    byte start_bytet = 0xF0;
    byte end_byte = 0xF7;


    // octave �ܰ踦 ǥ���ϴ� Text �� ����
    public Text octave;
    int int_octave;

    private void Awake()
    {
        //serialConnect ��� ��� ���� ������Ʈ �� ����������ʴٸ� ����Ǳ�.
        if(main_serial == null) { main_serial = GetComponent<SerialConnect>(); }
    }
    void Start()
    {
        //�׽�Ʈ�� ���� ��ư�� �Է� ���
        btn_Colors[0].onClick.AddListener(OnClick_Red_btn);
        btn_Colors[1].onClick.AddListener(OnClick_Green_btn);
        btn_Colors[2].onClick.AddListener(OnClick_Blue_btn);
        btn_Colors[3].onClick.AddListener(OnClick_White_btn);

        SoundPlay.onClick.AddListener(OnClick_Sound);

        btn_piano[0].onClick.AddListener(OnClick_Minus);
        btn_piano[1].onClick.AddListener(OnClick_Plus);

    }

    void Update()
    {
        
        
    }

    private void OnClick_Sound()
    {
        byte[] test = {0xF0,0x00,0x0A,0x07,0x00,0x01,0x00,0x00,0x10,0x68,0x07,0xF7 };
        //             �����ڵ�  output,����,    ,tone,cmd ,frequency, duration ,end 
        WriteData(test);

    }


    private void OnClick_Red_btn()
    {
        //0xF0 , 0x00 , 0x0A , 0x06 , 0x00 , 0x00 , 0x7F , 0x00 , 0x00 , 0x01 , 0xF7
        List<byte> red = Color_OutPut(0x7F, 0x00, 0x00, 0x01);
        WriteData(red);
    }
    private void OnClick_Green_btn()
    {
        //0xF0 , 0x00 , 0x0A , 0x06 , 0x00 , 0x00 , 0x00 , 0x7F , 0x00 , 0x02 , 0xF7
        List<byte> green = Color_OutPut(0x00, 0x7F, 0x00, 0x02);
        WriteData(green);
    }
    private void OnClick_Blue_btn()
    {
        //0xF0 , 0x00 , 0x0A , 0x06 , 0x00 , 0x00 , 0x00 , 0x00 , 0x7F , 0x04 , 0xF7
        List<byte> blue = Color_OutPut(0x00, 0x00, 0x7F, 0x04);
        WriteData(blue);
    }
    private void OnClick_White_btn()
    {
        //0xF0 , 0x00 , 0x0A , 0x06 , 0x00 , 0x00 , 0x7F , 0x7F , 0x7F , 0x06 , 0xF7
        List<byte> white = Color_OutPut(0x7F, 0x7F, 0x7F, 0x06);
        WriteData(white);
    }

    List<byte> Color_OutPut(byte r,byte g, byte b,byte msb)
    {
        List<byte> color = new List<byte>();

        color.Add(start_bytet);
        color.Add(0x00);
        color.Add(0x0A);
        color.Add(0x06);
        color.Add(0x00);
        color.Add(0x00);
        
        color.Add(r);//�������� ���� �����̴�. 
        color.Add(g);//
        color.Add(b);//
        color.Add(msb);

        color.Add(end_byte);
        return color;
    }

    List<byte> Sound(byte num)
    {
        List<byte> sound = new List<byte>();

        sound.Add(start_bytet);
        sound.Add(0x00);
        sound.Add(0x0A);
        sound.Add(0x03);
        sound.Add(0x00);
        sound.Add(0x03);

        sound.Add(num);

        sound.Add(end_byte);
        return sound;
    }


    public void WriteData(byte[] data)
    {
        //�ø��� ��Ʈ�� ���� ���� �ʴٸ� ����
        if (main_serial.sp_main.IsOpen == false) return;

        //�������� ����� �ø��� �Է� �ϱ� 
        main_serial.sp_main.Write(data,0,data.Length);

    }

    public void WriteData(List<byte> data)
    {
        //�ø��� ��Ʈ�� ���� ���� �ʴٸ� ����
        if (main_serial.sp_main.IsOpen == false) return;

        //����Ʈ�� �迭�� ��ȯ
        byte[] read = data.ToArray();
        //�������� ����� �ø��� �Է� �ϱ�
        main_serial.sp_main.Write(read, 0, read.Length);
    }

    public void OnClick_Minus()
    {
        int_octave--;// ��Ÿ�긦 �ϳ� ���߰�
        octave.text = int_octave.ToString();// ���ڷ� ǥ�����ش�.

        print(octave.text);
    }

    public void OnClick_Plus()
    {
        int_octave++;// ��Ÿ�긦 �ϳ� ���̰�
        octave.text = int_octave.ToString();// ���ڷ� ǥ�����ش�.

        print(octave.text);
    }

}
