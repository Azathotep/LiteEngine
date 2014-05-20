using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiteEngine.Rendering
{
    public class Timeline
    {
        List<TimelineEvent> _events = new List<TimelineEvent>();
        int _currentEventIndex = -1;
        int _elapsed;

        public void AddEvent(Action action, int duration)
        {
            _events.Add(new TimelineEvent(action, duration));
        }

        bool _loop;
        public bool Loop
        {
            get
            {
                return _loop;
            }
            set
            {
                _loop = value;
            }
        }

        public bool HasEnded
        {
            get
            {
                return _currentEventIndex >= _events.Count;
            }
        }

        public void Advance(int timeMs)
        {
            if (HasEnded)
                return;
            if (_currentEventIndex == -1)
            {
                _currentEventIndex = 0;
                _events[0].Action();
            }

            _elapsed += timeMs;

            TimelineEvent currentEvent = _events[_currentEventIndex];

            while (_elapsed > currentEvent.Duration)
            {
                _elapsed -= currentEvent.Duration;
                _currentEventIndex++;
                if (HasEnded)
                {
                    if (!Loop)
                        break;
                    _currentEventIndex = 0;
                }
                currentEvent = _events[_currentEventIndex];
                currentEvent.Action();
            }
        }

        public void Start()
        {
            _currentEventIndex = -1;
            _elapsed = 0;
        }
    }

    class TimelineEvent
    {
        Action _action;
        int _duration;
        public TimelineEvent(Action action, int duration)
        {
            _action = action;
            _duration = duration;
        }

        public Action Action
        {
            get
            {
                return _action;
            }
        }

        public int Duration
        {
            get
            {
                return _duration;
            }
        }
    }

}
