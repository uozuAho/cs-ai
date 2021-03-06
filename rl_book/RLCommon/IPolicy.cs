﻿namespace RLCommon
{
    public interface IPolicy<in TState, in TAction>
    {
        double PAction(TState state, TAction action);
    }
}