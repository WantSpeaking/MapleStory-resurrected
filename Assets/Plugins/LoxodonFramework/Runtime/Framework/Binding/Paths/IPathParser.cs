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

using System.Linq.Expressions;

namespace Loxodon.Framework.Binding.Paths
{
    public interface IPathParser
    {
        /// <summary>
        /// Parser object instance path.eg:vm => vm.User.Username
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        Path Parse(LambdaExpression expression);

        /// <summary>
        /// Parser text path.eg:User.Username
        /// </summary>
        /// <param name="pathText"></param>
        /// <returns></returns>
        Path Parse(string pathText);

        /// <summary>
        /// Parser static type path.
        /// </summary>
        /// <param name="pathText"></param>
        /// <returns></returns>
        Path ParseStaticPath(string pathText);

        /// <summary>
        /// Parser static type path.eg:System.Int32.MaxValue
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        Path ParseStaticPath(LambdaExpression expression);

        /// <summary>
        /// Parser member name.eg:vm => vm.User
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        string ParseMemberName(LambdaExpression expression);
    }
}
