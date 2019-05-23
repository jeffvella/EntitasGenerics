using Performance.ViewModels;

namespace Performance
{
    public interface IElementFactory
    {
        ElementViewModel Create(string assetName, int assetType);

        void Return(ElementViewModel element);
    }
}