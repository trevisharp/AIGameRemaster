using System;
using System.Drawing;

public class FalusBot : Player
{
    public FalusBot(PointF location) :
        base(location, Color.Pink, Color.Red, "Falus")
    { random = new Random(); }

    Random random;
    bool accurateSearch = true;
    PointF? nextPoint = null;

    PointF? lastBomb = null;

    bool isRunning = false;

    int points = 0;

    float angle = 0;
    float deltaAngle = 0.057f;

    protected override void loop()
    {
        if (Energy <= 20)
        {
            StopTurbo();
            return;
        }


        getPoint();
        if (LastDamage != lastBomb)
        {
            lastBomb = LastDamage;

            isRunning = true;
            runningForLife();
        }

        if (lastBomb.HasValue && getDistance(lastBomb.Value) > 10)
        {
            StopMove();
            isRunning = false;
        }

        if (isRunning)
        {
            float runX = lastBomb.Value.X - this.Location.X;
            float runY = lastBomb.Value.Y - this.Location.Y;

            StartTurbo();
            StartMove(new SizeF(runX, -runY));
            return;
        }

        if (points >= 20)
        {
            if (LastDamage != lastBomb)
            {
                isRunning = true;
                runningForLife();
                return;
            }

            Shoot(new SizeF(MathF.Cos(angle), MathF.Sin(angle)));
            angle += deltaAngle;
            return;
        }

        if (accurateSearch)
        {
            AccurateSonar();
            accurateSearch = false;
        }
        else
        {
            if (EntitiesInAccurateSonar.Count > 0)
            {
                StopMove();

                PointF objetivo = PointF.Empty;
                float shortestDistance = float.MaxValue;
                foreach (PointF ponto in EntitiesInAccurateSonar)
                {
                    if (LastDamage != lastBomb)
                        runningForLife();
                    float distance = getDistance(ponto);
                    InfraRedSensor(ponto);
                    PointF? enemy = EnemiesInInfraRed.Count > 0 ? EnemiesInInfraRed[0] : null;
                    if (enemy.HasValue)
                    {
                        Shoot(enemy.Value);
                        accurateSearch = true;
                    }

                    if (distance < shortestDistance)
                    {
                        shortestDistance = distance;
                        objetivo = ponto;
                    }
                }

                InfraRedSensor(objetivo);

                PointF? InfraRedEnemy = EnemiesInInfraRed.Count > 0 ? EnemiesInInfraRed[0] : null;
                PointF? InfraRedFood = FoodsInInfraRed.Count > 0 ? FoodsInInfraRed[0] : null;

                if (!InfraRedEnemy.HasValue && !InfraRedFood.HasValue)
                {
                    accurateSearch = true;
                    return;
                }

                if (isEnemy(InfraRedEnemy, InfraRedFood))
                {
                    if (LastDamage != lastBomb)
                    {
                        isRunning = true;
                        runningForLife();
                        return;
                    }

                    Shoot(InfraRedEnemy.Value);
                    accurateSearch = true;

                    OppositePoint(InfraRedEnemy.Value.X * 5, InfraRedEnemy.Value.Y * 5);
                }
                else
                {
                    StartTurbo();
                    StartMove(InfraRedFood.Value);

                    if (getDistance(InfraRedFood.Value) <= 10)
                    {
                        points++;
                        if (LastDamage != lastBomb)
                        {
                            isRunning = true;
                            runningForLife();
                            return;
                        }

                        StopMove();
                        accurateSearch = true;
                    }
                }
            }
            else
            {
                StartMove(nextPoint.Value);
                accurateSearch = true;
            }
        }
    }


    private void runningForLife()
    {
        float runX = lastBomb.Value.X - this.Location.X;
        float runY = lastBomb.Value.Y - this.Location.Y;

        OppositePoint(runX * 10, runY * 10);
    }

    private void OppositePoint(float x, float y)
    {
        SizeF direction = new SizeF(
            (float)Math.Cos(x * (2 * Math.PI) / 360f),
            (float)Math.Sin(-y * (2 * Math.PI) / 360f)
        );

        nextPoint = this.Location + direction;
    }

    private void getPoint()
    {
        double x = random.NextDouble();
        double y = random.NextDouble();

        float distance = getDistance(new PointF(nextPoint.HasValue ? nextPoint.Value.X : this.Location.X,
                                        nextPoint.HasValue ? nextPoint.Value.Y : this.Location.Y));

        if (distance <= 10 || nextPoint == null || this.Location.X <= 0 || this.Location.Y <= 0)
            nextPoint = new PointF((float)(x * 720), (float)(y * 1200));
    }

    private float getDistance(PointF point)
    {
        return (float)Math.Sqrt(Math.Pow(point.X - this.Location.X, 2) +
                Math.Pow(point.Y - this.Location.Y, 2));
    }

    private bool isEnemy(PointF? enemy, PointF? food)
    {
        if (!enemy.HasValue)
            return false;
        if (!food.HasValue)
            return true;

        if (getDistance(enemy.Value) < getDistance(food.Value))
            return true;
        else
            return false;
    }
}