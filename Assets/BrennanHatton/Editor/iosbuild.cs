using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
 
public class iosbuild : MonoBehaviour
{
 
	[PostProcessBuild(999)]
	public static void OnPostProcessBuild(BuildTarget buildTarget, string path)
	{
 
		if (buildTarget != BuildTarget.iOS)
		{
			return;
		}
		var projPath = PBXProject.GetPBXProjectPath(path);
		var project = new PBXProject();
		project.ReadFromFile(projPath);
		var mainTargetGuid = project.GetUnityMainTargetGuid();
		foreach (var targetGuid in new[] {mainTargetGuid, project.GetUnityFrameworkTargetGuid()})
		{
			project.SetBuildProperty(targetGuid, "ENABLE_BITCODE", "NO");
		}
		project.WriteToFile(projPath);
	}
}