using System;
using System.Drawing;

public class FalusBot : Player
{
    public FalusBot(PointF location) :
        base(location, Color.Black, Color.Red, "Falus")
    { random = new Random(); }

    Random random;
    bool accurateSearch = true;
    PointF? nextPoint = null;

    bool infraRedSearch = true;

    PointF? lastBomb = null;

    bool isRunning = false;

    protected override void loop()
    {
        getPoint();
        if (LastDamage != lastBomb)
        {
            lastBomb = LastDamage;

            float runX = lastBomb.Value.X - this.Location.X;
            float runY = lastBomb.Value.Y - this.Location.Y;

            StartTurbo();
            StartMove(new SizeF(-runX * 5, -runY * 5));
            isRunning = true;
        }

        if (lastBomb.HasValue && getDistance(lastBomb.Value) > 20)
        {
            StopTurbo();
            StopMove();
            isRunning = false;
        }

        if (isRunning)
            return;

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

                if (infraRedSearch)
                {
                    PointF objetivo = PointF.Empty;
                    float shortestDistance = float.MaxValue;
                    foreach (PointF ponto in EntitiesInAccurateSonar)
                    {
                        float distance = getDistance(ponto);
                        InfraRedSensor(ponto);
                        PointF? enemy = EnemiesInInfraRed.Count > 0 ? EnemiesInInfraRed[0] : null;
                        if (enemy.HasValue)
                        {
                            Shoot(enemy.Value);
                            accurateSearch = true;
                            infraRedSearch = true;
                        }

                        if (distance < shortestDistance)
                        {
                            shortestDistance = distance;
                            objetivo = ponto;
                        }
                    }

                    InfraRedSensor(objetivo);
                    infraRedSearch = false;
                }

                PointF? InfraRedEnemy = EnemiesInInfraRed.Count > 0 ? EnemiesInInfraRed[0] : null;
                PointF? InfraRedFood = FoodsInInfraRed.Count > 0 ? FoodsInInfraRed[0] : null;

                if (!InfraRedEnemy.HasValue && !InfraRedFood.HasValue)
                {
                    accurateSearch = true;
                    infraRedSearch = true;
                    return;
                }

                if (isEnemy(InfraRedEnemy, InfraRedFood))
                {
                    Shoot(InfraRedEnemy.Value);
                    accurateSearch = true;
                    infraRedSearch = true;
                }
                else
                {
                    StartTurbo();
                    StartMove(InfraRedFood.Value);

                    if (getDistance(InfraRedFood.Value) <= 10)
                    {
                        StopTurbo();
                        StopMove();
                        accurateSearch = true;
                        infraRedSearch = true;
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