using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CargoStrategy.Graphing;
using FSM;
using Condition;

namespace CargoStrategy.Units
{

    public abstract class BaseUnit : MonoBehaviour
    {
        public TeamIds m_team;
        public List<IGraphNode> Path;

        [Tooltip("Movement speed, units/second.")]
        public float MovementSpeed;
        [Tooltip("Turning speed, degrees/second.")]
        public float TurnSpeed;

        protected IGraphNode m_currentFrom;
        protected IGraphNode m_currentTo;
        protected IGraphNode m_targetNode;
        protected IGraphConnection m_currentConnection;

        protected const float angleThreshold = 0.1f;
        protected const float arrivalThreshold = 0.1f;

        protected StateMachine MotionMachine;

        protected Vector3? motionTarget;
        protected Quaternion rotationStart;
        protected Quaternion rotationEnd;
        protected float lerpTimer;
        protected float lerpTime;

        protected int m_pathProgress;
        protected int m_connectionProgress;
        protected int m_connectionDirection;

        [SerializeField]
        private TeamColorComponent m_colorComponent = null;

        public void Initialise(IGraphNode startingNode, TeamIds team)
        {
            m_team = team;
            m_currentFrom = startingNode;
            GetNewPath();
            SetupMovementMachine();

            // TODO Change Colour?
            if (m_colorComponent != null)
            {
                m_colorComponent.SetTeam(team);
            }
        }

        protected abstract List<GraphNode> GetNodeTargets();

        public void Kill()
        {
            if (m_currentConnection != null)
            {
                m_currentConnection.UnRegisterUnit(this);
            }
            if (UnitManager.Instance != null)
            {
                UnitManager.Instance.UnRegisterUnit(this);
            }

            // TODO death Anim?

            Destroy(this.gameObject);
        }

        public void GetNewPath()
        {
            if(m_targetNode != null)
            {
                --m_targetNode.SupplierCount[((int)m_team) - 1];
            }

            motionTarget = null;

            if(m_currentConnection != null)
            {
                m_currentConnection.UnRegisterUnit(this);
            }
            m_currentConnection = null;

            List<GraphNode> m_nodeTargets = GetNodeTargets();

            if(m_nodeTargets == null)
            {
                Debug.LogError("No Targets to go to. Killing Self." + this);
                this.Kill();
                return;
            }

            for(int i = 0; i < m_nodeTargets.Count; ++i)
            {
                Path = GraphManager.Instance.CalculateRoute(m_currentFrom, m_nodeTargets[i], m_team);                
                if(Path != null)
                {
                    m_targetNode = m_nodeTargets[i];
                    ++m_targetNode.SupplierCount[((int)m_team) - 1];
                    m_pathProgress = 0;
                    break;
                }
            }

        }

        protected virtual void Update()
        {
            MotionMachine.SMUpdate();
        }

        protected virtual void ArrivedAtTarget()
        {
            //Debug.Log("Arrived At Target!");
            this.Kill();
        }

        protected virtual void GetNextMotionTarget()
        {
            if(Path == null)
            {
                motionTarget = null;
                return;
            }

            if (!motionTarget.HasValue)
            {
                // if there is no motion target then get one from the node connection.
                m_currentTo = Path[m_pathProgress + 1];

                SetupNewConnection();

                // check to see if we are at the end of our path.
                if (Vector3.Distance(transform.position, m_targetNode.Position) <= arrivalThreshold)
                {
                    ArrivedAtTarget();
                }
            }
            else
            {
                // check to see if we are at the end of our path.
                if (Vector3.Distance(transform.position, m_targetNode.Position) <= arrivalThreshold)
                {
                    ArrivedAtTarget();
                }

                if (!motionTarget.HasValue) return;

                // if there is already a motion target check that we are within the threshold range.
                if (Vector3.Distance(transform.position, motionTarget.Value) <= arrivalThreshold)
                {
                    // if the current connection is null then we are at the center of a node and will need to update our status.
                    if (m_currentConnection == null)
                    {
                        //m_pathProgress;
                        Path.RemoveAt(0);
                        if (m_pathProgress >= Path.Count - 1)
                        {
                            motionTarget = m_targetNode.Position;
                        }
                        else
                        {
                            m_currentFrom = Path[m_pathProgress];
                            m_currentTo = Path[m_pathProgress + 1];
                            SetupNewConnection();
                        }
                    }
                    else
                    {
                        // if we are within the threshold then check to see if this connection has another point in it's path
                        //if (m_connectionProgress - 1 < 0 || m_connectionProgress + 1 >= m_currentConnection.GetRoute().Count)
                        if(m_connectionDirection > 0 ? m_connectionProgress + 1 >= m_currentConnection.GetRoute().Count : m_connectionProgress - 1 < 0)
                        {
                            motionTarget = m_currentTo.Position;
                            m_currentConnection.UnRegisterUnit(this);
                            m_currentConnection = null;
                        }
                        else
                        {
                            m_connectionProgress += m_connectionDirection;
                            motionTarget = m_currentConnection.GetRoute()[m_connectionProgress];
                        }
                    }
                    
                }
                else
                {
                    // if we are not within the threshold then do not do anything.
                }
                
            }

        }

