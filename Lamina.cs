using System;
using System.Drawing;
using System.Windows.Forms;

public class Lamina : Player
{
    public Lamina(PointF location)
        : base(location, Color.DeepPink, Color.Pink, "Lamina") { }


    int i = 0;
    PointF? enemy = null;
    PointF? food = null;

    int foodcount = 0;
    bool isloading = false;
    int searchindex = 0;
    int frame = 0;
    int points = 0;

    protected override void loop()
    {
        StartTurbo();
        if (EnemiesInInfraRed.Count > 0)
        {
            enemy = EnemiesInInfraRed[0];
        }
        else
        {
            enemy = null;
        }

        if (enemy == null && Energy > 20)
            InfraRedSensor(5f * i++);
        else if (enemy != null && Energy > 20)
        {
            InfraRedSensor(enemy.Value);
            float dx = enemy.Value.X - this.Location.X,
                  dy = enemy.Value.Y - this.Location.Y;
            if (dx*dx + dy*dy >= 280f*280f) {
                StopTurbo();
                StartMove(enemy.Value);
            }
            else
            {
                StopMove();
                if (i++ % 5 == 0)
                    Shoot(enemy.Value);
            }
        }

        if (FoodsInInfraRed.Count > 0)
        {
            food = FoodsInInfraRed[0];
        }
        else
        {
            food = null;
        }
        if (Energy < 5)
        {
            StopMove();
            isloading = true;
            food = null;
        }
        if (isloading)
        {
            if (Energy > 10)
                isloading = false;
            else return;
        }
        if (food == null && Energy > 5 )
            InfraRedSensor(5f * i++);
        else if (EnemiesInInfraRed.Count > 0 && Energy > 20)
        {
            InfraRedSensor(enemy.Value);
            float dx = enemy.Value.X - this.Location.X,
                dy = enemy.Value.Y - this.Location.Y;
            if (dx * dx + dy * dy >= 10f * 10f)
                StartMove(enemy.Value);
            else
            {
                StopMove();
                if (i++ % 5 == 0)
                    Shoot(enemy.Value);
            }

        }
        else if (food != null && Energy > 5)
        {

            InfraRedSensor(food.Value);
            StartMove(FoodsInInfraRed[0]);
            

        }
    }
}
