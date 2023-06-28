using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueSync
{
    [Serializable]
    public struct TSRect : IEquatable<TSRect>
    {             
        private FP m_XMin;

        private FP m_YMin;

        private FP m_Width;

        private FP m_Height;
        public TSRect(FP x, FP y,FP width,FP height)
        {
            this.m_XMin = x;
            this.m_YMin = y;
            this.m_Width = width;
            this.m_Height = height;
        }
        public TSRect(TSVector2 position, TSVector2 size)
        {
            this.m_XMin = position.x;
            this.m_YMin = position.y;
            this.m_Width = size.x;
            this.m_Height = size.y;
        }
        public TSRect(TSRect source)
        {
            this.m_XMin = source.m_XMin;
            this.m_YMin = source.m_YMin;
            this.m_Width = source.m_Width;
            this.m_Height = source.m_Height;
        }
        public static TSRect zero
        {
            get
            {
                return new TSRect(0f, 0f, 0f, 0f);
            }
        }
        public void Set(FP x, FP y, FP width, FP height)
        {
            this.m_XMin = x;
            this.m_YMin = y;
            this.m_Width = width;
            this.m_Height = height;
        }
        public FP x
        {
            get
            {
                return this.m_XMin;
            }
            set
            {
                this.m_XMin = value;
            }
        }
        public FP y
        {
            get
            {
                return this.m_YMin;
            }
            set
            {
                this.m_YMin = value;
            }
        }
        public TSVector2 position
        {
            get
            {
                return new TSVector2(this.m_XMin, this.m_YMin);
            }
            set
            {
                this.m_XMin = value.x;
                this.m_YMin = value.y;
            }
        }
        public TSVector2 center
        {
            get
            {
                return new TSVector2(this.x + this.m_Width / 2f, this.y + this.m_Height / 2f);
            }
            set
            {
                this.m_XMin = value.x - this.m_Width / 2f;
                this.m_YMin = value.y - this.m_Height / 2f;
            }
        }
        public FP xMin
        {
            get
            {
                return this.m_XMin;
            }
            set
            {
                FP xMax = this.xMax;
                this.m_XMin = value;
                this.m_Width = xMax - this.m_XMin;
            }
        }
        public FP yMax
        {
            get
            {
                return this.m_Height + this.m_YMin;
            }
            set
            {
                this.m_Height = value - this.m_YMin;
            }
        }
        public FP xMax
        {
            get
            {
                return this.m_Width + this.m_XMin;
            }
            set
            {
                this.m_Width = value - this.m_XMin;
            }
        }
        public FP yMin
        {
            get
            {
                return this.m_YMin;
            }
            set
            {
                FP yMax = this.yMax;
                this.m_YMin = value;
                this.m_Height = yMax - this.m_YMin;
            }
        }
        public TSVector2 size
        {
            get
            {
                return new TSVector2(this.m_Width, this.m_Height);
            }
            set
            {
                this.m_Width = value.x;
                this.m_Height = value.y;
            }
        }
        public FP height
        {
            get
            {
                return this.m_Height;
            }
            set
            {
                this.m_Height = value;
            }
        }
        public FP width
        {
            get
            {
                return this.m_Width;
            }
            set
            {
                this.m_Width = value;
            }
        }
        public TSVector2 min
        {
            get
            {
                return new TSVector2(this.xMin, this.yMin);
            }
            set
            {
                this.xMin = value.x;
                this.yMin = value.y;
            }
        }
        public TSVector2 max
        {
            get
            {
                return new TSVector2(this.xMax, this.yMax);
            }
            set
            {
                this.xMax = value.x;
                this.yMax = value.y;
            }
        }
        public bool Contains(TSVector point, bool allowInverse)
        {
            bool result;
            if (!allowInverse)
            {
                result = this.Contains(point);
            }
            else
            {
                bool flag = false;
                if ((this.width < 0f && point.x <= this.xMin && point.x > this.xMax) || (this.width >= 0f && point.x >= this.xMin && point.x < this.xMax))
                {
                    flag = true;
                }
                result = (flag && ((this.height < 0f && point.y <= this.yMin && point.y > this.yMax) || (this.height >= 0f && point.y >= this.yMin && point.y < this.yMax)));
            }
            return result;
        }
        private static TSRect OrderMinMax(TSRect rect)
        {
            if (rect.xMin > rect.xMax)
            {
                FP xMin = rect.xMin;
                rect.xMin = rect.xMax;
                rect.xMax = xMin;
            }
            if (rect.yMin > rect.yMax)
            {
                FP yMin = rect.yMin;
                rect.yMin = rect.yMax;
                rect.yMax = yMin;
            }
            return rect;
        }

        public bool Contains(TSVector point)
        {
            return point.x >= this.xMin && point.x < this.xMax && point.y >= this.yMin && point.y < this.yMax;
        }
        public bool Contains(TSVector2 point)
        {
            return point.x >= this.xMin && point.x < this.xMax && point.y >= this.yMin && point.y < this.yMax;
        }
        public bool Contains(TSCircle circle)
        {
            return circle.xMin > this.xMin && circle.yMin > this.yMin && circle.xMax < this.xMax && circle.yMax < this.yMax;
        }
        public static TSRect MinMaxRect(FP xmin, FP ymin, FP xmax, FP ymax)
        {
            return new TSRect(xmin, ymin, xmax - xmin, ymax - ymin);
        }
        public bool Intersect(TSCircle circle)
        {
            //TSVector2 p0 = min;
            //TSVector2 p1 = new TSVector2(min.x, max.y);
            //TSVector2 p2 = max;
            //TSVector2 p3 = new TSVector2(max.x, min.y);
            //FP circleRadiusSquared = circle.RadiusSquared;
            //if (TSVector2.DistanceSquared(p0, circle.Center) < circleRadiusSquared)
            //    return true;
            //if (TSVector2.DistanceSquared(p1, circle.Center) < circleRadiusSquared)
            //    return true;
            //if (TSVector2.DistanceSquared(p2, circle.Center) < circleRadiusSquared)
            //    return true;
            //if (TSVector2.DistanceSquared(p3, circle.Center) < circleRadiusSquared)
            //    return true;
            return false;
        }
        public bool Overlaps(TSRect other, bool allowInverse)
        {
            TSRect rect = this;
            if (allowInverse)
            {
                rect = TSRect.OrderMinMax(rect);
                other = TSRect.OrderMinMax(other);
            }
            return rect.Overlaps(other);
        }
        public static TSVector2 NormalizedToPoint(TSRect rectangle, TSVector2 normalizedRectCoordinates)
        {
            return new TSVector2(TSMath.Lerp(rectangle.x, rectangle.xMax, normalizedRectCoordinates.x), TSMath.Lerp(rectangle.y, rectangle.yMax, normalizedRectCoordinates.y));
        }
        public bool Overlaps(TSRect other)
        {
            return other.xMax > this.xMin && other.xMin < this.xMax && other.yMax > this.yMin && other.yMin < this.yMax;
        }


        public static TSVector2 PointToNormalized(TSRect rectangle, TSVector2 point)
        {
            return new TSVector2(TSMath.InverseLerp(rectangle.x, rectangle.xMax, point.x), TSMath.InverseLerp(rectangle.y, rectangle.yMax, point.y));
        }
        public static bool operator !=(TSRect lhs, TSRect rhs)
        {
            return !(lhs == rhs);
        }

        // Token: 0x060010B9 RID: 4281 RVA: 0x00018444 File Offset: 0x00016644
        public static bool operator ==(TSRect lhs, TSRect rhs)
        {
            return lhs.x == rhs.x && lhs.y == rhs.y && lhs.width == rhs.width && lhs.height == rhs.height;
        }

        // Token: 0x060010BA RID: 4282 RVA: 0x000184A8 File Offset: 0x000166A8
        public override int GetHashCode()
        {
            return this.x.GetHashCode() ^ this.width.GetHashCode() << 2 ^ this.y.GetHashCode() >> 2 ^ this.height.GetHashCode() >> 1;
        }

        // Token: 0x060010BB RID: 4283 RVA: 0x00018518 File Offset: 0x00016718
        public override bool Equals(object other)
        {
            return other is TSRect && this.Equals((TSRect)other);
        }

        // Token: 0x060010BC RID: 4284 RVA: 0x0001854C File Offset: 0x0001674C
        public bool Equals(TSRect other)
        {
            return this.x.Equals(other.x) && this.y.Equals(other.y) && this.width.Equals(other.width) && this.height.Equals(other.height);
        }

        /// <summary>
        ///   <para>Returns a nicely formatted string for this Rect.</para>
        /// </summary>
        /// <param name="format"></param>
        // Token: 0x060010BD RID: 4285 RVA: 0x000185CC File Offset: 0x000167CC
        public override string ToString()
        {
            return string.Format("(x:{0:F2}, y:{1:F2}, width:{2:F2}, height:{3:F2})", new object[]
            {
                this.x,
                this.y,
                this.width,
                this.height
            });
        }

        /// <summary>
        ///   <para>Returns a nicely formatted string for this Rect.</para>
        /// </summary>
        /// <param name="format"></param>
        public string ToString(string format)
        {
            return string.Format("(x:{0}, y:{1}, width:{2}, height:{3})", new object[]
            {
                this.x.ToString(format),
                this.y.ToString(format),
                this.width.ToString(format),
                this.height.ToString(format)
            });
        }

        [Obsolete("use xMin")]
        public FP left
        {
            get
            {
                return this.m_XMin;
            }
        }

 
        [Obsolete("use xMax")]
        public FP right
        {
            get
            {
                return this.m_XMin + this.m_Width;
            }
        }

        // Token: 0x17000382 RID: 898
        // (get) Token: 0x060010C1 RID: 4289 RVA: 0x000186DC File Offset: 0x000168DC
        [Obsolete("use yMin")]
        public FP top
        {
            get
            {
                return this.m_YMin;
            }
        }

        // Token: 0x17000383 RID: 899
        // (get) Token: 0x060010C2 RID: 4290 RVA: 0x000186F8 File Offset: 0x000168F8
        [Obsolete("use yMax")]
        public FP bottom
        {
            get
            {
                return this.m_YMin + this.m_Height;
            }
        }
    }
}
