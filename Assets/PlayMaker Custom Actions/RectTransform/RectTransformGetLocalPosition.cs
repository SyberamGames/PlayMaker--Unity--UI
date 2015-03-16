﻿// (c) Copyright HutongGames, LLC 2010-2015. All rights reserved.
/*--- __ECO__ __ACTION__
EcoMetaStart
{
"script dependancies":[
						"Assets/PlayMaker Custom Actions/__internal/FsmStateActionAdvanced.cs"
					]
}
EcoMetaEnd
---*/
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("RectTransform")]
	[Tooltip("Get the Local position of this RectTransform.")]
	public class RectTransformGetLocalPosition : FsmStateActionAdvanced
	{
		[RequiredField]
		[CheckForComponent(typeof(RectTransform))]
		[Tooltip("The GameObject target.")]
		public FsmOwnerDefault gameObject;

		[Tooltip("The position")]
		[UIHint(UIHint.Variable)]
		public FsmVector3 position;

		[Tooltip("The position in a Vector 2d ")]
		[UIHint(UIHint.Variable)]
		public FsmVector2 position2d;
		
		[Tooltip("The x component of the Position")]
		[UIHint(UIHint.Variable)]
		public FsmFloat x;
		
		[Tooltip("The y component of the Position")]
		[UIHint(UIHint.Variable)]
		public FsmFloat y;

		[Tooltip("The z component of the Position")]
		[UIHint(UIHint.Variable)]
		public FsmFloat z;
		
		RectTransform _rt;
		
		public override void Reset()
		{
			base.Reset();
			gameObject = null;
			position = null;
			position2d = null;
			x = null;
			y = null;
			z = null;
		}
		
		public override void OnEnter()
		{
			GameObject go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go != null)
			{
				_rt = go.GetComponent<RectTransform>();
			}
			
			DoGetValues();
			
			if (!everyFrame)
			{
				Finish();
			}		
		}
		
		public override void OnActionUpdate()
		{
			DoGetValues();
		}
		
		void DoGetValues()
		{
			if (_rt==null)
			{
				return;
			}

			Vector3 _pos = _rt.localPosition;

			if (!position.IsNone) position.Value = _pos;

			if (!position2d.IsNone) position.Value = new Vector2(_pos.x,_pos.y);

			if (!x.IsNone) x.Value = _pos.x;
			if (!y.IsNone) y.Value = _pos.y;
			if (!z.IsNone) z.Value = _pos.z;
		}
	}
}