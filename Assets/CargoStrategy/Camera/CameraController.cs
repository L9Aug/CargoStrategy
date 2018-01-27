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
            transform.localRotation = Quaternion.LookRotation(myCannon.CameraFocus.transform.position - myCannon.CameraTarget.transform.position);
        }

        public void FollowProjectile()
        {
            transform.position = myCannon.myProjectile.CameraTarget.transform.position;
            transform.localRotation = Quaternion.LookRotation(myCannon.myProjectile.CameraFocus.transform.position - myCannon.myProjectile.CameraTarget.transform.position);
        }

        public void MapView()
        {

        }



        private void SetupStateMachine()
        {
            //Conditions
            BoolCondition ShotFired = new BoolCondition(delegate() { return myCannon.myProjectile != null; } );
            BoolCondition ShotLanded = new BoolCondition(delegate () { return myCannon.myProjectile == null; });
            BoolCondition OpenMapViewMode = new BoolCondition(delegate () { return false; });
            NotCondition CloseMapViewMode = new NotCondition(OpenMapViewMode);
            AndCondition CloseMapViewModeToProjectileFollow = new AndCondition(CloseMapViewMode, ShotLanded); 

            //Transitions
            Transition ShotFiredTrans = new Transition("Shot fired", ShotFired, new List<Action>() { });
            Transition ShotLandedTrans = new Transition("Shot landed", ShotLanded, new List<Action>() { });
            Transition OpenMapViewModeTrans = new Transition("Map view", OpenMapViewMode, new List<Action>() { });
            Transition CloseMapViewModeTrans = new Transition("Close map view", CloseMapViewMode, new List<Action>() { });
            Transition CloseMapViewModeToProjectileFollowTrans = new Transition("Close map to proj follow", CloseMapViewModeToProjectileFollow, new List<Action>() { });

            //States
            State CanonFollowMode = new State("Cannon follow mode",
                new List<Transition>() { ShotFiredTrans, OpenMapViewModeTrans },
                new List<Action>() { },
                new List<Action>() { FollowCannon },
                new List<Action>() { });

            State ProjectileFollowMode = new State("Projectile follow mode",
                new List<Transition>() { ShotLandedTrans, OpenMapViewModeTrans },
                new List<Action>() { },
                new List<Action>() { FollowProjectile },
                new List<Action>() { });

            State MapViewMode = new State("Cannon follow mode",
                new List<Transition>() { CloseMapViewModeToProjectileFollowTrans, CloseMapViewModeTrans },
                new List<Action>() { },
                new List<Action>() { MapView },
                new List<Action>() { });

            //Targets
            ShotFiredTrans.SetTargetState(ProjectileFollowMode);
            ShotLandedTrans.SetTargetState(CanonFollowMode);
            OpenMapViewModeTrans.SetTargetState(MapViewMode);
            CloseMapViewModeTrans.SetTargetState(CanonFollowMode);
            CloseMapViewModeToProjectileFollowTrans.SetTargetState(ProjectileFollowMode);

            //Setup Machine
            CameraMachine = new StateMachine(CanonFollowMode, CanonFollowMode, ProjectileFollowMode, MapViewMode);
            CameraMachine.InitMachine();
        }
        
    }
}