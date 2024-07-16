using UnityEngine;
using UnityEngine.UI;

public class VersionNumber : MonoBehaviour 
{
    public Text field;
    public TextAsset textFile;

	void Start () 
    {
        this.field.text = this.textFile.text;
	}
}
