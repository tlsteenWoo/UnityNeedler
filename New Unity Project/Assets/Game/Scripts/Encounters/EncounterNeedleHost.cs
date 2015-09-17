using UnityEngine;
using System.Collections;

public class EncounterNeedleHost : EncounterScript<NeedleHost>
{
	public void HitHost(NeedleBehaviour source)
	{
		mostRecent.Hit (source);
	}
}

