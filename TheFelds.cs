// Screen.PrimaryScreen.Bounds.Width;
using System;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.Pkcs;
using System.Windows.Forms;

public class FeldPlayer : Player
{
    PointF? enemy = null;
    bool isloading = false;
    int i = 0;


    public FeldPlayer(PointF location) :
        base(location, Color.Yellow, Color.Blue, "Feldzinho")
    { }
    int position = 1;
    PointF go1 = new PointF(30, 30);
    PointF go2 = new PointF(30, 770);
    PointF go3 = new PointF(1250, 770);
    PointF go4 = new PointF(1250, 30);
    PointF? currentPosition = new PointF();
    bool isStart = true;
    int count = 0;
    double? lastlife = null;

    int frame = 0;

    private float distance(PointF? pointA, PointF? pointB)
    {
        float deltaX = pointB.Value.X - pointA.Value.X;
        float deltaY = pointB.Value.Y - pointA.Value.Y;

        return (deltaX * deltaX) + (deltaY * deltaY);
    }
    protected override void loop()
    {
        currentPosition = this.Location;

        // if (isStart)
        // {
        //     var p1 = distance(go4, currentPosition);
        //     var p2 = distance(go3, currentPosition);
        //     var p3 = distance(go2, currentPosition);
        //     var p4 = distance(go1, currentPosition);
        //     PointF[] points = { go1, go2, go3, go4 };
        //     float[] array = { p1, p2, p3, p4 };
        //     double minDistance = array[0];
        //     int minIndex = 0;
        //     for (int i = 1; i < array.Length; i++)
        //     {
        //         if (array[i] < minDistance)
        //         {
        //             minDistance = array[i];
        //             minIndex = i;
        //         }
        //     }
        //     position = minIndex;
        //     isStart = false;
        // }


        StartTurbo();

        if (lastlife != null && lastlife > Life)
        {
            count++;
        }
        if (count == 2)
        {
            count = 0;
            position++;
        }

        if (position == 1)
            StartMove(go4);

        if (distance(go4, currentPosition) <= 25)
        {
            InfraRedSensorGo();
            StopMove();
        }

        if (position == 2)
            StartMove(p: go3);

        if (distance(go3, currentPosition) <= 25)
        {
            InfraRedSensorGo();
            StopMove();
        }

        if (position == 3)
            StartMove(go2);

        if (distance(go2, currentPosition) <= 25)
        {

            InfraRedSensorGo();
            StopMove();
        }

        if (position == 4)
            StartMove(go1);

        if (distance(go1, currentPosition) <= 25)
        {
            InfraRedSensorGo();
            StopMove();
        }

        if (position == 5)
            position = 1;

        lastlife = Life;
        frame++;
    }

    void InfraRedSensorGo()
    {


        if (EnemiesInInfraRed.Count > 0)
        {
            enemy = EnemiesInInfraRed[0];
        }
        else
        {
            enemy = null;
        }
        if (Energy < 10)
        {
            StopTurbo();
            isloading = true;
            enemy = null;
        }
        if (isloading)
        {
            if (Energy > 40)
                isloading = false;
            else return;
        }
        if (enemy == null && Energy > 50)
            InfraRedSensor(5f * i++);
        else if (enemy != null && Energy > 60)
        {
            InfraRedSensor(enemy.Value);
            if (i++ % 5 == 0 && (frame % 3 == 0))
            {
                Shoot(enemy.Value);
                Shoot(enemy.Value);
                Shoot(enemy.Value);
            }
        }
    }
}
