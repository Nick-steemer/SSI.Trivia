namespace SSI.Trivia
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            Task.Run(async () =>
            {
                await SecureStorage.SetAsync("OpenAIApiKey",
                    "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");
            });
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new MainPage()) { Title = "SSI.Trivia" };
        }
    }
}
