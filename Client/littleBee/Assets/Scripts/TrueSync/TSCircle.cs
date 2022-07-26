namespace TrueSync
{
    public struct TSCircle
    {
        public FP Radius { get; private set; }
        public TSVector2 Center { get; private set; }
        public TSCircle(TSVector2 center,FP radius)
        {
            Radius = radius;
            Center = center;
        }
        public TSCircle(FP centerX,FP centerY ,FP radius)
        {
            Radius = radius;
            Center = new TSVector2(centerX, centerY);
        }
        public void Set(FP centerX,FP centerY,FP radius)
        {
            Radius = radius;
            Center = new TSVector2(centerX, centerY);
        }

        public bool Contain(TSVector2 point)
        {
            return TSVector2.DistanceSquared(point,Center) < RadiusSquared;
        }
        public FP RadiusSquared { get { return Radius * Radius; } }

        public bool Overlaps(TSCircle other)
        {
            return TSVector2.DistanceSquared(other.Center , Center) < (Radius + other.Radius) * (Radius + other.Radius);
        }

        public FP xMin 
        {
            get {
                return Center.x - Radius;
            }
        }
        public FP xMax
        {
            get {
                return Center.x + Radius;
            }
        }
        public FP yMin 
        {
            get {
                return Center.y - Radius;
            }
        }
        public FP yMax 
        {
            get {
                return Center.y + Radius;
            }
        }

    }
}
