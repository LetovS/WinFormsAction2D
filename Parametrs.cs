using System;
using System.Drawing;
using System.Windows.Forms;
using static Actions.Task4;
//using System.Windows.Controls;

namespace Actions
{
    public class Parametrs : Form
    {
        //TODO как динамически формировать панели со здвигом вниз с учетом обработки.
        private Label examplePoint;
        private Label exampleCurve;
        private Label exampleBezier;
        private Label examplePolygon;
        private Label exampleFilledCurve;

        string[] colorsPoint;
        string[] colorsCurve;
        string[] colorsPolygon;
        string[] colorsBezie;
        string[] colorsFillCurve;



        public (Color color, int size) setPoint = (Color.Black, 3);
        (Color color, int size) setPolygon = (Color.Black, 3);
        (Color color, int size) setCurve = (Color.Black, 3);
        (Color color, int size) setBezier = (Color.Black, 3);
        (Color color, int size) setFillCurve = (Color.Black, 3);

        enum TypeData { Point, Other}

        int index = 0;

        public Parametrs()
        {
            
            int delta = 5;
            int heightPanel = 90;
            colorsPoint = GetPenColors();
            colorsCurve = GetPenColors();
            colorsPolygon = GetPenColors();
            colorsBezie = GetPenColors();
            colorsFillCurve = GetPenColors();

            this.Size = new Size(350, 600);
            this.Text = "Settings";
            this.MaximizeBox = false;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.StartPosition = FormStartPosition.CenterParent;
            Label lbl = new Label() { Text = "Настройки отображения."};
            lbl.TextAlign = ContentAlignment.MiddleLeft;
            lbl.Font = new Font("Arial", 12, FontStyle.Bold);
            lbl.SetBounds(delta, delta, ClientSize.Width,20);

            #region Point

            Panel pPoint = new Panel();
            pPoint.SetBounds(delta, lbl.Bottom + delta+5, ClientSize.Width - 3 * delta, heightPanel);
            pPoint.BorderStyle = BorderStyle.FixedSingle;

            Label lblPoint = new Label() { Text = "Для точки:" };
            lblPoint.Font = new Font("Arial", 8, FontStyle.Bold);
            lblPoint.SetBounds(delta, 0, ClientSize.Width, 20);

            ComboBox colorPoint = new ComboBox();
            colorPoint.SetBounds(ClientSize.Width / 3, lblPoint.Bottom + delta, 110, 40);
            colorPoint.DataSource = colorsPoint;
            colorPoint.MaxDropDownItems = 15;
            colorPoint.DropDownStyle = ComboBoxStyle.DropDownList;

            

            TextBox sizePoint = new TextBox() { Text = setPoint.Item2.ToString()};
            sizePoint.SetBounds(colorPoint.Left, colorPoint.Bottom + delta, colorPoint.Width, colorPoint.Height);
            
                
            
            Label Point = new Label() { Text = "Тип пера"};
            Point.SetBounds(delta, lblPoint.Bottom + delta, ClientSize.Width /4, colorPoint.Height);
            Point.TextAlign = ContentAlignment.MiddleLeft;

            Label lbl2Point = new Label() { Text = "Разер точки" };
            lbl2Point.SetBounds(delta, Point.Height *2 + delta , Point.Width, colorPoint.Height);
            lbl2Point.TextAlign = ContentAlignment.MiddleLeft;


            examplePoint = new Label();
            examplePoint.BorderStyle = BorderStyle.FixedSingle;
            examplePoint.SetBounds(colorPoint.Right + delta, colorPoint.Top, pPoint.ClientSize.Width - Point.Width - colorPoint.Width - 8* delta, lbl2Point.Bottom - Point.Top );
            

            pPoint.Controls.Add(examplePoint);
            pPoint.Controls.Add(Point);
            pPoint.Controls.Add(lbl2Point);
            pPoint.Controls.Add(sizePoint);
            pPoint.Controls.Add(lblPoint);
            pPoint.Controls.Add(colorPoint);

            #endregion

            #region Curve

            Panel pCurve = new Panel();
            pCurve.SetBounds(delta, pPoint.Bottom + delta, pPoint.Width, pPoint.Height);
            pCurve.BorderStyle = BorderStyle.FixedSingle;

            Label lblCurve = new Label() { Text = "Для кривой:" };
            lblCurve.Font = new Font("Arial", 8, FontStyle.Bold);
            lblCurve.SetBounds(delta, 0, ClientSize.Width, 20);

            ComboBox colorCurve = new ComboBox();
            colorCurve.SetBounds(ClientSize.Width / 3, lblPoint.Bottom + delta, 110, 40);
            colorCurve.DataSource = colorsCurve;
            colorCurve.MaxDropDownItems = 15;
            colorCurve.DropDownStyle = ComboBoxStyle.DropDownList;
            


            TextBox sizeCurve = new TextBox() { Text = setPoint.Item2.ToString() };
            sizeCurve.SetBounds(colorPoint.Left, colorPoint.Bottom + delta, colorPoint.Width, colorPoint.Height);
            


            Label lbl1Curve = new Label() { Text = "Тип пера" };
            lbl1Curve.SetBounds(delta, lblPoint.Bottom + delta, ClientSize.Width / 4, colorPoint.Height);
            lbl1Curve.TextAlign = ContentAlignment.MiddleLeft;

            Label lbl2Curve = new Label() { Text = "Разер точки" };
            lbl2Curve.SetBounds(delta, Point.Height * 2 + delta, Point.Width, colorPoint.Height);
            lbl2Curve.TextAlign = ContentAlignment.MiddleLeft;


            exampleCurve = new Label();
            exampleCurve.BorderStyle = BorderStyle.FixedSingle;
            exampleCurve.SetBounds(colorPoint.Right + delta, colorPoint.Top, pPoint.ClientSize.Width - Point.Width - colorPoint.Width - 8 * delta, lbl2Point.Bottom - Point.Top);

            

            pCurve.Controls.Add(exampleCurve);
            pCurve.Controls.Add(lbl1Curve);
            pCurve.Controls.Add(lbl2Curve);
            pCurve.Controls.Add(sizeCurve);
            pCurve.Controls.Add(lblCurve);
            pCurve.Controls.Add(colorCurve);
            #endregion

            #region Bezier
            Panel pBezier = new Panel();
            pBezier.SetBounds(delta, pCurve.Bottom + delta, pPoint.Width, pPoint.Height);
            pBezier.BorderStyle = BorderStyle.FixedSingle;

            Label lblBezier = new Label() { Text = "Для безье:" };
            lblBezier.Font = new Font("Arial", 8, FontStyle.Bold);
            lblBezier.SetBounds(delta, 0, ClientSize.Width, 20);
            ComboBox colorBezier = new ComboBox();
            colorBezier.SetBounds(ClientSize.Width / 3, lblPoint.Bottom + delta, 110, 40);
            colorBezier.DataSource = colorsBezie;
            colorBezier.MaxDropDownItems = 15;
            colorBezier.DropDownStyle = ComboBoxStyle.DropDownList;
            


            TextBox sizeBezier = new TextBox() { Text = setPoint.Item2.ToString() };
            sizeBezier.SetBounds(colorPoint.Left, colorPoint.Bottom + delta, colorPoint.Width, colorPoint.Height);
            


            Label lbl1Bezier = new Label() { Text = "Тип пера" };
            lbl1Bezier.SetBounds(delta, lblPoint.Bottom + delta, ClientSize.Width / 4, colorPoint.Height);
            lbl1Bezier.TextAlign = ContentAlignment.MiddleLeft;

            Label lbl2Bezier = new Label() { Text = "Разер точки" };
            lbl2Bezier.SetBounds(delta, Point.Height * 2 + delta, Point.Width, colorPoint.Height);
            lbl2Bezier.TextAlign = ContentAlignment.MiddleLeft;


            exampleBezier = new Label();
            exampleBezier.BorderStyle = BorderStyle.FixedSingle;
            exampleBezier.SetBounds(colorPoint.Right + delta, colorPoint.Top, pPoint.ClientSize.Width - Point.Width - colorPoint.Width - 8 * delta, lbl2Point.Bottom - Point.Top);
            

            pBezier.Controls.Add(exampleBezier);
            pBezier.Controls.Add(lbl1Bezier);
            pBezier.Controls.Add(lbl2Bezier);
            pBezier.Controls.Add(sizeBezier);
            pBezier.Controls.Add(lblBezier);
            pBezier.Controls.Add(colorBezier);
            #endregion

            #region Polygon
            Panel pPolygon = new Panel();
            pPolygon.SetBounds(delta, pBezier.Bottom + delta, pPoint.Width, pPoint.Height);
            pPolygon.BorderStyle = BorderStyle.FixedSingle;

            Label lblPolygon = new Label() { Text = "Для ломанной:" };
            lblPolygon.Font = new Font("Arial", 8, FontStyle.Bold);
            lblPolygon.SetBounds(delta, 0, ClientSize.Width, 20);

            ComboBox colorPolygon = new ComboBox();
            colorPolygon.SetBounds(ClientSize.Width / 3, lblPoint.Bottom + delta, 110, 40);
            colorPolygon.DataSource = colorsPolygon;
            colorPolygon.MaxDropDownItems = 15;
            colorPolygon.DropDownStyle = ComboBoxStyle.DropDownList;
           


            TextBox sizePolygon = new TextBox() { Text = setPoint.Item2.ToString() };
            sizePolygon.SetBounds(colorPoint.Left, colorPoint.Bottom + delta, colorPoint.Width, colorPoint.Height);
            


            Label lbl1Polygon = new Label() { Text = "Тип пера" };
            lbl1Polygon.SetBounds(delta, lblPoint.Bottom + delta, ClientSize.Width / 4, colorPoint.Height);
            lbl1Polygon.TextAlign = ContentAlignment.MiddleLeft;

            Label lbl2Polygon = new Label() { Text = "Разер точки" };
            lbl2Polygon.SetBounds(delta, Point.Height * 2 + delta, Point.Width, colorPoint.Height);
            lbl2Polygon.TextAlign = ContentAlignment.MiddleLeft;


            examplePolygon = new Label();
            examplePolygon.BorderStyle = BorderStyle.FixedSingle;
            examplePolygon.SetBounds(colorPoint.Right + delta, colorPoint.Top, pPoint.ClientSize.Width - Point.Width - colorPoint.Width - 8 * delta, lbl2Point.Bottom - Point.Top);



            




            pPolygon.Controls.Add(examplePolygon);
            pPolygon.Controls.Add(lbl1Polygon);
            pPolygon.Controls.Add(lbl2Polygon);
            pPolygon.Controls.Add(sizePolygon);
            pPolygon.Controls.Add(lblPolygon);
            pPolygon.Controls.Add(colorPolygon);
            #endregion

            #region FilledCurve
            Panel pFilledCurve = new Panel();
            pFilledCurve.SetBounds(delta, pPolygon.Bottom + delta, pPoint.Width, pPoint.Height);
            pFilledCurve.BorderStyle = BorderStyle.FixedSingle;

            Label lblFilledCurve = new Label() { Text = "Для закрашенной:" };
            lblFilledCurve.Font = new Font("Arial", 8, FontStyle.Bold);
            lblFilledCurve.SetBounds(delta, 0, ClientSize.Width, 20);

            ComboBox colorFilledCurve = new ComboBox();
            colorFilledCurve.SetBounds(ClientSize.Width / 3, lblPoint.Bottom + delta, 110, 40);
            colorFilledCurve.DataSource = colorsFillCurve;
            colorFilledCurve.MaxDropDownItems = 15;
            colorFilledCurve.DropDownStyle = ComboBoxStyle.DropDownList;
            


            TextBox sizeFilledCurve = new TextBox() { Text = setPoint.Item2.ToString() };
            sizeFilledCurve.SetBounds(colorPoint.Left, colorPoint.Bottom + delta, colorPoint.Width, colorPoint.Height);
            


            Label lbl1FilledCurve = new Label() { Text = "Тип пера" };
            lbl1FilledCurve.SetBounds(delta, lblPoint.Bottom + delta, ClientSize.Width / 4, colorPoint.Height);
            lbl1FilledCurve.TextAlign = ContentAlignment.MiddleLeft;

            Label lbl2FilledCurve = new Label() { Text = "Разер точки" };
            lbl2FilledCurve.SetBounds(delta, Point.Height * 2 + delta, Point.Width, colorPoint.Height);
            lbl2FilledCurve.TextAlign = ContentAlignment.MiddleLeft;


            exampleFilledCurve = new Label();
            exampleFilledCurve.BorderStyle = BorderStyle.FixedSingle;
            exampleFilledCurve.SetBounds(colorPoint.Right + delta, colorPoint.Top, pPoint.ClientSize.Width - Point.Width - colorPoint.Width - 8 * delta, lbl2Point.Bottom - Point.Top);

            

            pFilledCurve.Controls.Add(exampleFilledCurve);
            pFilledCurve.Controls.Add(lbl1FilledCurve);
            pFilledCurve.Controls.Add(lbl2FilledCurve);
            pFilledCurve.Controls.Add(sizeFilledCurve);
            pFilledCurve.Controls.Add(lblFilledCurve);
            pFilledCurve.Controls.Add(colorFilledCurve);
            #endregion


            #region Actions about change color and/or size

            sizePoint.Tag = (LineType.Point, examplePoint);
            colorPoint.Tag = (LineType.Point, examplePoint);
            colorPoint.Name = "colorPoint";
            colorPoint.SelectedIndexChanged += ChangeToolColor;
            sizePoint.TextChanged += ChangeToolSize;
            
            examplePoint.Paint += ExampleDraw;
            colorCurve.SelectedIndexChanged += ChangeToolColor;
            sizeCurve.TextChanged += ChangeToolSize;
            exampleCurve.Paint += ExampleDraw;
            sizeCurve.Tag = (LineType.Curve, exampleCurve);
            colorCurve.Tag = (LineType.Curve, exampleCurve);


            colorBezier.SelectedIndexChanged += ChangeToolColor;
            sizeBezier.TextChanged += ChangeToolSize;
            exampleBezier.Paint += ExampleDraw;
            sizeBezier.Tag = (LineType.Bezier, exampleBezier);
            colorBezier.Tag = (LineType.Bezier, exampleBezier);

            colorPolygon.SelectedIndexChanged += ChangeToolColor;
            sizePolygon.TextChanged += ChangeToolSize;
            examplePolygon.Paint += ExampleDraw;
            sizePolygon.Tag = (LineType.Polygon, examplePolygon);
            colorPolygon.Tag = (LineType.Polygon, examplePolygon);

            colorFilledCurve.SelectedIndexChanged += ChangeToolColor;
            //colorFilledCurve.SelectedIndexChanged += new SelectionChangedEventHandler(OnTryGetSomethings);
            sizeFilledCurve.TextChanged += ChangeToolSize;
            exampleFilledCurve.Paint += ExampleDraw;
            sizeFilledCurve.Tag = (LineType.FilledCurve, exampleFilledCurve);
            colorFilledCurve.Tag = (LineType.FilledCurve, exampleFilledCurve);
            #endregion



            Button ok = new Button() { Text = "OK"};
            ok.SetBounds(ClientSize.Width / 2 - 40, ClientSize.Height - 10*delta, 80, 40);
            ok.DialogResult = DialogResult.OK;
            ok.Click += (o, e) => { this.Close(); };


            

            #region Add elements to Control Form
            this.Controls.Add(lbl);
            this.Controls.Add(ok);
            this.Controls.Add(pPoint);
            this.Controls.Add(pCurve);
            this.Controls.Add(pBezier);
            this.Controls.Add(pPolygon);
            this.Controls.Add(pFilledCurve);
            #endregion
        }

        

