using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookForPlayerState : State
{
    D_LookForPlayerState stateData;
    protected bool isPlayerInMinRange;
    protected bool isAllTurnsDone;
    protected bool isAllTurnsTimeDone;
    protected bool turnImmediately;

    protected float lastTurnTime;

    protected int amountOfTurns;
    public LookForPlayerState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_LookForPlayerState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();
        isPlayerInMinRange = entity.CheckPlayerInMinRange();

    }

    public override void Enter()
    {
        base.Enter();
        isAllTurnsTimeDone = false;
        isAllTurnsDone = false;
        amountOfTurns = 0;
        entity.SetVelocity(0f);
        lastTurnTime = startTime;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (turnImmediately)
        {
            entity.Flip();
            amountOfTurns++;
            lastTurnTime = Time.time;
            turnImmediately = false;
        }
        else if(Time.time >= lastTurnTime + stateData.timeBetweenTurns && !isAllTurnsDone)
        {
            entity.Flip();
            amountOfTurns++;
            lastTurnTime = Time.time;

        }

        if (amountOfTurns >= stateData.amountOfTurns)
        {
            isAllTurnsDone = true;
        }

        if(Time.time >= lastTurnTime + stateData.timeBetweenTurns && isAllTurnsDone)
        {
            isAllTurnsTimeDone = true;
        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
    public void SetTurnImmediateley(bool value)
    {
        turnImmediately = value;
    }
}
