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
        // 선택 오류를 막기 위해 처음에는 DropDown 을 비활성화 해둔다. 
        comBox.enabled = false;

        //Dropdown의 값이 바뀔떄 적용 되는 함수 추가하기. (UNITY 화면에서 지정해줄수 도 있음.)
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
        print("켜짐 ");
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
                //sp_State.text = "연결 중";
            }
            else { }//sp_State.text = "이름 지정안됨!!"; }
        }


        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            test_Port.Close();
            //sp_State.text = "연결 끊김";
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
        //포트가 열려있으면 리턴
        if (test_Port.IsOpen) return;

        if (!test_Port.PortName.Contains("1"))
        {
            print(test_Port.PortName);
            //test_Port.Open();
            //sp_State.text = "연결 중";
        }
        //else //{ sp_State.text = "이름 지정안됨!!"; }
    }
    public void OnClick_DisConnect_Ports()
    {
        //포트가 열려있지 않으면 리턴
        if (! test_Port.IsOpen) return;

        test_Port.Close();
        //sp_State.text = "연결 끊김";
    }

    public void OnClick_Search()
    {
        // 버튼을 누르면 DropDown이 활성화 된다. 
        comBox.enabled = true;

        // 연결된 포트의 수를 세어 
        int le = SerialPort.GetPortNames().Length;
        // 해당 길이 만큼의 배열을 만들고
        string[] port = new string[le];
        // 연결된 포트 이름 배열 가져오기.
        port = SerialPort.GetPortNames();


        //현재 Dropdown에 있는 모든 옵션제거
        comBox.ClearOptions();

        //새로운 옵션설정을 위한 OptionData 리스트 지우기 
        optiondata.Clear();

        //aa 배열에 있는 모든 문자열데이터를 optiondata 리스트에 저장 
        foreach (string item in port)
        {
            optiondata.Add(new TMP_Dropdown.OptionData(item));
        }
        // 생성한 optiondata 리스트를 DropDown 옵션값에 추가 
        comBox.AddOptions(optiondata);
        //  현제 dropdown 에 설택됨 옵션값을 0번으로 설정 
        comBox.value = 0;

        string ss = optiondata[0].text;
    }

    /// <summary>
    /// 드론다운 박스의  value 값이 바뀔떄 적용되는 함수이다. 
    /// </summary>
    /// <param name="index"></param>
    public void OnDropDownEvent(int index)
    {   
        // 포드 이름을 DropDown 리스트의 Value 값에서 찾아와 저장한다. 
        test_Port.PortName = optiondata[index].text;

        print(test_Port.PortName);
    }
}
