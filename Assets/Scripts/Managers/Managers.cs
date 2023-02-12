using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour       //@Managers(GameObject)¿¡ »ðÀÔµÊ(½ÃÀÛ½Ã »ý¼º)
{
    static Managers _instance;
    public static Managers Instance { get { Init(); return _instance; } }

    #region Content
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
    SceneManagerEx _scene = new SceneManagerEx();
    SoundManager _sound = new SoundManager();

    public static CollisionManager Collision { get { return Instance._collision; } }
    public static PoolManager Pool { get { return Instance._pool; } }
    public static ResourceManager Resource { get { return Instance._resource; } }
    public static SceneManagerEx Scene { get { return Instance._scene; } }
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
                go = new GameObject() { name = "@Managers" };
                go.AddComponent<Managers>();

            }
            DontDestroyOnLoad(go);
            _instance = go.GetComponent<Managers>();

            //Manager Init
            _instance._pool.Init();
            _instance._bpm.Init();
            _instance._sound.Init();
            //_instance._timing.Init();
        }
    }

    public static void Clear()
    {
        //Manager Clear
        Pool.Clear();
        Sound.Clear();
        Scene.Clear();
        Timing.Clear();
    }

   
    
}



