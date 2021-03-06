﻿using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using Tanks2DOnline.Core.Extensions;
using Tanks2DOnline.Core.Serialization.Attributes;

namespace Tanks2DOnline.Core.Serialization
{
    public abstract class SerializableObjectBase
    {
        private readonly Type _self = typeof (SerializableObjectBase);
        public const long UdpPacketMaxSize = 25535;

        public abstract DataType GetDataType();

        public virtual byte[] Serialize()
        {
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();

                BeforeSerialization();
                formatter.Serialize(stream, this.GetState());
                AfterSerialization();

                return stream.ToArray();
            }
        }

        public virtual void Desirialize(byte[] bytes, int count)
        {
            using (var stream = new MemoryStream(bytes, 0, count))
            {
                var formatter = new BinaryFormatter();
                var obj = (ObjectState)formatter.Deserialize(stream);
                BeforeDeserialization();
                this.SetState(obj);
                AfterDeserialization();
            }
        }

        public ObjectState GetState()
        {
            ObjectState objState = new ObjectState();

            TraverseObjectGraph(GetType(), objState, GetFieldState, GetPropertyState);

            return objState;
        }

        public void SetState(object state)
        {
            ObjectState objState = state as ObjectState;
            if (objState == null) return;

            TraverseObjectGraph(GetType(), objState, SetFieldState, SetPropertyState);
        }

        private static void TraverseObjectGraph(Type t, 
            ObjectState objState, 
            Action<FieldInfo, ObjectState> fieldPredicate,
            Action<PropertyInfo, ObjectState> propPredicate)
        {
            t.GetProperties()
                .Where(p => p.GetCustomAttribute<MarkAttribute>() != null)
                .ForEach(p => propPredicate(p, objState));

            t.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(f => f.GetCustomAttribute<MarkAttribute>() != null)
                .ForEach(f => fieldPredicate(f, objState));
        }

        #region Process Fields and Property
        private void GetFieldState(FieldInfo field, ObjectState objState)
        {
            if (field.FieldType.IsSubclassOf(_self))
            {
                var value = field.GetValue(this);
                objState.AddState(field.Name, (ObjectState)value.GetType().GetMethod("GetState").Invoke(value, null));
            }
            else objState.AddField(field.Name, field.GetValue(this));
        }

        private void GetPropertyState(PropertyInfo prop, ObjectState objState)
        {
            if (prop.PropertyType.IsSubclassOf(_self))
            {
                var value = prop.GetValue(this);
                objState.AddState(prop.Name, (ObjectState)value.GetType().GetMethod("GetState").Invoke(value, null));
            }
            else objState.AddProp(prop.Name, prop.GetValue(this));
        }

        private void SetPropertyState(PropertyInfo prop, ObjectState objState)
        {
            if (prop.PropertyType.IsSubclassOf(_self))
            {
                var value = prop.GetValue(this);
                prop.PropertyType.GetMethod("SetState").Invoke(value, new object[]{ objState.GetState(prop.Name) });
            }
            else prop.SetValue(this, objState.GetProp(prop.Name));
        }

        private void SetFieldState(FieldInfo field, ObjectState objState)
        {
            if (field.FieldType.IsSubclassOf(_self))
            {
                var value = field.GetValue(this);
                value.GetType().GetMethod("SetState").Invoke(value, new object[]{ objState.GetState(field.Name) });
            }
            else field.SetValue(this, objState.GetField(field.Name));
        }
#endregion 

        protected virtual void BeforeSerialization() { }
        protected virtual void AfterSerialization() { }
        protected virtual void BeforeDeserialization() { }
        protected virtual void AfterDeserialization() { }
    }
}
