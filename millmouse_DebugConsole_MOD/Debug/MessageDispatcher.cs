using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System;
using System.Collections.Generic;
using UnityEngine;

namespace MyMod
{
    public class MessageDispatcher<T>
    {
        private readonly Queue<T> messageQueue;
        private readonly float throttleInterval;
        private float lastDispatchTime;
        private readonly Action<T> dispatchAction;

        public MessageDispatcher(float throttleInterval, Action<T> dispatchAction)
        {
            this.throttleInterval = throttleInterval;
            this.dispatchAction = dispatchAction;
            messageQueue = new Queue<T>();
        }

        public void Enqueue(T message)
        {
            if (!messageQueue.Contains(message))
            {
                messageQueue.Enqueue(message);
            }
        }

        public void ProcessQueue()
        {
            float currentTime = Time.time;
            if (currentTime - lastDispatchTime >= throttleInterval && messageQueue.Count > 0)
            {
                lastDispatchTime = currentTime;
                T nextMessage = messageQueue.Dequeue();
                dispatchAction(nextMessage);
            }
        }
    }
}
