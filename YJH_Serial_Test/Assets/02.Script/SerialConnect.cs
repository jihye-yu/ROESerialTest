using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO.Ports;
using TMPro;




public class SerialConnect : MonoBehaviour
{
    //����� main ���� �ø��� ��Ʈ [sp_main]
    public SerialPort sp_main = new SerialPort();

    // ����� ��⿡���� serial port�������� �ٸ��� ������ �ش� �޼��� Ȯ�� ��
    public TextMeshProUGUI tmp_serial_state_message;
    // ���α׷��� ���� �ϰ���� ��� ���� 
    public TMP_Dropdown tdd_Divice;

    //����� ports �� ����Ʈ�� ���� �ɼ� �ִ�  Dropdown
    public TMP_Dropdown tdd_port;

    //DropDown�� �ɼ� ����Ʈ
    private List<TMP_Dropdown.OptionData> opt_ports = new List<TMP_Dropdown.OptionData>();

    //��ư OnClick �̺�Ʈ ������ ���� ��ư����Ʈȭ 
    public  List<Button> btns ;

    private void Awake()
    {
        //tdd_Divice�� �� ��ȭ �Լ� �� �־��ش�.
        tdd_Divice.onValueChanged.AddListener(OnDD_DiveceChange );
        //tdd_port�� ����ȭ �Լ��� �־��ش�. 
        tdd_port.onValueChanged.AddListener(OnDD_PortsChange);

        //��ư���� ���� �´� �Լ� �־��ֱ�  *���� �߿�*
        btns[0].onClick.AddListener(OnClick_Search);
        btns[1].onClick.AddListener(OnClick_Port_Open);
        btns[2].onClick.AddListener(OnClick_Port_Close);
    }
    void Start()
    {
        // ���۽� ��Ʈ�� �⺻������ ������ �ش�. 
        Setting_SP(9600, Parity.None, 8, StopBits.One);
        sp_main.PortName = "COM1";

        // ���۽� ��Ʈ�� �˻��ϱ������� �����Ҽ� �������Ѵ�. 
        tdd_port.enabled = false;

        byte[] aa = { 0xF0, 0x01, 0x02,0xF7 };

        print(string.Join(", ", aa));
    }


    void Update()
    {
        
    }

    public void OnDD_PortsChange(int index)
    {
        // ���� �̸��� DropDown ����Ʈ�� Value ������ ã�ƿ� �����Ѵ�. 
        sp_main.PortName = opt_ports[index].text;

        print(sp_main.PortName + "������");

        // opt_ports[index].text += " chenge for search";
        //// ���� �� ��Ʈ�� ������� �ٲܼ� ������ �����´�. 
        //tdd_port.enabled = false;
    }

    public void OnDD_DiveceChange(int index)
    {
        TMP_Dropdown.OptionData beSelected = tdd_Divice.options[index];
        string devideName = beSelected.text;

        switch (devideName)
        {
            case "Co_Drone":
                Setting_SP(57600,Parity.None,8,StopBits.One);
                break;
            case "Ro-E":
                Setting_SP(115200, Parity.None, 8, StopBits.One);
                break;
            case "Basic":
                Setting_SP(9600, Parity.None, 8, StopBits.One);
                break;

            default:
                Debug.LogWarning("�����ִ� �̷� ���� ����!");
                break;
        }
    }

    /// <summary>
    /// ��å�Ǵ� ��⿡ ���� serialport ���ΰ� ���� �Լ�
    /// </summary>
    public void Setting_SP(int baudRate,Parity paerty, int databits, StopBits stopbits)
    {
        sp_main.BaudRate = baudRate;
        sp_main.Parity = paerty;
        sp_main.DataBits = databits;
        sp_main.StopBits = stopbits;

        tmp_serial_state_message.text = " BaudRate : " + sp_main.BaudRate.ToString() 
            + "\n Parity : " + sp_main.Parity.ToString()
            + "\n DataBits: " + sp_main.DataBits.ToString()
            + "\n StopBits: " + sp_main.StopBits.ToString();
    }

