﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Snowflake.Ajax;

namespace Snowflake.Service.Manager
{
    /// <summary>
    /// The Ajax Manager manages Ajax loadables
    /// </summary>
    public interface IAjaxManager : ILoadableManager
    {
        /// <summary>
        /// The Ajax namespaces
        /// </summary>
        IReadOnlyDictionary<string, IBaseAjaxNamespace> GlobalNamespace { get; }
        /// <summary>
        /// Registers an Ajax namespace
        /// </summary>
        /// <param name="namespaceName">The name of the namespace</param>
        /// <param name="namespaceObject">The IBaseAjaxNamespace that is this namespace</param>
        void RegisterNamespace(string namespaceName, IBaseAjaxNamespace namespaceObject);
        /// <summary>
        /// Calls an Ajax method asynchronously
        /// </summary>
        /// <param name="request">The IJSRequest object</param>
        /// <returns>A task containing the serialized IJSResponse</returns>
        Task<string> CallMethodAsync(IJSRequest request);
        /// <summary>
        /// Calls an Ajax method synchronously
        /// </summary>
        /// <param name="request">The IJSRequest object</param>
        /// <returns>A task containing the serialized IJSResponse</returns>
        string CallMethod(IJSRequest request);
    }
}