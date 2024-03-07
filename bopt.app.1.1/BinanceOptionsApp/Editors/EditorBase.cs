using MultiTerminal.Connections.Models;

namespace BinanceOptionsApp.Editors
{
    public interface IEditorBase<T> where T : ConnectionModel
    {
        T Model { get; set; }
        void Construct(T source);
    }
}
