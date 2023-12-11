using System;
using System.Drawing;

public class BatataPlayer : Player
{
    public BatataPlayer(PointF location) : 
        base(location, Color.Red, Color.Gray, "Batata") { }

    int i = 0;
    int x = 0;
    int y = 0;

    PointF? enemyBatata = null;
    bool isloadingBatata = false;

    SizeF Dir = new(1280, 800);

    PointF go1 = new PointF(50, 50);

    private float distance(PointF? pointA, PointF? pointB)
    {
        float deltaX = pointB.Value.X - pointA.Value.X;
        float deltaY = pointB.Value.Y - pointA.Value.Y;

        return (deltaX * deltaX) + (deltaY * deltaY);
    }


    protected override void loop()
    {      
        StartTurbo();
        StartMove(go1);
 
        if(distance(Location, go1) < 20)
        {
            StopMove();

            if (EnemiesInInfraRed.Count > 0)
        {
            enemyBatata = EnemiesInInfraRed[0];
        }
        else
        {
           enemyBatata = null;
        }


            if (enemyBatata == null && Energy > 30)
                InfraRedSensor(5f * i++);
            else if (enemyBatata != null)
            {
                InfraRedSensor(enemyBatata.Value);
                if (i++ % 5 == 0)
                    Shoot(enemyBatata.Value);
        }

        }
    }
}