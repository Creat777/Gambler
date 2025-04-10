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
        // 최초 조우시 Encounter 파일번호 반환
        if (currentFile == eTextScriptFile.NPC_MunDuckBea_Encounter)
        {
            currentFile = eTextScriptFile.NPC_MunDuckBea_Acquaintance;
            return eTextScriptFile.NPC_MunDuckBea_Encounter;
        }

        // 그렇지 않으면 Acquaintance 파일번호 반환
        else
        {
            return currentFile;
        }
        
    }

    private void FixedUpdate()
    {
        // 스테이지 변경시 해당스테이지의 Encounter를 시작하도록 조정
        if (LastStage != currentStage)
        {
            LastStage = currentStage;
            currentFile = eTextScriptFile.NPC_MunDuckBea_Encounter;
        }
    }
}
