using System;
using System.Drawing;

public class BatataPlayer : Player
{
    public BatataPlayer(PointF location) : 
        base(location, Color.Black, Color.Red, "Batata") { }

    int i = 0;
    PointF? enemyBatata = null;
    bool isloadingBatata = false;

    PointF ponto = new(0, 0);

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
        
        if (Energy < 10)
        {
            StopMove();
            isloadingBatata = true;
            enemyBatata = null;
        }
        if (isloadingBatata)
        {
            if (Energy > 50)
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
            if (dx*dx + dy*dy >= 600f*600f)
                StartMove(enemyBatata.Value);
            else
            {
                StartMove(ponto);
                StartTurbo();
                if (i++ % 4 == 0)
                {
                    Shoot(enemyBatata.Value);
                }
            }
        }
    }
}