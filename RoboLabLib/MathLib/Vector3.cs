﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboLab.MathLib
{
    public class Vector3
    {
        public double x;
        public double y;
        public double z;

        public Vector3()
        {
            x = 0;
            y = 0;
            z = 0;
        }

        public Vector3(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static double dot(Vector3 a, Vector3 b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z;
        }

        public static double getAngleBetweenVectors(Vector3 a, Vector3 b)
        {
            double cos = Vector3.dot(a, b);
            return Math.Acos(cos);
        }

        public void multiplyScalar(double mult)
        {
            this.x *= mult;
            this.y *= mult;
            this.z *= mult;
        }

        public void rotateOnY(double angle)
        {
            this.x = this.x * Math.Cos(angle) - this.z * Math.Sin(angle);
            this.z = this.x * Math.Sin(angle) + this.z * Math.Cos(angle);
        }

        public void add(Vector3 vec)
        {
            this.x += vec.x;
            this.y += vec.y;
            this.z += vec.z;
        }

        public double length()
        {
            return Math.Sqrt(x * x + y * y + z * z);
        }

        public void normalize()
        {
            double l = length();
            x /= l;
            y /= l;
            z /= l;
        }

        public bool isNull()
        {
            return x == 0 && y == 0 && z == 0;
        }

        public bool isReverseDirection(Vector3 vec)
        {
            return x == -vec.x && y == -vec.y && z == -vec.z;
        }

        public bool isDifferentDirection(Vector3 vec)
        {
            return x != vec.x || y != vec.y || z != vec.z;
        }

        public Vector3 Clone()
        {
            return new Vector3(x, y, z);
        }

        public static double Distance(Vector3 vec1, Vector3 vec2)
        {
            Vector3 v = new Vector3(vec2.x - vec1.x, vec2.y - vec1.y, vec2.z - vec1.z);
            return v.length();
        }
    }
}
