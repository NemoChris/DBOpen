using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DBOpen.Controller
{
    public interface IController
    {
        #region model
        /// <summary>
        /// Insert a data
        /// </summary>
        /// <typeparam name="T">Type of model</typeparam>
        /// <param name="model">A model to be inserted</param>
        /// <returns>Whether successful(The unique identifier returned to the model.)</returns>
        bool Insert<T>(T model) where T : class;

        /// <summary>
        /// Delete a data
        /// </summary>
        /// <typeparam name="T">Type of model</typeparam>
        /// <param name="model">A model to be deleted(You should assign a ID to model)</param>
        /// <returns>Whether successful</returns>
        bool Delete<T>(T model) where T : class;

        /// <summary>
        /// Delete multiple data
        /// </summary>
        /// <typeparam name="T">Type of model</typeparam>
        /// <param name="idListsWillDel">Multiple identifiers separated by a comma</param>
        /// <returns>Whether successful</returns>
        bool Delete<T>(string idListsWillDel) where T : class;

        /// <summary>
        /// Update a data
        /// </summary>
        /// <typeparam name="T">Type of model</typeparam>
        /// <param name="model">A model to be updated(You should assign a ID to model)</param>
        /// <returns>Whether successful</returns>
        bool Update<T>(T model) where T : class;

        /// <summary>
        /// Fill a data
        /// </summary>
        /// <typeparam name="T">Type of model</typeparam>
        /// <param name="model">A model to be filled(You should assign a ID to model)</param>
        /// <returns>Whether successful</returns>
        bool Fill<T>(T model) where T : class;

        /// <summary>
        /// Save a data(There is a unique ID inserted or updated)
        /// </summary>
        /// <typeparam name="T">Type of model</typeparam>
        /// <param name="model">A model to be saved</param>
        /// <returns>Whether successful</returns>
        bool Save<T>(T model) where T : class;
        #endregion

        #region methods
        /// <summary>
        /// Query data
        /// </summary>
        /// <typeparam name="T">Type of model</typeparam>
        /// <param name="sql">SQL statement</param>
        /// <returns>Model collection</returns>
        List<T> Query<T>(string sql) where T : new();

        /// <summary>
        /// Query data
        /// </summary>
        /// <typeparam name="T">Type of model</typeparam>
        /// <param name="sql">SQL statement</param>
        /// <param name="paramters">parameters required</param>
        /// <returns>Model collection</returns>
        List<T> Query<T>(string sql, object[] paramters) where T : new();

        /// <summary>
        /// Query data
        /// </summary>
        /// <param name="conn">Database connection string</param>
        /// <param name="sql">SQL statement</param>
        /// <returns>DataSet</returns>
        DataSet Query(string conn, string sql);

        /// <summary>
        /// Query data
        /// </summary>
        /// <param name="conn">Database connection string</param>
        /// <param name="sql">SQL statement</param>
        /// <param name="paramters">parameters required</param>
        /// <returns>DataSet</returns>
        DataSet Query(string conn, string sql, object[] paramters);

        /// <summary>
        /// Excute CUD operation(Create Update Delete)
        /// </summary>
        /// <param name="conn">Database connection string</param>
        /// <param name="sql">SQL statement</param>
        /// <returns>Number of affected rows</returns>
        int NonQuery(string conn, string sql);

        /// <summary>
        /// Excute CUD operation(Create Update Delete)
        /// </summary>
        /// <param name="conn">Database connection string</param>
        /// <param name="sql">SQL statement</param>
        /// <param name="paramters">parameters required</param>
        /// <returns>Number of affected rows</returns>
        int NonQuery(string conn, string sql, object[] paramters);

        #endregion

    }
}
