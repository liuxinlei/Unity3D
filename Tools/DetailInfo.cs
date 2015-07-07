using UnityEngine;
using System.Collections;

[CSLight]
public class DetailInfo : MonoBehaviour {

	public UIFontLocalize []_labels;
	public UITexture []_textures;
	public UISprite []_sprites;
	public UIProgressBar []_progbars;
	public UIButton []_buttons;

	[System.Serializable]
	public class GridItems {
		public UIGrid _grid;
		public DetailInfo _firstItem;

		System.Collections.Generic.List<DetailInfo> _items = new System.Collections.Generic.List<DetailInfo>();

		public void Init()
		{
			_firstItem.gameObject.SetActive(false);
		}
		// use _firstItem as 1st item, if second or more items needed, copy _firstItem.
		public DetailInfo AddItem() {
			if(!_firstItem.gameObject.activeSelf) {
				_firstItem.gameObject.SetActive(true);
				return _firstItem;
			}
			else {
				GameObject go = NGUITools.AddChild(_grid.gameObject, _firstItem.gameObject);
				DetailInfo di = go.GetComponent<DetailInfo>();
				if(di != null) {
					_items.Add(di);
				}
				else {
					NGUITools.Destroy(go);
				}
				_grid.Reposition();
				return di;
			}
		}
		public void ClearItem() {
			foreach(DetailInfo di in _items) {
				NGUITools.Destroy(di.gameObject);
			}
			_items.Clear();
			_firstItem.gameObject.SetActive(false);
		}
	}

	public GridItems []_grids;

	public void SetLabel(string widget, params object []content) {
		if(_labelDict.ContainsKey(widget)) {
			_labelDict[widget].Localize(content);
		}
	}

	public void SetLabelActive (string widget, bool visible)
	{
		if(_labelDict.ContainsKey(widget)) {
			_labelDict[widget].gameObject.SetActive(visible);
		}
	}

	public void SetTexture(string widget, Texture content) {
		if(_texDict.ContainsKey(widget)) {
			_texDict[widget].mainTexture = content;
		}
	}
	public void SetSprite(string widget, string content) {
		if(_spriteDict.ContainsKey(widget)) {
			_spriteDict[widget].spriteName = content;
		}
	}
	public DetailInfo AddItemInGrid(string widget) {
		if(_gridDict.ContainsKey(widget)) {
			return _gridDict[widget].AddItem();
		}
		return null;
	}
	public void SetProgress(string widget, float prog) {
		if(_progDict.ContainsKey(widget)) {
			_progDict[widget].value = prog;
		}
	}
	public void SetButtonState(string widget, bool enable)
	{
		if(_btnDict.ContainsKey(widget)) {
			_btnDict[widget].isEnabled = enable;
		}
	}
	public void SetButtonVisible(string widget, bool visible)
	{
		if(_btnDict.ContainsKey(widget)) {
			_btnDict[widget].gameObject.SetActive(visible);
		}
	}
	public void SetSpriteVisible(string widget, bool visible)
	{
		if(_spriteDict.ContainsKey(widget)) {
			_spriteDict[widget].gameObject.SetActive(visible);
		}
	}
	public void SetTextureVisible(string widget, bool visible)
	{
		if(_texDict.ContainsKey(widget)) {
			_texDict[widget].gameObject.SetActive(visible);
		}
	}
	public void ClearAllGrids() {
		foreach(System.Collections.Generic.KeyValuePair<string, GridItems> pair in _gridDict) {
			pair.Value.ClearItem();
		}
	}
	public void ClearGrid(string grid) {
		if(_gridDict.ContainsKey(grid)) {
			_gridDict[grid].ClearItem();
		}
	}
	public GameObject GetWidget(string name) {
		if(_labelDict.ContainsKey(name)) {
			return _labelDict[name].gameObject;
		}
		if(_texDict.ContainsKey(name)) {
			return _texDict[name].gameObject;
		}
		if(_spriteDict.ContainsKey(name)) {
			return _spriteDict[name].gameObject;
		}
		if(_progDict.ContainsKey(name)) {
			return _progDict[name].gameObject;
		}
		if(_btnDict.ContainsKey(name)) {
			return _btnDict[name].gameObject;
		}
		if(_gridDict.ContainsKey(name)) {
			return _gridDict[name]._grid.gameObject;
		}
		return null;
	}

	protected System.Collections.Generic.Dictionary<string, UIFontLocalize> _labelDict = new System.Collections.Generic.Dictionary<string, UIFontLocalize>();
	protected System.Collections.Generic.Dictionary<string, UITexture> _texDict = new System.Collections.Generic.Dictionary<string, UITexture>();
	protected System.Collections.Generic.Dictionary<string, UISprite> _spriteDict = new System.Collections.Generic.Dictionary<string, UISprite>();
	protected System.Collections.Generic.Dictionary<string, GridItems> _gridDict = new System.Collections.Generic.Dictionary<string, GridItems>();
	protected System.Collections.Generic.Dictionary<string, UIProgressBar> _progDict = new System.Collections.Generic.Dictionary<string, UIProgressBar>();
	protected System.Collections.Generic.Dictionary<string, UIButton> _btnDict = new System.Collections.Generic.Dictionary<string, UIButton>();
	void Awake()
	{
		// widgets with same name is not allowed.
		foreach(UIFontLocalize l in _labels) {
			_labelDict.Add(l.gameObject.name, l);
		}
		foreach(UITexture t in _textures) {
			_texDict.Add(t.gameObject.name, t);
		}
		foreach(UISprite s in _sprites) {
			_spriteDict.Add(s.gameObject.name, s);
		}
		foreach(GridItems g in _grids) {
			_gridDict.Add(g._grid.gameObject.name, g);
			g.Init();
		}
		foreach(UIProgressBar bar in _progbars) {
			_progDict.Add(bar.gameObject.name, bar);
		}
		foreach(UIButton btn in _buttons) {
			_btnDict.Add(btn.gameObject.name, btn);
		}
	}

	public void Init()
	{
		foreach(System.Collections.Generic.KeyValuePair<string, UITexture> pair in _texDict) {
			pair.Value.mainTexture = null;
		}
		foreach(System.Collections.Generic.KeyValuePair<string, GridItems> pair in _gridDict) {
			pair.Value.ClearItem();
		}
	}
}
