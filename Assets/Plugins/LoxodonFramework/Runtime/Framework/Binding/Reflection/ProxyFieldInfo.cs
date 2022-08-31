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
using System.Reflection;

using Loxodon.Log;

using System.Linq.Expressions;

namespace Loxodon.Framework.Binding.Reflection
{
#pragma warning disable 0414
    public class ProxyFieldInfo : IProxyFieldInfo
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ProxyFieldInfo));

        private readonly bool isValueType;
        private TypeCode typeCode;
        protected FieldInfo fieldInfo;

        public ProxyFieldInfo(FieldInfo fieldInfo)
        {
            if (fieldInfo == null)
                throw new ArgumentNullException("fieldInfo");

            this.fieldInfo = fieldInfo;
            //this.isValueType = this.fieldInfo.DeclaringType.GetTypeInfo().IsValueType;
            this.isValueType = this.fieldInfo.DeclaringType.IsValueType;
        }

        public virtual bool IsValueType { get { return isValueType; } }

        public virtual Type ValueType { get { return fieldInfo.FieldType; } }

        public TypeCode ValueTypeCode
        {
            get
            {
                if (typeCode == TypeCode.Empty)
                {
#if NETFX_CORE
                    typeCode = WinRTLegacy.TypeExtensions.GetTypeCode(ValueType);
#else
                    typeCode = Type.GetTypeCode(ValueType);
#endif
                }
                return typeCode;
            }
        }

        public virtual Type DeclaringType { get { return this.fieldInfo.DeclaringType; } }

        public virtual string Name { get { return this.fieldInfo.Name; } }

        public virtual bool IsStatic { get { return this.fieldInfo.IsStatic(); } }

        public virtual object GetValue(object target)
        {
            return this.fieldInfo.GetValue(target);
        }

        public virtual void SetValue(object target, object value)
        {
            if (fieldInfo.IsInitOnly)
                throw new MemberAccessException("The value is read-only.");

            if (IsValueType)
                throw new NotSupportedException("Assignments of Value type are not supported.");

            this.fieldInfo.SetValue(target, value);
        }
    }

#pragma warning disable 0414
    public class ProxyFieldInfo<T, TValue> : ProxyFieldInfo, IProxyFieldInfo<T, TValue>
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ProxyFieldInfo<T, TValue>));

        private Func<T, TValue> getter;
        private Action<T, TValue> setter;

        public ProxyFieldInfo(string fieldName) : this(typeof(T).GetField(fieldName))
        {
        }

        public ProxyFieldInfo(FieldInfo fieldInfo) : base(fieldInfo)
        {
            if (!typeof(TValue).Equals(this.fieldInfo.FieldType) || !this.DeclaringType.IsAssignableFrom(typeof(T)))
                throw new ArgumentException("The field types do not match!");

            this.getter = this.MakeGetter(fieldInfo);
            this.setter = this.MakeSetter(fieldInfo);
        }

        public ProxyFieldInfo(string fieldName, Func<T, TValue> getter, Action<T, TValue> setter) : this(typeof(T).GetField(fieldName), getter, setter)
        {
        }

        public ProxyFieldInfo(FieldInfo fieldInfo, Func<T, TValue> getter, Action<T, TValue> setter) : base(fieldInfo)
        {
            if (!typeof(TValue).Equals(this.fieldInfo.FieldType) || !this.DeclaringType.IsAssignableFrom(typeof(T)))
                throw new ArgumentException("The field types do not match!");

            this.getter = getter;
            this.setter = setter;
        }

        private Action<T, TValue> MakeSetter(FieldInfo fieldInfo)
        {
            if (this.IsValueType)
                return null;

            if (fieldInfo.IsInitOnly)
                return null;

            try
            {
                bool expressionSupportRestricted = false;
#if ENABLE_IL2CPP
                //Only reference types are supported; value types are not supported
                expressionSupportRestricted = true;
#endif
                if (!expressionSupportRestricted || !(typeof(T).IsValueType || typeof(TValue).IsValueType))
                {
                    var targetExp = Expression.Parameter(typeof(T), "target");
                    var paramExp = Expression.Parameter(typeof(TValue), "value");
                    var fieldExp = Expression.Field(fieldInfo.IsStatic ? null : targetExp, fieldInfo);
                    var assignExp = Expression.Assign(fieldExp, paramExp);
                    var lambda = Expression.Lambda<Action<T, TValue>>(assignExp, targetExp, paramExp);
                    return lambda.Compile();
                }
            }
            catch (Exception e)
            {
                if (log.IsWarnEnabled)
                    log.WarnFormat("{0}", e);
            }
            return null;
        }

        private Func<T, TValue> MakeGetter(FieldInfo fieldInfo)
        {
            try
            {
                bool expressionSupportRestricted = false;
#if ENABLE_IL2CPP
                //Only reference types are supported; value types are not supported
                expressionSupportRestricted = true;
#endif
                if (!expressionSupportRestricted || !(typeof(T).IsValueType || typeof(TValue).IsValueType))
                {
                    var targetExp = Expression.Parameter(typeof(T), "target");
                    var fieldExp = Expression.Field(fieldInfo.IsStatic ? null : targetExp, fieldInfo);
                    var lambda = Expression.Lambda<Func<T, TValue>>(fieldExp, targetExp);
                    return lambda.Compile();
                }
            }
            catch (Exception e)
            {
                if (log.IsWarnEnabled)
                    log.WarnFormat("{0}", e);
            }
            return null;
        }

        public override Type DeclaringType { get { return typeof(T); } }

        public TValue GetValue(T target)
        {
            if (this.getter != null)
                return this.getter(target);

            return (TValue)this.fieldInfo.GetValue(target);
        }

        public override object GetValue(object target)
        {
            if (this.getter != null)
                return this.getter((T)target);

            return this.fieldInfo.GetValue(target);
        }

        TValue IProxyFieldInfo<TValue>.GetValue(object target)
        {
            return this.GetValue((T)target);
        }

        public void SetValue(T target, TValue value)
        {
            if (fieldInfo.IsInitOnly)
                throw new MemberAccessException("The value is read-only.");

            if (IsValueType)
                throw new NotSupportedException("Assignments of Value type are not supported.");

            if (this.setter != null)
            {
                this.setter(target, value);
                return;
            }

            this.fieldInfo.SetValue(target, value);
        }

        public override void SetValue(object target, object value)
        {
            if (fieldInfo.IsInitOnly)
                throw new MemberAccessException("The value is read-only.");

            if (IsValueType)
                throw new NotSupportedException("Assignments of Value type are not supported.");

            if (this.setter != null)
            {
                this.setter((T)target, (TValue)value);
                return;
            }

            this.fieldInfo.SetValue(target, value);
        }

        public void SetValue(object target, TValue value)
        {
            this.SetValue((T)target, value);
        }
    }
}
