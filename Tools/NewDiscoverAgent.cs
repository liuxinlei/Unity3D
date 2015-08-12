using UnityEngine;
using System.Collections;

public class NewDiscoverAgent : MonoBehaviour
{
	private ActionController _owner;
	public int _sightLevel;
	public float _sightRaduisFront;
	public float _sightAngle;
	public float _sightRaduisBack;
	
	protected AIStatePipline _statePipline;
	
	private bool _isInDiscover = false;
	private bool _isInCheckEnermyClear = false;

	public void Init(EWObject owner,AIStatePipline statePipline)
	{
		_owner = owner as ActionController;
		_statePipline = statePipline;
		_statePipline.onStartMove += delegate() {
			BeginToDiscover(false);
		};
	}
	
	public void BeginToDiscover(bool useMaxPower)
	{
		_isInDiscover = true;
		StartCoroutine(DISCOVER(useMaxPower));
	}

	IEnumerator DISCOVER(bool useMaxPower)
	{
		float timeLast = 0.1f;
		while(_isInDiscover)
		{
			if(timeLast>0)
			{
				timeLast -= Time.deltaTime;
				if(timeLast<=0)
				{
					timeLast+=0.1f;
					ActionController ac = null;
					bool foundByOthers = false;
					if(useMaxPower) {
						ac = ActionControllerManager.Instance.GetEnemyTargetBySight(_owner.ThisTransform,0.0f,65535.0f,_owner.Faction,0.0f,false, out foundByOthers);
					}
					else {
						ac = ActionControllerManager.Instance.GetEnemyTargetBySight(_owner.ThisTransform,_sightRaduisFront,_sightRaduisBack,_owner.Faction,_sightAngle,false, out foundByOthers);
					}
					if(ac !=null)
					{
						CommandManager.Instance.Send(EWCommand.CMD.TARGET_FINDED,ac,EW_PARAM_TYPE.INT,_owner.ObjectID, EWCommand.STATE.RIGHTNOW,true);
						ActionControllerManager.Instance.NotifyTargetFound(ac, _owner.ThisTransform.localPosition);
						_isInDiscover = false;
						DialogueManager.Instance.Trigger(DialogueTriggerType.STORY_MEET,LevelManager.Singleton.GetCurrentLevelData()._level,_owner.Data._id,null);
						if(null != _statePipline.onFoundEnermy)
						{
							_isInCheckEnermyClear = true;
							_statePipline.onFoundEnermy();
							StartCoroutine(CHECK_ENERMY_CLEAR());
						}
					}
				}
			}
			yield return null;
		}
	}

	IEnumerator CHECK_ENERMY_CLEAR()
	{
		float timeLast = 0.1f;
		while(_isInCheckEnermyClear)
		{
			if(timeLast>0)
			{
				timeLast -= Time.deltaTime;
				if(timeLast<=0)
				{
					timeLast+=0.1f;
					ActionController ac = null;
					bool foundByOthers = false;
					ac = ActionControllerManager.Instance.GetEnemyTargetBySight(_owner.ThisTransform,_sightRaduisFront,_sightRaduisBack,_owner.Faction,_sightAngle,false, out foundByOthers);
					if(ac ==null)
					{
						if(null != _statePipline.onEnermyClear)
						{
							_isInCheckEnermyClear = false;
							_statePipline.onEnermyClear();
						}
					}
				}
			}
			yield return null;
		}
	}
}

