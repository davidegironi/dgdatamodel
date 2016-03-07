#region License
// Copyright (c) 2014 Davide Gironi
//
// Please refer to LICENSE file for licensing information.
#endregion

using System;
using System.Reflection;

namespace DG.Data.Model
{
    public partial class GenericDataModel
    {
        /// <summary>
        /// Get or Set context Type
        /// </summary>
        public Type ContextType { get; private set; }

        /// <summary>
        /// Get or Set context creation parameters
        /// </summary>
        public object[] ContextParameters { get; private set; }

        /// <summary>
        /// Reference to the model
        /// </summary>
        protected object _baseModel = null;

        /// <summary>
        /// Language helper
        /// </summary>
        public GenericDataModelLanguageHelper LanguageHelper { get; private set; }

        /// <summary>
        /// Initialize the Model
        /// </summary>
        /// <param name="contextType"></param>
        /// <param name="contextParameters"></param>
        public void Initialize(Type contextType, object[] contextParameters)
        {
            // initialize Properties
            ContextType = contextType;
            ContextParameters = contextParameters;
            _baseModel = this;

            // instantiate the language helper
            LanguageHelper = new GenericDataModelLanguageHelper(this);

            // instantiate Repositories
            foreach (PropertyInfo prop in this.GetType().GetProperties())
            {
                if (prop.PropertyType.BaseType.Name.ToString().StartsWith("GenericDataRepository"))
                {
                    var instance = Activator.CreateInstance(prop.PropertyType);
                    prop.SetValue(this, instance, null);

                    object[] initializeParameters = new object[] { ContextType, ContextParameters, _baseModel };
                    MethodInfo initialize = prop.PropertyType.GetMethod("Initialize", new Type[] { typeof(Type), typeof(object[]), typeof(object) });
                    initialize.Invoke(prop.GetValue(this, null), initializeParameters);
                }
            }
        }
    }
}
