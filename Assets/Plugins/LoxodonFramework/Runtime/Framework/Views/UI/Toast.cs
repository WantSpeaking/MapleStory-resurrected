﻿/*
 * MIT License
 *
 * Copyright (c) 2018 Clark Yang
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy of 
 * this software and associated documentation files (the "Software"), to deal in 
 * the Software without restriction, including without limitation the rights to 
 * use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies 
 * of the Software, and to permit persons to whom the Software is furnished to do so, 
 * subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all 
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE 
 * SOFTWARE.
 */

using System.Collections;
using UnityEngine;

using Loxodon.Framework.Contexts;
using System;
using Loxodon.Log;

namespace Loxodon.Framework.Views
{
    public class Toast
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Toast));

        private const string DEFAULT_VIEW_LOCATOR_KEY = "_DEFAULT_VIEW_LOCATOR";
        private const string DEFAULT_VIEW_NAME = "UI/Toast";

        private static string viewName;
        public static string ViewName
        {
            get { return string.IsNullOrEmpty(viewName) ? DEFAULT_VIEW_NAME : viewName; }
            set { viewName = value; }
        }

        private static IUIViewLocator GetUIViewLocator()
        {
            ApplicationContext context = Context.GetApplicationContext();
            IUIViewLocator locator = context.GetService<IUIViewLocator>();
            if (locator == null)
            {
                if (log.IsWarnEnabled)
                    log.Warn("Not found the \"IUIViewLocator\" in the ApplicationContext.Try loading the Toast using the DefaultUIViewLocator.");

                locator = context.GetService<IUIViewLocator>(DEFAULT_VIEW_LOCATOR_KEY);
                if (locator == null)
                {
                    locator = new DefaultUIViewLocator();
                    context.GetContainer().Register(DEFAULT_VIEW_LOCATOR_KEY, locator);
                }
            }
            return locator;
        }

        public static IUIViewGroup GetCurrentViewGroup()
        {
            GlobalWindowManager windowManager = GlobalWindowManager.Root;
            IWindow window = windowManager.Current;
            while (window is WindowContainer windowContainer)
            {
                window = windowContainer.Current;
            }
            return window as IUIViewGroup;
        }

        public static Toast Show(string text, float duration = 3f)
        {
            return Show(ViewName, null, text, duration, null, null);
        }

        public static Toast Show(string text, float duration, UILayout layout)
        {
            return Show(ViewName, null, text, duration, layout, null);
        }

        public static Toast Show(string text, float duration, UILayout layout, Action callback)
        {
            return Show(ViewName, null, text, duration, layout, callback);
        }

        public static Toast Show(IUIViewGroup viewGroup, string text, float duration = 3f)
        {
            return Show(ViewName, viewGroup, text, duration, null, null);
        }

        public static Toast Show(IUIViewGroup viewGroup, string text, float duration, UILayout layout)
        {
            return Show(ViewName, viewGroup, text, duration, layout, null);
        }

        public static Toast Show(IUIViewGroup viewGroup, string text, float duration, UILayout layout, Action callback)
        {
            return Show(ViewName, viewGroup, text, duration, layout, callback);
        }

        public static Toast Show(string viewName, IUIViewGroup viewGroup, string text, float duration, UILayout layout, Action callback)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ViewName;

            IUIViewLocator locator = GetUIViewLocator();
            ToastView view = locator.LoadView<ToastView>(viewName);
            if (view == null)
                throw new NotFoundException("Not found the \"ToastView\".");

            if (viewGroup == null)
                viewGroup = GetCurrentViewGroup();

            Toast toast = new Toast(view, viewGroup, text, duration, layout, callback);
            toast.Show();
            return toast;
        }

        private readonly IUIViewGroup viewGroup;
        private readonly float duration;
        private readonly string text;
        private readonly ToastView view;
        private readonly UILayout layout;
        private readonly Action callback;

        protected Toast(ToastView view, IUIViewGroup viewGroup, string text, float duration) : this(view, viewGroup, text, duration, null, null)
        {
        }

        protected Toast(ToastView view, IUIViewGroup viewGroup, string text, float duration, UILayout layout) : this(view, viewGroup, text, duration, layout, null)
        {
        }

        protected Toast(ToastView view, IUIViewGroup viewGroup, string text, float duration, UILayout layout, Action callback)
        {
            this.view = view;
            this.viewGroup = viewGroup;
            this.text = text;
            this.duration = duration;
            this.layout = layout;
            this.callback = callback;
        }

        public float Duration
        {
            get { return this.duration; }
        }

        public string Text
        {
            get { return this.text; }
        }

        public ToastView View
        {
            get { return this.view; }
        }

        public void Cancel()
        {
            if (this.view == null || this.view.Owner == null)
                return;

            if (!this.view.Visibility)
            {
                GameObject.Destroy(this.view.Owner);
                return;
            }

            if (this.view.ExitAnimation != null)
            {
                this.view.ExitAnimation.OnEnd(() =>
                {
                    this.view.Visibility = false;
                    this.viewGroup.RemoveView(this.view);
                    GameObject.Destroy(this.view.Owner);
                    this.DoCallback();
                }).Play();
            }
            else
            {
                this.view.Visibility = false;
                this.viewGroup.RemoveView(this.view);
                GameObject.Destroy(this.view.Owner);
                this.DoCallback();
            }
        }

        public void Show()
        {
            if (this.view.Visibility)
                return;

            this.viewGroup.AddView(this.view, this.layout);
            this.view.Visibility = true;
            this.view.text.text = this.text;

            if (this.view.EnterAnimation != null)
                this.view.EnterAnimation.Play();

            this.view.StartCoroutine(DelayDismiss(duration));
        }

        protected IEnumerator DelayDismiss(float duration)
        {
            yield return new WaitForSeconds(duration);
            this.Cancel();
        }

        protected void DoCallback()
        {
            try
            {
                if (this.callback != null)
                    this.callback();
            }
            catch (Exception)
            {
            }
        }
    }
}
