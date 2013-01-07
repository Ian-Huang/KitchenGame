using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Speech.Recognition;


namespace KitchenGame
{
    /// <summary>
    /// HomeMenu.xaml 的互動邏輯
    /// </summary>
    public partial class HomeMenu : Page
    {
        private SpeechRecognitionEngine speechEngine;  //語音辨識引擎

        public HomeMenu()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 語音辨識初始化
        /// </summary>
        private void CreateSpeechRecongnition()
        {
            //Initialize speech recognition            
            var recognizerInfo = (from a in SpeechRecognitionEngine.InstalledRecognizers()
                                  where a.Culture.Name == "en-US"
                                  select a).FirstOrDefault();

            if (recognizerInfo != null)
            {
                this.speechEngine = new SpeechRecognitionEngine(recognizerInfo.Id);
                Choices recognizerString = new Choices();

                recognizerString.Add("start");

                GrammarBuilder grammarBuilder = new GrammarBuilder();

                //Specify the culture to match the recognizer in case we are running in a different culture.                                 
                grammarBuilder.Culture = recognizerInfo.Culture;
                grammarBuilder.Append(recognizerString);

                // Create the actual Grammar instance, and then load it into the speech recognizer.
                var grammar = new Grammar(grammarBuilder);

                //載入辨識字串
                this.speechEngine.LoadGrammarAsync(grammar);
                this.speechEngine.SpeechRecognized += SreSpeechRecognized;

                //設定input音源(目前使用預設音源)
                this.speechEngine.SetInputToDefaultAudioDevice();
                this.speechEngine.RecognizeAsync(RecognizeMode.Multiple);
            }
        }

        void SreSpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Confidence < 0.1f)     //肯定度低於0.1，判為錯誤語句            
                return;

            // 前往遊戲頁面(GamePage.xaml)
            if (e.Result.Text.Equals("start"))
                NavigationService.Navigate(new Uri("GamePage.xaml", UriKind.Relative));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //頁面長寬等於NavigationWindow的長寬            
            this.Width = Application.Current.Windows[0].Width;
            this.Height = Application.Current.Windows[0].Height;

            this.CreateSpeechRecongnition();
        }

        //頁面尺寸被改動後呼叫(將一些UI定位而會隨不同解析度改變位置)
        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Canvas.SetLeft(this.StartImage, (e.NewSize.Width - this.StartImage.Width) / 2);
            Canvas.SetTop(this.StartImage, (e.NewSize.Height - this.StartImage.Height) / 2);
        }

        //頁面卸載時，關閉語音辨識機制 
        private void page_Unloaded(object sender, RoutedEventArgs e)
        {
            if (this.speechEngine != null)
            {
                this.speechEngine.RecognizeAsyncCancel();
                this.speechEngine.RecognizeAsyncStop();
            }
        }
    }
}
