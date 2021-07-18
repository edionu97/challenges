using System;

namespace Challenge3.Helpers.Items
{
    public class Point3d
    {
       public int X { get; }
       public int Y { get; }
       public int Z { get; }

       public Point3d(int x, int y, int z)
       {
           X = x;
           Y = y;
           Z = z;
       }
       public void Deconstruct(out int x, out int y, out int z)
       {
           x = X;
           y = Y;
           z = Z;
       }

       protected bool Equals(Point3d other)
       {
           var (x, y, z) = other;
           return X == x && Y == y && Z == z;
       }

       public override bool Equals(object obj)
       {
           if (obj is null)
           {
               return false;
           }

           if (ReferenceEquals(this, obj))
           {
               return true;
           }

           return obj.GetType() == GetType() && Equals((Point3d) obj);
       }

       public override int GetHashCode()
       {
           return HashCode.Combine(X, Y, Z);
       }

       public override string ToString()
       {
           return $"(x:{X}, y:{Y}, z:{Z})";
       }
    }
}
