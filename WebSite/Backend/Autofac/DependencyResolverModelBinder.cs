using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RetroShark.Application.Backend.Autofac
{
    /// <summary>
    /// A model binder that resolves types using the DependencyResolver.Current.GetService(ModelType)
    /// </summary>
    public class DependencyResolverModelBinder : DefaultModelBinder
    {
        /// <summary>
        /// Creates the model.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="bindingContext">The binding context.</param>
        /// <param name="modelType">Type of the model.</param>
        /// <returns>The resolved type</returns>
        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, System.Type modelType)
        {
            return DependencyResolver.Current.GetService(modelType);
        }
    }
}