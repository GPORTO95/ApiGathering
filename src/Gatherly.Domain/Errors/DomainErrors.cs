﻿using Gatherly.Domain.Shared;

namespace Gatherly.Domain.Errors;

public static class DomainErrors
{
    public static class Gathering
    {
        public static readonly Error InvitingCreator = new (
            "Gathering.InvitingCreator", 
            "Can't send invitation to the gathering creator.");

        public static readonly Error AlreadyPassed = new (
            "Gathering.AlreadyPassed", 
            "Can't send invitation for gathering in the past");

        public static readonly Error Expired = new(
            "Gathering.Expired",
            "Can't accept invitation for expired gathering");
    }

    public static class FirstName
    {
        public static readonly Error NameIsEmpty = new (
            "FirstName.Empty",
            "First name is empty.");

        public static readonly Error NameIsTooLong = new(
            "FirstName.TooLong",
            "First name is too long.");
    }
}
