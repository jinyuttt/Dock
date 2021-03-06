﻿// Copyright (c) Wiesław Šoltés. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Dock.Avalonia.Controls;
using Xunit;

namespace Dock.Avalonia.UnitTests.Controls
{
    public class DockTargetTests
    {
        [Fact]
        public void DockTarget_Ctor()
        {
            var actual = new DockTarget();
            Assert.NotNull(actual);
        }
    }
}
