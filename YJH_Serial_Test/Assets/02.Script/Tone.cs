using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tone : MonoBehaviour
{
    //통신연결을 위한 주축
    public SerialConnect main_serial;

    // 빠른 버튼 관리를 위해 리스트화
    public List<Button> btn_piano = new List<Button>();

    public Button minus;
    public Button plus;

    // octave 단계를 표시하는 Text 와 숫자
    public Text octave;
    int int_octave;


    private void Awake()
    {
        if (main_serial == null) { main_serial = GetComponent<SerialConnect>(); }
    }
    void Start()
    {
        int_octave = 7;
        octave.text = int_octave.ToString();

        //btn_piano[0].onClick.AddListener(OnClick_Minus);
        //btn_piano[1].onClick.AddListener(OnClick_Plus);
        //minus.onClick.AddListener(OnClick_Minus);
        //plus.onClick.AddListener(OnClick_Plus);

    }

    void Update()
    {
        
    }

    public void OnClick_Minus()
    {
        int_octave--;// 옥타브를 하나 낮추고
        octave.text = int_octave.ToString();// 글자로 표현해준다.

        print(octave.text);
    }
    public void OnClick_Plus()
    {
        int_octave++;// 옥타브를 하나 높이고
        octave.text = int_octave.ToString();// 글자로 표현해준다.

        print(octave.text);
    }

}
