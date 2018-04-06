﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Chat_App_MVC_Core
{
    public class DynamicChat :DynamicHub
    {
        public override async Task OnConnectedAsync()
        {
            await Clients.All.Send($"{Context.ConnectionId} joined");
        }

        public override async Task OnDisconnectedAsync(Exception ex)
        {
            await Clients.Others.Send($"{Context.ConnectionId} left");
        }

        public Task Send(string message)
        {
            return Clients.All.Send($"{Context.ConnectionId}: {message}");
        }

        public Task SendToOthers(string message)
        {
            return Clients.Others.Send($"{Context.ConnectionId}: {message}");
        }

        public Task SendToGroup(string groupName, string message)
        {
            return Clients.Group(groupName).Send($"{Context.ConnectionId}@{groupName}: {message}");
        }

        public Task SendToOthersInGroup(string groupName, string message)
        {
            return Clients.OthersInGroup(groupName).Send($"{Context.ConnectionId}@{groupName}: {message}");
        }

        public async Task JoinGroup(string groupName)
        {
            await Groups.AddAsync(Context.ConnectionId, groupName);

            await Clients.Group(groupName).Send($"{Context.ConnectionId} joined {groupName}");
        }

        public async Task LeaveGroup(string groupName)
        {
            await Clients.Group(groupName).Send($"{Context.ConnectionId} left {groupName}");

            await Groups.RemoveAsync(Context.ConnectionId, groupName);
        }

        public Task Echo(string message)
        {
            return Clients.Caller.Send($"{Context.ConnectionId}: {message}");
        }
    }

}
