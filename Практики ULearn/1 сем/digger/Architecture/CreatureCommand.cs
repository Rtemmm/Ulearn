﻿namespace Digger
{
    public class CreatureCommand
    {
        public int DeltaX;
        public int DeltaY;
        public ICreature TransformTo;
        public bool NeedToDie;
        public bool IsHidden;
    }
}