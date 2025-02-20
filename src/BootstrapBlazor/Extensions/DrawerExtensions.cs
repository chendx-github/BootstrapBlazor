﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License
// See the LICENSE file in the project root for more information.
// Maintainer: Argo Zhang(argo@live.ca) Website: https://www.blazor.zone

namespace BootstrapBlazor.Components;

/// <summary>
/// 抽屉服务扩展方法
/// </summary>
public static class DrawerExtensions
{
    /// <summary>
    /// 弹出编辑抽屉
    /// </summary>
    /// <param name="service"><see cref="DrawerService"/> 服务实例</param>
    /// <param name="editDialogOption"><see cref="ITableEditDialogOption{TModel}"/> 配置类实例</param>
    /// <param name="option"><see cref="DrawerOption"/> 配置类实例</param>
    public static async Task ShowEditDrawer<TModel>(this DrawerService service, TableEditDrawerOption<TModel> editDialogOption, DrawerOption option)
    {
        var parameters = editDialogOption.ToParameter();
        parameters.Add(nameof(EditDialog<TModel>.OnCloseAsync), editDialogOption.OnCloseAsync);
        option.ChildContent = BootstrapDynamicComponent.CreateComponent<EditDialog<TModel>>(parameters).Render();
        await service.Show(option);
    }
}
