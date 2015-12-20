﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGT.Utilities.EventHandlers;

namespace MGT.HRM
{
    public interface IHRMLogger : IDisposable
    {
        event GenericEventHandler<bool> LoggerStatusChanged;

        bool Running { get; }

        event GenericEventHandler<string> FileNameChanged;

        string FileName { get; set; }

        void Start(HeartRateMonitor hrm);

        void Stop();

        void Log(IHRMPacket hrmPacket);

        void ResetSubscriptions();
    }
}
