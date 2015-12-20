﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MGT.Utilities.EventHandlers;

namespace MGT.HRM.Emulator
{
    public abstract class HRMEmulatorLogger : IHRMLogger
    {
        public event GenericEventHandler<bool> LoggerStatusChanged;

        protected bool running = false;
        public bool Running 
        { 
            get 
            { 
                return running;
            }
        }

        public event GenericEventHandler<string> FileNameChanged;

        protected string fileName;
        public string FileName
        {
            get
            {
                return fileName;
            }
            set
            {
                if (running)
                    throw new Exception("Filename cannot be changed when the logger is running");

                string bck = fileName;

                fileName = value;
                if (bck != value)
                    if (FileNameChanged != null)
                        FileNameChanged(this, value);
            }
        }

        protected HRMEmulator hrmEmulator;

        public void Start(HeartRateMonitor hrm)
        {
            if (hrm is HRMEmulator)
                hrmEmulator = (HRMEmulator)hrm;
            else
                throw new Exception("Invalid HRM, HRMEmulator expected");

            HRMEmulatorStart();

            running = true;

            if (LoggerStatusChanged != null)
                LoggerStatusChanged(this, running);
        }

        protected abstract void HRMEmulatorStart();

        public void Stop()
        {
            if (!running)
                return;

            running = false;

            HRMEmulatorStop();

            if (LoggerStatusChanged != null)
                LoggerStatusChanged(this, running);
        }

        protected abstract void HRMEmulatorStop();

        public void Log(IHRMPacket hrmPacket)
        {
            HRMEmulatorPacket hrmEmulatorPacket;
            if (hrmPacket is HRMEmulatorPacket)
                hrmEmulatorPacket = (HRMEmulatorPacket)hrmPacket;
            else
                throw new Exception("Invalid IHRMPacket, ZephyrPacket expected");

            HRMEmulatorLog(hrmEmulatorPacket);
        }

        public void ResetSubscriptions()
        {
            LoggerStatusChanged = null;
            FileNameChanged = null;
        }

        protected abstract void HRMEmulatorLog(HRMEmulatorPacket hrmEmulatorPacket);

        public abstract void Dispose();
    }
}
