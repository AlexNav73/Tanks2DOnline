using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tanks2DOnline.Core.Serialization
{
    [Serializable]
    public class ObjectState
    {        
        private readonly Dictionary<string, object> _props = new Dictionary<string, object>(); 
        private readonly Dictionary<string, object> _fields = new Dictionary<string, object>();
        private readonly Dictionary<string, ObjectState> _states = new Dictionary<string, ObjectState>(); 

        public void AddProp(string name, object value) { _props.Add(name, value); }
        public void AddField(string name, object value) { _fields.Add(name, value); }
        public void AddState(string name, ObjectState state) { _states.Add(name, state); }

        public object GetProp(string name) { return _props.ContainsKey(name) ? _props[name] : null; }
        public object GetField(string name) { return _fields.ContainsKey(name) ? _fields[name] : null; }
        public ObjectState GetState(string name) { return _states.ContainsKey(name) ? _states[name] : null; }
    }
}
