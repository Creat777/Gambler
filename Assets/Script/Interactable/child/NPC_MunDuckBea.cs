using PublicSet;
using Unity.VisualScripting;
using UnityEngine;

public class NPC_MunDuckBea : InteractableObject
{
    eCsvFile_InterObj currentFile;
    eStage currentStage;

    private void Start()
    {
        currentFile = eCsvFile_InterObj.NPC_MunDuckBea_Encounter;
        currentStage = GameManager.Instance.currentStage;
    }
    public override eCsvFile_InterObj GetInteractableEnum()
    {
        // ���� ����� Encounter ���Ϲ�ȣ ��ȯ
        if (currentFile == eCsvFile_InterObj.NPC_MunDuckBea_Encounter)
        {
            currentFile = eCsvFile_InterObj.NPC_MunDuckBea_Acquaintance;
            return eCsvFile_InterObj.NPC_MunDuckBea_Encounter;
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
            currentFile = eCsvFile_InterObj.NPC_MunDuckBea_Encounter;
        }
    }
}
