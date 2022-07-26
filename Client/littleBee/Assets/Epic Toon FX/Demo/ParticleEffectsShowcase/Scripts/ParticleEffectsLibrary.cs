using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParticleEffectsLibrary : MonoBehaviour {
	public static ParticleEffectsLibrary GlobalAccess;
	void Awake () {
		GlobalAccess = this;

		currentActivePEList = new List<Transform> ();

		TotalEffects = ParticleEffectPrefabs.Length;

		CurrentParticleEffectNum = 1;

		// Warn About Lengths of Arrays not matching
		if (ParticleEffectSpawnOffsets.Length != TotalEffects) {
			Debug.LogError ("ParticleEffectsLibrary-ParticleEffectSpawnOffset: Not all arrays match length, double check counts.");
		}
		if (ParticleEffectPrefabs.Length != TotalEffects) {
			Debug.LogError ("ParticleEffectsLibrary-ParticleEffectPrefabs: Not all arrays match length, double check counts.");
		}

		// Setup Starting PE Name String
		effectNameString = ParticleEffectPrefabs [CurrentParticleEffectIndex].name + " (" + CurrentParticleEffectNum.ToString() + " of " + TotalEffects.ToString() + ")";
	}

	// Stores total number of effects in arrays - NOTE: All Arrays must match length.
	public int TotalEffects = 0;
	public int CurrentParticleEffectIndex = 0;
	public int CurrentParticleEffectNum = 0;
//	public string[] ParticleEffectDisplayNames;
	public Vector3[] ParticleEffectSpawnOffsets;
	// How long until Particle Effect is Destroyed - 0 = never
	public float[] ParticleEffectLifetimes;
	public GameObject[] ParticleEffectPrefabs;

	// Storing for deleting if looping particle effect
	private string effectNameString = "";
	private List<Transform> currentActivePEList;

	void Start () {
	}

	public string GetCurrentPENameString() {
		return ParticleEffectPrefabs [CurrentParticleEffectIndex].name + " (" + CurrentParticleEffectNum.ToString() + " of " + TotalEffects.ToString() + ")";
	}

	public void PreviousParticleEffect() {
		// Destroy Looping Particle Effects
		if (ParticleEffectLifetimes [CurrentParticleEffectIndex] == 0) {
			if (currentActivePEList.Count > 0) {
				for (int i = 0; i < currentActivePEList.Count; i++) {
					if (currentActivePEList [i] != null) {
						Destroy (currentActivePEList [i].gameObject);
					}
				}
				currentActivePEList.Clear ();
			}
		}

		// Select Previous Particle Effect
		if (CurrentParticleEffectIndex > 0) {
			CurrentParticleEffectIndex -= 1;
		} else {
			CurrentParticleEffectIndex = TotalEffects - 1;
		}
		CurrentParticleEffectNum = CurrentParticleEffectIndex + 1;

		// Update PE Name String
		effectNameString = ParticleEffectPrefabs [CurrentParticleEffectIndex].name + " (" + CurrentParticleEffectNum.ToString() + " of " + TotalEffects.ToString() + ")";
	}
	public void NextParticleEffect() {
		// Destroy Looping Particle Effects
		if (ParticleEffectLifetimes [CurrentParticleEffectIndex] == 0) {
			if (currentActivePEList.Count > 0) {
				for (int i = 0; i < currentActivePEList.Count; i++) {
					if (currentActivePEList [i] != null) {
						Destroy (currentActivePEList [i].gameObject);
					}
				}
				currentActivePEList.Clear ();
			}
		}

		// Select Next Particle Effect
		if (CurrentParticleEffectIndex < TotalEffects - 1) {
			CurrentParticleEffectIndex += 1;
		} else {
			CurrentParticleEffectIndex = 0;
		}
		CurrentParticleEffectNum = CurrentParticleEffectIndex + 1;

		// Update PE Name String
		effectNameString = ParticleEffectPrefabs [CurrentParticleEffectIndex].name + " (" + CurrentParticleEffectNum.ToString() + " of " + TotalEffects.ToString() + ")";
	}

	private Vector3 spawnPosition = Vector3.zero;
	public void SpawnParticleEffect(Vector3 positionInWorldToSpawn) {
		// Spawn Currently Selected Particle Effect
		spawnPosition = positionInWorldToSpawn + ParticleEffectSpawnOffsets[CurrentParticleEffectIndex];
		GameObject newParticleEffect = GameObject.Instantiate(ParticleEffectPrefabs[CurrentParticleEffectIndex], spawnPosition, ParticleEffectPrefabs[CurrentParticleEffectIndex].transform.rotation) as GameObject;
		newParticleEffect.name = "PE_" + ParticleEffectPrefabs[CurrentParticleEffectIndex];
		// Store Looping Particle Effects Systems
		if (ParticleEffectLifetimes [CurrentParticleEffectIndex] == 0) {
			currentActivePEList.Add (newParticleEffect.transform);
		}
		currentActivePEList.Add(newParticleEffect.transform);
		// Destroy Particle Effect After Lifetime expired
		if (ParticleEffectLifetimes [CurrentParticleEffectIndex] != 0) {
			Destroy(newParticleEffect, ParticleEffectLifetimes[CurrentParticleEffectIndex]);
		}
	}
}
