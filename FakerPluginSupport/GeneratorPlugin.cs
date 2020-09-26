namespace FakerLib.PluginSupport
{
    public abstract class GeneratorPlugin<T> : Generator<T>, IPlugin
    {
        public abstract string PluginName { get; }
    }
}
