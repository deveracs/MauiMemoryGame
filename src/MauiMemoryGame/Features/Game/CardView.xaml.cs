namespace MauiMemoryGame.Features;

public partial class CardView
{
    private CompositeDisposable disposables;

    public static readonly BindableProperty CardProperty = BindableProperty.Create(nameof(Card), typeof(Card), typeof(CardView), defaultBindingMode: BindingMode.OneWay, propertyChanged: CardChanged);
    public bool IsShowingContent => frContent.IsVisible == true;

    public event EventHandler Clicked;

    public CardView()
	{
		InitializeComponent();
        disposables = new CompositeDisposable();
        CreateEvents();
    }

    ~CardView()
    {
        disposables?.Dispose();
    }

    public Card Card
    {
        get => (Card)GetValue(CardProperty);
        set => SetValue(CardProperty, value);
    }

    public async Task ShowContent()
    {
        frContent.RotationY = -270;
        await frBackwards.RotateYTo(-90);
        frBackwards.IsVisible = false;
        frContent.IsVisible = true;
        await frContent.RotateYTo(-360);
        frContent.RotationY = 0;
    }

    public async Task HideContent()
    {
        frBackwards.RotationY = -270;
        await frContent.RotateYTo(-90);
        frContent.IsVisible = false;
        frBackwards.IsVisible = true;
        await frBackwards.RotateYTo(-360);
        frBackwards.RotationY = 0;
    }

    private void CreateEvents()
    {
        IObservable<EventPattern<object>> gridContentClicked = Observable.FromEventPattern(h => tapContent.Tapped += h, h => tapContent.Tapped -= h);
        disposables.Add(gridContentClicked.Subscribe(x => Clicked?.Invoke(this, null)));

        IObservable<EventPattern<object>> tapBackwardslicked = Observable.FromEventPattern(h => tapBackwards.Tapped += h, h => tapBackwards.Tapped -= h);
        disposables.Add(tapBackwardslicked.Subscribe(x => Clicked?.Invoke(this, null)));
    }

    private static void CardChanged(BindableObject bindable, object oldValue, object newValue)
    {
        Card newCard = newValue as Card;
        ((CardView)bindable).imgContent.Source = newCard != null ? 
            ImageSource.FromFile(newCard.ImagePath) : 
            null;
    }
}