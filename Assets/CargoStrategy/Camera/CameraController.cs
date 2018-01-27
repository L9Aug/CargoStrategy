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

        public GameObject myProjectile = null;

        private void Start()
        {
            SetupStateMachine();
        }

        private void Update()
        {
            CameraMachine.SMUpdate();
        }

        public void FollowCannon()
        {

        }

        public void FollowProjectile()
        {

        }

        public void MapView()
        {

        }



        private void SetupStateMachine()
        {
            //Conditions
            BoolCondition ShotFired = new BoolCondition(delegate() { return false; } );
            BoolCondition ShotLanded = new BoolCondition(delegate () { return false; });
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
        }
        
    }
}