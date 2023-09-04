using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Mlok.Web.Sites.JobChIN.Controllers
{
    public interface IModelController<TModel, TId>
    {
        Expression<Func<TModel, TId>> GetModelId();
        /// <summary>
        /// Returns the detailed model form databese with joined data.
        /// </summary>
        TModel GetById(TId id);
        /// <summary>
        /// Updates the model database table. If the model id is defauld then the model is inserted into database.
        /// </summary>
        TModel Update(TModel model);
        /// <summary>
        /// Delete record from the database.
        /// </summary>
        void Delete(IEnumerable<TId> ids);
        /// <summary>
        /// Get all models from databse.
        /// </summary>
        IEnumerable<TModel> GetAll();
        /// <summary>
        /// Get all models from database that are mapped int T. The mapping configuration between TModel and T has to exist.
        /// </summary>
        IEnumerable<T> GetAll<T>();
    }
}