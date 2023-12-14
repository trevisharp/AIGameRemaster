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
    PointF closest;
    bool isloading = false;
    PointF fleePoint = new(0500, 500);
    bool flee = false;
    int fleeTime = 0;
    int countSonar = 0;

    int countLaser = 0;
    int countShoot = 0;

    float angle = 100;
    float deltaAngle = 0.017f;
    int searchindex = 0;
    int frame = 0;
    int points = 0;
    protected override void loop()
    {

        frame++;
        if (Energy < 10 || frame % 10 == 0)
            return;
        if (EntitiesInStrongSonar == 0)
        {
            StrongSonar();
            points = Points;
        }
        else if (EntitiesInAccurateSonar.Count == 0)
        {
            AccurateSonar();
        }
        else if (FoodsInInfraRed.Count == 0)
        {
            InfraRedSensor(EntitiesInAccurateSonar[searchindex++ % EntitiesInAccurateSonar.Count]);
        }
        else
        {
            StartMove(FoodsInInfraRed[0]);
            if (Points < points)
            {
                StartTurbo();
                StrongSonar();
                StopMove();
                ResetInfraRed();
                ResetSonar();
            }
        }

        // if (EnergyRegeneration < 5)
        // {
            countSonar++;
            if (countSonar > 50 && Energy > 20)
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

                    float dxc = closest.X - this.Location.X,
                            dyc = closest.Y - this.Location.Y;
                    float dc = dxc * dxc + dyc * dyc;

                    if(di < dc)
                        closest = item;
                }
            }

        countShoot++;
        // PointF ps = new(12f * countShoot);
        if (Energy > 10)
            Shoot(new SizeF(MathF.Cos(angle), MathF.Sin(angle)));
        angle += deltaAngle;

            if(EntitiesInAccurateSonar.Count > 0)
            {
                closest = EntitiesInAccurateSonar[0];

                foreach (var item in EntitiesInAccurateSonar)
                {
                    float dxi = item.X - this.Location.X,
                            dyi = item.Y - this.Location.Y;
                    float di = dxi * dxi + dyi * dyi;

                    float dxc = closest.X - this.Location.X,
                            dyc = closest.Y - this.Location.Y;
                    float dc = dxc * dxc + dyc * dyc;

                    if(di < dc)
                        closest = item;
                }
            }

        //     food = closest;

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

        if (food == null && Energy > 5)
            InfraRedSensor(5f * i++);

        else if (food != null && Energy > 5)
        {
            InfraRedSensor(food.Value);

            float dx = food.Value.X - this.Location.X,
                  dy = food.Value.Y - this.Location.Y;
            if (dx * dx + dy * dy >= 10f * 10f)
                StartMove(closest);
        }
        // return;
        // }
    

        countLaser++;

        // if (LastDamage is not null && Life < 70)
        // {
        //     flee = true;
        //     // if (LastDamage.Value.X > Location.X)
        //     //     fleePoint.X = LastDamage.Value.X - 45;
        //     // else
        //     //     fleePoint.X = LastDamage.Value.X + 45;
        //     // if (LastDamage.Value.Y > Location.Y)
        //     //     fleePoint.Y = LastDamage.Value.Y - 20;
        //     // else
        //     //     fleePoint.Y = LastDamage.Value.Y + 20;

        //     fleeTime = 0;
        // }

        if (fleeTime > 5)
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

            fleePoint.X = (float) (LastDamage.Value.X + (dx*dx + dy*dy) * Math.Cos(50));
            fleePoint.Y = (float) (LastDamage.Value.Y + (dx*dx + dy*dy) * Math.Sin(50));

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
            if (Energy > 30)
                isloading = false;
            else return;
        }

        if (enemy == null && Energy > 10 && countLaser > 5)
        {
            countLaser = 0;
            InfraRedSensor(25f * i++);
        }

        // else if (enemy != null && Energy > 10)
        // {
        //     if (countLaser > 5)
        //         InfraRedSensor(enemy.Value);

        //     float dx = enemy.Value.X - this.Location.X,
        //           dy = enemy.Value.Y - this.Location.Y;

        //     if (dx * dx + dy * dy >= 300f * 300f)
        //     {
        //         // StartMove(enemy.Value);
        //         Shoot(enemy.Value);
        //     }
        //     else
        //     {
        //         StopMove();
        //         if (i++ % 5 == 0)
        //             Shoot(enemy.Value);
        //     }
        // }
    }
}
