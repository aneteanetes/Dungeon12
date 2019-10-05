using System;
using System.Collections.Generic;
using System.Text;

namespace Rogue.Map
{
    public class FlowMapObject : MapObject, IFlowable
    {
        private object flowContext = null;

        public T GetFlowProperty<T>(string property) => flowContext.GetProperty<T>(property);

        public bool SetFlowProperty<T>(string property, T value)
        {
            try
            {
                flowContext.SetProperty(property, value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void SetFlowContext(object context) => flowContext = context;

        public object GetFlowContext() => flowContext;

        private IFlowable flowparent = null;

        public void SetParentFlow(IFlowable parent) => flowparent = parent;

        public IFlowable GetParentFlow() => flowparent;
    }
}
