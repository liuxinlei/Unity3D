using UnityEngine;
using System.Collections;

/// <summary>
/// 游戏壳的入口，用于加载真实的代码
/// </summary>
public class Index : MonoBehaviour {

    private WWW www;

    public static WWW uiWWW;

    private System.Reflection.Assembly assembly;

	// Use this for initialization
	void Start () 
    {
        StartCoroutine(loadSprite());

        //GameObject go = new GameObject();
        //go.AddComponent<Game>();
	}

    void Update()
    {

    }

    private IEnumerator loadSprite()
    {
        www = new WWW("http://game.gamerisker.com/assets/Core.assetbundle");

        yield return www;

        if (www.isDone)
        {
            AssetBundle bundle = www.assetBundle;

            TextAsset asset = bundle.Load("Core", typeof(TextAsset)) as TextAsset;

            assembly = System.Reflection.Assembly.Load(asset.bytes);

            System.Type script = assembly.GetType("Game");

            Debug.Log("Load Game Script Complete...");
            gameObject.AddComponent(script);
        }

    }
}
