using System;
using System.Drawing;

public class BatataPlayer : Player
{
    public BatataPlayer(PointF location) : 
        base(location, Color.Red, Color.Gray, "Batata") { }

    int i = 0;
    PointF? enemyBatata = null;
    bool isloadingBatata = false;

    PointF ponto = new(0, 0);

    int searchindex = 0;
    int frame = 0;
    int points = 0;

    protected override void loop()
    {      
        if (EnemiesInInfraRed.Count > 0)
        {
            enemyBatata = EnemiesInInfraRed[0];
        }
        else
        {
            enemyBatata = null;
        }

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
            if (Points != points)
            {
                StartTurbo();
                StrongSonar();
                StopMove();
                ResetInfraRed();
                ResetSonar();
            }
        }
        
        if (Energy < 10)
        {
            StopMove();
            isloadingBatata = true;
            enemyBatata = null;
        }
        if (isloadingBatata)
        {
            if (Energy > 60)
                isloadingBatata = false;
            else return;
        }
        if (enemyBatata == null && Energy > 10)
            InfraRedSensor(11f * i++);
        else if (enemyBatata != null && Energy > 10)
        {
            InfraRedSensor(enemyBatata.Value);
            float dx = enemyBatata.Value.X - this.Location.X,
                  dy = enemyBatata.Value.Y - this.Location.Y;
            if (dx*dx + dy*dy <= 600f*600f)
            {
                // StartMove(enemyBatata.Value);
            
                StartTurbo();
                if (i++ % 4 == 0)
                {
                    Shoot(enemyBatata.Value);
                }
            }
        }
    }
}