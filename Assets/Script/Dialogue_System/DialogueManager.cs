using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class DialogueManager : MonoBehaviour
{
    [Header("UI 요소 - Instector에서 연결")]
    public GameObject       DialoguePanel;                  //대화창 전체 패널(처음엔 비활성화)
    public Image            characterImage;                 //캐릭터 이미지 표시하는 Image UI
    public TextMeshProUGUI  characternameText;              //캐릭터 이름 표시하는 텍스트
    public TextMeshProUGUI  dialogueText;                   //대화 내용 표시하는 텍스트
    public Button           nextButton;                     //"다음" 버튼 (클릭 시 다음 대화로)

    [Header("기본설정")]
    public Sprite           defaultCharacterImage;          //캐릭터 이미지가 없을 때 사용할 기본 이미지

    [Header("타이핑 효과 설정")]
    public float            typingSpeed = 0.05f;            //글자 하나당 출력 속도 (초단위)
    public bool             skipTypingOnClick = true;       //클릭 시 타이핑 즉시 완료 할 지 여부

    //내부 변수들//
    private DialogueDataSO  currentDialogue;                //현재 진행 중인 대화 데이터
    private int             currentLineIndex = 0;           //현재 몇 번째 대화 중인지 (0부터 시작)
    private bool            isDialogueActive = false;       //대화가 진행 중인지 확인하는 플래그
    private bool            isTyping = false;               //현재 타이핑 효과가 진행 중인지
    private Coroutine       typingCoroutine;                //타이핑 효과 코루틴 참조 (중지용)


    void Start()
    {
        DialoguePanel.SetActive(false);                     //대화창 숨기기
        nextButton.onClick.AddListener(HandleNextInput);    //"다음"버튼에 새로운 입력 처리 연결
    }

    void Update()
    {
        if(isDialogueActive && Input.GetKeyDown(KeyCode.Space))
        {
            HandleNextInput();                              //다음 입력 처리 ( 타이핑 중이면 완료, 아니면 다음줄)
        }
    }

    public void StartDialogue(DialogueDataSO dialogue)
    {
        if (dialogue == null || dialogue.dialogueLines.Count == 0) return;          //대화 데이터 없거나 대화 내용이 비어있으면 실행 하지 않음

        //대화 시작 준비
        currentDialogue = dialogue;                         //현재 대화 데이터 설정
        currentLineIndex = 0;                               //첫 번째 대화 부터 시작
        isDialogueActive = true;                            //대화 활성화 플래그 ON

        //UI 업데이트
        DialoguePanel.SetActive(true);                      //대화창 보이기
        characternameText.text = dialogue.characterName;    //캐릭터 이름 표시

        if (characterImage != null)
        {
            if(dialogue.characterImage != null)
            {
                characterImage.sprite = dialogue.characterImage;            //대화 데이터의 이미지 사용
            }
            else
            {
                characterImage.sprite = defaultCharacterImage;              //없으면 기본 이미지 사용
            }

        }

        ShowCurrentLine();                                  //첫 번째 대화 내용 표시
    }

    IEnumerator TypeText(string textToType)
    {
        isTyping = true;                                    //타이핑 시작
        dialogueText.text = "";                             //텍스트 초기화

        //텍스트를 한 글자씩 추가
        for(int i = 0; i < textToType.Length; i++)
        {
            dialogueText.text += textToType[i];             //한 글자 추가
            yield return new WaitForSeconds(typingSpeed);   //대기 시간 설정
        }

        isTyping = false;                                   //타이핑 완료
    }

    private void CompleteTyping()                           //타이핑 효과를 즉시 완료 하는 함수
    {
        if(typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);                 //코루틴 중지
        }

        isTyping = false;                                   //타이핑 상태 해제

        //현재 줄의 전체 텍스트를 즉시 표시
        if(currentDialogue != null && currentLineIndex < currentDialogue.dialogueLines.Count)
        {
            dialogueText.text = currentDialogue.dialogueLines[currentLineIndex];
        }
    }

    void ShowCurrentLine()                                  //현재 대화 줄의 내용을 타이핑 효과와 함께 화면에 표시하는 함수
    {
        if(currentDialogue != null && currentLineIndex < currentDialogue.dialogueLines.Count)   //대화 데이터와 인덱스가 유효한지 확인
        {
            if(typingCoroutine != null)                     //이전 타이핑 효과가 있다면 중지
            {
                StopCoroutine(typingCoroutine);
            }

            //현재 줄의 대화 내용으로 타이핑 효과 시작
            string currentText = currentDialogue.dialogueLines[currentLineIndex];
            typingCoroutine = StartCoroutine(TypeText(currentText));
        }
    }

    public void ShowNextLine()                              //다음 대화 줄로 이동 시키는 함수 (타이핑이 완료된 후에만 호출)
    {
        currentLineIndex++;                                 //다음 줄로 인덱스 증가

        //마지막 대화였는지 확인
        if(currentLineIndex >= currentDialogue.dialogueLines.Count)
        {

        }
        else
        {
            ShowCurrentLine();                              //대화가 남아있다면 다음 줄 표시
        }
    }

    void EndDialogue()                                      //대화를 완전히 종료 하는 함수
    {
        if(typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        isDialogueActive = false;                           //대화 비활성화
        isTyping = false;                                   //타이핑 상태 해제
        DialoguePanel.SetActive(false);                     //대화창 숨기기
        currentLineIndex = 0;                               //인덱스 초기화
    }

    public void HandleNextInput()                           //스페이스바나 버튼 클릭 시 호출되는 입력 처리 함수
    {
        if(isTyping && skipTypingOnClick)
        {
            CompleteTyping();                               //타이핑 중이면 즉시 완료
        }
        else
        {
            ShowNextLine();                                 //타이핑 완료 상태면 다음 줄로
        }
    }

    public void SkipDialogue()                              //대화 전체를 바로 스킵하는 함수
    {
        EndDialogue();
    }

    public bool IsDialogueActive()                          //현재 대화가 진행 중인지 확인 하는 함수
    {
        return isDialogueActive;
    }
}
