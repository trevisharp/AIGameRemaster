// Screen.PrimaryScreen.Bounds.Width;
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
    int position = 1;
    PointF go1 = new PointF(30, 30);
    PointF go2 = new PointF(30, 770);
    PointF go3 = new PointF(1250, 770);
    PointF go4 = new PointF(1250, 30);
    PointF? currentPosition = new PointF();
    int count = 0;
    int i = 0;
    int j = 0;
    PointF? enemy = null;
    double? lastlife = null;

    private float distance(PointF? pointA, PointF? pointB)
    {
        float deltaX = pointB.Value.X - pointA.Value.X;
        float deltaY = pointB.Value.Y - pointA.Value.Y;

        return (deltaX * deltaX) + (deltaY * deltaY);
    }
    protected override void loop()
    {
        currentPosition = this.Location;


        StartTurbo();

        if (lastlife != null && lastlife > Life)
        {
            count++;
        }
        if (count == 1)
        {
            count = 0;
            position++;
        }

        if (position == 1)
        {
            StartTurbo();
            StartMove(go2);
        }

        if (distance(go4, currentPosition) <= 25)
        {
            StopMove();
            StopTurbo();
            if (EnemiesInInfraRed.Count > 0)
            {
                enemy = EnemiesInInfraRed[0];
            }
            else
            {
                enemy = null;
            }
            if (enemy == null && Energy > 10)
                InfraRedSensor(5f * i++);
            if (Energy > 50)
            {
                if (enemy is not null)
                {
                    Shoot(enemy.Value);
                    enemy = null;
                }
            }
        }

        if (position == 2)
        {
            StartTurbo();
            StartMove(p: go3);
        }

        if (distance(go3, currentPosition) <= 25)
        {
            StopTurbo();
            StopMove();
            if (EnemiesInInfraRed.Count > 0)
            {
                enemy = EnemiesInInfraRed[0];
            }
            else
            {
                enemy = null;
            }
            if (enemy == null && Energy > 10)
                InfraRedSensor(5f * i++);
            if (Energy > 50)
            {
                if (enemy is not null)
                {
                    Shoot(enemy.Value);
                    enemy = null;
                }
            }
        }

        if (position == 3)
        {
            StartTurbo();
            StartMove(go4);
        }

        if (distance(go2, currentPosition) <= 25)
        {
            StopMove();
            StopTurbo();
            if (EnemiesInInfraRed.Count > 0)
            {
                enemy = EnemiesInInfraRed[0];
            }
            else
            {
                enemy = null;
            }
            if (enemy == null && Energy > 10)
                InfraRedSensor(5f * i++);
            if (Energy > 50)
            {
                if (enemy is not null)
                {
                    Shoot(enemy.Value);
                    enemy = null;
                }
            }
        }

        if (position == 4)
        {
            StartTurbo();
            StartMove(go1);
        }

        if (distance(go1, currentPosition) <= 25)
        {
            StopTurbo();
            StopMove();
            if (EnemiesInInfraRed.Count > 0)
            {
                enemy = EnemiesInInfraRed[0];
            }
            else
            {
                enemy = null;
            }
            if (enemy == null && Energy > 10)
                InfraRedSensor(5f * i++);
            if (Energy > 50)
            {
                if (enemy is not null)
                {
                    Shoot(enemy.Value);
                    enemy = null;
                }
            }
        }

        if (position == 5)
            position = 0;

        lastlife = Life;
    }

}
