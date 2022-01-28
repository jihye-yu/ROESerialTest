using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Codrone : MonoBehaviour
{
    //통신연결을 위한 주축
    public SerialConnect main_serial;

    private void Awake()
    {
        //serialConnect 라는 통신 연결 컨포넌트 가 연결되있지않다면 연결되기.
        if (main_serial == null) { main_serial = GetComponent<SerialConnect>(); }
    }
    void Start()
    {
        
    }


    byte[] to = { 0x0A, 0x55, 0x11, 0x02, 0x70, 0x10, 0x07, 0x11, 0x36, 0x81, 0xFF };
    byte[] li = { 0x0A, 0x55, 0x11, 0x02, 0x70, 0x10, 0x07, 0x12, 0x55, 0xB1, 0xFF };

    void Update()
    {

        //take off : 0A 55 11 02 70 10 07 11 36 81 FF
        // landing : 0A 55 11 02 70 10 07 12 55 B1 FF

        if (main_serial.sp_main.IsOpen == true)
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                main_serial.sp_main.Write(to,0,to.Length);
                print("take off");
            }
            else if (Input.GetKeyDown(KeyCode.K))
            {
                main_serial.sp_main.Write(li,0,li.Length);
                print("landing");
            }
        }
    }
}
