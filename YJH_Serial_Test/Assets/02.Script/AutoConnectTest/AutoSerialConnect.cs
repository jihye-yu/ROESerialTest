using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO.Ports;

public class AutoSerialConnect : MonoBehaviour
{
    SerialPort sp_main = new SerialPort();

    // 연결된 포트 이름 마다 연결될 포트의 리스트 이다. 
    List<SerialPort> searchPorts = new List<SerialPort>();

    byte[] codrone_Request = { 0x0A, 0x55, 0x04, 0x01, 0x70, 0x10, 0x71, 0x7F, 0x4A, 0xFF };

    /// <summary>
    /// 코드론 용으로 먼저 제작하기 
    /// Boundrate = 57600
    /// Pariaty = None
    /// StopBit = One
    /// bytebits = 8
    /// </summary>
     



    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //print("space!");
            AutoSearch_Codrone();
        }
    }


    private void AutoSearch_Codrone()
    { 
        // 이름 들을 가져온다.
        int le = SerialPort.GetPortNames().Length;
        string[] portNames = new string[le];
        portNames = SerialPort.GetPortNames();

        print(string.Join(", ", portNames));
        //시리얼 포트 리스트 비워주고,
        searchPorts.Clear();
        foreach (string name in portNames)
        {
            SerialPort auto = new SerialPort();
            // 새 포트의 이름을 설정해주고
            auto = CodronePort_Setting(name);
            // 리스트에 추가함
            searchPorts.Add(auto);
        }

        /// //print(searchPorts[searchPorts.Count - 1].PortName); 중간 확인 을 위해 
        //이름마다 새로생긴 리스트 별로 코루틴 함수 실행하기
        for (int i = 0; i < searchPorts.Count; i++)
        {
            StartCoroutine(SearchPort(i));
        }
    }

    /// <summary>
    /// 포트이름만 넣으면 코드론용 포트로 새팅하고 리턴시켜주는 함수
    /// </summary>
    SerialPort CodronePort_Setting(string name)
    {
        SerialPort _serialport = new SerialPort();
        _serialport.PortName = name;
        _serialport.BaudRate = 57600;
        _serialport.DataBits = 8;
        _serialport.StopBits = StopBits.One;
        _serialport.Parity = Parity.None;

        return _serialport;
    }

    /// <summary>
    /// 해당 인덱스의 포트를 열고 10초동안 read 데이타를 읽으면서 맞는지 아닌지 확인하고 닫는다. 
    /// 
    /// </summary>
    IEnumerator SearchPort(int index)
    {
        // 일정 시간뒤에 닫기위한 시간변수
        float time = 0; 
        //해당 index 의 시리얼 포트를 열어주고 
        searchPorts[index].Open();
        // Request 를 날려줘야함
        searchPorts[index].Write(codrone_Request,0,codrone_Request.Length);

        yield return new WaitForSeconds(index);

        // [00] 초 뒤에 꺼짐
        while (time < 6)
        {
            //리드데이터를 받기위한 byte 배열
            int length = searchPorts[index].ReadBufferSize;
            byte[] readData = new byte[length];
            searchPorts[index].Read(readData, 0, readData.Length);

            ////바이트배열의 길이가 있다면 읽어라. 
            //if (length > 0)
            //{
            //    print(string.Join(", ", readData));
            //    if (readData[0] == 0x0A && readData[1] == 0x55)
            //    {
            //        print(searchPorts[index].PortName + " : 코드론 회선임");
            //    }
            //}

            print(index.ToString() + "번 포트 수신중 중");
            time += Time.deltaTime;
            time += 2;
            yield return new WaitForSeconds(2.0f);
        }

            //yield return null;
        yield return null;
        
        //해당 index 의 시리얼 포트를 닫는다. 
        searchPorts[index].Close();
        print(time.ToString());
    }
}
