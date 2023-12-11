using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.Pkcs;

public class FeldPlayer : Player
{
    public FeldPlayer(PointF location) :
        base(location, Color.Yellow, Color.Blue, "Feldzinho")
    { }
    bool init = true;
    int i = 0;
    PointF go1 = new PointF(30, 30);
    PointF go2 = new PointF(30, 1050);
    PointF go3 = new PointF(1900, 1050);
    PointF go4 = new PointF(1900, 30);
    PointF? currentPosition = new PointF();
    private float distance(PointF? pointA, PointF? pointB)
    {
        float deltaX = pointB.Value.X - pointA.Value.X;
        float deltaY = pointB.Value.Y - pointA.Value.Y;

        return (deltaX * deltaX) + (deltaY * deltaY);
    }


    PointF? enemy = null;

    protected override void loop()
    {
        currentPosition = this.Location;
        StartTurbo();
        if (init)
        {
            StartMove(go1);
            init = false;
        }

        if (distance(go1, currentPosition) <= 25)
        {
            StartMove(p: go2);
        }
        if (distance(go2, currentPosition) <= 25)
        {
            StartMove(go3);
        }
        if (distance(go3, currentPosition) <= 25)
        {
            StartMove(go4);
        }
        if (distance(go4, currentPosition) <= 25)
        {
            init = true;
        }
    }

}