using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteController : MonoBehaviour     //Note2 객체(한 개)가 생성되고 파괴 되는 것을 담당
{
    double currentTime = 0;
    [SerializeField] Transform noteAppearLocation = null;//notePrefab이 생성될 위치
    //[SerializeField] GameObject notePrefab = null;//생성할 Note 프리팹 연결

    
    public void Update()        //TimingMananger의 Behaveaction 에 종속되도록 바꾸지 못하나?
    {
        //특정 시간 간격으로 노트 생성
        currentTime += Time.deltaTime;
        if (currentTime >= 60d / Managers.Bpm.BPM)
        {
            Debug.Log("Note2 created"+currentTime);
            GameObject t_note = Managers.Resource.Instantiate("Note2",gameObject.transform);//------------------------------
            
            //GameObject t_note = ObjectPool.objectPool.noteQueue.Dequeue();//notePool에서 obj(Note) 하나 꺼냄      //--------------
            t_note.transform.position = noteAppearLocation.position;//obj가 Scene에 활성화될 자리 설정
            //t_note.SetActive(true); //가져온 obj Scene에 활성화      //----------------------
            //GameObject t_note = GameObject.Instantiate(notePrefab, noteAppearLocation.position, Quaternion.identity);
            //t_note.transform.SetParent(this.transform);
            Managers.Timing.noteList.Add(t_note);//TimingManager2의 noteList에 생성된 Note 추가
            currentTime -= 60d / Managers.Bpm.BPM;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)      //노트가 화면 밖으로 나가면 삭제
    {
        if (collision.CompareTag("Note2"))
        {
            Managers.Resource.Destroy(collision.gameObject);        //-----------------------
            Managers.Timing.noteList.Remove(collision.gameObject);//TimingManager2의 noteList에서 제거
            //ObjectPool.objectPool.noteQueue.Enqueue(collision.gameObject);//notePool에 obj(Note) 반환      //-------------------
            //collision.gameObject.SetActive(false);//obj 비활성화      //---------------------

            //Destroy(collision.gameObject);
           
        }
    }
}
