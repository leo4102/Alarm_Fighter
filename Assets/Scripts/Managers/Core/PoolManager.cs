using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Object 별 pool
class Pool
{
    //pool에 담을 오브젝트의 원본
    public GameObject Original { get; private set; }
    //pool의 root 오브젝트 : push된 오브젝트의 부모 오브젝트
    public Transform Root { get; set; }

    //pool에 push된 오브젝트를 담고있는 stack
    Stack<Poolable> _poolStack = new Stack<Poolable>();

    //pool의 root오브젝트 생성
    public void Init(GameObject original, int count = 5)
    {
        Original = original;
        Root = new GameObject().transform;
        Root.name = $"{original.name}_ROOT";

        for (int i = 0; i < count; i++)
            Push(Create());

        }

    //pool에 들어갈 object 생성
    Poolable Create()
    {
        GameObject go = Object.Instantiate<GameObject>(Original);
        go.name = Original.name;
        return go.GetOrAddComponent<Poolable>();
    }

    //오브젝트를 pool에 집어넣는다 (= Destroy)
    public void Push(Poolable poolable)
    {
        if (poolable == null)
            return;

        //poolable을 pool로 옮기고 비활성화
        poolable.transform.parent = Root;
        poolable.gameObject.SetActive(false);
        poolable.IsUsing = false;

        _poolStack.Push(poolable);
    }

    //오브젝트를 pool에서 꺼낸다 (= Instantiate)
    public Poolable Pop(Transform parent)
    {
        //pop할 오브젝트
        Poolable poolable;

        //pool에 남은 오브젝트가 있다면 
        if (_poolStack.Count > 0)
            poolable = _poolStack.Pop();
        //없다면 생성
        else
            poolable = Create();

        //오브젝트 활성화
        poolable.gameObject.SetActive(true);

        //DontDestroyOnLoad 해제용 : DontDestroyOnLoad 산하에 들어가면 명시적으로 나오지않는한 못나옴
        //if(parent == null)
        //    poolable.transform.parent = Managers.Scene.CurrentScene.transform;

        poolable.transform.parent = parent;
        poolable.IsUsing = true;

        return poolable;

    }
}
public class PoolManager
{
    //object name을 key로 같은 pool dictionary
    Dictionary<string, Pool> _poolDict = new Dictionary<string, Pool>();
    //pool들의 부모 오브젝트
    Transform _root;

    public void Init()
    {
        if(_root == null)
        {
            _root = new GameObject { name = "@Pool_Root" }.transform;
            Object.DontDestroyOnLoad(_root);
        }
    }

    //pool 생성하고 dictionary에 추가
    public void CreatePool(GameObject original, int count = 5)//prefab객체를 pool에 몇개를 만들것인가
    {
        Pool pool = new Pool();
        pool.Init(original, count);
        pool.Root.parent = _root;

        _poolDict.Add(original.name, pool);
    }

    //pool이 있다면 pool에 push 없으면 destroy
    public void Push(Poolable poolable)
    {
        //poolable의 pool이 없다면 그냥 destroy
        string name = poolable.gameObject.name;
        if(_poolDict.ContainsKey(name) == false)
        {
            GameObject.Destroy(poolable.gameObject);
            return;
        }

        _poolDict[name].Push(poolable);
    }

    public Poolable Pop(GameObject original, Transform parent =null)
    {
        //pool이 없다면 pool생성 후 pop
        if (_poolDict.ContainsKey(original.name) == false)
            CreatePool(original);

        return _poolDict[original.name].Pop(parent);
    }

    public GameObject GetOriginal(string name)
    {
        if (_poolDict.ContainsKey(name) == false)
            return null;
        return _poolDict[name].Original;
    }

    //Scene 이동시 모두 pool오브젝트 삭제
    public void Clear()
    {
        foreach(Transform child in _root)
        {
            GameObject.Destroy(child.gameObject);
        }

        _poolDict.Clear();
    }
}
