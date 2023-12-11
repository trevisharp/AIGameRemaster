using System;
using System.Drawing;

public class BatataPlayer : Player
{
    public BatataPlayer(PointF location) : 
        base(location, Color.Red, Color.Gray, "Batata") { }

    int i = 0;
    PointF? enemyBatata = null;
    bool isloadingBatata = false;

    PointF ponto = new(25, 25);
    PointF ponto2 = new(250, 250);
    PointF ponto3 = new(600, 1100);

    DateTime tempoInfravermelho = DateTime.Now;

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

        if(Location == ponto2)
        {
            StopMove();
            ponto = ponto3;
        }

        if(Location == ponto3)
        {
            StopMove();
            ponto = ponto2;
        }

        if(Life < 90)
        {
            StartTurbo();
            StartMove(ponto);   
        }

        if(Life > 90)
        {
            StopTurbo();   
            StopMove();   
        }

        if (isloadingBatata)
        {
            if (Energy > 50)
                isloadingBatata = false;
            else return;
        }
        if (enemyBatata == null && Energy > 20)
        {
            InfraRedSensor(6f * i++);
        }
        else if (enemyBatata != null && Energy > 10)
        {
            InfraRedSensor(enemyBatata.Value);
            float dx = enemyBatata.Value.X - this.Location.X,
                  dy = enemyBatata.Value.Y - this.Location.Y;
            if (dx*dx + dy*dy >= 800f*800f)
            {
                ResetInfraRed();
            }
            else
            {
                if (i++ % 5 == 0)
                {
                    Shoot(enemyBatata.Value);
                }
            }
        }
    }
}