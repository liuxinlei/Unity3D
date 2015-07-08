using UnityEngine;
using System.Collections;

/// <summary>
/// 这个类是真正的游戏入口类，这个类中存放业务逻辑  代码热更新的就是这边的代码！~
/// </summary>
public class Game : MonoBehaviour
{
    private WWW www;

    private WWW fontWWW;

    // Use this for initialization
    void Start()
    {
        Debug.Log("Start Load UIAtlas!");
        StartCoroutine(LoadAtlas());
        StartCoroutine(LoadFont());
    }

    void Update()
    {
        if (www != null && !www.isDone)
        {
            Debug.Log("atlas progress : " + www.progress);
        }
        if (fontWWW != null && !fontWWW.isDone)
        {
            Debug.Log("font progress : " + fontWWW.progress);
        }
    }

    private IEnumerator LoadFont()
    {
        fontWWW = new WWW("http://game.gamerisker.com/assets/Font.assetbundle");

        yield return fontWWW;

        Complete();
    }

    private IEnumerator LoadAtlas()
    {
        www = new WWW("http://game.gamerisker.com/assets/Atlas.assetbundle");

        yield return www;

        Complete();
    }

    private void Complete()
    {
        if (fontWWW.isDone && www.isDone)
        {
            GameObject atlas = www.assetBundle.Load("Default", typeof(GameObject)) as GameObject;

            UISprite sprite = NGUITools.AddChild<UISprite>(gameObject);

            UIAtlas uiatlas = atlas.GetComponent<UIAtlas>();
            sprite.atlas = uiatlas;
            sprite.spriteName = "default_tishikuang";
            sprite.width = 300;
            sprite.height = 300;

            //这里的字体 是NGUI 例子里面的字体，我直接将他的prefab 打成了assetbundle 使用
            GameObject font = fontWWW.assetBundle.Load("SciFi Font - Normal",typeof(GameObject)) as GameObject;
            UIFont uifont = font.GetComponent<UIFont>();

            UILabel label = NGUITools.AddChild<UILabel>(sprite.gameObject);
            label.text = "Load Script Succeed!";
            label.transform.localPosition = Vector3.zero;
            label.transform.localScale = Vector3.one;
            label.width = 300;
            label.bitmapFont = uifont;

            Debug.Log("Complete.");
        }
    }
}