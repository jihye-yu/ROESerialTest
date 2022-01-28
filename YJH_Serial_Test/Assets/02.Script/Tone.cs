using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tone : MonoBehaviour
{
    //��ſ����� ���� ����
    public SerialConnect main_serial;

    // ���� ��ư ������ ���� ����Ʈȭ
    public List<Button> btn_piano = new List<Button>();

    public Button minus;
    public Button plus;

    // octave �ܰ踦 ǥ���ϴ� Text �� ����
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
