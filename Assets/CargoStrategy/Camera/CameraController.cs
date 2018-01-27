using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM;
using Condition;
using CargoStrategy.Cannon;

namespace CargoStrategy.Camera
{
    public class CameraController : MonoBehaviour
    {
        private const float lookMax = 3;
        public const float cameraChangeDelay = 0.6f;

        StateMachine CameraMachine;

        public CannonController myCannon;

        private void Awake()
        {
            SetupStateMachine();
        }

        private void Update()
        {
            CameraMachine.SMUpdate();
        }

        public void FollowCannon()
        {

            transform.position = myCannon.CameraTarget.transform.position;

            Vector3 lookOffset = myCannon.transform.right * UserInput.UserInputDispatcher.Instance.GetPlayerHorizontalLook(myCannon.myPlayer) * lookMax +
                         myCannon.transform.up * UserInput.UserInputDispatcher.Instance.GetPlayerVerticalLook(myCannon.myPlayer) * lookMax;


            transform.localRotation = Quaternion.LookRotation((myCannon.CameraFocus.transform.position + lookOffset) - myCannon.CameraTarget.transform.position);
        }


        public void FollowProjectile()
        {

            transform.position = myCannon.myProjectile.CameraTarget.transform.position;

            Vector3 lookOffset = myCannon.myProjectile.transform.right * UserInput.UserInputDispatcher.Instance.GetPlayerHorizontalLook(myCannon.myPlayer) * lookMax +
                                   myCannon.myProjectile.transform.up * UserInput.UserInputDispatcher.Instance.GetPlayerVerticalLook(myCannon.myPlayer) * lookMax;

            transform.localRotation = Quaternion.LookRotation((myCannon.myProjectile.CameraFocus.transform.position + lookOffset) - myCannon.myProjectile.CameraTarget.transform.position);

        }

        public void MapView()
        {

        }

        float FiredTime;
        bool FiredWaitTime = false;


        float LandedTime;
        bool LandedWaitTime = false;



        private void SetupStateMachine()
        {
            //Conditions
            BoolCondition ShotFired = new BoolCondition(delegate() { return myCannon.myProjectile != null; } );
            BoolCondition ShotFiredAndWait = new BoolCondition(delegate () { return FiredWaitTime; });
            BoolCondition ShotLanded = new BoolCondition(delegate () { return myCannon.myProjectile.myModel == null; });
            BoolCondition ShotLanded2 = new BoolCondition(delegate () { return myCannon.myProjectile.myModel == null; });
            BoolCondition ShotLandedAndWait = new BoolCondition(delegate () { return LandedWaitTime; });
            BoolCondition OpenMapViewMode = new BoolCondition(delegate () { return false; });
            NotCondition CloseMapViewMode = new NotCondition(OpenMapViewMode);

            //Transitions
            Transition ShotFiredTrans = new Transition("Shot fired", ShotFired, new List<Action>() { });
            Transition ShotFiredAndWaitTrans = new Transition("Shot fired and wait", ShotFiredAndWait, new List<Action>() { });
            Transition ShotLandedTrans = new Transition("Shot landed", ShotLanded, new List<Action>() {});
            Transition ShotLanded2Trans = new Transition("Shot landed2", ShotLanded2, new List<Action>() { });
            Transition ShotLandedAndWaitTrans = new Transition("Shot landed and wait", ShotLandedAndWait, new List<Action>() { });
            Transition OpenMapViewModeTrans = new Transition("Map view", OpenMapViewMode, new List<Action>() { });
            Transition CloseMapViewModeTrans = new Transition("Close map view", CloseMapViewMode, new List<Action>() { });

            //States
            State CanonFollowMode = new State("Cannon follow mode",
                new List<Transition>() { ShotFiredTrans, OpenMapViewModeTrans },
                new List<Action>() { },
                new List<Action>() { FollowCannon },
                new List<Action>() { });

            State CannonWatchMode = new State("CannonWatchMode",
                new List<Transition>() { ShotLandedTrans, ShotFiredAndWaitTrans },
                new List<Action>() { delegate() { FiredTime = Time.time; } },
                new List<Action>() { FollowCannon, delegate() { FiredWaitTime = (Time.time - FiredTime) > cameraChangeDelay ? true : false; } },
                new List<Action>() { delegate () { FiredTime = 0; FiredWaitTime = false; } });

            State ProjectileWatchMode = new State("ProjetileWatchMode",
                new List<Transition>() { ShotLandedAndWaitTrans },
                new List<Action>() { delegate () { LandedTime = Time.time; } },
                new List<Action>() { FollowProjectile, delegate() { LandedWaitTime = (Time.time - LandedTime) > cameraChangeDelay ? true : false; } },
                new List<Action>() { delegate() { LandedTime = 0; LandedWaitTime = false; } });

            State ProjectileFollowMode = new State("Projectile follow mode",
                new List<Transition>() { ShotLanded2Trans },
                new List<Action>() { },
                new List<Action>() { FollowProjectile },
                new List<Action>() { });

            State MapViewMode = new State("Cannon follow mode",
                new List<Transition>() { CloseMapViewModeTrans },
                new List<Action>() { },
                new List<Action>() { MapView },
                new List<Action>() { });

            //Targets
            ShotFiredTrans.SetTargetState(CannonWatchMode);
            ShotLandedTrans.SetTargetState(CanonFollowMode);
            ShotLanded2Trans.SetTargetState(ProjectileWatchMode);
            OpenMapViewModeTrans.SetTargetState(MapViewMode);
            CloseMapViewModeTrans.SetTargetState(CanonFollowMode);
            ShotFiredAndWaitTrans.SetTargetState(ProjectileFollowMode);
            ShotLandedAndWaitTrans.SetTargetState(CanonFollowMode);


            //Setup Machine
            CameraMachine = new StateMachine(CanonFollowMode, CanonFollowMode, ProjectileFollowMode, MapViewMode, CannonWatchMode, ProjectileFollowMode);
            CameraMachine.InitMachine();
        }
        
    }
}