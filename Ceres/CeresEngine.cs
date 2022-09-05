using Ceres.Windowing;

namespace Ceres;

public class CeresEngine
{
    private CeresEngine() { }
    
    public static CeresApplicationBuilder<T> Create<T>(InitialisationOptions options) where T : CeresApplication
    {
        return new CeresApplicationBuilder<T>(options);
    }

    public static CeresApplicationBuilder<T> Create<T>() where T : CeresApplication => Create<T>(new InitialisationOptions());
}