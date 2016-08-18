using UnityEngine;
using System.IO;
using System.Collections.Generic;

namespace GeneratedCuboids
{
    public class UVBitmapGenerator
    {
        private Texture2D texture;
        private List<Pair<Point, Point>> lines;
        private List<Vector2> uvs;
        private int uvSize;

        private Color DRAWING_COLOR = Color.black;

        private Texture2D DrawTexture(List<Vector2> uvs, int uvSize)
        {
            ResetVariables(uvs, uvSize);

            FindLines();
            DrawLines();

            texture.Apply();
            return texture;
        }

        private void DrawLines()
        {
            for (int x = 0; x <= uvSize; x++)
            {
                for (int y = 0; y <= uvSize; y++)
                {
                    if (LinesContainPoint(new Point(x, y)))
                    {
                        texture.SetPixel(x, y, DRAWING_COLOR);
                    }
                }
            }
        }

        private void FindLines()
        {
            foreach (Vector2 a in uvs)
            {
                foreach (Vector2 b in uvs)
                {
                    if (a.Equals(b))
                    {
                        continue;
                    }

                    if (Mathf.Approximately(a.x, b.x) || Mathf.Approximately(a.y, b.y))
                    {
                        lines.Add(new Pair<Point, Point>(
                            new Point(Mathf.RoundToInt(a.x * uvSize), Mathf.RoundToInt(a.y * uvSize)),
                            new Point(Mathf.RoundToInt(b.x * uvSize), Mathf.RoundToInt(b.y * uvSize))
                            ));
                    }
                }
            }

            RemoveDoubles();
        }

        private void RemoveDoubles()
        {
            List<Pair<Point, Point>> newLines = new List<Pair<Point, Point>>();
            bool isContained = false;

            foreach (Pair<Point, Point> a in lines)
            {
                isContained = false;
                foreach (Pair<Point, Point> b in lines)
                {
                    if (a.Equals(b))
                    {
                        // same line -> ignore
                        continue;
                    }

                    if (a.First.Equals(b.Second) && b.First.Equals(a.Second))
                    {
                        // Same line (points are swapped) -> ignore
                        continue;
                    }

                    if (LineContainsLine(b, a))
                    {
                        // Line b contains line a -> don't add
                        isContained = true;
                        break;
                    }
                }

                if (!isContained)
                {
                    MaybeAddLine(newLines, a);
                }
            }

            lines = newLines;
        }

        private void MaybeAddLine(List<Pair<Point, Point>> newLines, Pair<Point, Point> a)
        {
            foreach (Pair<Point, Point> n in newLines)
            {
                if (a.First.Equals(n.Second) && n.First.Equals(a.Second))
                {
                    // Same line (points swapped) already added -> break
                    return;
                }
            }

            newLines.Add(a);
        }

        private void ResetVariables(List<Vector2> uvs, int uvSize)
        {
            this.lines = new List<Pair<Point, Point>>();
            this.texture = new Texture2D(uvSize, uvSize);
            this.uvs = uvs;
            this.uvSize = uvSize - 1;
        }

        public string StoreUVMap(List<Vector2> uvs, int uvSize)
        {
            Texture2D texture = DrawTexture(uvs, uvSize);

            // Encode texture into PNG
            byte[] bytes = texture.EncodeToPNG();
            string filename =
                "/CuboidUvMap_" +
                System.DateTime.Now.Year +
                System.DateTime.Now.Month +
                System.DateTime.Now.Day +
                System.DateTime.Now.Hour +
                System.DateTime.Now.Minute +
                System.DateTime.Now.Second +
                ".png";
            File.WriteAllBytes(Application.dataPath + filename, bytes);

            UnityEngine.Object.DestroyImmediate(texture);

            return "UV map has been stored in: " + Application.dataPath + filename;
        }

        /// <summary>
        /// Returns true, if the list of lines has a line which contains the provided Point
        /// </summary>
        private bool LinesContainPoint(Point point)
        {
            foreach (Pair<Point, Point> line in lines)
            {
                if (LineContainsPoint(point, line))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns true if the line a contains the line b
        /// </summary>
        private bool LineContainsLine(Pair<Point, Point> a, Pair<Point, Point> b)
        {
            return LineContainsPoint(b.First, a) && LineContainsPoint(b.Second, a);
        }

        private bool LineContainsPoint(Point point, Pair<Point, Point> line)
        {
            return Mathf.Approximately(Point.Distance(line.First, point) + Point.Distance(line.Second, point), Point.Distance(line.First, line.Second));
        }
    }
}
