using System;
using System.Collections.Generic;
using System.Text;
using EFCoreSpike5.CommonModels;

namespace EFCoreSpike5.ConstraintsModels
{
    public class SortField<T> where T : class
    {
        public SortOrder SortOrder { get; set; }
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
                    propertyName = value;
                }
            }
        }
        protected Type ModelType { get; private set; }
        private string propertyName;

        public SortField()
        {
            ModelType = typeof(T);
        }

        protected void ValidatePropertyName(string propertyName)
        {
            if (propertyName == null)
            {
                throw new ArgumentNullException("propertyName");
            }
            if (ModelType.GetProperty(propertyName) == null)
            {
                throw new ArgumentException($"{propertyName} is not a public property of {ModelType.FullName}");
            }
        }
    }
}
