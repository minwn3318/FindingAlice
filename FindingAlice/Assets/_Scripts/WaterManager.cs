using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;

public class WaterManager : MonoBehaviour
{
    enum OxygenType {AddOxygen = -1,MinusOxygen = +1 }
    //=========================================
    //�̱���
    private WaterManager() { }
    public GameObject waringImage;
    private static WaterManager instance = null;
    public static WaterManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new WaterManager();
            }
            return instance;
        }
    }
    //========================================
    public float maxOxygen;
    private float curOxygen;
    private bool isInOxygenArea;

    private OxygenType oxygenType;
    private bool isWaring = false;
    private Collider playerCollider;
    public float _curOxygen
    {
        get { return curOxygen; }
        set { curOxygen = value; }
    }
    public float _maxOxygen
    {
        get { return maxOxygen; }
        set { maxOxygen = value; }
    }

    void Awake()
    {
        instance = this;
        maxOxygen = 15.0f;
        curOxygen = maxOxygen;
        oxygenType = OxygenType.MinusOxygen;
        playerCollider = GetComponent<Collider>();
        waringImage.SetActive(false);
    }

    void Update()
    {
        if (curOxygen < 0)
        {
            PlayerManager.Instance().isGameOver = true;
        }
        else if (curOxygen <= maxOxygen)
        {
            curOxygen -= (int)oxygenType * Time.deltaTime;
        }
        else if(curOxygen> maxOxygen)
        {
            curOxygen = maxOxygen;
        }
        else if (!isWaring && curOxygen < 5)
        {
            isWaring = true;
            waringImage.SetActive(true);
        }
        else if(isWaring && curOxygen >= 5)
        {
            isWaring = false;
            waringImage.SetActive(false);
        }
    }
    public void GetOxygenItem()
    {
        curOxygen += 7.5f;
        if(curOxygen > maxOxygen)
        {
            curOxygen = maxOxygen;
        }
    }
    void OnTriggerStay(Collider collider)   //��� ���� ������ ��������
    {
        if (!isInOxygenArea && collider.gameObject.CompareTag("OxygenArea"))    //isInOxygenArea������ ��� ���ӿ�����Ʈ�� tag�� Ž���ϹǷ� ��� �߻�
        {
            isInOxygenArea = true;
            oxygenType = OxygenType.AddOxygen;
        }
    }

    void OnTriggerExit(Collider collider)   //��� ���� ������ ��������
    {
        if (isInOxygenArea && collider.gameObject.CompareTag("OxygenArea"))
        {
            isInOxygenArea = false;
            oxygenType =  OxygenType.MinusOxygen;
        }
    }
}
