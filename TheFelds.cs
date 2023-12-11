using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.Pkcs;

public class FeldPlayer : Player
{
    public FeldPlayer(PointF location) :
        base(location, Color.Purple, Color.HotPink, "Feldzinho") {}

    int i = 0;
    PointF go = new PointF(30, 30);
    PointF currentPosition = new PointF();
    PointF lastPosition = new PointF();
    private float distance (PointF pointA, PointF pointB){
        float x = pointB.X - pointA.X;
        float x2 = x*x;
        float y = pointB.Y - pointA.Y;
        float y2 = y*y;
        return x2 + y2;
    }

    PointF? enemy = null;

    protected override void loop()
    {
        // var go = LastDamage;
        // var ngo = new PointF(go.Value.X, go.Value.Y);
        // StartMove(LastDamage.Value); 
        currentPosition = this.Location;
        StartTurbo();
        if(this.Location.X != go.X)
            StartMove(go);
        if(distance(lastPosition, currentPosition) <= 10)
        {
            StopMove();
        }
        lastPosition = currentPosition;
    }

}