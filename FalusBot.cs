using System.Drawing;

public class FalusBot : Player
{
    public FalusBot(PointF location) :
        base(location, Color.Black, Color.Red, "Falus")
    { }

    int i = 0;
    PointF? food = null;
    PointF? enemy = null;
    int frame = 0;

    protected override void loop()
    {
        frame++;
        // if (Energy < 10 || frame % 10 == 0)
        //     return;
        
        // if (Energy <= 25 || Life <= 25 || EntitiesInAccurateSonar.Count == 0)
        // {
        //     AccurateSonar();
        // }

        // if (EntitiesInAccurateSonar.Count > 0)
        // {
        //     if (food == null)
        //         InfraRedSensor(5f * i++);
        //     else if (food != null || FoodsInInfraRed.Count > 0)
        //     {
        //         food = FoodsInInfraRed[0];
        //         float dx = food.Value.X - this.Location.X,
        //           dy = food.Value.Y - this.Location.Y;
        //         if (dx*dx + dy*dy >= 300f*300f)
        //             StartMove(food.Value);
        //         else
        //             StopMove();
        //     }
        // }


        if (EnemiesInInfraRed.Count > 0)
        {
            enemy = EnemiesInInfraRed[0];
        }
        else
        {
           enemy = null;
        }

        if (enemy == null && Energy > 30)
            InfraRedSensor(5f * i++);
        else if (enemy != null && Energy > 30)
        {
            InfraRedSensor(enemy.Value);
            if (i++ % 5 == 0)
                Shoot(enemy.Value);
        }

        // else if (EntitiesInAccurateSonar.Count == 0)
        // {
        //     AccurateSonar();
        //     if (EntitiesInAccurateSonar.Count == 0)
        //     {
        //         InfraRedSensor(5f * i++);
        //         if (FoodsInInfraRed.Count > 0)
        //         {
        //             food = FoodsInInfraRed[0];
        //         }
        //         if (food != null)
        //         {
        //             float dx = food.Value.X - this.Location.X,
        //                 dy = food.Value.Y - this.Location.Y;
        //             if (dx * dx + dy * dy >= 300f * 300f)
        //                 StartMove(food.Value);
        //         }
        //     }
        //     else
        //     {
        //         enemy = EntitiesInAccurateSonar[0];
        //         InfraRedSensor(enemy.Value);
        //         float dx = enemy.Value.X - this.Location.X,
        //             dy = enemy.Value.Y - this.Location.Y;
        //         if (dx * dx + dy * dy >= 300f * 300f)
        //             StartMove(enemy.Value);
        //         else
        //         {
        //             StopMove();
        //             if (i++ % 5 == 0)
        //                 Shoot(enemy.Value);
        //         }
        //     }
        // }
    }
}