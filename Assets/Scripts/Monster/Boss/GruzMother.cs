using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GruzMotherState;
using System;

public class GruzMother : Monster, IHittable
{
    [SerializeField] LayerMask groundLayer;

    private Animator animator;
    private Collider2D col;
    private SpriteRenderer render;
    private StateBase[] states;
    private StateGruzMother curState;
    private bool isGround;
    [NonSerialized] public Transform playerTransform;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        render = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
        curState = StateGruzMother.Sleep;
    }

    private void Update()
    {
        states[(int)curState].Update();
    }

    public void ChangeState(StateGruzMother state)
    {
        states[(int)curState].Exit();
        curState = state;
        states[(int)curState].Enter();
        states[(int)curState].Update();
    }

    private void GroundCheck()
    {
        //Physics2D.OverlapCollider(col,
    }

    public void TakeHit()
    {
        Debug.Log("¸ÂÀ½");
    }
}

namespace GruzMotherState
{
    public enum StateGruzMother { Sleep, Idle, Rush, WildSlam, Die, Size }

    public class SleepState : StateBase
    {
        private GruzMother gruzMother;

        public SleepState(GruzMother gruzMother)
        {
            this.gruzMother = gruzMother;
        }

        public override void Enter()
        {
            
        }

        public override void Exit()
        {
            
        }

        public override void Setup()
        {
            
        }

        public override void Transition()
        {
            
        }

        public override void Update()
        {
            
        }
    }

    public class IdleState : StateBase
    {
        private GruzMother gruzMother;

        public IdleState(GruzMother gruzMother)
        {
            this.gruzMother = gruzMother;
        }

        public override void Enter()
        {
            
        }

        public override void Exit()
        {

        }

        public override void Setup()
        {

        }

        public override void Transition()
        {

        }

        public override void Update()
        {

        }

        public class RushState : StateBase
        {
            private GruzMother gruzMother;

            public RushState(GruzMother gruzMother)
            {
                this.gruzMother = gruzMother;
            }

            public override void Enter()
            {

            }

            public override void Exit()
            {

            }

            public override void Setup()
            {

            }

            public override void Transition()
            {

            }

            public override void Update()
            {

            }
        }

        public class WildSlamState : StateBase
        {
            private GruzMother gruzMother;

            public WildSlamState(GruzMother gruzMother)
            {
                this.gruzMother = gruzMother;
            }

            public override void Enter()
            {

            }

            public override void Exit()
            {

            }

            public override void Setup()
            {

            }

            public override void Transition()
            {

            }

            public override void Update()
            {

            }
        }

        public class DieState : StateBase
        {
            private GruzMother gruzMother;

            public DieState(GruzMother gruzMother)
            {
                this.gruzMother = gruzMother;
            }

            public override void Enter()
            {

            }

            public override void Exit()
            {

            }

            public override void Setup()
            {

            }

            public override void Transition()
            {

            }

            public override void Update()
            {

            }
        }
    }
}
