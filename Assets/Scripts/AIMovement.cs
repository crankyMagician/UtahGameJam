using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace J
{


public class AIMovement : MonoBehaviour
{
	public delegate void OnEnemyReachedBottomDelegate(AIMovement movement);

	static OnEnemyReachedBottomDelegate OnEnemyReachedBottom;
	OnEnemyReachedBottomDelegate OnReachedBottom;

	/// This should be 'top of the screen location'
	float yStartLocation = 5;

	/// Bottom of the screen location (determines when this gets past the player )
	float yTargetLocation = 0;

	/// Amount of time it takes for this to reach the bottom
	float timeToBottom = 10;

	/// Amount this has been alive (Used for lerping from top to bottom)
	float lifetime = 0;

	/// Amount of time this has been going left/right - used in lerping from left to right
	float sideLifetime = 0;

	/// Denotes the furthest point leftwards they can be on the side of the screen
	/// When the enemy hits this point it will attemp to start moving in the other
	/// Direction
	/// we can change this to be smaller/larger then the screen to change the timing of this
	float xLowerBound = -10;

	/// Denotes the furthest point rightwards they can be on the side of the screen
	float xUpperBound = 10;
	float currentTargetSide = -10;

	/// Amount of time its going to take to reach the next side
	float timeToNextSide = 5;

	/// The initial x position when they switched the last left/right direction, this is used in smoothing left/right
	float initialX;

	private void Awake()
	{
		yTargetLocation = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.1f, 10)).y;
		yTargetLocation -= 1;
	}

	private void Start()
	{
		lifetime = 0;
		sideLifetime = 0;

		//TODO: make this init etter
		currentTargetSide = xLowerBound;
		initialX = transform.position.x;
	}

	private void Update()
	{
		lifetime += Time.deltaTime;
		sideLifetime += Time.deltaTime;

		float alphaLeftRight = sideLifetime / timeToNextSide;
		float tweenAlphaLeftRight = EasingFunctions.InOutBounce(alphaLeftRight);

		float alpha = lifetime / timeToBottom;
		float tweenAlpha = EasingFunctions.InOutBounce(alpha);

		Vector3 pos = transform.position;
		pos.y = Mathf.Lerp(
				yStartLocation,
				yTargetLocation,
				tweenAlpha);

		pos.x = Mathf.Lerp(
				initialX,
				currentTargetSide,
				tweenAlphaLeftRight);

		transform.position = pos;

		///Check if we are close to Side Bound
		/// Swap the swides if we hit the other side
		if (Mathf.Abs(transform.position.x - currentTargetSide) < .005)
		{
			if (currentTargetSide == xLowerBound)
				currentTargetSide = xUpperBound;
			else if (currentTargetSide == xUpperBound)
				currentTargetSide = xLowerBound;

			initialX = transform.position.x;
			sideLifetime = 0;
		}
		///Check if we are close
		if (Mathf.Abs(transform.position.y - yTargetLocation) < .005)
		{
			OnEnemyReachedBottom?.Invoke(this);
			OnReachedBottom?.Invoke(this);
			DestroyBot();
		}
	}

	///Should we destroy here ??
	private void DestroyBot()
	{
		Destroy(gameObject);
	}
}


}