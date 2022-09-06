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

        public struct DrawSetting
        {
            /// <summary>
            /// Цвет инструмента.
            /// </summary>
            public Color color;
            /// <summary>
            /// Толщина линии/точки.
            /// </summary>
            public int size;
            LineType name;
            public DrawSetting(Color color, int size, LineType name)
            {
                this.color = color;
                this.size = size;
                this.name = name;
            }
            public override string ToString()
            {
                return $"Цвет: {color.Name} , Размер:{size}";
            }
        }
        DrawSetting[] pointsSet;
        //    { new DrawSetting(Color.Black, 3,"Point"),
        //      new DrawSetting(Color.Red, 1, "Curve"),
        //      new DrawSetting(Color.Orange, 1, "Bezier"),
        //      new DrawSetting(Color.Green, 1, "Polygon"),
        //      new DrawSetting(Color.Blue, 1, "FilledCurve")
        //    };
        


        private readonly string[] title = { "Цвет пера", "Размер пера" };
        public Parametrs(DrawSetting[] settings)
        {
            pointsSet = settings ?? GetDefaultSettings();
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
            //colorPoint.Items.AddRange(GetPenColors());
            //if (colorPoint.Items.Count != 0)
            //{
            //    colorPoint.SelectedIndex = SetStartValue(pointsSet[0].color);
            //}
            //; // SetStartValue(pointsSet[0].color);
            colorPoint.MaxDropDownItems = 15;
            colorPoint.DropDownStyle = ComboBoxStyle.DropDownList;

            
            


            TextBox sizePoint = new TextBox() { Text = pointsSet[0].size.ToString()};
            sizePoint.SetBounds(colorPoint.Left, colorPoint.Bottom + delta, colorPoint.Width, colorPoint.Height);
            
                
            
            Label Point = new Label() { Text = title[0] };
            Point.SetBounds(delta, lblPoint.Bottom + delta, ClientSize.Width /4, colorPoint.Height);
            Point.TextAlign = ContentAlignment.MiddleLeft;

            Label lbl2Point = new Label() { Text = title[1] };
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
            this.Load += Parametrs_Load;
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
            

            
            TextBox sizeCurve = new TextBox() { Text = pointsSet[2].size.ToString() };
            sizeCurve.SetBounds(colorPoint.Left, colorPoint.Bottom + delta, colorPoint.Width, colorPoint.Height);
            


            Label lbl1Curve = new Label() { Text = title[0] };
            lbl1Curve.SetBounds(delta, lblPoint.Bottom + delta, ClientSize.Width / 4, colorPoint.Height);
            lbl1Curve.TextAlign = ContentAlignment.MiddleLeft;

            Label lbl2Curve = new Label() { Text = title[1] };
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
            


            TextBox sizeBezier = new TextBox() { Text = pointsSet[3].size.ToString() };
            sizeBezier.SetBounds(colorPoint.Left, colorPoint.Bottom + delta, colorPoint.Width, colorPoint.Height);
            


            Label lbl1Bezier = new Label() { Text = title[0] };
            lbl1Bezier.SetBounds(delta, lblPoint.Bottom + delta, ClientSize.Width / 4, colorPoint.Height);
            lbl1Bezier.TextAlign = ContentAlignment.MiddleLeft;

            Label lbl2Bezier = new Label() { Text = title[1] };
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
           
            TextBox sizePolygon = new TextBox() { Text = pointsSet[1].size.ToString() };
            sizePolygon.SetBounds(colorPoint.Left, colorPoint.Bottom + delta, colorPoint.Width, colorPoint.Height);
            


            Label lbl1Polygon = new Label() { Text = title[0] };
            lbl1Polygon.SetBounds(delta, lblPoint.Bottom + delta, ClientSize.Width / 4, colorPoint.Height);
            lbl1Polygon.TextAlign = ContentAlignment.MiddleLeft;

            Label lbl2Polygon = new Label() { Text = title[1] };
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
            


            TextBox sizeFilledCurve = new TextBox() { Text = pointsSet[4].size.ToString() };
            sizeFilledCurve.SetBounds(colorPoint.Left, colorPoint.Bottom + delta, colorPoint.Width, colorPoint.Height);
            


            Label lbl1FilledCurve = new Label() { Text = title[0] };
            lbl1FilledCurve.SetBounds(delta, lblPoint.Bottom + delta, ClientSize.Width / 4, colorPoint.Height);
            lbl1FilledCurve.TextAlign = ContentAlignment.MiddleLeft;

            Label lbl2FilledCurve = new Label() { Text = title[1] };
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

        private int SetStartValue(Color color)
        {
            int k = Array.FindIndex(colorsPoint, x => Color.FromName(x) == color);
            return k;
        }


        /// <summary>
        /// Получение настроек отрисовки по умолчанию.
        /// </summary>
        private DrawSetting[] GetDefaultSettings()
        {
            return new DrawSetting[]
                        {
                          new DrawSetting(Color.Black, 3, LineType.Point),
                          new DrawSetting(Color.Red, 1, LineType.Curve),
                          new DrawSetting(Color.Orange, 1,LineType.Bezier),
                          new DrawSetting(Color.Green, 1, LineType.Polygon),
                          new DrawSetting(Color.Blue, 1, LineType.FilledCurve)
                        };
        }

        /// <summary>
        /// Установка толщины рисования инструмента для объекта отрисовки.
        /// </summary>
        private void ChangeToolSize(object sender, EventArgs e)
        {
            var temp = (sender as TextBox);
            
            var obj = (ValueTuple<LineType, Label>)temp.Tag;
            int valueSize;
            if (obj.Item1 == LineType.Point)
            {
                valueSize = (int.TryParse(temp.Text, out int value)) ? (value < 3 ? 3 : value) : 3;
            }
            else
            {
                valueSize = (int.TryParse(temp.Text, out int value)) ? value : 1;
            }
            
            switch (obj.Item1)
            {
                case LineType.Point:
                    pointsSet[0].size = valueSize;
                    obj.Item2.Tag = LineType.Point;
                    break;
                case LineType.Curve:
                    pointsSet[1].size = valueSize;
                    obj.Item2.Tag = LineType.Curve;
                    break;
                case LineType.Bezier:
                    pointsSet[2].size = valueSize;
                    obj.Item2.Tag = LineType.Bezier;
                    break;
                case LineType.Polygon:
                    pointsSet[3].size = valueSize;
                    obj.Item2.Tag = LineType.Polygon;
                    break;
                case LineType.FilledCurve:
                    pointsSet[4].size = valueSize;
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
                        //По имеющемуся цвету установить SelectIndex
                        
                        index = t.SelectedIndex;
                        temp = Color.FromName(colorsPoint[index]);
                        pointsSet[0].color = temp;
                        paintObj.Tag = LineType.Point;
                        break;
                    case LineType.Curve:
                        index = t.SelectedIndex;
                        temp = Color.FromName(colorsCurve[index]);
                        pointsSet[1].color = temp;
                        paintObj.Tag = LineType.Curve;
                        break;
                    case LineType.Bezier:
                        index = t.SelectedIndex;
                        temp = Color.FromName(colorsBezie[index]);
                        pointsSet[2].color = temp;
                        paintObj.Tag = LineType.Bezier;
                        break;
                    case LineType.Polygon:
                        index = t.SelectedIndex;
                        temp = Color.FromName(colorsPolygon[index]);
                        pointsSet[3].color = temp;
                        paintObj.Tag = LineType.Polygon;
                        break;
                    case LineType.FilledCurve:
                        index = t.SelectedIndex;
                        temp = Color.FromName(colorsFillCurve[index]);
                        pointsSet[4].color = temp;
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
            if (this.Tag != null)
            {
                pointsSet = (DrawSetting[])this.Tag;
            }

            var g = e.Graphics;
            var t = sender as Label;
            if (t.Tag != null)
            {
                switch ((LineType)t.Tag)
                {
                    case LineType.Point:
                        g.FillEllipse(new SolidBrush(pointsSet[0].color), t.ClientSize.Width / 2 - pointsSet[0].size /2, t.ClientSize.Height/ 2 - pointsSet[0].size / 2, pointsSet[0].size, pointsSet[0].size);
                        break;
                    case LineType.Curve:
                        g.DrawLine(new Pen(pointsSet[1].color, pointsSet[1].size), 4, t.ClientSize.Height/2, t.ClientSize.Width - 2*2, t.ClientSize.Height / 2);
                        break;
                    case LineType.Bezier:
                        g.DrawLine(new Pen(pointsSet[2].color, pointsSet[2].size), 4, t.ClientSize.Height / 2, t.ClientSize.Width - 2 * 2, t.ClientSize.Height / 2);
                        break;
                    case LineType.Polygon:
                        g.DrawLine(new Pen(pointsSet[3].color, pointsSet[3].size), 4, t.ClientSize.Height / 2, t.ClientSize.Width - 2 * 2, t.ClientSize.Height / 2);
                        break;
                    case LineType.FilledCurve:
                        g.DrawLine(new Pen(pointsSet[4].color, pointsSet[4].size), 4, t.ClientSize.Height / 2, t.ClientSize.Width - 2 * 2, t.ClientSize.Height / 2);
                        break;
                }
            }
            else
            {
                if (t == examplePoint)
                {
                    g.FillEllipse(new SolidBrush(pointsSet[0].color),t.ClientSize.Width/2 - pointsSet[0].size /2, t.ClientSize.Height / 2 - pointsSet[0].size / 2, pointsSet[0].size, pointsSet[0].size);
                }
                else if (t == exampleCurve)
                {
                    g.DrawLine(new Pen(pointsSet[2].color, pointsSet[2].size), 4, t.ClientSize.Height / 2, t.ClientSize.Width - 2 * 2, t.ClientSize.Height / 2);
                }
                else if (t == exampleBezier)
                {
                    g.DrawLine(new Pen(pointsSet[3].color, pointsSet[3].size), 4, t.ClientSize.Height / 2, t.ClientSize.Width - 2 * 2, t.ClientSize.Height / 2);
                }
                else if (t == examplePolygon)
                {
                    g.DrawLine(new Pen(pointsSet[1].color, pointsSet[1].size), 4, t.ClientSize.Height / 2, t.ClientSize.Width - 2 * 2, t.ClientSize.Height / 2);
                }
            
                else if (t == exampleFilledCurve)
                {
                    g.DrawLine(new Pen(pointsSet[4].color, pointsSet[4].size), 4, t.ClientSize.Height / 2, t.ClientSize.Width - 2 * 2, t.ClientSize.Height / 2);
                }
            }
        }
        private string[] GetPenColors()
        {
            var t = Enum.GetNames(typeof(KnownColor));
            Array.Sort(t);
            return t;
        }
        /// <summary>
        /// Получение настроек для отрисовки.
        /// </summary>
        /// <returns>Array с настройками.</returns>
        public DrawSetting [] GetSettingsForDrawing()
        {
            return pointsSet ?? null;
        }

        private void Parametrs_Load(object sender, EventArgs e)
        {
            var t = sender as Parametrs;
            foreach (var item in t.Controls)
            {
                if (item is Panel obj)
                {
                    foreach (var combo in obj.Controls)
                    {
                        if (combo is ComboBox box)
                        {
                            var k = (LineType)((ValueTuple<LineType, Label>)box.Tag).Item1;
                            switch (k)
                            {
                                case LineType.Point:
                                    box.SelectedIndex = SetStartValue(pointsSet[0].color);

                                    break;
                                case LineType.Curve:
                                    box.SelectedIndex = SetStartValue(pointsSet[1].color);
                                    break;
                                case LineType.Bezier:
                                    box.SelectedIndex = SetStartValue(pointsSet[2].color);
                                    break;
                                case LineType.Polygon:
                                    box.SelectedIndex = SetStartValue(pointsSet[3].color);
                                    break;
                                case LineType.FilledCurve:
                                    box.SelectedIndex = SetStartValue(pointsSet[4].color);
                                    break;
                                default:
                                    break;
                            }
                        }
                        if (combo is TextBox text)
                        {
                            var k = (LineType)((ValueTuple<LineType, Label>)text.Tag).Item1;
                            switch (k)
                            {
                                case LineType.Point:
                                    text.Text = pointsSet[0].size.ToString();
                                    break;
                                case LineType.Curve:
                                    text.Text = pointsSet[1].size.ToString();
                                    break;
                                case LineType.Bezier:
                                    text.Text = pointsSet[2].size.ToString();
                                    break;
                                case LineType.Polygon:
                                    text.Text = pointsSet[3].size.ToString();
                                    break;
                                case LineType.FilledCurve:
                                    text.Text = pointsSet[4].size.ToString();
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }
        }
    }
}

