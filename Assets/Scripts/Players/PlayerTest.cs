using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
// 이 클래스는 PlayerText클래스이며 FieldObject를 상속받아 생성. FieldObject가 MonoBehaviour를 상속받고 있다. (1.17 재윤 추가)
// Issue (1.17) 처음 시작할 때 키보드 한번을 생략하고 시작함
public class PlayerTest : FieldObject
{

    TimingManager timingManager;
    int maxHp = 3;
    int currentHp;


    HpBar hpBar;
    public int CurrentHp
    {
        get { return currentHp; }
        set
        {
            currentHp = value;
            hpBar.updateValue(currentHp);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHp = maxHp;

        timingManager = FindObjectOfType<TimingManager>();
        hpBar = Util.FindChild<HpBar>(gameObject, null, true);

        // type을 초기화하고 objectField를 받아온 뒤, objectList에 PlayerField를 받아온다.
        type = 1;
        objectField = Managers.Field.getField();
        objectList = objectField.getGridArray(type);

        currentInd = objectList.Count / 2; // 이 초기화의 위치는 Field의 Width가 어떻든, 가운데에 오게할 수 있음 (1.18 재윤 추가)
        transform.position = objectList[currentInd].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        BitBehave();
    }

    protected override void BitBehave()
    {
        if (Input.GetKeyDown(KeyCode.W) && timingManager.CheckTiming()) { mayGo(Define.PlayerMove.Up); Managers.Sound.Play("Click"); }
        else if (Input.GetKeyDown(KeyCode.A) && timingManager.CheckTiming()) { mayGo(Define.PlayerMove.Left); Managers.Sound.Play("Click"); }
        else if (Input.GetKeyDown(KeyCode.S) && timingManager.CheckTiming()) { mayGo(Define.PlayerMove.Down); Managers.Sound.Play("Click"); }
        else if (Input.GetKeyDown(KeyCode.D) && timingManager.CheckTiming()) { mayGo(Define.PlayerMove.Right); Managers.Sound.Play("Click"); }
        else if (Input.GetKeyDown(KeyCode.K) && timingManager.CheckTiming()) { Attack(); Managers.Sound.Play("KnifeAttack1"); }
    }

    protected override void Attack()
    {
        int[] pattern = GetComponent<Weapon>().CalculateAttackRange(currentInd);
        Managers.Field.WarningAttack(pattern);
        Managers.Field.Attack(pattern);


        Transform attack = transform.GetChild(0);
        attack.GetComponent<PlayerAttack>().Attacking();
    }

    protected override void Hit()
    {
        Debug.Log("Hit!!!!");
        GetComponent<Animator>().SetTrigger("isHit");
        CurrentHp -= 1;
        Managers.Sound.Play("Hit");
        if (CurrentHp <= 0)
            Die();

    }
    void Die()
    {
        Debug.Log("Player Die!!");
        Managers.Game.GameOver();
        Managers.Sound.Play("Die");

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Hit();
    }

}
