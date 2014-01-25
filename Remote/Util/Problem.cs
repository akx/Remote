﻿using System;

namespace Remote.Util
{
    internal class Problem : Exception
    {
        public Problem(string message) : base(message)
        {
        }

        public Problem(string message, Exception inner) : base(message, inner)
        {
        }
    }
}