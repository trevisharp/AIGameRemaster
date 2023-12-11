using System.Drawing;


[Test]
public class MMMA : Player
{
    public MMMA(PointF location) : 
        base(location, Color.Lime, Color.Gray, "MMMA") { }

    int i = 0;
    PointF? enemy = null;
    bool isloading = false;
    PointF fleePoint = new(0,0);
    bool flee = false;
    int fleeTime = 0;

    protected override void loop()
    {
        if(Life < 80)
            flee = true;

        if(LastDamage is not null && Life < 70)
        {
            flee = true;
            if (LastDamage.Value.X > Location.X)
                fleePoint.X = LastDamage.Value.X + 50;
            else 
                fleePoint.X = LastDamage.Value.X - 50;
            if (LastDamage.Value.Y > Location.Y)
                fleePoint.Y = LastDamage.Value.Y - 100;
            else 
                fleePoint.Y = LastDamage.Value.Y + 100;
            fleeTime = 0;
        }

        if(fleeTime > 20)
        {
            flee = false;
            fleeTime = 0;
        }

        if(flee)
        {
            if(Energy < 10)
            {
                StopMove();
                return;
            }
            if(Energy > 30)
                StartTurbo();
            else
                StopTurbo();
            fleeTime++;
            enemy = LastDamage;
            StartMove(fleePoint);
            return;
        }




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
            if (Energy > 60)
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

            if (dx*dx + dy*dy >= 300f*300f)
                StartMove(enemy.Value);

            else
            {
                StopMove();
                if (i++ % 5 == 0)
                    Shoot(enemy.Value);
            }
        }
    }
}