        protected void SetupNewConnection()
        {

            m_currentConnection = m_currentFrom.GetAdjacentConnectionTo(m_currentTo);

            if (m_currentConnection == null)
            {
                Debug.LogError(string.Format("No connection between {0} and {1} killing self!", m_currentFrom, m_currentTo));
                this.Kill();
            }

            m_currentConnection.RegisterUnit(this);

            m_connectionDirection = m_currentConnection.From == m_currentFrom ? 1 : -1;

            m_connectionProgress = m_connectionDirection > 0 ? 0 : m_currentConnection.GetRoute().Count - 1;

            motionTarget = m_currentConnection.GetRoute()[m_connectionProgress];
        }

        protected void BeginRotation()
        {
            rotationStart = transform.rotation;
            rotationEnd = Quaternion.FromToRotation(transform.forward, motionTarget.Value - transform.position);
            float angleDif = Quaternion.Angle(rotationStart, rotationEnd);

            lerpTime = angleDif / TurnSpeed;
            lerpTimer = 0;
        }

        protected void RotationUpdate()
        {
            //lerpTimer = Mathf.Clamp01(lerpTimer + (Time.deltaTime / lerpTime));
            //transform.rotation = Quaternion.Lerp(rotationStart, rotationEnd, lerpTimer);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(motionTarget.Value - transform.position), Time.deltaTime * TurnSpeed);
        }

        protected void MotionUpdate()
        {
            transform.Translate(Vector3.forward * Time.deltaTime * MovementSpeed, Space.Self);

            GetNextMotionTarget();
        }

        bool IsRotationRequired()
        {
            return motionTarget != null ? Vector3.Angle(transform.forward, motionTarget.Value - transform.position) > angleThreshold : false;
        }

        protected void SetupMovementMachine()
        {
            // conditions
            BoolCondition NullTargetCond = new BoolCondition(delegate () { return motionTarget == null; });
            BoolCondition RotationRequiredCond = new BoolCondition(IsRotationRequired);
            NotCondition RotationNotRequiredCond = new NotCondition(RotationRequiredCond);

            // transitions
            Transition NullTargetTrans = new Transition("null target trans", NullTargetCond);
            Transition RotationRequiredTrans = new Transition("rotation required trans", RotationRequiredCond);
            Transition MotionTrans = new Transition("Motion trans", RotationNotRequiredCond);

            // states
            State Idle = new State("Idle",
                new List<Transition>() { RotationRequiredTrans, MotionTrans },
                new List<Action>() { GetNextMotionTarget },
                new List<Action>() { },
                new List<Action>() { });

            State Rotating = new State("Rotating",
                new List<Transition>() { NullTargetTrans, MotionTrans },
                new List<Action>() { },
                new List<Action>() { RotationUpdate },
                new List<Action>() { });

            State Moving = new State("Moving",
                new List<Transition>() { NullTargetTrans, RotationRequiredTrans },
                new List<Action>() { },
                new List<Action>() { MotionUpdate },
                new List<Action>() { });

            // target states
            NullTargetTrans.SetTargetState(Idle);
            RotationRequiredTrans.SetTargetState(Rotating);
            MotionTrans.SetTargetState(Moving);

            // create machine
            MotionMachine = new StateMachine(null, Idle, Rotating, Moving);
            MotionMachine.InitMachine();
        }

    }

}