﻿// Copyright (c) Argo Zhang (argo@163.com). All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Website: https://www.blazor.zone or https://argozhang.github.io/

using BootstrapBlazor.Localization.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

namespace BootstrapBlazor.Components
{
    /// <summary>
    /// 
    /// </summary>
    public static class JsonHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="option"></param>
        /// <returns></returns>
        public static IEnumerable<IConfigurationSection> GetConfigurationSections(JsonLocalizationOptions option)
        {
            var cultureName = CultureInfo.CurrentUICulture.Name;
            var langHandler = GetLangHandlers(cultureName, option);

            var builder = new ConfigurationBuilder();
            foreach (var h in langHandler)
            {
                builder.AddJsonStream(h);
            }

            // 获得配置外置资源文件
            if (option.AdditionalJsonFiles != null)
            {
                var file = option.AdditionalJsonFiles.FirstOrDefault(f =>
                {
                    var fileName = Path.GetFileNameWithoutExtension(f);
                    return fileName.Equals(cultureName, StringComparison.OrdinalIgnoreCase);
                });
                if (!string.IsNullOrEmpty(file))
                {
                    builder.AddJsonFile(file, true, true);
                }
            }

            var config = builder.Build();

            // dispose json stream
            foreach (var h in langHandler)
            {
                h.Dispose();
            }
            return config.GetChildren();
        }

        private static List<Stream> GetLangHandlers(string cultureInfoName, JsonLocalizationOptions option)
        {
            // 获取程序集中的资源文件
            var langHandler = GetResourceStream(typeof(JsonHelper).Assembly, option, cultureInfoName);

            // 获取外部设置程序集中的资源文件
            if (option.AdditionalJsonAssemblies != null)
            {
                foreach (var assembly in option.AdditionalJsonAssemblies)
                {
                    langHandler.AddRange(GetResourceStream(assembly, option, cultureInfoName));
                }
            }
            return langHandler;
        }

        private static List<Stream> GetResourceStream(Assembly assembly, JsonLocalizationOptions option, string cultureInfoName)
        {
            var ret = new List<Stream>();
            if (option.FallBackToParentUICultures)
            {
                // 查找回落资源
                var parentName = GetParentCultureName(cultureInfoName).Value;
                if (!string.IsNullOrEmpty(parentName))
                {
                    var fallbackJson = $"{assembly.GetName().Name}.{option.ResourcesPath}.{parentName}.json";
                    var stream = assembly.GetManifestResourceStream(fallbackJson);
                    if (stream != null)
                    {
                        ret.Add(stream);
                    }
                }
            }

            // 当前文化资源
            var json = $"{assembly.GetName().Name}.{option.ResourcesPath}.{cultureInfoName}.json";
            var s = assembly.GetManifestResourceStream(json);
            if (s != null)
            {
                ret.Add(s);
            }
            return ret;
        }

        private static StringSegment GetParentCultureName(StringSegment cultureInfoName)
        {
            var ret = new StringSegment();
            var index = cultureInfoName.IndexOf('-');
            if (index > 0)
            {
                ret = cultureInfoName.Subsegment(0, index);
            }
            return ret;
        }
    }
}
