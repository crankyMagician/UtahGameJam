using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace J
{

/// Global Constants
public class AIMovementConstantParameters
{
	/// Choose a value > 5 so that the bots don't spawn in the screen
	public float yStartLocation = 5.5f;

	/// -5 is bottom of the screen -7 so if the AI
	//// is ease to the bottom it doesn't just hover at the botom
	public float yTargetLocation = -7;

	public float xMin = -10;
	public float xMax = 10;

	public float timeToBottomMin = 9;
	public float timeToBottomMax = 14;

	public float timeToSideMin = 5;
	public float timeToSideMax = 7;

	public static AIMovementConstantParameters Instance = new AIMovementConstantParameters();
	public static Vector3 RandomInitialPosition()
	{
		var position = new Vector3();
		position.y = Instance.yStartLocation;
		position.x = Random(Instance.xMin, Instance.xMax);
		return position;
	}

	//TODO: These should be in a 'random' FILE but this is crappy
	public static float Random(float min, float max)
	{
		return ((float)random.NextDouble() * (max - min)) + min;
	}

	public static int RandomInt(int min, int max)
	{
		return (int)((random.NextDouble() * (max - min)) + min);
	}

	private static System.Random random = new System.Random();
};

/// Defines a specific path the AI will take
public class AIEaseToPosition
{
	/// Time its going to take to shift to a single side
	public float seconds = 0;

	/// Time its going to take to shift to a single side
	public Func<float, float> easeFunction;

	/// If this is AIEaseTo for top to bottom this should always be
	/// the same as yTargetLocation
	///
	/// Otherwise this should be constrained between xMin and xMax
	public float targetPosition = 0;

	/// A factory function that initializes the data to random valid values
	public static AIEaseToPosition MakeSide()
	{
		var data = new AIEaseToPosition();

		while (data.seconds == 0)
			data.seconds = Random(Constants.timeToSideMin, Constants.timeToSideMax);
		data.easeFunction = RandomEaseFunction();
		data.targetPosition = Random(Constants.xMin, Constants.xMax);
		return data;
	}

	/// A factory function that initializes the data to random valid values
	public static AIEaseToPosition MakeTop()
	{
		var data = new AIEaseToPosition();

		while (data.seconds == 0)
			data.seconds = Random(Constants.timeToBottomMin, Constants.timeToBottomMax);
		data.easeFunction = RandomEaseFunction_TopBot();
		data.targetPosition = Constants.yTargetLocation;
		return data;
	}

	//TODO: These should be in a 'random' FILE but this is crappy
	public static float Random(float min, float max)
	{
		return ((float)random.NextDouble() * (max - min)) + min;
	}

	public static int RandomInt(int min, int max)
	{
		return (int)((random.NextDouble() * (max - min)) + min);
	}

	public static Func<float, float> RandomEaseFunction()
	{
		int index = RandomInt(0, EasingFunctions.FunctionList.Count());
		return EasingFunctions.FunctionList[index];
	}

	public static Func<float, float> RandomEaseFunction_TopBot()
	{
		int index = RandomInt(0, EasingFunctions.TopBotFunctionList.Count());
		return EasingFunctions.TopBotFunctionList[index];
	}
	private static AIMovementConstantParameters Constants => AIMovementConstantParameters.Instance;
	private static System.Random random = new System.Random();
};

/// Defines how the AI is going to move
public class AIParameters
{
	public float initialXPosition;

	public List<AIEaseToPosition> timeToSides = new List<AIEaseToPosition>();

	///...Hmmm we could make this a list if we want to have dynamic shift in how the AI reaches the bottom///
	public AIEaseToPosition timeToBottom;

	public static AIParameters Make(int sidePaths = 10)
	{
		var parameters = new AIParameters();
		for (int i = 0; i < sidePaths; ++i)
		{
			parameters.timeToSides.Add(AIEaseToPosition.MakeSide());
		}

		parameters.initialXPosition = AIEaseToPosition.Random(
			AIMovementConstantParameters.Instance.xMin,
			AIMovementConstantParameters.Instance.xMax);

		parameters.timeToBottom = AIEaseToPosition.MakeTop();
		return parameters;
	}
};

public class AIMovement : MonoBehaviour
{
	[SerializeField] private ParticleSystem deathParticlesPrefab;

	/// Shouldn't be here but hacks
	[SerializeField] private RandomSoundPlayer onBotDestroySoundPlayer;

	public delegate void OnEnemyReachedBottomDelegate(AIMovement movement);

	static OnEnemyReachedBottomDelegate OnEnemyReachedBottom;
	OnEnemyReachedBottomDelegate OnReachedBottom;

	public static AIMovementConstantParameters Constants => AIMovementConstantParameters.Instance;

	// This is going to be shared across a single-mob set
	public AIParameters movementParameters = null;

	public int movementParametersSideIndex = 0;

	/// Amount this has been alive (Used for lerping from top to bottom)
	float lifetime = 0;

	/// Amount of time this has been going left/right - used in lerping from left to right
	float sideLifetime = 0;

	/// The initial x position when they switched the last left/right direction, this is used in smoothing left/right
	float initialX = 0;

	AIEaseToPosition CurrentSideParams
	{
		get
		{
			if (movementParameters.timeToSides.Count() > 0)
			{
				int index = movementParametersSideIndex % movementParameters.timeToSides.Count();
				return movementParameters.timeToSides[index];
			}
			return null;
		}
	}

	float CurrentTargetX => CurrentSideParams.targetPosition;
	float CurrentTimeToSide => CurrentSideParams.seconds;
	float CurrentTimeToBottom => movementParameters.timeToBottom.seconds;

	Func<float, float> CurrentSideEase => CurrentSideParams.easeFunction;
	Func<float, float> CurrentBottomEase => movementParameters.timeToBottom.easeFunction;

	private void Awake()
	{
		// Constants.yTargetLocation = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.1f, 10)).y;
		// Constants.yTargetLocation -= 1;
	}

	private void Start()
	{
		lifetime = 0;
		sideLifetime = 0;
		movementParametersSideIndex = 0;

		//TODO: make this init etter
		initialX = transform.position.x;

		GameManager.OnGameRestart_OneShot += () =>
		{
			DestroyBot();
		};
	}

	private void Update()
	{
		lifetime += Time.deltaTime;
		sideLifetime += Time.deltaTime;

		float alphaLeftRight = sideLifetime / CurrentTimeToSide;
		float tweenAlphaLeftRight = CurrentSideEase.Invoke(alphaLeftRight);

		float alpha = lifetime / CurrentTimeToBottom;
		float tweenAlpha = CurrentBottomEase.Invoke(alpha);

		Vector3 pos = transform.position;
		pos.y = Mathf.Lerp(
				Constants.yStartLocation,
				Constants.yTargetLocation,
				tweenAlpha);

		// Check for nans - idk why this happens
		if (pos.y != pos.y)
			pos.y = transform.position.y;

		pos.x = Mathf.Lerp(
				initialX,
				CurrentTargetX,
				tweenAlphaLeftRight);

		// Check for nans - idk why this happens
		if (pos.x != pos.x)
			pos.x = transform.position.x;

		transform.position = pos;

		///Check if we are close to Side Bound
		/// Swap the swides if we hit the other side
		if (Mathf.Abs(transform.position.x - CurrentTargetX) < .005)
		{
			initialX = CurrentTargetX;
			sideLifetime = 0;
			++movementParametersSideIndex;
		}
		///Check if we are close
		if (Mathf.Abs(transform.position.y - Constants.yTargetLocation) < .005)
		{
			OnEnemyReachedBottom?.Invoke(this);
			OnReachedBottom?.Invoke(this);
			DestroyBot();

			GameManager.Instance.RemoveTimeFromActiveTimer();
		}
	}

	///Should we destroy here ??
	public void DestroyBot()
	{
		onBotDestroySoundPlayer.PlayRandomSoundGlobal();
		ParticleSystem particle = Instantiate(deathParticlesPrefab);
		particle.transform.position = transform.position;
		particle.collision.AddPlane(PlayerController.Player.transform);
		Destroy(gameObject);
	}
}


}