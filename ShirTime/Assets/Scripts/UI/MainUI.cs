using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using UniRx;
using System;

public class MainUI : MonoBehaviour ,IMainUICallbacks {
    [SerializeField]
    private Button startTime,stopTime,openCustomTimePanel;
    [SerializeField]
    private Text status;
    private AddDifferentTimeUI addDifferent;

    public IObservable<Unit> StartTimeClicked { get; private set; }
    public IObservable<Unit> StopTimeClicked { get;private set;}

    [Inject]
    void Construct(AddDifferentTimeUI addDifferent)
    {
        this.addDifferent = addDifferent;
    }
	void Start()
    {
        StartTimeClicked= startTime.OnClickAsObservable();
        StopTimeClicked= stopTime.OnClickAsObservable();
        openCustomTimePanel.OnClickAsObservable().Subscribe(x=>addDifferent.gameObject.SetActive(true));
    }
}
