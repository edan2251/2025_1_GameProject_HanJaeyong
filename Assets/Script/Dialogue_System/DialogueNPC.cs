using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueNPC : MonoBehaviour
{

    public DialogueDataSO   myDialogue;              //npc 만의 고유 대화 데이터

    private DialogueManager dialogueManager;         //대화 매니저 참조

    // Start is called before the first frame update
    void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();

        if(dialogueManager == null)
        {
            Debug.LogError("다이얼로그 매니저가 없습니다");
        }
    }

    void OnMouseDown()                                      //마우스로 NPC를 클릭 했을때
    {
        if (dialogueManager == null) return;                //맴니저가 없으면 실행 안함
        if (dialogueManager.IsDialogueActive()) return;     //이미 대화 중이면 실행 안함
        if (myDialogue == null) return;                     //대화 데이터가 없으면 실행 안함

        dialogueManager.StartDialogue(myDialogue);          //모든 조건이 만족되면 내 대화 시작
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
