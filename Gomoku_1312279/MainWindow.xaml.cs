using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.ComponentModel;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json.Linq;
using Quobject.SocketIoClientDotNet.Client;
using Gomoku_1312279.ViewModels;
using Gomoku_1312279.Properties;

namespace Gomoku_1312279
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        GomokuViewModel game;
        bool online = false;
        Socket socket;
        
        public MainWindow()
        {
            InitializeComponent();
            NewGame();
        }

        private void canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            //tính vị trí đánh rồi truyền vào
            if (cbBox.SelectedIndex == 0)
            {
               game.PlayAt(canvas, ChessManPos(e.GetPosition(canvas)));
            }

            if (cbBox.SelectedIndex == 1)
            {
                bool validStep = game.PlayMachine(canvas, ChessManPos(e.GetPosition(canvas)));
                validStep = false;
                //gọi hàn random để sinh ra vị trí trên ô cờ
                while (validStep == false)
                {
                    validStep = game.PlayMachine(canvas, new Point(RandomPos(), RandomPos()));
                }
            }
        }

        void NewGame()
        {
            string ten = name.Text;

            if (canvas.Children.Count != 0)
                canvas.Children.Clear();
            game = new GomokuViewModel();
            game.Paint_ChessBoard(canvas);

            // mở kết nối
            if (online)
            {
                socket = IO.Socket(Settings.Default.connectStr);
                socket.On(Socket.EVENT_CONNECT, () =>
                {
                    MessageBox.Show( "Connected");
                    socket.Emit("ChatMessage", "hh");
                    socket.Emit("message:" + "hh", "from:" + "hello");
                });
                socket.On(Socket.EVENT_MESSAGE, (data) =>
                {
                    MessageBox.Show(((JObject)data)["message"].ToString());
                });
                socket.On(Socket.EVENT_CONNECT_ERROR, (data) =>
                {
                    MessageBox.Show(((JObject)data)["message"].ToString());
                });
                socket.On("ChatMessage", (data) =>
                {
                    if (((JObject)data)["message"].ToString() == "Welcome!")
                    {
                        socket.Emit("MyNameIs", ten);
                        socket.Emit("ConnectToOtherPlayer");
                    }
                    // nhận chat
                    if (data.ToString().Contains("from"))
                    {
                        MessageBox.Show(data.ToString());
                    }
                });
                
                socket.On(Socket.EVENT_ERROR, (data) =>
                {
                    MessageBox.Show(((JObject)data)["message"].ToString());
                });
                socket.On("NextStepIs", (data) =>
                {
                    MessageBox.Show(data.ToString());
                });
            }
        }

        private void btn_new_Click(object sender, RoutedEventArgs e)
        {           
                NewGame();
        }

        private void canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (canvas.Children.Count != 0)
                game.Zoom_ChessBoard(canvas);
        }

        private void Messeage_GotFocus(object sender, RoutedEventArgs e)
        {
            txt_message.Text = "";
        }

        private void btn_SendMessage(object sender, RoutedEventArgs e)
        {
            if (online)
            {
                if (txt_message.Text != "Type your message here...")
                {
                    string s = txt_message.Text;
                    
                    socket.Emit("ChatMessage", s);
                    socket.Emit("message:" + s, "from:" + name.Text);
                }

            }
            else {
                if (txt_message.Text != "Type your message here...")
                {
                    chat.FontSize = 15;
                    chat.FontWeight = FontWeights.Bold;
                    chat.AppendText(name.Text + "\n");
                    chat.FontWeight = FontWeights.Regular;

                    chat.AppendText(txt_message.Text + "\n");
                    chat.FontSize = 10;
                    chat.AppendText(string.Format("{0:HH:mm:ss tt}", DateTime.Now) + "\n" + "-----------------------------" + "\n");
                    chat.ScrollToEnd();
                    txt_message.Text = "Type your message here...";
                }
            }
        }

        private void btn_ChangeName(object sender, RoutedEventArgs e)
        {
            //Không cần thiết
        }

        //tính vị trí của quân cờ
        //tính ra vị trí tương ứng trên ô cờ khi click ví dụ chuột trỏ 10 10 thì ô cờ là 0 0
        private Point ChessManPos(Point pos)
        {
            double x = canvas.ActualWidth / 12;
            double y = canvas.ActualHeight / 12;

            double row = pos.X / x;
            double col = pos.Y / y;
            return new Point(row, col);
        }

        // tự đông sinh ra 1 số nguuyeenf nằm trong vùng từ 0 -> 11
        private int RandomPos()
        {
            Random ran = new Random();
            return ran.Next(0, 11);
        }

        private void btn_OnlineClick(object sender, RoutedEventArgs e)
        {
            online = true;
            NewGame();
        }
    }
}
