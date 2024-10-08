﻿// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

namespace BootstrapBlazor.Components;

/// <summary>
/// 模块加载器
/// </summary>
/// <param name="jSObjectReference"></param>
public class JSModule(IJSObjectReference? jSObjectReference) : IAsyncDisposable
{
    /// <summary>
    /// InvokeVoidAsync 方法
    /// </summary>
    /// <param name="identifier"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public virtual ValueTask InvokeVoidAsync(string identifier, params object?[]? args) => InvokeVoidAsync(identifier, CancellationToken.None, args);

    /// <summary>
    /// InvokeVoidAsync 方法
    /// </summary>
    /// <param name="identifier"></param>
    /// <param name="timeout"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public virtual ValueTask InvokeVoidAsync(string identifier, TimeSpan timeout, params object?[]? args)
    {
        using CancellationTokenSource? cancellationTokenSource = ((timeout == Timeout.InfiniteTimeSpan) ? null : new CancellationTokenSource(timeout));
        CancellationToken cancellationToken = cancellationTokenSource?.Token ?? CancellationToken.None;
        return InvokeVoidAsync(identifier, cancellationToken, args);
    }

    /// <summary>
    /// InvokeVoidAsync 方法
    /// </summary>
    /// <param name="identifier"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public virtual async ValueTask InvokeVoidAsync(string identifier, CancellationToken cancellationToken = default, params object?[]? args)
    {
        var paras = new List<object?>();
        if (args != null)
        {
            paras.AddRange(args);
        }
        await InvokeVoidAsync();

        async ValueTask InvokeVoidAsync()
        {
            try
            {
                if (jSObjectReference != null)
                {
                    await jSObjectReference.InvokeVoidAsync(identifier, cancellationToken, [.. paras]);
                }
            }
            catch (JSException)
            {
#if DEBUG
                System.Console.WriteLine($"identifier: {identifier} args: {string.Join(" ", args!)}");
                throw;
#endif
            }
            catch (JSDisconnectedException) { }
            catch (OperationCanceledException) { }
            catch (ObjectDisposedException) { }
        }
    }

    /// <summary>
    /// InvokeAsync 方法
    /// </summary>
    /// <param name="identifier"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public virtual ValueTask<TValue?> InvokeAsync<TValue>(string identifier, params object?[]? args) => InvokeAsync<TValue?>(identifier, CancellationToken.None, args);

    /// <summary>
    /// InvokeAsync 方法
    /// </summary>
    /// <param name="identifier"></param>
    /// <param name="timeout"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public virtual ValueTask<TValue?> InvokeAsync<TValue>(string identifier, TimeSpan timeout, params object?[]? args)
    {
        using CancellationTokenSource? cancellationTokenSource = ((timeout == Timeout.InfiniteTimeSpan) ? null : new CancellationTokenSource(timeout));
        CancellationToken cancellationToken = cancellationTokenSource?.Token ?? CancellationToken.None;
        return InvokeAsync<TValue?>(identifier, cancellationToken, args);
    }

    /// <summary>
    /// InvokeAsync 方法
    /// </summary>
    /// <param name="identifier"></param>
    /// <param name="cancellationToken"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public virtual async ValueTask<TValue?> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken = default, params object?[]? args)
    {
        var paras = new List<object?>();
        if (args != null)
        {
            paras.AddRange(args!);
        }
        return await InvokeAsync();

        async ValueTask<TValue?> InvokeAsync()
        {
            TValue? ret = default;
            try
            {
                if (jSObjectReference != null)
                {
                    ret = await jSObjectReference.InvokeAsync<TValue?>(identifier, cancellationToken, [.. paras]);
                }
            }
            catch (JSException)
            {
#if DEBUG
                System.Console.WriteLine($"identifier: {identifier} args: {string.Join(" ", args!)}");
                throw;
#endif
            }
            catch (JSDisconnectedException) { }
            catch (OperationCanceledException) { }
            catch (ObjectDisposedException) { }

            return ret;
        }
    }

    /// <summary>
    /// Dispose 方法
    /// </summary>
    /// <param name="disposing"></param>
    protected virtual async ValueTask DisposeAsyncCore(bool disposing)
    {
        if (disposing)
        {
            try
            {
                if (jSObjectReference != null)
                {
                    await jSObjectReference.DisposeAsync();
                }
            }
            catch { }
        }
    }

    /// <summary>
    /// Dispose 方法
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        await DisposeAsyncCore(true);
        GC.SuppressFinalize(this);
    }
}
