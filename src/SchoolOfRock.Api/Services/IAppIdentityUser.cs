﻿namespace SchoolOfRock.Api.Services
{
    public interface IAppIdentityUser
    {
        string GetUsername();

        int GetUserId();

        bool IsAuthenticated();

        bool IsInRole(string role);

        string? GetRemoteIpAddress();

        string? GetLocalIpAddress();

        bool IsAdmin();
    }
}