        /// <summary>
        /// Установка толщины рисования инструмента для объекта отрисовки.
        /// </summary>
        private void ChangeToolSize(object sender, EventArgs e)
        {
            var temp = (sender as TextBox);
            
            var obj = (ValueTuple<LineType, Label>)temp.Tag;
            int valueSize = (int.TryParse(temp.Text, out int value)) ? (value < 3 ? 3 : value) : 3;
            switch (obj.Item1)
            {
                case LineType.Point:
                    setPoint.size = valueSize;
                    obj.Item2.Tag = LineType.Point;
                    break;
                case LineType.Curve:
                    setCurve.size = valueSize;
                    obj.Item2.Tag = LineType.Curve;
                    break;
                case LineType.Bezier:
                    setBezier.size = valueSize;
                    obj.Item2.Tag = LineType.Bezier;
                    break;
                case LineType.Polygon:
                    setPolygon.size = valueSize;
                    obj.Item2.Tag = LineType.Polygon;
                    break;
                case LineType.FilledCurve:
                    setFillCurve.size = valueSize;
                    obj.Item2.Tag = LineType.FilledCurve;
                    break;
                default:
                    break;
            }
            obj.Item2.Refresh();
        }
        /// <summary>
        /// Установка цвета для объекта отрисовки.
        /// </summary>
        private void ChangeToolColor(object sender, EventArgs e)
        {
            var t = sender as ComboBox;

            if (t.Tag != null)
            {
                (LineType type, Label graph) k = (ValueTuple<LineType, Label>)(t.Tag);
                var paintObj = k.graph;
                int index;
                Color temp;
                switch (k.type)
                {
                    case LineType.Point:
                        index = t.SelectedIndex;
                        temp = Color.FromName(colorsPoint[index]);
                        setPoint.color = temp;
                        paintObj.Tag = LineType.Point;
                        break;
                    case LineType.Curve:
                        index = t.SelectedIndex;
                        temp = Color.FromName(colorsCurve[index]);
                        setCurve.color = temp;
                        paintObj.Tag = LineType.Curve;
                        break;
                    case LineType.Bezier:
                        index = t.SelectedIndex;
                        temp = Color.FromName(colorsBezie[index]);
                        setBezier.color = temp;
                        paintObj.Tag = LineType.Bezier;
                        break;
                    case LineType.Polygon:
                        index = t.SelectedIndex;
                        temp = Color.FromName(colorsPolygon[index]);
                        setPolygon.color = temp;
                        paintObj.Tag = LineType.Polygon;
                        break;
                    case LineType.FilledCurve:
                        index = t.SelectedIndex;
                        temp = Color.FromName(colorsFillCurve[index]);
                        setFillCurve.color = temp;
                        paintObj.Tag = LineType.FilledCurve;
                        break;
                    default:
                        break;
                }
                paintObj.Refresh();
            }
           
        }
        /// <summary>
        /// Отрисовка объекта рисования.
        /// </summary>
        private void ExampleDraw(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            var t = sender as Label;
            if (t.Tag != null)
            {
                switch ((LineType)t.Tag)
                {
                    case LineType.Point:
                        g.FillEllipse(new SolidBrush(setPoint.color), t.ClientSize.Width / 2 - setPoint.size /2, t.ClientSize.Height/ 2 - setPoint.size / 2, setPoint.size, setPoint.size);
                        break;
                    case LineType.Curve:
                        g.DrawLine(new Pen(setCurve.color, setCurve.size), 4, t.ClientSize.Height/2, t.ClientSize.Width - 2*2, t.ClientSize.Height / 2);
                        break;
                    case LineType.Bezier:
                        g.DrawLine(new Pen(setBezier.color, setBezier.size), 4, t.ClientSize.Height / 2, t.ClientSize.Width - 2 * 2, t.ClientSize.Height / 2);
                        break;
                    case LineType.Polygon:
                        g.DrawLine(new Pen(setPolygon.color, setPolygon.size), 4, t.ClientSize.Height / 2, t.ClientSize.Width - 2 * 2, t.ClientSize.Height / 2);
                        break;
                    case LineType.FilledCurve:
                        g.DrawLine(new Pen(setFillCurve.color, setFillCurve.size), 4, t.ClientSize.Height / 2, t.ClientSize.Width - 2 * 2, t.ClientSize.Height / 2);
                        break;
                }
            }
            else
            {
                if (t == examplePoint)
                {
                    g.FillEllipse(new SolidBrush(Color.FromName(colorsPoint[index])),t.ClientSize.Width/2 - setPoint.size/2, t.ClientSize.Height / 2 - setPoint.size / 2, setPoint.size, setPoint.size);
                }
                else if (t == exampleCurve)
                {
                    g.DrawLine(new Pen(Color.FromName(colorsCurve[index]), setCurve.size), 4, t.ClientSize.Height / 2, t.ClientSize.Width - 2 * 2, t.ClientSize.Height / 2);
                }
                else if (t == exampleBezier)
                {
                    g.DrawLine(new Pen(Color.FromName(colorsBezie[index]), setBezier.size), 4, t.ClientSize.Height / 2, t.ClientSize.Width - 2 * 2, t.ClientSize.Height / 2);
                }
                else if (t == examplePolygon)
                {
                    g.DrawLine(new Pen(Color.FromName(colorsPolygon[index]), setPolygon.size), 4, t.ClientSize.Height / 2, t.ClientSize.Width - 2 * 2, t.ClientSize.Height / 2);
                }
            
                else if (t == exampleFilledCurve)
                {
                    g.DrawLine(new Pen(Color.FromName(colorsFillCurve[index]), setFillCurve.size), 4, t.ClientSize.Height / 2, t.ClientSize.Width - 2 * 2, t.ClientSize.Height / 2);
                }
            }
        }
        private string[] GetPenColors()
        {
            
            var t = Enum.GetNames(typeof(KnownColor));
            return t;
        }
        
    }
}
