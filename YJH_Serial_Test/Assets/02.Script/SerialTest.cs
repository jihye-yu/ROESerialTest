using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
//using UnityEngine.UI;
using TMPro;

public class SerialTest : MonoBehaviour
{
    public SerialPort test_Port = new SerialPort();

   // public Text sp_State;

    public TMP_Dropdown comBox;

    List<TMP_Dropdown.OptionData> optiondata = new List<TMP_Dropdown.OptionData>();

    private void Awake()
    {
        // ���� ������ ���� ���� ó������ DropDown �� ��Ȱ��ȭ �صд�. 
        comBox.enabled = false;

        //Dropdown�� ���� �ٲ��� ���� �Ǵ� �Լ� �߰��ϱ�. (UNITY ȭ�鿡�� �������ټ� �� ����.)
        comBox.onValueChanged.AddListener(OnDropDownEvent);
    }

    void Start()
    {
        test_Port.BaudRate = 57600;
        test_Port.Parity = Parity.None;
        test_Port.DataBits = 8;
        test_Port.StopBits = StopBits.One;

        //test_Port.PortName = "";

        //test_Port.Open();
        print("���� ");
    }

    void Update()
    {
        Update_open_Test();

        
    }




    private void Update_open_Test()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (test_Port.PortName != null)
            {
                test_Port.Open();
                //sp_State.text = "���� ��";
            }
            else { }//sp_State.text = "�̸� �����ȵ�!!"; }
        }


        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            test_Port.Close();
            //sp_State.text = "���� ����";
        }

        if (Input.GetButtonDown("Fire2"))
        {
            print("Ports is Open?");

            if (!test_Port.IsOpen)
            {
                print("is Close");
            }
            else
            {
                print("is Open");
            }
        }
    }

    public void OnClick_Connect_Ports()
    {
        //��Ʈ�� ���������� ����
        if (test_Port.IsOpen) return;

        if (!test_Port.PortName.Contains("1"))
        {
            print(test_Port.PortName);
            //test_Port.Open();
            //sp_State.text = "���� ��";
        }
        //else //{ sp_State.text = "�̸� �����ȵ�!!"; }
    }
    public void OnClick_DisConnect_Ports()
    {
        //��Ʈ�� �������� ������ ����
        if (! test_Port.IsOpen) return;

        test_Port.Close();
        //sp_State.text = "���� ����";
    }

    public void OnClick_Search()
    {
        // ��ư�� ������ DropDown�� Ȱ��ȭ �ȴ�. 
        comBox.enabled = true;

        // ����� ��Ʈ�� ���� ���� 
        int le = SerialPort.GetPortNames().Length;
        // �ش� ���� ��ŭ�� �迭�� �����
        string[] port = new string[le];
        // ����� ��Ʈ �̸� �迭 ��������.
        port = SerialPort.GetPortNames();


        //���� Dropdown�� �ִ� ��� �ɼ�����
        comBox.ClearOptions();

        //���ο� �ɼǼ����� ���� OptionData ����Ʈ ����� 
        optiondata.Clear();

        //aa �迭�� �ִ� ��� ���ڿ������͸� optiondata ����Ʈ�� ���� 
        foreach (string item in port)
        {
            optiondata.Add(new TMP_Dropdown.OptionData(item));
        }
        // ������ optiondata ����Ʈ�� DropDown �ɼǰ��� �߰� 
        comBox.AddOptions(optiondata);
        //  ���� dropdown �� ���õ� �ɼǰ��� 0������ ���� 
        comBox.value = 0;

        string ss = optiondata[0].text;
    }

    /// <summary>
    /// ��дٿ� �ڽ���  value ���� �ٲ��� ����Ǵ� �Լ��̴�. 
    /// </summary>
    /// <param name="index"></param>
    public void OnDropDownEvent(int index)
    {   
        // ���� �̸��� DropDown ����Ʈ�� Value ������ ã�ƿ� �����Ѵ�. 
        test_Port.PortName = optiondata[index].text;

        print(test_Port.PortName);
    }
}
