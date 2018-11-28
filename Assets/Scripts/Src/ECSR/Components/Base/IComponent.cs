namespace Components
{
    /// <summary>
    /// 组件数据
    /// </summary>
    public interface IComponent
    {
        string EntityId { set; get; }
        IComponent Clone();
        int GetCommand();
    }

    /// <summary>
    /// 参数可更新
    /// </summary>
    public interface IParamsUpdatable
    {
        void UpdateParams(string[] paramsStrs);

        //void ShrinkInteractiveComponent();
    }

    /// <summary>
    /// 碰撞数据更新
    /// </summary>
    public interface ICollisionUpdatable
    {
        VCollisionShape Collider { set; get; }
        void UpdateCollision(VInt3 location);
    }
}
