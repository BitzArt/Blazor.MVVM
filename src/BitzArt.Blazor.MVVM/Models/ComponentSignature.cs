namespace BitzArt.Blazor.MVVM;

internal class RootComponentSignature : ComponentSignature
{
    public override bool IsRoot() => true;

    public RootComponentSignature() : base(null) { }
}

internal class ComponentSignature
{
    public virtual bool IsRoot() => false;

    public virtual ComponentSignature? ParentSignature { get; set; }

    public ComponentSignature(ComponentSignature? parent)
    {
        ParentSignature = parent;
    }

    public ComponentSignature NestNew()
    {
        return new ComponentSignature(parent: this);
    }
}
