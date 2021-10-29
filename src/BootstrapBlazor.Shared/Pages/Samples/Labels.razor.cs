﻿// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using BootstrapBlazor.Shared.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using System.Diagnostics.CodeAnalysis;

namespace BootstrapBlazor.Shared.Pages
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Labels : ComponentBase
    {
        [Inject]
        [NotNull]
        private IStringLocalizer<Foo>? Localizer { get; set; }

        [NotNull]
        private Foo? Dummy { get; set; }

        /// <summary>
        /// OnInitialized 方法
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();

            Dummy = Foo.Generate(Localizer);
        }
    }
}
