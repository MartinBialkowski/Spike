﻿using System;

namespace EFCoreSpike5.ConstraintsModels
{
    public class FilterField<T> where T : class
    {
        public string PropertyName
        {
            get
            {
                return propertyName;
            }
            set
            {
                if (value != propertyName)
                {
                    ValidatePropertyName(value);
                    PropertyType = ModelType.GetProperty(propertyName).PropertyType;
                    propertyName = value;
                }
            }
        }
        public Object FilterValue
        {
            get
            {
                return filterValue;
            }
            set
            {
                if (value != filterValue)
                {
                    ValidateFilterValue(value);
                    filterValue = value;
                }
            }
        }
        public Type PropertyType { get; set; }
        protected Type ModelType { get; private set; }
        private string propertyName;
        private object filterValue;

        public FilterField()
        {
            ModelType = typeof(T);
        }

        public FilterField(string propertyName, object filterValue)
        {
            ModelType = typeof(T);
            PropertyName = propertyName;
            FilterValue = filterValue;
        }

        protected void ValidatePropertyName(string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentNullException("propertyName");
            }
            if (ModelType.GetProperty(propertyName) == null)
            {
                throw new ArgumentException($"{propertyName} is not a public property of {ModelType.FullName}");
            }
        }

        protected void ValidateFilterValue(object filterValue)
        {
            if (filterValue == null)
            {
                throw new ArgumentNullException("filterValue");
            }
            if (filterValue.GetType() != PropertyType)
            {
                throw new ArgumentException($"Value of {propertyName} is of type {filterValue.GetType().FullName}, expected type {PropertyType.FullName}");
            }
        }
    }
}
