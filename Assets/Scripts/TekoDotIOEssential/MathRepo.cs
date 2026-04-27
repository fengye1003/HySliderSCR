using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

internal class MathRepo // for unity, by cxt & hyrecv. v.2026.4.6.a.unity
{
    public static double SmoothMovePhysicFormula(double start, double end, double maxt, double t)
    {
        var delta = end - start;
        var x = -delta * (maxt / 2 - t) / (2 * Math.Abs(maxt / 2 - t)) + delta / 2 + (maxt / 2 - t) * 4 * delta * Math.Pow(-Math.Abs(t - maxt / 2) + maxt / 2, 2) / (Math.Abs(maxt / 2 - t) * 2 * Math.Pow(maxt, 2));
        return x + start;
    }
    public static ArrayList CreatePhysicalSmoothMovePointsSet(double start, double end, double max, double step)
    {
        double t = 0;
        ArrayList pointsSet = new();
        while (t < max)
        {
            pointsSet.Add(SmoothMovePhysicFormula(start, end, max, t));
            t += step;
        }
        return pointsSet;
    }
}