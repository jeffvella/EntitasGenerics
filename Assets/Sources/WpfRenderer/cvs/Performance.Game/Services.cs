﻿using Entitas.MatchLine;

public class TestServices : IServices
{
    public IViewService ViewService { get; set; }
    public IInputService InputService { get; set; }
    public ITimeService TimeService { get; set; }
    public IElementService ElementService { get; set; }
}