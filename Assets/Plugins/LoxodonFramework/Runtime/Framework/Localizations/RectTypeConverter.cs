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
using System.Text.RegularExpressions;
using UnityEngine;

namespace Loxodon.Framework.Localizations
{
    public class RectTypeConverter : ITypeConverter
    {
        public bool Support(string typeName)
        {
            switch (typeName)
            {
                case "rect":
                    return true;
                default:
                    return false;
            }
        }

        public Type GetType(string typeName)
        {
            switch (typeName)
            {
                case "rect":
                    return typeof(Rect);
                default:
                    throw new NotSupportedException();
            }
        }

        public object Convert(Type type, object value)
        {
            if (type == null)
                throw new NotSupportedException();

            try
            {
                var val = Regex.Replace(((string)value).Trim(), @"(^\()|(\)$)", "");
                string[] s = val.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (s.Length == 4)
                    return new Rect(float.Parse(s[0]), float.Parse(s[1]), float.Parse(s[2]), float.Parse(s[3]));

            }
            catch (Exception)
            {
            }
            throw new FormatException(string.Format("The '{0}' is illegal Rect.", value));
        }
    }
}
