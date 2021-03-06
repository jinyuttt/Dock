// Copyright (c) Wiesław Šoltés. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Dock.Model;
using Dock.Model.Controls;

namespace Dock.Avalonia.Controls
{
    /// <summary>
    /// Dock tool chrome content control.
    /// </summary>
    public class DockToolChrome : ContentControl
    {
        /// <summary>
        /// Define <see cref="Title"/> property.
        /// </summary>
        public static readonly StyledProperty<string> TitleProprty =
            AvaloniaProperty.Register<DockToolChrome, string>(nameof(Title));

        /// <summary>
        /// Define the <see cref="IsActive"/> property.
        /// </summary>
        public static readonly StyledProperty<bool> IsActiveProperty =
            AvaloniaProperty.Register<DockToolChrome, bool>(nameof(IsActive));

        /// <summary>
        /// Initialize the new instance of the <see cref="DockToolChrome"/>.
        /// </summary>
        public DockToolChrome()
        {
            UpdatePseudoClasses(IsActive);
        }

        /// <summary>
        /// Gets or sets chrome tool title.
        /// </summary>
        public string Title
        {
            get => GetValue(TitleProprty);
            set => SetValue(TitleProprty, value);
        }

        /// <summary>
        /// Gets or sets if this is the currently active Tool.
        /// </summary>
        public bool IsActive
        {
            get => GetValue(IsActiveProperty);
            set => SetValue(IsActiveProperty, value);
        }

        internal Control? Grip { get; private set; }

        internal Button? CloseButton { get; private set; }

        /// <inheritdoc/>
        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);
            AddHandler(InputElement.PointerPressedEvent, Pressed, RoutingStrategies.Tunnel);
        }

        /// <inheritdoc/>
        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnDetachedFromVisualTree(e);
        }

        private void Pressed(object sender, PointerPressedEventArgs e)
        {
            if (this.DataContext is IDock dock && dock.Factory is IFactory factory)
            {
                if (dock.ActiveDockable != null)
                {
                    if (factory.FindRoot(dock.ActiveDockable) is IRootDock root)
                    {
                        Debug.WriteLine($"{nameof(DockToolChrome)} SetFocusedDockable {dock.ActiveDockable.GetType().Name}, owner: {dock.Title}");
                        factory.SetFocusedDockable(root, dock.ActiveDockable);
                    }
                }
            }
        }

        /// <inheritdoc/>
        protected override void OnTemplateApplied(TemplateAppliedEventArgs e)
        {
            base.OnTemplateApplied(e);

            if (VisualRoot is HostWindow window)
            {
                Grip = e.NameScope.Find<Control>("PART_Grip");
                CloseButton = e.NameScope.Find<Button>("PART_CloseButton");

                window.AttachGrip(this);

                this.PseudoClasses.Set(":floating", true);
            }
        }

        /// <inheritdoc/>
        protected override void OnPropertyChanged<T>(
            AvaloniaProperty<T> property,
            Optional<T> oldValue,
            BindingValue<T> newValue,
            BindingPriority priority)
        {
            base.OnPropertyChanged(property, oldValue, newValue, priority);

            if (property == IsActiveProperty)
            {
                UpdatePseudoClasses(newValue.GetValueOrDefault<bool>());
            }
        }

        private void UpdatePseudoClasses(bool isActive)
        {
            PseudoClasses.Set(":active", isActive);
        }
    }
}
