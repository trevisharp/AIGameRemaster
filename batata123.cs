using System;
using System.Drawing;

public class BatataPlayer : Player
{
    public BatataPlayer(PointF location) : 
        base(location, Color.Red, Color.Yellow, "Fenix") { }
int i = 0;
    PointF? enemy = null;
    bool isloading = false;

    protected override void loop()
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
            StopMove();
            isloading = true;
            enemy = null;
        }
        if (isloading)
        {
            if (Energy > 40)
                isloading = false;
            else return;
        }
        if (enemy == null && Energy > 10)
            InfraRedSensor(5f * i++);
        else if (enemy != null && Energy > 10)
        {
            InfraRedSensor(enemy.Value);
            float dx = enemy.Value.X - this.Location.X,
                  dy = enemy.Value.Y - this.Location.Y;
            if (dx*dx + dy*dy >= 400f*400f)
                StartMove(enemy.Value);
            else
            {
                StopMove();
                if (i++ % 6 == 0)
                    Shoot(enemy.Value);
            }
        }
    }
}
