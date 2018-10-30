using UnityEngine;
using System.Collections.Generic;
using ILRuntime.Other;
using System;
using System.Collections;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Stack;
using ILRuntime.CLR.TypeSystem;
using Google.Protobuf;

public class IMessageAdapter : CrossBindingAdaptor
{
    public override Type BaseCLRType
    {
        get
        {
            return typeof(Google.Protobuf.IMessage<ILTypeInstance>);
        }
    }

    public override Type AdaptorType
    {
        get
        {
            return typeof(Adaptor);
        }
    }

    public override object CreateCLRInstance(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
    {
        return new Adaptor(appdomain, instance);
    }

    public class Adaptor : MyAdaptor, IMessage<ILTypeInstance>, CrossBindingAdaptorType
    {
        public Adaptor() { }

        public Adaptor(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance) : base(appdomain, instance) { }

        protected override Dictionary<string, AdaptHelper.AdaptMethod> GetAdaptMethods()
        {
            var methods = new Dictionary<string, AdaptHelper.AdaptMethod>
            {
                {"MergeFromCodedInputStream", new AdaptHelper.AdaptMethod {Name = "MergeFrom", ParamCount = 1, param = new List<IType>{AppDomain.GetType(typeof(CodedInputStream)) } } },
                {"MergeFromThis", new AdaptHelper.AdaptMethod {Name = "MergeFrom", ParamCount = 1, param = new List<IType>{ AppDomain.GetType(typeof(ILTypeInstance)) } } },
                {"WriteTo", new AdaptHelper.AdaptMethod {Name = "WriteTo", ParamCount = 1} },
                {"CalculateSize", new AdaptHelper.AdaptMethod {Name = "CalculateSize", ParamCount = 0} },
            };
            return methods;
        }

        public ILTypeInstance Clone()
        {
            return null;
        }

        public void MergeFrom(CodedInputStream input)
        {
            Invoke("MergeFromCodedInputStream", input);
        }

        public void MergeFrom(ILTypeInstance input)
        {
            Invoke("MergeFromThis", input);
        }

        public void WriteTo(CodedOutputStream output)
        {
            Invoke("WriteTo", output);
        }

        public int CalculateSize()
        {
            return (int)Invoke("CalculateSize");
        }

        object[] data1 = new object[1];
        IMethod mEquals = null;
        bool mEqualsGot = false;
        public bool Equals(ILTypeInstance other)
        {
            if (!mEqualsGot)
            {
                mEquals = ILInstance.Type.GetMethod("Equals", 1);
                if (mEquals == null)
                {
                    mEquals = ILInstance.Type.GetMethod("System.IEquatable.Equals", 1);
                }
                mEqualsGot = true;
            }
            if (mEquals != null)
            {
                data1[0] = other;
                return (bool)AppDomain.Invoke(mEquals, ILInstance, data1);
            }
            return false;
        }

        public override string ToString()
        {
            IMethod m = AppDomain.ObjectType.GetMethod("ToString", 0);
            m = ILInstance.Type.GetVirtualMethod(m);
            if (m == null || m is ILMethod)
            {
                return ILInstance.ToString();
            }
            else
                return ILInstance.Type.FullName;
        }
    }
}
