﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Snowflake.Ajax;
using Snowflake.Events;
using Snowflake.Events.ServiceEvents;
using Snowflake.Extensions;

namespace Snowflake.Service.Manager
{
    public class AjaxManager : IAjaxManager
    {

        private readonly IDictionary<string, IBaseAjaxNamespace> globalNamespace;
        public IReadOnlyDictionary<string, IBaseAjaxNamespace> GlobalNamespace => this.globalNamespace.AsReadOnly();
        public AjaxManager()
        {
            this.globalNamespace = new Dictionary<string, IBaseAjaxNamespace>();
        }
        public void RegisterNamespace(string namespaceName, IBaseAjaxNamespace namespaceObject)
        {
            if (!this.globalNamespace.ContainsKey(namespaceName))
                this.globalNamespace.Add(namespaceName, namespaceObject);
        }
        public string CallMethod(IJSRequest request)
        {
            var callMethodEvent = new AjaxRequestReceivedEventArgs(CoreService.LoadedCore, request);
            SnowflakeEventManager.EventSource.RaiseEvent(callMethodEvent);
            request = callMethodEvent.ReceivedRequest;
            AjaxResponseSendingEventArgs sendResultEvent;
            try
            {
                IJSResponse result;
                IJSMethod jsMethod = this.GlobalNamespace[request.NameSpace].JavascriptMethods[request.MethodName];
                foreach (AjaxMethodParameterAttribute attr in jsMethod.MethodInfo.GetCustomAttributes<AjaxMethodParameterAttribute>()
                    .Where(attr => attr.Required)
                    .Where(attr => !(request.MethodParameters.Keys.Contains(attr.ParameterName))))
                {
                    result = new JSResponse(request, JSResponse.GetErrorResponse($"missing required param {attr.ParameterName}"), false);
                    sendResultEvent = new AjaxResponseSendingEventArgs(CoreService.LoadedCore, result);
                    SnowflakeEventManager.EventSource.RaiseEvent(sendResultEvent);
                    return sendResultEvent.SendingResponse.GetJson();
                }

                result = jsMethod.Method.Invoke(request);
                sendResultEvent = new AjaxResponseSendingEventArgs(CoreService.LoadedCore, result);
                SnowflakeEventManager.EventSource.RaiseEvent(sendResultEvent);
                return sendResultEvent.SendingResponse.GetJson();
            }
            catch (KeyNotFoundException)
            {
                var result = new JSResponse(request, JSResponse.GetErrorResponse($"method {request.MethodName} not found in namespace {request.NameSpace}"), false);
                sendResultEvent = new AjaxResponseSendingEventArgs(CoreService.LoadedCore, result);
                SnowflakeEventManager.EventSource.RaiseEvent(sendResultEvent);
                return sendResultEvent.SendingResponse.GetJson();
            }
            catch (Exception e)
            {
                var result = new JSResponse(request, e, false);
                sendResultEvent = new AjaxResponseSendingEventArgs(CoreService.LoadedCore, result);
                SnowflakeEventManager.EventSource.RaiseEvent(sendResultEvent);
                return sendResultEvent.SendingResponse.GetJson();
            }
        }
        public async Task<string> CallMethodAsync(IJSRequest request)
        {
            return await Task.Run(() => this.CallMethod(request));
        }


        public void Initialize(IPluginManager pluginManager)
        {
            foreach (var instance in pluginManager.Plugins<IBaseAjaxNamespace>().Select(ajaxNamespace => ajaxNamespace.Value))
            {
                this.RegisterNamespace(instance.PluginInfo["namespace"], instance);
            }
        }

    }
}
