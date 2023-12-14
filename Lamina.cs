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
    int lifeTh = 80;
    PointF fleePoint = new PointF(0, 0);

    protected override void loop()
    {
        frame++;
        StartTurbo();

        if (this.Location.X < 0)
        {
            StartMove(new PointF(300, 600));
        }
        else if (this.Location.X > 1250)
        {
            StartMove(new PointF(300, 600));
        }
        else if (this.Location.Y < 0)
        {
            StartMove(new PointF(300, 600));
        }
        else if (this.Location.Y > 770)
        {
            StartMove(new PointF(300, 600));
        }

        if (Life < 20 && fleeTime < 3)
        {
            flee = true;
            if (LastDamage.Value.X > Location.X)
                fleePoint.X = LastDamage.Value.X - 600;
            else
                fleePoint.X = LastDamage.Value.X + 600;
            if (LastDamage.Value.Y > Location.Y)
                fleePoint.Y = LastDamage.Value.Y - 400;
            else
                fleePoint.Y = LastDamage.Value.Y + 400;
            fleeTime = 0;
        }

        if (fleeTime >= 3)
        {
            flee = false;
            fleeTime = 0;
        }

        if (flee)
        {
            if (Energy < 10)
            {
                StopMove();
                return;
            }
            if (Energy > 20)
            {
                StartTurbo();
            }
            else
                StopTurbo();
            fleeTime++;
            enemy = LastDamage;
            if (frame % 7 == 0)
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

        if (Energy < 10)
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

        if (food == null && EnergyRegeneration < 7)
            InfraRedSensor(3.5f * i++);
        else if (food != null && EnergyRegeneration < 7)
        {
            float dx = food.Value.X - this.Location.X,
                  dy = food.Value.Y - this.Location.Y;
            if (dx * dx + dy * dy <= 5f * 5f)
                foodcount++;
            // MessageBox.Show(foodcount.ToString());
            if (frame % 20 == 0)
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

        if (enemy == null && EnergyRegeneration >= 7)
            InfraRedSensor(5f * i++);
        else if (enemy != null && EnergyRegeneration >= 7)
        {
            if (frame % 12 == 0)
                InfraRedSensor(enemy.Value);
            float dx = enemy.Value.X - this.Location.X,
                dy = enemy.Value.Y - this.Location.Y;
            if (dx * dx + dy * dy >= 280f * 280f)
            {
                StopTurbo();
                StartMove(enemy.Value);
                Shoot(pointPrevision(enemy.Value));
            }
            else
            {
                StopMove();
                if (i++ % 5 == 0)
                    Shoot(pointPrevision(enemy.Value));
            }
        }
    }

    private PointF pointPrevision(PointF enemyPosition)
    {

        float distanceX = this.Location.X - enemyPosition.X;
        float distanceY = this.Location.Y - enemyPosition.Y;

        float estimatedDistanceX = distanceX + (float)(this.Velocity.Width * Math.Cos(Math.Atan2(distanceY, distanceX)));
        float estimatedDistanceY = distanceY + (float)(this.Velocity.Width * Math.Sin(Math.Atan2(distanceY, distanceX)));

        return new Point((int)(enemyPosition.X + estimatedDistanceX), (int)(enemyPosition.Y + estimatedDistanceY));
    }

}
