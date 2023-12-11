using System;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.Pkcs;
using System.Windows.Forms;

public class FeldPlayer : Player
{
    public FeldPlayer(PointF location) :
        base(location, Color.Yellow, Color.Blue, "Feldzinho")
    { }
    bool init = true;
    int position = 1;
    PointF go1 = new PointF(30, 30);
    PointF go2 = new PointF(30, 770);
    PointF go3 = new PointF(1250, 770);
    PointF go4 = new PointF(1250, 30);
    PointF? currentPosition = new PointF();

    double? lastlife = null;

    private float distance(PointF? pointA, PointF? pointB)
    {
        float deltaX = pointB.Value.X - pointA.Value.X;
        float deltaY = pointB.Value.Y - pointA.Value.Y;

        return (deltaX * deltaX) + (deltaY * deltaY);
    }

    double minLife = 75;
    PointF? enemy = null;

    protected override void loop()
    {
        // Screen.PrimaryScreen.Bounds.Width;
        currentPosition = this.Location;

        StartTurbo();
        if (lastlife != null && lastlife > Life)
        {
            if (Life < minLife)
            {
                position++;
                minLife = Life - 30;
            }
        }

        if (position == 1)
            StartMove(go4);

        if (distance(go4, currentPosition) <= 25)
            StopMove();

        if (position == 2)
            StartMove(p: go3);

        if (distance(go3, currentPosition) <= 25)
            StopMove();

        if (position == 3)
            StartMove(go2);

        if (distance(go2, currentPosition) <= 25)
            StopMove();

        if (position == 4)
            StartMove(go1);

        if (distance(go1, currentPosition) <= 25)
            StopMove();

        if (position == 5)
            position = 0;

        lastlife = Life;
    }

}