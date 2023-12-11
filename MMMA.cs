using System;
using System.Drawing;
using System.Threading;

public class MMMA : Player
{
    public MMMA(PointF location) :
        base(location, Color.Lime, Color.Gray, "MMMA")
    { }

    int i = 0;
    PointF? enemy = null;
    PointF? food = null;
    PointF? closest = null;
    bool isloading = false;
    PointF fleePoint = new(0, 0);
    bool flee = false;
    int fleeTime = 0;
    int countSonar = 0;

    protected override void loop()
    {
        if (Life < 80)
            flee = true;

        if (EnergyRegeneration < 5)
        {
            countSonar++;
            if (countSonar > 50)
            {
                countSonar = 0;
                AccurateSonar();
            }

            if(EntitiesInAccurateSonar.Count > 0)
            {
                closest = EntitiesInAccurateSonar[0];

                foreach (var item in EntitiesInAccurateSonar)
                {
                    float dxi = item.X - this.Location.X,
                            dyi = item.Y - this.Location.Y;
                    float di = dxi * dxi + dyi * dyi;

                    float dxc = closest.Value.X - this.Location.X,
                            dyc = closest.Value.Y - this.Location.Y;
                    float dc = dxc * dxc + dyc * dyc;

                    if(di < dc)
                        closest = item;
                }
            }
            // if(closest is not null)
            //     InfraRedSensor(closest);
            // food = closest;

            // if (FoodsInInfraRed.Count > 0)
            // {
            //     food = FoodsInInfraRed[0];
            // }

            // else
            // {
            //     food = null;
            // }

            // if (Energy < 5)
            // {
            //     StopMove();
            //     isloading = true;
            //     food = null;
            // }

            // if (isloading)
            // {
            //     if (Energy > 10)
            //         isloading = false;
            //     else return;
            // }

            if (food == null && Energy > 5)
                InfraRedSensor(5f * i++);

            else if (food != null && Energy > 5)
            {
                InfraRedSensor(food.Value);

                float dx = food.Value.X - this.Location.X,
                      dy = food.Value.Y - this.Location.Y;
                if (dx * dx + dy * dy >= 10f * 10f)
                    StartMove(FoodsInInfraRed[0]);
            }
            return;
        }



        if (LastDamage is not null && Life < 70)
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

        if (fleeTime > 10)
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
            if (Energy > 30)
                StartTurbo();
            else
                StopTurbo();
            fleeTime++;
            enemy = LastDamage;

            float dx = enemy.Value.X - this.Location.X,
                  dy = enemy.Value.Y - this.Location.Y;

            // fleePoint.X = (float) (LastDamage.Value.X + (dx*dx + dy*dy) * Math.Cos(50));
            // fleePoint.Y = (float) (LastDamage.Value.Y + (dx*dx + dy*dy) * Math.Sin(50));

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

            if (dx * dx + dy * dy >= 300f * 300f)
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
