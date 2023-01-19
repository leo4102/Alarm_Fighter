 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers _instance;
    public static Managers Instance { get { Init(); return _instance; } }

    #region Content     //비슷한 코드끼리 묶어 가독성 향상
    BpmManager _bpm = new BpmManager();
    FieldManager _field = new FieldManager();
    GameManagerEx _game = new GameManagerEx();
    TimingManager _timing = new TimingManager();
    public static BpmManager Bpm { get { return Instance._bpm; } }
    public static FieldManager Field { get { return Instance._field; } }
    public static GameManagerEx Game { get { return Instance._game; } }
    public static TimingManager Timing { get { return Instance._timing; } }
    #endregion

    #region Core
    CollisionManager _collision = new CollisionManager();
    PoolManager _pool = new PoolManager();
    ResourceManager _resource = new ResourceManager();
    ScenceManagerEx _scene = new ScenceManagerEx();
    SoundManager _sound = new SoundManager();

    public static CollisionManager Collision { get { return Instance._collision; } }
    public static PoolManager Pool { get { return Instance._pool; } }
    public static ResourceManager Resource { get { return Instance._resource; } }
    public static ScenceManagerEx SceneManagerEx { get { return Instance._scene; } }
    public static SoundManager Sound { get { return Instance._sound; } }

    #endregion

    void Start()
    {
        Init();

    }

    void Update()
    {

    }
    static void Init()
    {
        if (_instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if (go == null)
            {
                go = new GameObject() { name = "@Managers" };//게임 실행 시 @Managers라는 GameObject 생성
                go.AddComponent<Managers>();//@Manager에 Managers script 드래그 하여 component 구성

            }
            DontDestroyOnLoad(go);//scene 넘어가도 계속 존재할 GameObject 설정
            _instance = go.GetComponent<Managers>();

            //Manager Init
            _instance._pool.Init();
            _instance._bpm.Init();
        }
    }

    public static void Clear()
    {
        //Manager Clear
        Pool.Clear();
    }
}
