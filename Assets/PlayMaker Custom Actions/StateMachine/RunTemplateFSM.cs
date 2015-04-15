// (c) copyright Hutong Games, LLC 2010-2015. All rights reserved.

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(ActionCategory.StateMachine)]
    [Tooltip("Creates an FSM from a saved FSM Template. Let's you define the template at run time via a FsmObject Variable.")]
    public class RunTemplateFSM : FsmStateAction
    {
		[RequiredField]
		[Tooltip("The Fsm Template.")]
		[ObjectType(typeof(FsmTemplate))]
		public FsmObject template;

        [UIHint(UIHint.Variable)]
        public FsmInt storeID;

        [Tooltip("Event to send when the FSM has finished (usually because it ran a Finish FSM action).")]
        public FsmEvent finishEvent;

		private FsmTemplateControl fsmTemplateControl = new FsmTemplateControl();
        private Fsm runFsm;


        public override void Reset()
        {
			template = null;

            fsmTemplateControl = new FsmTemplateControl();
            storeID = null;
            runFsm = null;
        }

        /// <summary>
        /// Initialize FSM on awake so it doesn't cause hitches later
        /// </summary>
        public override void Awake()
        {
			fsmTemplateControl.fsmTemplate = (FsmTemplate)template.Value;

            if (fsmTemplateControl.fsmTemplate != null && Application.isPlaying)
            {
                runFsm = Fsm.CreateSubFsm(fsmTemplateControl);
            }
        }

        /// <summary>
        /// Forward global events to the sub FSM
        /// </summary>
        public override bool Event(FsmEvent fsmEvent)
        {
            if (runFsm != null && (fsmEvent.IsGlobal || fsmEvent.IsSystemEvent))
            {
                runFsm.Event(fsmEvent);
            }

            return false;
        }

        /// <summary>
        /// Start the FSM on entering the state
        /// </summary>
        public override void OnEnter()
        {
            if (runFsm == null)
            {
                Finish();
                return;
            }

            fsmTemplateControl.UpdateValues();
            fsmTemplateControl.ApplyOverrides(runFsm);

            runFsm.OnEnable();

            if (!runFsm.Started)
            {
                runFsm.Start();
            }

            storeID.Value = fsmTemplateControl.ID;

            CheckIfFinished();
        }

        public override void OnUpdate()
        {
            if (runFsm != null)
            {
                runFsm.Update();                
                CheckIfFinished();
            }
            else
            {
                Finish();
            }
        }

        public override void OnFixedUpdate()
        {
            if (runFsm != null)
            {
                runFsm.FixedUpdate();
                CheckIfFinished();
            }
            else
            {
                Finish();
            }
        }

        public override void OnLateUpdate()
        {
            if (runFsm != null)
            {
                runFsm.LateUpdate();
                CheckIfFinished();
            }
            else
            {
                Finish();
            }
        }

        public override void DoTriggerEnter(Collider other)
        {
            if (runFsm.HandleTriggerEnter)
            {
                runFsm.OnTriggerEnter(other);
            }
        }

        public override void DoTriggerStay(Collider other)
        {
            if (runFsm.HandleTriggerStay)
            {
                runFsm.OnTriggerStay(other);
            }
        }

        public override void DoTriggerExit(Collider other)
        {
            if (runFsm.HandleTriggerExit)
            {
                runFsm.OnTriggerExit(other);
            }
        }

        public override void DoCollisionEnter(Collision collisionInfo)
        {
            if (runFsm.HandleCollisionEnter)
            {
                runFsm.OnCollisionEnter(collisionInfo);
            }
        }

        public override void DoCollisionStay(Collision collisionInfo)
        {
            if (runFsm.HandleCollisionStay)
            {
                runFsm.OnCollisionStay(collisionInfo);
            }
        }

        public override void DoCollisionExit(Collision collisionInfo)
        {
            if (runFsm.HandleCollisionExit)
            {
                runFsm.OnCollisionExit(collisionInfo);
            }
        }

        public override void DoControllerColliderHit(ControllerColliderHit collisionInfo)
        {
            runFsm.OnControllerColliderHit(collisionInfo);
        }

        public override void OnGUI()
        {
            if (runFsm != null && runFsm.HandleOnGUI)
            {
                runFsm.OnGUI();
            }
        }

        /// <summary>
        /// Stop the FSM on exiting the state
        /// </summary>
        public override void OnExit()
        {
            if (runFsm != null)
            {
                runFsm.Stop();
            }
        }

        void CheckIfFinished()
        {
            if (runFsm == null || runFsm.Finished)
            {
                Fsm.Event(finishEvent);
                Finish();
            }
        }


    }
}
