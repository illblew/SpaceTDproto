// (c) Copyright HutongGames, LLC 2010-2013. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.StateMachine)]
	[Tooltip("Set the value of a Vector3 Variable in another FSM.")]
	public class SetFsmVector3 : FsmStateAction
	{
		[RequiredField]
        [Tooltip("The GameObject that owns the FSM.")]
		public FsmOwnerDefault gameObject;

		[UIHint(UIHint.FsmName)]
		[Tooltip("Optional name of FSM on Game Object")]
		public FsmString fsmName;

		[RequiredField]
		[UIHint(UIHint.FsmVector3)]
        [Tooltip("The name of the FSM variable.")]
		public FsmString variableName;

		[RequiredField]
        [Tooltip("Set the value of the variable.")]
		public FsmVector3 setValue;

        [Tooltip("Repeat every frame. Useful if the value is changing.")]
        public bool everyFrame;

		GameObject goLastFrame;
		PlayMakerFSM fsm;
		
		public override void Reset()
		{
			gameObject = null;
			fsmName = "";
			setValue = null;
		}

		public override void OnEnter()
		{
			DoSetFsmVector3();
			
			if (!everyFrame)
				Finish();		
		}

		void DoSetFsmVector3()
		{
			if (setValue == null)
			{
			    return;
			}

			var go = Fsm.GetOwnerDefaultTarget(gameObject);
			if (go == null)
			{
			    return;
			}
			
			if (go != goLastFrame)
			{
				goLastFrame = go;
				
				// only get the fsm component if go has changed
				
				fsm = ActionHelpers.GetGameObjectFsm(go, fsmName.Value);
			}			
			
			if (fsm == null)
			{
                LogWarning("Could not find FSM: " + fsmName.Value);
			    return;
			}
			
			var fsmVector3 = fsm.FsmVariables.GetFsmVector3(variableName.Value);
			
			if (fsmVector3 != null)
			{
                fsmVector3.Value = setValue.Value;
			}
            else
            {
                LogWarning("Could not find variable: " + variableName.Value);
            }
		}

		public override void OnUpdate()
		{
			DoSetFsmVector3();
		}

	}
}