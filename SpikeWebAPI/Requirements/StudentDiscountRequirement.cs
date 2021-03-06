﻿using Microsoft.AspNetCore.Authorization;

namespace Spike.WebApi.Requirements
{
    public class StudentDiscountRequirement : IAuthorizationRequirement
    {
        public int MaxAge { get; }

        public StudentDiscountRequirement(int maxAge = 26)
        {
            MaxAge = maxAge;
        }
    }
}
