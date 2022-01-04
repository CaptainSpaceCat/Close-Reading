using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextInputHandler
{
	private Dictionary<int, List<string>> wordDict;

    public TextInputHandler(TextAsset corpus, int randomSeed) {
        Random.InitState(randomSeed);
        wordDict = new Dictionary<int, List<string>>();
        string[] wordList = corpus.ToString().Split('\n');
        for (int i = 0; i < wordList.Length; i++) {
            string currentWord = wordList[i].Substring(0, wordList[i].Length - 1);
            if (!wordDict.ContainsKey(currentWord.Length)) {
                wordDict[currentWord.Length] = new List<string>();
            }
            wordDict[currentWord.Length].Add(currentWord);
        }
    }

    public TextInputHandler(TextAsset corpus) {
        wordDict = new Dictionary<int, List<string>>();
        string[] wordList = corpus.ToString().Split('\n');
        for (int i = 0; i < wordList.Length; i++) {
            string currentWord = wordList[i].Substring(0, wordList[i].Length - 1);
            if (!wordDict.ContainsKey(currentWord.Length)) {
                wordDict[currentWord.Length] = new List<string>();
            }
            wordDict[currentWord.Length].Add(currentWord);
        }
    }

    public string getWordOfLength(int len) {
    	List<string> wordList = wordDict[len];
    	int choice = Random.Range(0, wordList.Count);
    	return wordList[choice];
    }
}
