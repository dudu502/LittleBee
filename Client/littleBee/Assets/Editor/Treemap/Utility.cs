using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Treemap
{
    public class Utility
    {
        public static Rect[] GetTreemapRects(float[] values, Rect targetRect)
        {
			if (values.Length == 0)
				throw new ArgumentException ("You need to at least pass in one valid value", "values");
				
            Rect[] result = new Rect[values.Length];
           
            float totalInputArea = 0f;
            for (int i = 0; i < values.Length; i++)
                totalInputArea += values[i];

            float totalOutputArea = targetRect.width * targetRect.height;
            bool vertical = targetRect.width > targetRect.height;

            var unfinishedRects = new List<Rect>();

            for (int index = 0; index < values.Length; index++)
            {
                bool lastItem = index == values.Length - 1;

                float currentInputValue = values[index];

				if (currentInputValue < 0f)
					throw new ArgumentException ("only positive float values are supported. found: " + currentInputValue);

                float currentOutputArea = currentInputValue * totalOutputArea / totalInputArea;
                unfinishedRects = AddRect(unfinishedRects, currentOutputArea, targetRect, vertical);
                float currentAspect = GetAverageAspect(unfinishedRects);

                float nextInputValue = lastItem ? 0f : values[index + 1];
                float nextOutputArea = nextInputValue * totalOutputArea / totalInputArea;
                float nextAspect = GetNextAspect(unfinishedRects, nextOutputArea, targetRect, vertical);

                if (Mathf.Abs(1f - currentAspect) < Mathf.Abs(1f - nextAspect) || lastItem)
                {
                    int resultIndex = index - unfinishedRects.Count + 1;
                    for (int rectIndex = 0; rectIndex < unfinishedRects.Count; rectIndex++)
                    {
                        result[resultIndex++] = unfinishedRects[rectIndex];
                    }

                    targetRect = GetNewTarget(unfinishedRects, targetRect, vertical);
                    vertical = !vertical;
                    unfinishedRects.Clear();
                }
            }

            return result;
        }

        private static List<Rect> AddRect(List<Rect> existing, float area, Rect space, bool vertical)
        {
            List<Rect> result = new List<Rect>();
            if (vertical)
            {
                if (existing.Count == 0)
                {
                    result.Add(new Rect(space.xMin, space.yMin, area / space.height, space.height));
                }
                else
                {
                    float totalSize = GetArea(existing) + area;
                    float width = totalSize / space.height;
                    float yPosition = space.yMin;
                    foreach (Rect old in existing)
                    {
                        float itemArea = GetArea(old);
                        result.Add(new Rect(old.xMin, yPosition, width, itemArea / width));
                        yPosition += itemArea / width;
                    }
                    result.Add(new Rect(space.xMin, yPosition, width, area / width));
                }
            }
            else
            {
                if (existing.Count == 0)
                {
                    result.Add(new Rect(space.xMin, space.yMin, space.width, area / space.width));
                }
                else
                {
                    float totalSize = GetArea(existing) + area;
                    float height = totalSize / space.width;
                    float xPosition = space.xMin;
                    foreach (Rect old in existing)
                    {
                        float itemArea = GetArea(old);
                        result.Add(new Rect(xPosition, old.yMin, itemArea / height, height));
                        xPosition += itemArea / height;
                    }
                    result.Add(new Rect(xPosition, space.yMin, area / height, height));
                }
            }
            return result;
        }

        private static Rect GetNewTarget(List<Rect> unfinished, Rect oldTarget, bool vertical)
        {
            if (vertical)
            {
                return new Rect(oldTarget.xMin + unfinished[0].width, oldTarget.yMin, oldTarget.width - unfinished[0].width, oldTarget.height);
            }
            else
            {
                return new Rect(oldTarget.xMin, oldTarget.yMin + unfinished[0].height, oldTarget.width, oldTarget.height - unfinished[0].height);
            }
        }

        private static float GetNextAspect(List<Rect> existing, float area, Rect space, bool vertical)
        {
            List<Rect> newExisting = AddRect(existing, area, space, vertical);
            return newExisting[newExisting.Count - 1].height / newExisting[newExisting.Count - 1].width;
        }

        private static float GetAverageAspect(List<Rect> rects)
        {
            float aspect = 0f;
            foreach (Rect r in rects)
            {
                aspect += r.height / r.width;
            }
            return aspect / rects.Count;
        }

        private static float GetArea(Rect rect)
        {
            return rect.width * rect.height;
        }

        private static float GetArea(List<Rect> rects)
        {
            return rects.Sum(x => GetArea(x));
        }

        public static Color GetColorForName(string name)
        {
            int r = 0, g = 0, b = 0;

            for (int i = 0; i < name.Length; i++)
            {
                if (i % 3 == 0)
                {
                    r += (int)name[i];
                }
                else if (i % 3 == 1)
                {
                    g += (int)name[i];
                }
                else
                {
                    b += (int)name[i];
                }
            }

            r %= 128;
            g %= 128;
            b %= 128;

            return new Color32((byte)(r + 96), (byte)(g + 96), (byte)(b + 96), 255);
        }

        public static bool IsInside(Rect lhs, Rect rhs)
        {
            return lhs.xMax > rhs.xMin && lhs.xMin < rhs.xMax && lhs.yMax > rhs.yMin && lhs.yMin < rhs.yMax;
        }
    }
}
