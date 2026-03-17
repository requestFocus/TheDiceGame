using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Rotate : MonoBehaviour
{
	[SerializeField] private Button _rotateButton;
	[SerializeField] private Transform _ball;

	public float BallSpeed = 10;

	private bool _isRotating;

	private void Start()
	{
		_rotateButton.onClick.AddListener(OnButtonClicked);
		
		Application.targetFrameRate = 60;
	}

	private void OnButtonClicked()
	{
		_isRotating = !_isRotating;

		if (_isRotating)
		{
			RotateObjectAsyncMethod().Forget(); 
		}
	}

	private async UniTaskVoid RotateObjectAsyncMethod()
	{
		while (_isRotating)
		{
			_ball.transform.Rotate(Vector3.forward, BallSpeed * Time.deltaTime);
			await UniTask.Yield(PlayerLoopTiming.Update);
		}	
	}
}