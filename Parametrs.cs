using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using static Actions.Task4;

namespace Actions
{
    public class Parametrs : Form
    {
        string[] colorToolForDraw;
        (Pen, int) setPoint = (Pens.Black, 3);
        (Pen, int) setLine = (Pens.Black, 3);

        public Parametrs()
        {
            
            int delta = 5;
            int heightPanel = 90;
            colorToolForDraw = GetPenColors();

            this.Size = new Size(350, 600);

            Label lbl = new Label() { Text = "Настройки отображения."};
            lbl.TextAlign = ContentAlignment.MiddleLeft;
            lbl.Font = new Font("Arial", 12, FontStyle.Bold);
            lbl.SetBounds(delta, delta, ClientSize.Width,20);
            #region POint
            Panel pPoint = new Panel();
            pPoint.SetBounds(delta, lbl.Bottom + delta+5, ClientSize.Width - 3 * delta, heightPanel);
            pPoint.BorderStyle = BorderStyle.FixedSingle;

            Label lblPoint = new Label() { Text = "Для точки:" };
            lblPoint.Font = new Font("Arial", 8, FontStyle.Bold);
            lblPoint.SetBounds(delta, 0, ClientSize.Width, 20);

            ComboBox names = new ComboBox();
            names.SetBounds(ClientSize.Width / 3, lblPoint.Bottom + delta, 110, 40);
            names.DataSource = colorToolForDraw;
            names.MaxDropDownItems = 15;
            names.DropDownStyle = ComboBoxStyle.DropDownList;
            names.SelectedIndexChanged += ChangeToolColor; 
            

            TextBox size = new TextBox() { Text = setPoint.Item2.ToString()};
            size.SetBounds(names.Left, names.Bottom + delta, names.Width, names.Height);
            size.TextChanged += ChangeToolSize; 
                
            
            Label lbl1 = new Label() { Text = "Тип пера"};
            lbl1.SetBounds(delta, lblPoint.Bottom + delta, ClientSize.Width /4, names.Height);
            lbl1.TextAlign = ContentAlignment.MiddleLeft;

            Label lbl2 = new Label() { Text = "Разер точки" };
            lbl2.SetBounds(delta, lbl1.Height *2 + delta , lbl1.Width, names.Height);
            lbl2.TextAlign = ContentAlignment.MiddleLeft;


            Label example = new Label();
            example.BorderStyle = BorderStyle.FixedSingle;
            example.SetBounds(names.Right + delta, names.Top, pPoint.ClientSize.Width - lbl1.Width - names.Width - 8* delta, lbl2.Bottom - lbl1.Top );
            example.Paint += ExampleDraw;
            
            names.Tag = example;
            size.Tag = example;
            pPoint.Controls.Add(example);
            pPoint.Controls.Add(lbl1);
            pPoint.Controls.Add(lbl2);
            pPoint.Controls.Add(size);
            pPoint.Controls.Add(lblPoint);
            pPoint.Controls.Add(names);

            #endregion
            #region Curve
            Panel pCurve = new Panel();
            pCurve.SetBounds(delta, pPoint.Bottom + delta, pPoint.Width, pPoint.Height);
            pCurve.BorderStyle = BorderStyle.FixedSingle;

            Label lblCurve = new Label() { Text = "Для кривой:" };
            lblCurve.Font = new Font("Arial", 8, FontStyle.Bold);
            lblCurve.SetBounds(delta, 0, ClientSize.Width, 20);
            ComboBox namesCurve = new ComboBox();
            namesCurve.SetBounds(ClientSize.Width / 3, lblPoint.Bottom + delta, 110, 40);
            namesCurve.DataSource = colorToolForDraw;
            namesCurve.MaxDropDownItems = 15;
            namesCurve.DropDownStyle = ComboBoxStyle.DropDownList;
            namesCurve.SelectedIndexChanged += ChangeToolColor;


            TextBox sizeCurve = new TextBox() { Text = setPoint.Item2.ToString() };
            sizeCurve.SetBounds(names.Left, names.Bottom + delta, names.Width, names.Height);
            sizeCurve.TextChanged += ChangeToolSize;


            Label lbl1Curve = new Label() { Text = "Тип пера" };
            lbl1Curve.SetBounds(delta, lblPoint.Bottom + delta, ClientSize.Width / 4, names.Height);
            lbl1Curve.TextAlign = ContentAlignment.MiddleLeft;

            Label lbl2Curve = new Label() { Text = "Разер точки" };
            lbl2Curve.SetBounds(delta, lbl1.Height * 2 + delta, lbl1.Width, names.Height);
            lbl2Curve.TextAlign = ContentAlignment.MiddleLeft;


            Label exampleCurve = new Label();
            exampleCurve.BorderStyle = BorderStyle.FixedSingle;
            exampleCurve.SetBounds(names.Right + delta, names.Top, pPoint.ClientSize.Width - lbl1.Width - names.Width - 8 * delta, lbl2.Bottom - lbl1.Top);
            exampleCurve.Paint += ExampleDraw;

            namesCurve.Tag = exampleCurve;
            sizeCurve.Tag = exampleCurve;
            pCurve.Controls.Add(exampleCurve);
            pCurve.Controls.Add(lbl1Curve);
            pCurve.Controls.Add(lbl2Curve);
            pCurve.Controls.Add(sizeCurve);
            pCurve.Controls.Add(lblCurve);
            pCurve.Controls.Add(namesCurve);
            #endregion
            #region Bezier
            Panel pBezier = new Panel();
            pBezier.SetBounds(delta, pCurve.Bottom + delta, pPoint.Width, pPoint.Height);
            pBezier.BorderStyle = BorderStyle.FixedSingle;

            Label lblBezier = new Label() { Text = "Для безье:" };
            lblBezier.Font = new Font("Arial", 8, FontStyle.Bold);
            lblBezier.SetBounds(delta, 0, ClientSize.Width, 20);
            ComboBox namesBezier = new ComboBox();
            namesBezier.SetBounds(ClientSize.Width / 3, lblPoint.Bottom + delta, 110, 40);
            namesBezier.DataSource = colorToolForDraw;
            namesBezier.MaxDropDownItems = 15;
            namesBezier.DropDownStyle = ComboBoxStyle.DropDownList;
            namesBezier.SelectedIndexChanged += ChangeToolColor;


            TextBox sizeBezier = new TextBox() { Text = setPoint.Item2.ToString() };
            sizeBezier.SetBounds(names.Left, names.Bottom + delta, names.Width, names.Height);
            sizeBezier.TextChanged += ChangeToolSize;


            Label lbl1Bezier = new Label() { Text = "Тип пера" };
            lbl1Bezier.SetBounds(delta, lblPoint.Bottom + delta, ClientSize.Width / 4, names.Height);
            lbl1Bezier.TextAlign = ContentAlignment.MiddleLeft;

            Label lbl2Bezier = new Label() { Text = "Разер точки" };
            lbl2Bezier.SetBounds(delta, lbl1.Height * 2 + delta, lbl1.Width, names.Height);
            lbl2Bezier.TextAlign = ContentAlignment.MiddleLeft;


            Label exampleBezier = new Label();
            exampleBezier.BorderStyle = BorderStyle.FixedSingle;
            exampleBezier.SetBounds(names.Right + delta, names.Top, pPoint.ClientSize.Width - lbl1.Width - names.Width - 8 * delta, lbl2.Bottom - lbl1.Top);
            exampleBezier.Paint += ExampleDraw;

            namesBezier.Tag = exampleCurve;
            sizeBezier.Tag = exampleCurve;
            pCurve.Controls.Add(exampleBezier);
            pCurve.Controls.Add(lbl1Bezier);
            pCurve.Controls.Add(lbl2Bezier);
            pCurve.Controls.Add(sizeBezier);
            pCurve.Controls.Add(lblBezier);
            pCurve.Controls.Add(namesBezier);
            #endregion


            //        case LineType.Bezier:
            //        case LineType.Polygon:
            //        case LineType.FilledCurve:



            Button ok = new Button();
            ok.SetBounds(40, pCurve.Bottom + delta*5, 80, 40);
            ok.DialogResult = DialogResult.OK;




            this.Controls.Add(lbl);
            this.Controls.Add(ok);
            this.Controls.Add(pPoint);
            this.Controls.Add(pCurve);
            this.Controls.Add(pBezier);
            //this.Controls.Add(pPolygon);
            //this.Controls.Add(pFilledCurve);
        }

