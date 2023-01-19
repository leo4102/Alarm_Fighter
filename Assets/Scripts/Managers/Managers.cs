 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers _instance;
    public static Managers Instance { get { Init(); return _instance; } }

    #region Content     //����� �ڵ峢�� ���� ������ ���
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
                go = new GameObject() { name = "@Managers" };//���� ���� �� @Managers��� GameObject ����
                go.AddComponent<Managers>();//@Manager�� Managers script �巡�� �Ͽ� component ����

            }
            DontDestroyOnLoad(go);//scene �Ѿ�� ��� ������ GameObject ����
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
