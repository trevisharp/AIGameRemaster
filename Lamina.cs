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
    bool flee = false;
    long fleeTime = 0;
    PointF fleePoint = new PointF(0,0);

    protected override void loop()
    {
        frame++;
        StartTurbo();
        if (Life < 80)
            flee = true;

        if(LastDamage is not null && Life < 70)
        {
            flee = true;
            if (LastDamage.Value.X > Location.X)
                fleePoint.X = LastDamage.Value.X + 600;
            else 
                fleePoint.X = LastDamage.Value.X - 600;
            if (LastDamage.Value.Y > Location.Y)
                fleePoint.Y = LastDamage.Value.Y - 400;
            else 
                fleePoint.Y = LastDamage.Value.Y + 400;
            fleeTime = 0;
        }

        if(fleeTime >= 3)
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
            if(Energy > 20)
            {
                flee = false;
                StartTurbo();
            }
            else
                StopTurbo();
            fleeTime++;
            enemy = LastDamage;
            Shoot(enemy.Value);
            StartMove(fleePoint);
            return;
        }

        if (FoodsInInfraRed.Count > 0)
        {
            food = FoodsInInfraRed[0];
        }
        else
        {
            food = null;
        }
        if (Energy < 1)
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
        if (food == null && EnergyRegeneration < 5)
            InfraRedSensor(3.5f * i++);
        else if (food != null && EnergyRegeneration < 5)
        {
            float dx = food.Value.X - this.Location.X,
                  dy = food.Value.Y - this.Location.Y;
            if (dx * dx + dy * dy <= 5f * 5f)
                foodcount++;
            // MessageBox.Show(foodcount.ToString());
            if (frame % 10 == 0)
                InfraRedSensor(food.Value);
            StartMove(FoodsInInfraRed[0]);


        }

        if (EnemiesInInfraRed.Count > 0)
        {
            enemy = EnemiesInInfraRed[0];
        }
        else
        {
            enemy = null;
        }

        if (enemy == null && EnergyRegeneration >= 5)
            InfraRedSensor(5f * i++);
        else if (enemy != null && EnergyRegeneration >= 5)
        {
            InfraRedSensor(enemy.Value);
            float dx = enemy.Value.X - this.Location.X,
                dy = enemy.Value.Y - this.Location.Y;
            if (dx * dx + dy * dy >= 280f * 280f)
            {
                StopTurbo();
                StartMove(enemy.Value);
                Shoot(enemy.Value);   
            }
            else
            {
                StopMove();
                if (i++ % 5 == 0)
                    Shoot(enemy.Value);
            }
        }




    }
}
