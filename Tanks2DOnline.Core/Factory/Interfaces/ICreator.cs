using System;

namespace Tanks2DOnline.Core.Factory.Interfaces
{
    public interface ICreator
    {
        object Create(Params prms);
    }
}