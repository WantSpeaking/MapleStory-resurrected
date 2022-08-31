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

using System;

namespace Loxodon.Framework.Views.Animations
{
    public delegate void AnimationAction<T>(T view, Action startCallback, Action endCallback) where T : IUIView;

    public class GenericUIAnimation<T> : IAnimation where T : IUIView
    {
        private T view;
        private AnimationAction<T> animation;

        private Action _onStart;
        private Action _onEnd;

        public GenericUIAnimation(T view, AnimationAction<T> animation)
        {
            this.view = view;
            this.animation = animation;
        }

        protected virtual void OnStart()
        {
            try
            {
                if (this._onStart != null)
                {
                    this._onStart();
                    this._onStart = null;
                }
            }
            catch (Exception) { }
        }

        protected virtual void OnEnd()
        {
            try
            {
                if (this._onEnd != null)
                {
                    this._onEnd();
                    this._onEnd = null;
                }
            }
            catch (Exception) { }
        }

        public IAnimation OnStart(Action onStart)
        {
            this._onStart += onStart;
            return this;
        }

        public IAnimation OnEnd(Action onEnd)
        {
            this._onEnd += onEnd;
            return this;
        }

        public virtual IAnimation Play()
        {
            if (this.animation != null)
                this.animation(this.view, OnStart, OnEnd);
            return this;
        }
    }
}
