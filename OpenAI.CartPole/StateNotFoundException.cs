using System;

namespace OpenAI.CartPole
{
    public class StateNotFoundException : Exception 
    {
        public StateNotFoundException() : base() 
        {

        }
    }
}