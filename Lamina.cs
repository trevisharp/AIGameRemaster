using System;
using System.Drawing;

public class Lamina : Player
{
    public Lamina(PointF location)
        : base(location, Color.DeepPink, Color.Pink, "Lamina") { }


    int i = 0;
    PointF? enemy = null;
    PointF? food = null;
    bool isloading = false;
    int searchindex = 0;
    int frame = 0;
    int points = 0;

    protected override void loop()
    {
        StartTurbo();
        if (FoodsInInfraRed.Count > 0)
        {
            food = FoodsInInfraRed[0];
        }
        else
        {
            food = null;
        }
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
            if (dx*dx + dy*dy >= 10f*10f)
                StartMove(FoodsInInfraRed[0]);



        }
    }
}
