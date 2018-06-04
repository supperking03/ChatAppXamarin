using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

using ChattingApp.Enums;
using ChattingApp.Events;
using ChattingApp.Models;

using Prism.Events;
using Prism.Navigation;

using Xamarin.Forms;

namespace ChattingApp.ViewModels
{
	public class SearchPageViewModel : BindableBase
	{
	    private GenderEnum _selectedGender;
	    private bool _isFindingPartner;

	    private readonly IEventAggregator _eventAggregator;

	    public GenderEnum SelectedGender
	    {
	        get
	        {
	            return _selectedGender;
	        }
	        set
	        {
	            SetProperty(ref _selectedGender, value);
	        }
	    }

	    private INavigationService _navigationService;

	    public bool IsFindingPartner
	    {
	        get
	        {
	            return _isFindingPartner;
	        }
	        set
	        {
	            SetProperty(ref _isFindingPartner, value);
	        }
	    }


	    public ICommand FindPartnerCommand
	    {
	        get
	        {
	            return new DelegateCommand(FindPartner);
	        }
	    }

	    private void FindPartner()
	    {
	        if (IsFindingPartner)
	        {
	            IsFindingPartner = false;
	            _eventAggregator.GetEvent<StopFindingEvent>().Publish();
	        }
	        else
	        {
	            IsFindingPartner = true;
	            _eventAggregator.GetEvent<FindEvent>().Publish(SelectedGender);
	        }
	    }


	    public SearchPageViewModel(INavigationService navigationService, IEventAggregator eventAggregator)
	    {
	        _navigationService = navigationService;
	        _isFindingPartner = false;
	        _eventAggregator = eventAggregator;
            InitEvent ( _eventAggregator );
	    }

	    private void InitEvent(IEventAggregator eventAggregator)
	    {
	        eventAggregator.GetEvent<RoomFoundEvent>().Subscribe(OnRoomFound);
	    }

	    private void DestroyEvent(IEventAggregator eventAggregator)
	    {
	        eventAggregator.GetEvent<RoomFoundEvent>().Unsubscribe(OnRoomFound);
	    }

	    private void OnRoomFound()
	    {
	        IsFindingPartner = false;
	        Device.BeginInvokeOnMainThread(async () =>
	        {
	            await _navigationService.NavigateAsync("ChatPage");
	        });
	    }

	    public void OnNavigatedFrom(NavigationParameters parameters)
	    {
	        DestroyEvent(_eventAggregator);
	    }

	    public void OnNavigatedTo(NavigationParameters parameters)
	    {
	        InitEvent(_eventAggregator);
	        IsFindingPartner = false;
	    }

	    public void OnNavigatingTo(NavigationParameters parameters)
	    {
	    }
    }
}
