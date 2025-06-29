using System;

namespace Invento.Shared.Models
{
    public enum UserRole
    {
        Administrator = 1,
        Manager = 2,
        Employee = 3,
        ReadOnly = 4,
        Custom = 99
    }
}