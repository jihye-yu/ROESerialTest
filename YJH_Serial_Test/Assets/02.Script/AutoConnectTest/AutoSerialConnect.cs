using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO.Ports;

public class AutoSerialConnect : MonoBehaviour
{
    SerialPort sp_main = new SerialPort();

    // ����� ��Ʈ �̸� ���� ����� ��Ʈ�� ����Ʈ �̴�. 
    List<SerialPort> searchPorts = new List<SerialPort>();

    byte[] codrone_Request = { 0x0A, 0x55, 0x04, 0x01, 0x70, 0x10, 0x71, 0x7F, 0x4A, 0xFF };

    /// <summary>
    /// �ڵ�� ������ ���� �����ϱ� 
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
        // �̸� ���� �����´�.
        int le = SerialPort.GetPortNames().Length;
        string[] portNames = new string[le];
        portNames = SerialPort.GetPortNames();

        print(string.Join(", ", portNames));
        //�ø��� ��Ʈ ����Ʈ ����ְ�,
        searchPorts.Clear();
        foreach (string name in portNames)
        {
            SerialPort auto = new SerialPort();
            // �� ��Ʈ�� �̸��� �������ְ�
            auto = CodronePort_Setting(name);
            // ����Ʈ�� �߰���
            searchPorts.Add(auto);
        }

        /// //print(searchPorts[searchPorts.Count - 1].PortName); �߰� Ȯ�� �� ���� 
        //�̸����� ���λ��� ����Ʈ ���� �ڷ�ƾ �Լ� �����ϱ�
        for (int i = 0; i < searchPorts.Count; i++)
        {
            StartCoroutine(SearchPort(i));
        }
    }

    /// <summary>
    /// ��Ʈ�̸��� ������ �ڵ�п� ��Ʈ�� �����ϰ� ���Ͻ����ִ� �Լ�
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
    /// �ش� �ε����� ��Ʈ�� ���� 10�ʵ��� read ����Ÿ�� �����鼭 �´��� �ƴ��� Ȯ���ϰ� �ݴ´�. 
    /// 
    /// </summary>
    IEnumerator SearchPort(int index)
    {
        // ���� �ð��ڿ� �ݱ����� �ð�����
        float time = 0; 
        //�ش� index �� �ø��� ��Ʈ�� �����ְ� 
        searchPorts[index].Open();
        // Request �� ���������
        searchPorts[index].Write(codrone_Request,0,codrone_Request.Length);

        yield return new WaitForSeconds(index);

        // [00] �� �ڿ� ����
        while (time < 6)
        {
            //���嵥���͸� �ޱ����� byte �迭
            int length = searchPorts[index].ReadBufferSize;
            byte[] readData = new byte[length];
            searchPorts[index].Read(readData, 0, readData.Length);

            ////����Ʈ�迭�� ���̰� �ִٸ� �о��. 
            //if (length > 0)
            //{
            //    print(string.Join(", ", readData));
            //    if (readData[0] == 0x0A && readData[1] == 0x55)
            //    {
            //        print(searchPorts[index].PortName + " : �ڵ�� ȸ����");
            //    }
            //}

            print(index.ToString() + "�� ��Ʈ ������ ��");
            time += Time.deltaTime;
            time += 2;
            yield return new WaitForSeconds(2.0f);
        }

            //yield return null;
        yield return null;
        
        //�ش� index �� �ø��� ��Ʈ�� �ݴ´�. 
        searchPorts[index].Close();
        print(time.ToString());
    }
}
