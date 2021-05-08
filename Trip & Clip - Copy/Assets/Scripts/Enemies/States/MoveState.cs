using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : State
{
    D_MoveState stateData;

    protected bool isDetectingLedge;
    protected bool isDetectingWall;
    protected bool isPlayerInMinRange;
    protected bool isPlayerInMaxRange;
    public MoveState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_MoveState stateData) : base(entity, stateMachine, animBoolName)    // call base constructor, add specific constructor below
    {
        this.stateData = stateData;
    }

    public override void Enter()
    {
        base.Enter();
        entity.SetVelocity(stateData.movementSpeed);
       
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
       
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isDetectingLedge = entity.CheckLedge();
        isDetectingWall = entity.CheckWall();
        isPlayerInMinRange = entity.CheckPlayerInMinRange();
        isPlayerInMaxRange = entity.CheckPlayerInMaxRange();
    }
}