    public void OnClick_Search()
    {
        // ��ư�� ������ DropDown�� Ȱ��ȭ �ȴ�. 
        tdd_port.enabled = true;

        // ����� ��Ʈ�� ���� ���� 
        int le = SerialPort.GetPortNames().Length;
        // �ش� ���� ��ŭ�� �迭�� �����
        string[] port = new string[le];
        // ����� ��Ʈ �̸� �迭 ��������.
        port = SerialPort.GetPortNames();



        //���ο� �ɼǼ����� ���� OptionData ����Ʈ ����� 
        opt_ports.Clear();

        //aa �迭�� �ִ� ��� ���ڿ������͸� optiondata ����Ʈ�� ���� 
        foreach (string item in port)
        {
            opt_ports.Add(new TMP_Dropdown.OptionData(item));
        }


        //���� Dropdown�� �ִ� ��� �ɼ�����
        tdd_port.ClearOptions();
        // ������ optiondata ����Ʈ�� DropDown �ɼǰ��� �߰� 
        tdd_port.AddOptions(opt_ports);
        //  ���� dropdown �� ���õ� �ɼǰ��� 0������ ���� 
        tdd_port.value = 0;

    }

    void OnClick_Port_Open()
    {
        //��Ʈ�� ���������� ����
        if (sp_main.IsOpen) 
        {
            print("������");
        return;
        }

        if (sp_main.BaudRate == 115200)
        {
            sp_main.Open();

            byte[] ver_req = { 0xF0, 0x01, 0x7F, 0x00, 0xF7 }; // ������û Request
            byte[] rt_set = { 0xF0, 0x00, 0x7D, 0x01, 0x00, 0xF7 }; // �ǽð� ���� ����
            //byte[] melode = { 0xF0, 0x00, 0x0A, 0x03, 0x00, 0x02,0x01, 0xF7 }; // �ǽð� ���� ����
            
            sp_main.Write(ver_req, 0, ver_req.Length); //�����а�
            
            //sp_main.Write(rt_set, 0, rt_set.Length); // �ǽð����� ���� 
            

            // �����͸� ���� �ڷ�ƾ�Լ� 
            StartCoroutine(Read_RoE());

            
            ////  ��� ������ ��ư�̳�, �ٸ� �̺�Ʈ�� �޾Ƴ��°��� Ȯ���ϱ� ���մϴ�.
            //sp_main.Write(melode, 0, melode.Length); // ��ε� ����ϱ�

            return;
        }

        sp_main.Open();

        //try
        //{
        //    print(sp_main.PortName);
        //    sp_main.Open();

        //    sp_main.DataReceived += new SerialDataReceivedEventHandler(sp_main_DataReceived);
        //    // ����Ÿ ���Ž� ó�� �Ҽ� �ְ� �ȴ�.

        //    print("���� ����");
        //}
        //catch (System.Exception)
        //{
        //    Debug.LogWarning("��Ʈ���� ���� ����");
        //    throw;
        //}

    }

    void OnClick_Port_Close()
    {
        //��Ʈ�� �������� ������ ����
        if (!sp_main.IsOpen) return;

        StopAllCoroutines();//����ڤǷ�ƾ�� ���߰� ����. 

        // ���̿� ���� ���� �� ��� �ൿ�� ������Ű�� ��ɾ�
        byte[] stop_mode = { 0xF0, 0x00, 0x7D, 0x01, 0x05, 0xF7 };
        
        sp_main.Write(stop_mode, 0, stop_mode.Length);//������� ���� �� 

        sp_main.Close(); //  ���� ����
        print("���� ����");
    }


    IEnumerator Read_RoE()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            int re = sp_main.BytesToRead;
            byte[] readData = new byte[re];
            sp_main.Read(readData, 0, readData.Length); //����ް�

            if (readData.Length != 0)
            {
                print(string.Join(", ", readData));
            }
        }

    }
}
