using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace MyMod
{
    public class DebugMessage
    {
        public string Message { get; }
        public Color Color { get; }

        public DebugMessage(string message, Color color)
        {
            Message = message;
            Color = color;
        }

        public override bool Equals(object obj)
        {
            if (obj is DebugMessage other)
            {
                return Message == other.Message && Color == other.Color;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Message.GetHashCode() ^ Color.GetHashCode();
        }
    }
}
