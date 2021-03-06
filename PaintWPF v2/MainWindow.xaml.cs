﻿using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PaintWPF_v2
{
    public partial class MainWindow
    {
        private int cntClr = 2; //Счетчик нажатия ластика
        public bool TogglePolygon { get; set; } //включатель многоугольника

        public MainWindow()
        {
            InitializeComponent();
            InkCanvas1.EditingMode = InkCanvasEditingMode.Ink; 
        }

        public PointCollection Points { get; set; } //коллекция точек
        public Point Coordinates { get; set; } //Текущие координаты курсора
        public double StrokeShape { get; set; } = 2; //толщина границы фигуры
        public string Shape { get; set; } //тип фигуры
        public bool Toggle { get; set; } //Включение рисования многоугольника

        public Brush ShcolorBrush = Brushes.Black; //цвета = стандартный цвет
        private Brush ShapeColorBrush;

        private void Clear_Click(object sender, RoutedEventArgs e) //Стереть все
        {
            InkCanvas1.Strokes.Clear();
            InkCanvas1.Children.Clear();
        }

        private void Save_Click(object sender, RoutedEventArgs e) //Сохранить все
        {
            var saveimg = new SaveFileDialog
            {
                FileName = "Изображение",
                Filter =
                    "Ink Serialized Format (*.isf)|*.isf|" +
                    "Bitmap Image (.bmp)|*.bmp|Gif Image (.gif)|*.gif|JPEG Image (.jpeg)|*.jpeg|Png Image (.png)|*.png"
            };

            var result = saveimg.ShowDialog();
            if (result == true)
            {
                if (saveimg.FilterIndex == 1)
                {
                    string strXAML = System.Windows.Markup.XamlWriter.Save(InkCanvas1);

                    using (var fs = File.Create(saveimg.FileName))
                    {
                        using (var streamwriter = new StreamWriter(fs))
                        {
                            streamwriter.Write(strXAML);
                        }
                    }
                }
                else
                {
                    using (FileStream file = new FileStream(saveimg.FileName,
                        FileMode.Create, FileAccess.Write))
                    {
                        var marg = int.Parse(InkCanvas1.Margin.Left.ToString());
                        RenderTargetBitmap rtb = new RenderTargetBitmap((int) InkCanvas1.ActualWidth - marg,
                            (int) InkCanvas1.ActualHeight - marg, 0, 0, PixelFormats.Default);
                        rtb.Render(InkCanvas1);
                        BmpBitmapEncoder encoder = new BmpBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create(rtb));
                        encoder.Save(file);
                        file.Close();
                    }
                }
            }
        }

        private void Color_Click(object sender, RoutedEventArgs e) //Обработчик кнопок цвета
        {
            switch ((sender as Button).Name)
            {
                case "Black":
                    InkCanvas1.DefaultDrawingAttributes.Color = Colors.Black;
                    ShapeColorBrush = Brushes.Black;
                    break;

                case "Gray":
                    InkCanvas1.DefaultDrawingAttributes.Color = Colors.Gray;
                    ShapeColorBrush = Brushes.Gray;
                    break;

                case "White":
                    InkCanvas1.DefaultDrawingAttributes.Color = Colors.White;
                    ShapeColorBrush = Brushes.White;
                    break;

                case "Red":
                    InkCanvas1.DefaultDrawingAttributes.Color = Colors.Red;
                    ShapeColorBrush = Brushes.Red;
                    break;

                case "Orange":
                    InkCanvas1.DefaultDrawingAttributes.Color = Colors.Orange;
                    ShapeColorBrush = Brushes.Orange;
                    break;

                case "Yellow":
                    InkCanvas1.DefaultDrawingAttributes.Color = Colors.Yellow;
                    ShapeColorBrush = Brushes.Yellow;
                    break;

                case "Pink":
                    InkCanvas1.DefaultDrawingAttributes.Color = Colors.Pink;
                    ShapeColorBrush = Brushes.Pink;
                    break;

                case "Green":
                    InkCanvas1.DefaultDrawingAttributes.Color = Colors.Green;
                    ShapeColorBrush = Brushes.Green;
                    break;

                case "Aquamarine":
                    InkCanvas1.DefaultDrawingAttributes.Color = Colors.Aquamarine;
                    ShapeColorBrush = Brushes.Aquamarine;
                    break;

                case "SkyBlue":
                    InkCanvas1.DefaultDrawingAttributes.Color = Colors.SkyBlue;
                    ShapeColorBrush = Brushes.SkyBlue;
                    break;

                case "Blue":
                    InkCanvas1.DefaultDrawingAttributes.Color = Colors.Blue;
                    ShapeColorBrush = Brushes.Blue;
                    break;

                case "Purple":
                    InkCanvas1.DefaultDrawingAttributes.Color = Colors.Purple;
                    ShapeColorBrush = Brushes.Purple;
                    break;
            }
        }

        private void Thickness(object sender, RoutedEventArgs e) //Обработчик кнопок толщины линии
        {
            switch ((sender as Button).Name)
            {
                case "ThicknessSmall":
                    InkCanvas1.DefaultDrawingAttributes.Height = 2;
                    InkCanvas1.DefaultDrawingAttributes.Width = 2;
                    StrokeShape = 2;
                    break;

                case "ThicknessMedium":
                    InkCanvas1.DefaultDrawingAttributes.Height = 10;
                    InkCanvas1.DefaultDrawingAttributes.Width = 10;
                    StrokeShape = 10;
                    break;

                case "ThicknessLarge":
                    InkCanvas1.DefaultDrawingAttributes.Height = 20;
                    InkCanvas1.DefaultDrawingAttributes.Width = 20;
                    StrokeShape = 20;
                    break;
            }
        }

        private void Resize_Click(object sender, RoutedEventArgs e) //Изменение размера канвы
        {
            try
            {
                int tempHeight = Convert.ToInt16(HeightCanvas.Text);
                int tempWidth = Convert.ToInt16(WidthCanvas.Text);
                InkCanvas1.Height = tempHeight;
                InkCanvas1.Width = tempWidth;
            }
            catch
            {
                MessageBox.Show("Вы ввели слишком большие числа или не целые");
            }
        }

        private void Open_Click(object sender, RoutedEventArgs e) //Открыть изображение
        {
            var openImg = new OpenFileDialog();
            openImg.FileName = "*";
            openImg.Filter =
                "Ink Serialized Format (*.isf)|*.isf|" +
                "Bitmap Image (.bmp)|*.bmp|Gif Image (.gif)|*.gif|JPEG Image (.jpeg)|*.jpeg|Png Image (.png)|*.png";
            var result = openImg.ShowDialog();
            if (result == true)
            {
                InkCanvas1.Background = Brushes.White;
                InkCanvas1.Strokes.Clear();
                try
                {
                    using (FileStream file = new FileStream(openImg.FileName,
                        FileMode.Open, FileAccess.Read))
                    {
                        if (openImg.FileName.ToLower().EndsWith(".isf"))
                        {
                            DeSerializeXAML(InkCanvas1, openImg.FileName);
                            file.Close();
                        }
                        else
                        {
                            ImageBrush brush = new ImageBrush();
                            brush.ImageSource = new BitmapImage(new Uri(openImg.FileName, UriKind.Relative));
                            InkCanvas1.Background = brush;
                        }
                    }
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message, Title);
                }
            }
        }

        private void ClearingTool_Click(object sender, RoutedEventArgs e) //Ластик
        {
            Standartcursor();
            Shape = "Clear";
            cntClr++;
            if (cntClr % 2 == 1)
            {
                //InkCanvas1.EditingMode = InkCanvasEditingMode.EraseByPoint;
                InkCanvas1.DefaultDrawingAttributes.Height = 40;
                InkCanvas1.DefaultDrawingAttributes.Width = 40;
                InkCanvas1.DefaultDrawingAttributes.Color = Colors.White;
            }
            else
                InkCanvas1.EditingMode = InkCanvasEditingMode.Ink;
        }

        private void ShapeChanged(object sender, RoutedEventArgs e) //Обработчик кнопок фигур
        {
            Standartcursor();
            switch ((sender as Button).Name)
            {
                case "Rectangle":
                    TogglePolygon = false;
                    Shape = "Rectangle";
                    InkCanvas1.EditingMode = InkCanvasEditingMode.None;
                    break;
                case "Ellipse":
                    TogglePolygon = false;
                    Shape = "Ellipse";
                    InkCanvas1.EditingMode = InkCanvasEditingMode.None;
                    break;
                case "Line":
                    TogglePolygon = false;
                    Shape = "Line";
                    InkCanvas1.EditingMode = InkCanvasEditingMode.None;
                    break;
                case "Circle":
                    TogglePolygon = false;
                    Shape = "Circle";
                    InkCanvas1.EditingMode = InkCanvasEditingMode.None;
                    break;
                case "Polygon":
                    Shape = "Polygon";
                    InkCanvas1.EditingMode = InkCanvasEditingMode.None;
                    break;
            }
        }

        private void Back(object sender, RoutedEventArgs e) //Отмена последнего действия
        {
            try
            {
                InkCanvas1.Children.RemoveAt(InkCanvas1.Children.Count - 1);
            }
            catch
            {
                // ignored
            }

            try
            {
                InkCanvas1.Strokes.RemoveAt(InkCanvas1.Strokes.Count - 1);
            }
            catch
            {
                // ignored
            }
        }

        private void DrawLine(Point p1, Point p2) //рисует линию
        {
            var myLine = new Line
            {
                Stroke = ShapeColorBrush,
                X1 = p1.X,
                X2 = p2.X,
                Y1 = p1.Y,
                Y2 = p2.Y,
                StrokeThickness = StrokeShape
            };
            InkCanvas1.Children.Add(myLine);
        }

        private void DrawRectangle(double X, double Y) //Рисует прямоугольник
        {
            var myRectangle = new Rectangle
            {
                Fill = ShapeColorBrush,
                Stroke = ShapeColorBrush,
                Margin = new Thickness(X, Y, 0, 0),
                StrokeThickness = StrokeShape,
                Height = 1,
                Width = 1
            };
            InkCanvas1.Children.Add(myRectangle);
        }

        private void DrawEllipse(double X, double Y) //Рисует овал
        {
            var myEllipse = new Ellipse
            {
                Fill = ShapeColorBrush,
                Stroke = ShapeColorBrush,
                Margin = new Thickness(X, Y, 0, 0),
                StrokeThickness = StrokeShape,
                Height = 1,
                Width = 1
            };
            InkCanvas1.Children.Add(myEllipse);
        }

        private void BuildShape(Point p) //
        {
            var newShape = (Shape) InkCanvas1.Children[InkCanvas1.Children.Count - 1];
            double tempX, tempY;
            if (p.X - Coordinates.X > 0)
            {
                newShape.Width = p.X - Coordinates.X;
                tempX = Coordinates.X;
            }
            else
            {
                newShape.Width = Coordinates.X - p.X;
                tempX = p.X;
            }

            if (p.Y - Coordinates.Y > 0)
            {
                newShape.Height = p.Y - Coordinates.Y;
                tempY = Coordinates.Y;
            }
            else
            {
                newShape.Height = Coordinates.Y - p.Y;
                tempY = p.Y;
            }

            newShape.Margin = new Thickness(tempX, tempY, 0, 0);
        }

        private void BuildLine(Point p) //Завершение
        {
            Line newLine = (Line) InkCanvas1.Children[InkCanvas1.Children.Count - 1];
            newLine.X2 = p.X;
            newLine.Y2 = p.Y;
        }

        private void BuildCircle(Point p) //Завершение круга
        {
            Ellipse newCircle = (Ellipse) InkCanvas1.Children[InkCanvas1.Children.Count - 1];
            double tempX, tempY;

            //определение диаметра круга

            var tempW = 2 * Math.Abs((p.X) - Coordinates.X);
            var tempH = 2 * Math.Abs((p.Y) - Coordinates.Y);

            if (tempH >= tempW) //определение диаметра круга
            {
                newCircle.Height = tempH;
                newCircle.Width = tempH;
                tempX = Coordinates.X - tempH / 2;
                tempY = Coordinates.Y - tempH / 2;
            }
            else
            {
                newCircle.Width = tempW;
                newCircle.Height = tempW;
                tempX = Coordinates.X - tempW / 2;
                tempY = Coordinates.Y - tempW / 2;
            }

            newCircle.Margin = new Thickness(tempX, tempY, 0, 0); //определение координат круга, от краёв альбома
        }

        private void BuildPolygon() //добавление на рисунок обьекта многоугольника
        {
            Polygon Poly = (Polygon) InkCanvas1.Children[InkCanvas1.Children.Count - 1];
            Poly.Points.Add(Coordinates);
        }

        private void StartDrawFigure() //Класс вызова начала рисования при нажатии лкм
        {
            switch (Shape)
            {
                case "Rectangle":
                    DrawRectangle(Coordinates.X, Coordinates.Y);
                    break;
                case "Ellipse":
                    DrawEllipse(Coordinates.X, Coordinates.Y);
                    break;
                case "Line":
                    DrawLine(Coordinates, Coordinates);
                    break;
                case "Circle":
                    DrawEllipse(Coordinates.X, Coordinates.Y);
                    break;
                case "Polygon":
                    if (TogglePolygon)
                    {
                        BuildPolygon();
                    }
                    else DrawPolygon();

                    break;
            }
        }

        private void DrawPolygon() //создание обьекта многоугольника
        {
            TogglePolygon = true;
            Points = new PointCollection
            {
                Coordinates
            };
            Polygon myPolygon = new Polygon
            {
                Fill = ShapeColorBrush,
                Stroke = ShcolorBrush,
                StrokeThickness = StrokeShape
            };
            myPolygon.Points = Points;
            InkCanvas1.Children.Add(myPolygon);
        }

        private void InkCanvas1_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Coordinates = new Point(e.GetPosition(InkCanvas1).X, e.GetPosition(InkCanvas1).Y);
            StartDrawFigure();
            Toggle = true;
        }

        private void InkCanvas1_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Toggle = false;
        }

        private void InkCanvas1_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            NumCh.Content = InkCanvas1.Children.Count;
            Coord.Text = e.GetPosition(InkCanvas1).X + " , " + e.GetPosition(InkCanvas1).Y;
            if (Toggle == false) return;
            var MovePoint = new Point
            {
                X = e.GetPosition(InkCanvas1).X,
                Y = e.GetPosition(InkCanvas1).Y
            };
            switch (Shape)
            {
                case "Rectangle":
                    BuildShape(MovePoint);
                    break;
                case "Ellipse":
                    BuildShape(MovePoint);
                    break;
                case "Circle":
                    BuildCircle(MovePoint);
                    break;
                case "Line":
                    BuildLine(MovePoint);
                    break;
            }
        }

        private void Pen_Click(object sender, RoutedEventArgs e)//Кнопка ручки
        {
            Mouse.OverrideCursor = Cursors.Pen;
            Mouse.Capture(Pen);
            Shape = string.Empty;
            InkCanvas1.EditingMode = InkCanvasEditingMode.Ink;
        }
         
        private void Cursor_OnClick(object sender, RoutedEventArgs e) //Кнопка выделения
        {
            Standartcursor();
            Shape = "Cursor";
            InkCanvas1.EditingMode = InkCanvasEditingMode.Select;
        }
        public static void SerializeToXAML(InkCanvas elements, string filename)
        {
            string strXAML = System.Windows.Markup.XamlWriter.Save(elements);

            using (var fs = File.Create(filename))
            {
                using (var streamwriter = new StreamWriter(fs))
                {
                    streamwriter.Write(strXAML);
                }
            }
        }

        public static void DeSerializeXAML(InkCanvas elements, string filename)
        {
            var context = System.Windows.Markup.XamlReader.GetWpfSchemaContext();

            var settings = new System.Xaml.XamlObjectWriterSettings
            {
                RootObjectInstance = elements
            };
            using (var reader = new System.Xaml.XamlXmlReader(filename))
            using (var writer = new System.Xaml.XamlObjectWriter(context, settings))
            {
                System.Xaml.XamlServices.Transform(reader, writer);
            }
        }
        private void Standartcursor()
        {
            Mouse.Capture(null);
            Mouse.OverrideCursor = null;
        }

        public static void CopyUIElementToClipboard(FrameworkElement element) //Копирование канвы в bitmap в буфер обмена
        {
            double width = element.ActualWidth;
            double height = element.ActualHeight;
            RenderTargetBitmap bmpCopied = new RenderTargetBitmap((int) Math.Round(width), (int) Math.Round(height), 96,
                96, PixelFormats.Default);
            DrawingVisual dv = new DrawingVisual();
            using (DrawingContext dc = dv.RenderOpen())
            {
                VisualBrush vb = new VisualBrush(element);
                dc.DrawRectangle(vb, null, new Rect(new Point(), new Size(width, height)));
            }

            bmpCopied.Render(dv);
            Clipboard.SetImage(bmpCopied);
        }

        private void CopyCommand(object sender, ExecutedRoutedEventArgs e) //Обработчик ктрл+с
        {
            CopyUIElementToClipboard(InkCanvas1);
        }

        private void PasteCommand(object sender, ExecutedRoutedEventArgs e) //обработчик ктрл+в
        {
            if (InkCanvas1.CanPaste())
            {
                InkCanvas1.Paste(new Point(100, 100));
            }
        }
    }
}