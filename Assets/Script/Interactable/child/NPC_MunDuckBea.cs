using PublicSet;
using Unity.VisualScripting;
using UnityEngine;

public class NPC_MunDuckBea : InteractableObject
{
    eTextScriptFile currentFile;
    eStage currentStage;

    private void Start()
    {
        currentFile = eTextScriptFile.NPC_MunDuckBea_Encounter;
        currentStage = GameManager.Instance.currentStage;
    }
    public override eTextScriptFile GetInteractableEnum()
    {
        // ���� ����� Encounter ���Ϲ�ȣ ��ȯ
        if (currentFile == eTextScriptFile.NPC_MunDuckBea_Encounter)
        {
            currentFile = eTextScriptFile.NPC_MunDuckBea_Acquaintance;
            return eTextScriptFile.NPC_MunDuckBea_Encounter;
        }

        // �׷��� ������ Acquaintance ���Ϲ�ȣ ��ȯ
        else
        {
            return currentFile;
        }
        
    }

    private void FixedUpdate()
    {
        // �������� ����� �ش罺�������� Encounter�� �����ϵ��� ����
        if (currentStage != GameManager.Instance.currentStage)
        {
            currentStage = GameManager.Instance.currentStage;
            currentFile = eTextScriptFile.NPC_MunDuckBea_Encounter;
        }
    }
}
