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
        // 최초 조우시 Encounter 파일번호 반환
        if (currentFile == eCsvFile_InterObj.NPC_MunDuckBea_Encounter)
        {
            currentFile = eCsvFile_InterObj.NPC_MunDuckBea_Acquaintance;
            return eCsvFile_InterObj.NPC_MunDuckBea_Encounter;
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
        if (currentStage != GameManager.Instance.currentStage)
        {
            currentStage = GameManager.Instance.currentStage;
            currentFile = eCsvFile_InterObj.NPC_MunDuckBea_Encounter;
        }
    }
}
