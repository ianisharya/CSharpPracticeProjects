namespace AnimalMatchingGame;

public partial class MainPage : ContentPage
{

	Button lastClicked = new();
	bool findingMatch = false;
	int numMatchesFound = 0;
	int tenthsOfSecondsElapsed = 0;
	double progress = 0.0;

	List<int> timeScores = new(); // to keep the track of completed game-times

		/// <summary>
		/// When the user clicks the animal emojis, this Event handler method checks for pairs ad if found deletes the pairs from the users' screen.
		/// This method also provides a progress bar to enhance the user experience.
		/// </summary>
    private void AnimalButton_Clicked(object sender, EventArgs e)
	{
		if( sender is Button buttonClicked )
		{
			if(!string.IsNullOrWhiteSpace(buttonClicked.Text) && (findingMatch == false) )
			{ // user clicks on the first button in match finding process
				buttonClicked.BackgroundColor = Colors.Red;
				lastClicked = buttonClicked;
				findingMatch = true;
			}
			else
			{
				if( (buttonClicked != lastClicked) && (buttonClicked.Text == lastClicked.Text)
						&& (!string.IsNullOrWhiteSpace(buttonClicked.Text)) )
				{
					// user clicks on the second button in match finding process and the match is found
					++numMatchesFound;
					lastClicked.Text = " ";
					buttonClicked.Text = " ";
					// increase the 
					this.progress += 0.125;
					ProgressBar.ProgressTo(this.progress, 100, Easing.Linear); // 12.5 percent because of 8 pairs to be matched (100/8)
				}
				// irrespective of the user clicking the second button, the bg of both the buttons must return to original state.
				lastClicked.BackgroundColor = Colors.LightBlue;
				buttonClicked.BackgroundColor = Colors.LightBlue;
				findingMatch = false;
			}
		}

		// stop the game nd reset the majot parameters if all the pairs are found by the user.
		if( numMatchesFound == 8 )
		{
			numMatchesFound = 0;
			AnimalEmojisButtonsLayout.IsVisible = false;
			PlayAgainButton.IsVisible = true;
			this.progress = 0.0;
			ProgressBar.ProgressTo(this.progress, 100, Easing.Linear);
			ProgressBar.IsVisible = false;
			BestScore.IsVisible = true;
		}
	}
// //////////////////////////////////////// #endregion

	/// <summary>
	/// Moves the time ticker and resets when the game is completed.
	/// </summary>
	private bool TimerTick()
	{
			if(!this.IsLoaded) return false;
			++this.tenthsOfSecondsElapsed;
			TimeElapsed.Text = "Time Elapsed: " +
				(this.tenthsOfSecondsElapsed / 10F).ToString("0.0s");
			
			if(PlayAgainButton.IsVisible)
			{
				TimeElapsed.Text = "Last Game Score: " +
				 (this.tenthsOfSecondsElapsed / 10F).ToString("0.0 seconds.");
				this.timeScores.Add(this.tenthsOfSecondsElapsed);
				this.tenthsOfSecondsElapsed = 0;
				BestScore.Text = "Best Game Score: " +
					(this.timeScores.Min() / 10F).ToString("0.0 seconds.") ;
				return false;
			}
			return true;
	}

	/// <summary>
	/// When the user clicks play again button, it resets the game board.
	/// </summary>
	private void PlayAgainButton_Clicked(object sender, EventArgs e) 
	{
		AnimalEmojisButtonsLayout.IsVisible = true;
		PlayAgainButton.IsVisible = false;
		ProgressBar.IsVisible = true;
		BestScore.IsVisible = false;
		
		List<string>  animalEmojis = [
			"🐅", "🐅", "🦁", "🦁", "🦭", "🦭", "🐬", "🐬",
			"🦋", "🦋", "🦅", "🦅", "🦚", "🦚", "🐄", "🐄" ,
		];

		foreach( var button in AnimalEmojisButtonsLayout.Children.OfType<Button>() )
		{
			int index = Random.Shared.Next(animalEmojis.Count);
			string nextEmoji = animalEmojis[index];
			button.Text = nextEmoji;
			animalEmojis.RemoveAt(index);
		}

		Dispatcher.StartTimer(interval: TimeSpan.FromSeconds(0.1), TimerTick);
	}

// //////////////////////////////////////// #endregion

	public MainPage()
	{
		InitializeComponent();
	}

}