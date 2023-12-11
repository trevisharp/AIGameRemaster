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

    protected override void loop()
    {
        if (Energy < 20)
        {
            return;
        }

        getPoint();
        if (LastDamage != lastBomb)
        {
            lastBomb = LastDamage;

            isRunning = true;
        }

        if (lastBomb.HasValue && getDistance(lastBomb.Value) > 10)
        {
            StopTurbo();
            StopMove();
            isRunning = false;
        }

        if (isRunning){
            float runX = lastBomb.Value.X - this.Location.X;
            float runY = lastBomb.Value.Y - this.Location.Y;

            StartTurbo();
            StartMove(new SizeF(runX, -runY));
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
                        return;
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
                    Shoot(InfraRedEnemy.Value);
                    accurateSearch = true;
                }
                else
                {
                    if (LastDamage != lastBomb)
                    {
                        isRunning = true;
                        return;
                    }
                    StartTurbo();
                    StartMove(InfraRedFood.Value);

                    if (getDistance(InfraRedFood.Value) <= 10)
                    {
                        StopTurbo();
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

    private void getPoint()
    {
        double x = random.NextDouble();
        double y = random.NextDouble();

        float dx = (float)((nextPoint.HasValue ? nextPoint.Value.X : this.Location.X) - this.Location.X),
              dy = (float)((nextPoint.HasValue ? nextPoint.Value.Y : this.Location.Y) - this.Location.Y);

        if (dx * dx + dy * dy <= 300f * 300f || nextPoint == null)
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