        private void ChangeToolSize(object sender, EventArgs e)
        {
            var temp = (sender as TextBox);
            var obj = temp.Tag as Label;
            int valueSize = (int.TryParse(temp.Text, out int value)) ? (value < 3 ? 3 : value) : 3;
            obj.Tag = LineType.Point;
            setPoint.Item2 = valueSize;
            obj.Refresh();
        }

        private void ChangeToolColor(object sender, EventArgs e)
        {
            var t = sender as ComboBox;
            var obj = t.Tag as Label;
            int index = t.SelectedIndex;
            Color temp = Color.FromName(colorToolForDraw[index]);
            obj.Tag = (LineType.Point, temp);
            obj.Refresh();
        }

        private void ExampleDraw(object sender, PaintEventArgs e)
        {
            var t = sender as Label;
            var g = e.Graphics;
            //if (t.Tag != null)
            //{
                
            //    Label obgDraw = sender as Label;
            //    switch (((ValueTuple<LineType, Color>)t.Tag).Item1)
            //    {
            //        case LineType.Point:

            //            int size = setPoint.Item2;

            //            g.DrawEllipse(Pens.Red, obgDraw.ClientSize.Width / 2 - size / 2, obgDraw.ClientSize.Height / 2 - size / 2, size, size);

            //            break;
            //        case LineType.Curve:
            //            break;
            //        case LineType.Bezier:
            //            break;
            //        case LineType.Polygon:
            //            break;
            //        case LineType.FilledCurve:
            //            break;
            //        default:
            //            break;
            //    }
            //}
            //else
            //    g.DrawEllipse(Pens.Black, t.ClientSize.Width / 2 - 3 / 2, t.ClientSize.Height / 2 - 3 / 2, 3, 3);



        }

        private string[] GetPenColors()
        {
            
            var t = Enum.GetNames(typeof(ConsoleColor));
            return t;
        }
        
    }
}
