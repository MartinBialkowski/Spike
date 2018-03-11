﻿using Autofac;
using FluentValidation;
using System;

namespace Spike.AuthenticationServer.IdentityServer.Types.Validators
{
    public class ValidatorFactory : IValidatorFactory
    {
        private readonly IComponentContext container;

        public ValidatorFactory(IComponentContext container)
        {
            this.container = container;
        }

        public IValidator<T> GetValidator<T>()
        {
            return (IValidator<T>)GetValidator(typeof(T));
        }

        public IValidator GetValidator(Type type)
        {
            var genericType = typeof(IValidator<>).MakeGenericType(type);
            if (container.TryResolve(genericType, out object validator))
            {
                return (IValidator)validator;
            }
            return null;
        }
    }
}
