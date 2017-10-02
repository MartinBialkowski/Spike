using System;
using EFCoreSpike5.CommonModels;

namespace EFCoreSpike5.ConstraintsModels
{
    public class SortField<T> : ISortField where T : class
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

        public SortField(string propertyName, SortOrder sortOrder)
        {
            ModelType = typeof(T);
            PropertyName = propertyName;
            SortOrder = sortOrder;
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
    }
}
