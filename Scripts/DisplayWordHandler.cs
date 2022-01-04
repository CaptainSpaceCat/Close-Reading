using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayWordHandler : MonoBehaviour
{
	private List<Coroutine> activeCoroutines = new List<Coroutine>();
	[SerializeField]
    private string currentWord;
    public TextMeshPro displayWordText;
    private ModifierList currentModifiers = new ModifierList();

    //assumes that this is defined with 4 gameobjects, each with a box collider 2D
    public GameObject[] walls;

    public void UpdateBoundingBox(Vector3 scale) {
    	walls[0].transform.position = new Vector3(-scale.x/2 - .5f, 0, 0);
    	walls[0].transform.localScale = new Vector3(1, scale.y, 1);

    	walls[1].transform.position = new Vector3(scale.x/2 + .5f, 0, 0);
    	walls[1].transform.localScale = new Vector3(1, scale.y, 1);

    	walls[2].transform.position = new Vector3(0, scale.y/2 + .5f, 0);
    	walls[2].transform.localScale = new Vector3(scale.x, 1, 1);

    	walls[3].transform.position = new Vector3(0, -scale.y/2 - .5f, 0);
    	walls[3].transform.localScale = new Vector3(scale.x, 1, 1);
    }

    void Start() {
    	// SetText("TestWord");
    	// ModifierList mflist = new ModifierList();
    	// mflist.backwards = true;
    	// SetModifiers(mflist);
    	// Flush();
    }

    public void SetText(string newWord) {
    	currentWord = newWord;
    }

    public string GetText() {
    	return currentWord;
    }

    public void SetModifiers(ModifierList modifiers) {
    	currentModifiers = modifiers;
    }

    public void Flush() {
    	foreach(Coroutine routine in activeCoroutines) {
    		StopCoroutine(routine);
    	}
    	activeCoroutines.Clear();

    	if (currentModifiers.flippedVertically) {
    		Vector3 sc = displayWordText.transform.localScale;
    		displayWordText.transform.localScale = new Vector3(-sc.x, sc.y, sc.z);
    		displayWordText.transform.Rotate(0, 0, 180);
    	}
    	if (currentModifiers.flippedHorizontally) {
    		Vector3 sc = displayWordText.transform.localScale;
    		displayWordText.transform.localScale = new Vector3(-sc.x, sc.y, sc.z);
    	}
    	if (currentModifiers.backwards) { //maybe dont do this one
    		currentWord = ReverseWord(currentWord);
    	}
    	if (currentModifiers.rotatedRandom) {
    		displayWordText.transform.Rotate(0, 0, Random.Range(0, 360));
    	}
    	if (currentModifiers.shaky) {//smooth this out
    		StartCoroutine(ShakeWord());
    	}
    	displayWordText.text = currentWord;
    	displayWordText.ForceMeshUpdate();
    	UpdateBounds();
    }

    public GameObject backgroundQuad;
    private void UpdateBounds() {
    	Vector3 sc = displayWordText.textBounds.extents;
    	backgroundQuad.transform.localScale = new Vector3(sc.x * 2 + .2f, sc.y * 2 + 0f, 1);
    	UpdateBoundingBox(backgroundQuad.transform.localScale);
    }

    public Vector3 GetBounds() {
    	return backgroundQuad.transform.localScale;
    }

    private string ReverseWord(string s) {
		char[] charArray = s.ToCharArray();
		char[] reversed = new char[charArray.Length];
		for (int i = 0; i < charArray.Length; i++) {
			reversed[i] = charArray[charArray.Length - 1 - i];
		}
		return new string(reversed);
	}

	public float shakeScale = .05f;
	private IEnumerator ShakeWord() {
		Vector3 initialPos = displayWordText.transform.position;
		while (true) {
			yield return new WaitForSeconds(.05f);
			Vector2 choice = Random.insideUnitCircle.normalized * shakeScale;
			displayWordText.transform.position = new Vector3(initialPos.x + choice.x, initialPos.y + choice.y, initialPos.z);
		}
	}
}
