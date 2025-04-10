using PublicSet;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditorInternal.VersionControl.ListControl;

public class NPC_MunDuckBea : InteractableObject
{
    eTextScriptFile currentFile;
    eStage currentStage { get { return GameManager.Instance.currentStage; } }
    eStage LastStage;

    private void Start()
    {
        currentFile = eTextScriptFile.NPC_MunDuckBea_Encounter;
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
        if (LastStage != currentStage)
        {
            LastStage = currentStage;
            currentFile = eTextScriptFile.NPC_MunDuckBea_Encounter;
        }
    }
}
