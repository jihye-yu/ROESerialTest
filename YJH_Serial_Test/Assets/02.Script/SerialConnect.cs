using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO.Ports;
using TMPro;




public class SerialConnect : MonoBehaviour
{
    //연결될 main 연결 시리얼 포트 [sp_main]
    public SerialPort sp_main = new SerialPort();

    // 연결된 기기에따른 serial port설정값이 다르기 떄문에 해당 메세지 확인 용
    public TextMeshProUGUI tmp_serial_state_message;
    // 프로그램에 연결 하고싶은 기기 종류 
    public TMP_Dropdown tdd_Divice;

    //연결된 ports 의 리스트를 선택 될수 있는  Dropdown
    public TMP_Dropdown tdd_port;

    //DropDown의 옵션 리스트
    private List<TMP_Dropdown.OptionData> opt_ports = new List<TMP_Dropdown.OptionData>();

    //버튼 OnClick 이벤트 적용을 위해 버튼리스트화 
    public  List<Button> btns ;

    private void Awake()
    {
        //tdd_Divice의 값 변화 함수 를 넣어준다.
        tdd_Divice.onValueChanged.AddListener(OnDD_DiveceChange );
        //tdd_port의 값변화 함수를 넣어준다. 
        tdd_port.onValueChanged.AddListener(OnDD_PortsChange);

        //버튼들의 각자 맞는 함수 넣어주기  *순서 중요*
        btns[0].onClick.AddListener(OnClick_Search);
        btns[1].onClick.AddListener(OnClick_Port_Open);
        btns[2].onClick.AddListener(OnClick_Port_Close);
    }
    void Start()
    {
        // 시작시 포트를 기본값으로 설정해 준다. 
        Setting_SP(9600, Parity.None, 8, StopBits.One);
        sp_main.PortName = "COM1";

        // 시작시 포트는 검색하기전에는 설정할수 없도록한다. 
        tdd_port.enabled = false;

        byte[] aa = { 0xF0, 0x01, 0x02,0xF7 };

        print(string.Join(", ", aa));
    }


    void Update()
    {
        
    }

    public void OnDD_PortsChange(int index)
    {
        // 포드 이름을 DropDown 리스트의 Value 값에서 찾아와 저장한다. 
        sp_main.PortName = opt_ports[index].text;

        print(sp_main.PortName + "선택함");

        // opt_ports[index].text += " chenge for search";
        //// 선택 후 포트를 마음대로 바꿀수 없도록 꺼놓는다. 
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
                Debug.LogWarning("문제있다 이런 기계는 읎따!");
                break;
        }
    }

    /// <summary>
    /// 선책되는 기기에 따라 serialport 내부값 세팅 함수
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
        // 버튼을 누르면 DropDown이 활성화 된다. 
        tdd_port.enabled = true;

        // 연결된 포트의 수를 세어 
        int le = SerialPort.GetPortNames().Length;
        // 해당 길이 만큼의 배열을 만들고
        string[] port = new string[le];
        // 연결된 포트 이름 배열 가져오기.
        port = SerialPort.GetPortNames();



        //새로운 옵션설정을 위한 OptionData 리스트 지우기 
        opt_ports.Clear();

        //aa 배열에 있는 모든 문자열데이터를 optiondata 리스트에 저장 
        foreach (string item in port)
        {
            opt_ports.Add(new TMP_Dropdown.OptionData(item));
        }


        //현재 Dropdown에 있는 모든 옵션제거
        tdd_port.ClearOptions();
        // 생성한 optiondata 리스트를 DropDown 옵션값에 추가 
        tdd_port.AddOptions(opt_ports);
        //  현제 dropdown 에 설택됨 옵션값을 0번으로 설정 
        tdd_port.value = 0;

    }

    void OnClick_Port_Open()
    {
        //포트가 열려있으면 리턴
        if (sp_main.IsOpen) 
        {
            print("연결중");
        return;
        }

        if (sp_main.BaudRate == 115200)
        {
            sp_main.Open();

            byte[] ver_req = { 0xF0, 0x01, 0x7F, 0x00, 0xF7 }; // 버전요청 Request
            byte[] rt_set = { 0xF0, 0x00, 0x7D, 0x01, 0x00, 0xF7 }; // 실시간 모드로 변경
            //byte[] melode = { 0xF0, 0x00, 0x0A, 0x03, 0x00, 0x02,0x01, 0xF7 }; // 실시간 모드로 변경
            
            sp_main.Write(ver_req, 0, ver_req.Length); //버전읽고
            
            //sp_main.Write(rt_set, 0, rt_set.Length); // 실시간모드로 변경 
            

            // 데이터를 읽을 코루틴함수 
            StartCoroutine(Read_RoE());

            
            ////  얘는 별도의 버튼이나, 다른 이벤트에 달아놓는것이 확인하기 편합니다.
            //sp_main.Write(melode, 0, melode.Length); // 멜로디 재생하기

            return;
        }

        sp_main.Open();

        //try
        //{
        //    print(sp_main.PortName);
        //    sp_main.Open();

        //    sp_main.DataReceived += new SerialDataReceivedEventHandler(sp_main_DataReceived);
        //    // 데이타 수신시 처리 할수 있게 된다.

        //    print("연결 성공");
        //}
        //catch (System.Exception)
        //{
        //    Debug.LogWarning("포트연결 오픈 오류");
        //    throw;
        //}

    }

    void OnClick_Port_Close()
    {
        //포트가 열려있지 않으면 리턴
        if (!sp_main.IsOpen) return;

        StopAllCoroutines();//모든코ㅗ루틴을 멈추고 간다. 

        // 로이용 연결 끊을 때 모든 행동을 정지시키는 명령어
        byte[] stop_mode = { 0xF0, 0x00, 0x7D, 0x01, 0x05, 0xF7 };
        
        sp_main.Write(stop_mode, 0, stop_mode.Length);//정지모드 전송 후 

        sp_main.Close(); //  연결 끊기
        print("연결 끊김");
    }


    IEnumerator Read_RoE()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            int re = sp_main.BytesToRead;
            byte[] readData = new byte[re];
            sp_main.Read(readData, 0, readData.Length); //응답받고

            if (readData.Length != 0)
            {
                print(string.Join(", ", readData));
            }
        }

    }
}
