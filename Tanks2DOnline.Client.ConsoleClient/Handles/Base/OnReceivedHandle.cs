using Tanks2DOnline.Core.Serialization;

namespace Tanks2DOnline.Client.ConsoleClient.Handles.Base
{
    public abstract class OnReceivedHandle<TEntity> : IHandle where TEntity : SerializableObjectBase
    {
        protected abstract void Process(TEntity obj);
        protected abstract bool CanProcess(object obj);

        public void Process(object obj)
        {
            if (CanProcess(obj))
                Process(obj as TEntity);
        }
    }
}
